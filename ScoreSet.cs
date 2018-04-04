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

namespace SpaceWars
{
    /// <summary>
    /// a row in a dataset table of Player names and scores
    /// </summary>
    class ScoreSet
    {
        public int score;       
        public String name;

        public ScoreSet(int score, string name)
        {
            this.score = score;
            this.name = name;
        }
    }
}
