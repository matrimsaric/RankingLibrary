using RankingLibrary.SupportObjects.PlayerObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace RankingLibrary.CalculationEngine
{
    public enum GAME_RESULT
    {
        LOSE = 0,
        WIN = 1
    }
    public abstract class CalculateMaster
    {
        public abstract bool GetRating(ref Elo player1, ref Elo player2, GAME_RESULT outcome);

        public virtual double GetDefaultRating() { return 1500; }

        

        public static void ExpectationToWin(double player1Rating, double player2Rating, out double p1Value, out double p2Value)
        {
            p1Value = 1.0 / (1.0 + (Math.Pow(10.0, (player2Rating - player1Rating) / 400.0)));
            p2Value = 1.0 / (1.0 + (Math.Pow(10.0, (player1Rating - player2Rating) / 400.0)));

        }



    }

    

}
