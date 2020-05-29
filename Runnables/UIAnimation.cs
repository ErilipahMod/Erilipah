using Erilipah.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Terraria.UI;

namespace Erilipah.Runnables
{
    public abstract class UIAnimation
    {
        private UserInterfaceWrapper uiWrapper;
        private bool finished;

        public event Action OnFinish;

        public abstract string AboveLayer { get; }

        public static bool IsActive(Type type)
        {
            return Erilipah.Instance.UIs.Any(uiw =>
                uiw.Interface.CurrentState is AnimState animState &&
                animState.container.GetType() == type
                );
        }

        public bool Active => !finished;

        public void Finish()
        {
            OnFinish?.Invoke();
            finished = true;
        }

        public void Start()
        {
            UserInterface ui = new UserInterface();
            ui.SetState(new AnimState(this));

            string name = "Erilipah: " + GetType().FullName + GetHashCode().ToString();
            uiWrapper = new UserInterfaceWrapper(ui, ui.GetModifyInterfaceDel(AboveLayer, name));

            Erilipah.Instance.UIs.Add(uiWrapper);
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
        }

        private void PrivateUpdate(GameTime gameTime)
        {
            if (finished)
            {
                Erilipah.Instance.UIs.Remove(uiWrapper);
            }
            else
            {
                Update(gameTime);
            }
        }

        private class AnimState : UIState
        {
            internal readonly UIAnimation container;

            public AnimState(UIAnimation container)
            {
                this.container = container;
            }

            public override void Update(GameTime gameTime)
            {
                container.PrivateUpdate(gameTime);
            }

            public override void Draw(SpriteBatch spriteBatch)
            {
                container.Draw(spriteBatch);
            }
        }
    }
}