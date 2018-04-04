/*****************************************************************************************
 * Assignment 05 - Team Project
 * Jason Carter and Melinda Frandsen
 * 
 * CSIS 2410 12/02/17
 * 
 * Space Wars
 * 
 * This is a space combat game with 3 levels of game play.
 * The player must shoot all enemies and not allow them to reach his position.
 * To move, use the left and right arrow keys.
 * To shoot use the spacebar.
 * **************************************************************************************/
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceWars
{
    /// <summary>
    /// Creates instance of Player   (X-Wing Fighter)
    /// with a Texture2D image and a Rectangle set to size of the image
    /// </summary>
    public class Player : Microsoft.Xna.Framework.Game
    {
        public Texture2D xWingFighter { get; }
        public Rectangle rXWingFighter;

        public Player(Texture2D x, Rectangle r)
        {
            xWingFighter = x;
            rXWingFighter = r;
        }
    }
}
