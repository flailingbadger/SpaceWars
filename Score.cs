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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceWars
{
    /// <summary>
    /// The score class keeps a tally of ememy kills.
    /// Score is displayed at the bottom.
    /// Reads and writes scores to and from a text file.
    /// Determines the top 3 scores and displays them at game end.
    /// </summary>
    class Score
    {
        const String PATH = "Files\\Scores.txt";
        public static int highScore = 2900;
        public static int currentScore = 0;
        public static String currentPlayer;
        static int killPoints = 100;
        public static String stringTop3;

        public static IList<ScoreSet> allScores = new List<ScoreSet>();

        /// <summary>
        /// Reads in scores from a text file and adds to ArrayList allScores
        /// </summary>
        /// <param name="allScores"></param>
        public static void ReadScores()
        {
            allScores.Clear();
            try
            {
                using (StreamReader reader = new StreamReader(PATH))
                {
                    String line = reader.ReadLine();
                    while (line != null)
                    {
                        String[] tokens = line.Split(',');
                        
                        int score = int.Parse(tokens[0]);
                        String name = tokens[1];

                        allScores.Add(new ScoreSet(score, name));
                        line = reader.ReadLine();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read");
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// At Game End - adds current score to ArrayList allScores
        /// </summary>
        /// <param name="allScores"></param>
        public static void AddCurrentToAllScoresList()// param of name entered
        {
            // get name input
            allScores.Add(new ScoreSet(currentScore, currentPlayer));
        }

        /// <summary>
        /// Finds 3 highest scores in ArrayList allScores
        /// </summary>
        /// <param name="allScores"></param>
        public static String FindTopThreeScores()
        {
            ////LINQ
            IList<ScoreSet> topThree = new List<ScoreSet>();
            IList<ScoreSet> temp = allScores.ToList();

            for (int i = 0; i < 3; i++)
            {
                int item = temp.Max(x => x.score);
                IEnumerable<ScoreSet> query1 =
                    from el in temp
                    where el.score == item
                    select el;
                topThree.Add(query1.ElementAt(0));
                temp.Remove(query1.ElementAt(0));
            }

            StringBuilder sb = new StringBuilder();
            
            foreach (var el in topThree)
            {
                sb.Append(el.name).Append(" ").Append(el.score).Append("\n");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Reads scores from ArrayList allScores and saves to text file
        /// </summary>
        public static void WriteScores()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(PATH))
                {

                    foreach (ScoreSet el in allScores)
                    {
                        writer.WriteLine("{0},{1}", el.score, el.name);
                    }
                    writer.Flush();
                    writer.Close();
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("PROBLEM");
                Console.WriteLine("A problem occurred: ");
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Increases score as each enemy is killed
        /// </summary>
        public static void ShipKilled()
        {
            currentScore += killPoints;
        }

        /// <summary>
        /// converts scores to higher value as befits a Big Boss
        /// </summary>
        public static void VaderKilled()
        {
            currentScore += (killPoints * 5);
        }
    }
}
