using Microsoft.Xna.Framework.Graphics;
using System.Collections;
using System.Collections.Generic;

namespace Erilipah.Runnables
{
    public class ParticleCollection : IEnumerable<IParticle>
    {
        private readonly IList<IParticle> particles;

        public ParticleCollection(IList<IParticle> particles)
        {
            this.particles = particles;
        }

        public ParticleCollection() : this(new List<IParticle>()) { }

        public void Add(IParticle particle)
        {
            particles.Add(particle);
        }

        public void UpdateAll()
        {
            for (int i = particles.Count - 1; i >= 0; i--)
            {
                particles[i].Update();
                if (!particles[i].Active)
                {
                    particles[i].OnKill();
                    particles.RemoveAt(i);
                }
            }
        }

        public void DrawAll(SpriteBatch sb)
        {
            for (int i = 0; i < particles.Count; i++)
            {
                particles[i].Draw(sb);
            }
        }

        IEnumerator<IParticle> IEnumerable<IParticle>.GetEnumerator()
        {
            return particles.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return particles.GetEnumerator();
        }
    }
}
