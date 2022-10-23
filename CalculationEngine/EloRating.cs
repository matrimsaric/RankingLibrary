using RankingLibrary.SupportObjects.PlayerObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace RankingLibrary.CalculationEngine
{
    public class EloRating : CalculateMaster
    {
        private double ratingConstant = 32.0;
        public EloRating()
        {
            int overrideElo = 0;

            if (Int32.TryParse(Properties.Resources.DefaultEloKNumber, out overrideElo) == true)
            {
                // safe to override, no one has used a non integer in the resource
                ratingConstant = overrideElo;
            }
        }
        /// <summary>
        ///  formula is Rn = Ro + C * (S - Se)
        /// Rn = new Rating
        /// Ro = old Rating
        /// S = Score
        /// Se = expected Score
        /// C = constant
        /// </summary>
        /// <param name="player1"></param>
        /// <param name="player2"></param>
        /// <param name="outcome"></param>
        /// <returns></returns>
        public override bool GetRating(ref Elo player1, ref Elo player2, GAME_RESULT outcome)
        {
            try
            {

                double expectedScorePlayer1;
                double expectedScorePlayer2;

                ExpectationToWin(player1.EloValue, player2.EloValue, out expectedScorePlayer1, out expectedScorePlayer2);


                if (outcome == GAME_RESULT.WIN)
                {
                    player1.EloValue = player1.EloValue + (ratingConstant * (1.0 - expectedScorePlayer1));
                    player2.EloValue = player2.EloValue + (ratingConstant * (0 - expectedScorePlayer2));
                }
                else
                {
                    player1.EloValue = player1.EloValue + (ratingConstant * (0 - expectedScorePlayer1));
                    player2.EloValue = player2.EloValue + (ratingConstant * (1.0 - expectedScorePlayer2));
                }
                

                return true;

               
            }
            catch (Exception ex)
            {
                return false;
            }

        }
     


    }
}
