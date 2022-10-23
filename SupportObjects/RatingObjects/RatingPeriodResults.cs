using RankingLibrary.SupportObjects.MatchObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RankingLibrary.SupportObjects.RatingObjects
{
    public class RatingPeriodResults
    {
        private readonly List<Result> results = new List<Result>();
        private readonly HashSet<Rating> participants = new HashSet<Rating>();

        public RatingPeriodResults()
        {

        }

        public RatingPeriodResults(HashSet<Rating> inParticiants)
        {
            participants = inParticiants;
        }

        public void AddResult(Rating winner, Rating loser)
        {
            Result result = new Result(winner, loser);
            results.Add(result);
        }

        public void AddDraw(Rating player1, Rating player2)
        {
            Result result = new Result(player1, player2, true);

            results.Add(result);
        }

        public IList<Result> GetResults(Rating player)
        {
            List<Result> filteredResults = new List<Result>();

            foreach(Result result in results)
            {
                if (result.Participated(player))
                {
                    filteredResults.Add(result);
                }
            }

            return filteredResults;
        }

        public IEnumerable<Rating> GetParticipants()
        {
            foreach(var result in results)
            {
                participants.Add(result.GetWinner());
                participants.Add(result.GetLoser());
            }
            return participants;
        }

        public void AddParticipant(Rating rating)
        {
            participants.Add(rating);
        }

        public void Clear()
        {
            results.Clear();
        }
    }
}
