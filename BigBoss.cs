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
    /// Creates instance of Big Boss bad guy  (Darth Vader)
    /// with a Texture2D image and a Rectangle set to size of the image
    /// </summary>
    public class BigBoss : Microsoft.Xna.Framework.Game
    {
        // class for the Boss level fight  Darth Vader
        public Texture2D vaderFighter { get; }
        public Rectangle rVaderFighter;

        public BigBoss(Texture2D v, Rectangle r)
        {
            vaderFighter = v;
            rVaderFighter = r;
        }
    }
}
