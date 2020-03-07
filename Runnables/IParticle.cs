using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erilipah.Runnables
{
    public interface IParticle
    {
        bool Active { get; }
        void Update();
        void Draw(SpriteBatch spriteBatch);
    }
}
