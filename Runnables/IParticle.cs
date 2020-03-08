using Microsoft.Xna.Framework.Graphics;

namespace Erilipah.Runnables
{
    public interface IParticle
    {
        bool Active { get; }
        void OnKill();
        void Update();
        void Draw(SpriteBatch sb);
    }
}
