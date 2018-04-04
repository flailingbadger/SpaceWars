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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceWars
{
    /// <summary>
    /// Creates instance of Minion bad guy  (Tie Fighters)
    /// with a Texture2D image and a Rectangle set to size of the image
    /// </summary>
    public class Minion : Microsoft.Xna.Framework.Game
    {
        public Texture2D minion { get; }
        public Rectangle[,] rMinion;

        public Minion(Texture2D m, Rectangle[,] r)
        {
            minion = m;
            rMinion = r;
        }
    }
}
