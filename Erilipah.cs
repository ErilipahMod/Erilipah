using Erilipah.Core;
using Erilipah.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;
using Terraria.UI;

namespace Erilipah
{
    public class Erilipah : Mod
    {
        public static Erilipah Instance => ModContent.GetInstance<Erilipah>();

        public event Action OnUnload;

        public event Action<Type> OnCrawlType;

        public ICollection<UserInterfaceWrapper> UIs { get; private set; }

        public override void Load()
        {
            base.Load();

            UIs = new SafeList<UserInterfaceWrapper>();

            Action runLoadFields = delegate { };
            Action runLoadMethods = delegate { };
            Action runCrawl = delegate { };
            Action runPostLoad = delegate { };

            // Get all the types
            foreach (var type in Code.GetExportedTypes())
            {
                runLoadFields += delegate
                {
                    FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                    foreach (var field in fields)
                    {
                        AutoInitAttribute attribute = field.GetCustomAttribute<AutoInitAttribute>();
                        if (attribute == null)
                        {
                            continue;
                        }

                        if (attribute.hooks.HasFlag(InitHooks.Load))
                        {
                            field.SetValue(null, Activator.CreateInstance(attribute.assignTo ?? field.FieldType));
                        }
                        if (attribute.hooks.HasFlag(InitHooks.Unload))
                        {
                            OnUnload += () => field.SetValue(null, null);
                        }
                    }
                };
                runLoadMethods += delegate
                {
                    MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                    foreach (var method in methods)
                    {
                        if (method.GetParameters().Length > 0)
                        {
                            continue;
                        }

                        HookLoadingAttribute attribute = method.GetCustomAttribute<HookLoadingAttribute>();
                        if (attribute == null)
                        {
                            continue;
                        }

                        if (attribute.hooks.HasFlag(LoadHooks.Load))
                        {
                            method.Invoke(null, null);
                        }
                        if (attribute.hooks.HasFlag(LoadHooks.PostLoad))
                        {
                            runPostLoad += delegate { method.Invoke(null, null); };
                        }
                        if (attribute.hooks.HasFlag(LoadHooks.Unload))
                        {
                            OnUnload += () => method.Invoke(null, null);
                        }
                    }
                };
                runCrawl += delegate
                {
                    OnCrawlType?.Invoke(type);
                };
            }

            runLoadFields();
            runLoadMethods();
            runCrawl();
            runPostLoad();

            OnCrawlType = null;
        }

        public override void Unload()
        {
            base.Unload();

            UIs = null;

            OnUnload?.Invoke();
            OnUnload = null;
        }

        public override void UpdateUI(GameTime gameTime)
        {
            base.UpdateUI(gameTime);

            foreach (var ui in UIs)
            {
                ui.Interface.Update(gameTime);
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            base.ModifyInterfaceLayers(layers);

            foreach (var ui in UIs)
            {
                ui.ModifyInterface(layers);
            }
        }
    }
}