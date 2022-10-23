using RankingLibrary.CalculationEngine;
using RankingLibrary.SupportObjects.RatingObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RankingLibrary.SupportObjects.MatchObjects
{
    public class Result
    {
        private const double POINTS_FOR_WIN = 1.0;
        private const double POINTS_FOR_LOSS = 0.0;
        private const double POINTS_FOR_DRAW = 0.5;

        private readonly bool isDraw;
        private readonly Rating winner;
        private readonly Rating loser;

        public Result(Rating inputWinner, Rating inputLoser, bool inputIsDraw = false)
        {
            winner = inputWinner;
            loser = inputLoser;
            isDraw = inputIsDraw;

            if(!ValidPlayers(inputWinner, inputLoser))
            {
                throw new ArgumentException("Player winner and loser are the same player");
            }



        }

        private static bool ValidPlayers(Rating player1, Rating player2)
        {
            return player1.Id != player2.Id;
        }

        public bool Participated(Rating player)
        {
            return player.Id == winner.Id || player.Id == loser.Id;
        }

        public double GetScore(Rating player)
        {
            double score;

            if(winner == player)
            {
                score = POINTS_FOR_WIN;
            }
            else if(loser == player)
            {
                score = POINTS_FOR_LOSS;
            }
            else if(isDraw == true)
            {
                score = POINTS_FOR_DRAW;
            }
            else
            {
                throw new ArgumentException("Player did not participate in match", "player");
            }

            return score;
        }

        public Rating GetOpponent(Rating player)
        {
            Rating opponent;

            if(winner == player)
            {
                opponent = loser;
            }
            else if(loser == player)
            {
                opponent = loser;
            }
            else if (isDraw == true)
            {
                opponent = loser;
            }
            else
            {
                throw new ArgumentException("Player did not participate in match", "player");
            }
            return opponent;
        }

        public Rating GetWinner()
        {
            return winner;
        }

        public Rating GetLoser()
        {
            return loser;
        }
    }
}
