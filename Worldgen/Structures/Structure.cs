using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;
using System.Collections.Generic;

namespace Erilipah.Worldgen.Structures
{
    public abstract class Structure : IEnumerable<Color>
    {
        private Color[] image;

        public int Width { get; private set; }
        public int Height { get; private set; }

        public virtual string Texture => GetType().FullName.Replace('.', '/');

        public Color this[int x, int y]
        {
            get
            {
                return image[x + Width * y];
            }
        }

        public void Load(Texture2D structureMap)
        {
            Width = structureMap.Width;
            Height = structureMap.Height;
            image = new Color[Width * Height];
            structureMap.GetData(image, 0, image.Length);
        }

        IEnumerator<Color> IEnumerable<Color>.GetEnumerator()
        {
            return (IEnumerator<Color>)image.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return image.GetEnumerator();
        }
    }
}