using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Erilipah.Runnables
{
    public class Particle : IParticle
    {
        protected readonly Texture2D texture;

        public Color DrawColor = Color.White;
        public Vector2 Position;
        public Vector2 Velocity;
        public Vector2 Acceleration;
        public Rectangle? Frame;
        public float Scale = 1;
        public float Rotation;
        public float RotationChange;

        public Particle(Texture2D texture)
        {
            this.texture = texture;
        }

        public virtual bool Active => Scale > 0 &&
            Position.X > 0 && 
            Position.Y > 0 && 
            Position.X < Main.screenWidth && 
            Position.Y < Main.screenHeight;

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, Frame, DrawColor, Rotation, (Frame?.Size() ?? texture.Size()) / 2, Scale, SpriteEffects.None, 0);
        }

        public virtual void Update()
        {
            Velocity += Acceleration;
            Position += Velocity;
            Rotation += RotationChange;
        }
    }
}
