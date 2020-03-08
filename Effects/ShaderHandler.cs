using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erilipah.Effects
{
    public abstract class ShaderHandler
    {
        private readonly string pass;
        private readonly Effect effect;

        protected ShaderHandler(string file, string technique, string pass)
        {
            effect = Erilipah.Instance.GetEffect(file);
            effect.CurrentTechnique = effect.Techniques[technique];
            this.pass = pass;
        }

        public void Apply()
        {
            Handle(effect);
            effect.CurrentTechnique.Passes[pass].Apply();
        }

        protected abstract void Handle(Effect effect);
    }
}
