using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceWars
{
    class Menu
    {
        /// <summary>
        /// Creates instance of Menu buttons   (Han, Luke, Leia)
        /// with a Texture2D image and a Rectangle set to size of the image
        /// </summary>
        public Texture2D menuButton { get; }
        public Rectangle rMenuButton;

        public Menu(Texture2D menuButton, Rectangle rMenuButton)
        {
            this.menuButton = menuButton;
            this.rMenuButton = rMenuButton;
        }
    }
}
