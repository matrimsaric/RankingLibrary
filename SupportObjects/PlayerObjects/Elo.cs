using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RankingLibrary.SupportObjects.PlayerObjects
{
    public class Elo
    {
        public int PlayerId { get; set; }
        public double EloValue { get; set; }
        public double DeviationValue { get; set; }

        public DateTime DataCreated { get; set; }

        public Elo(int playerId, double eloValue, double deviationValue, DateTime dataCreated)
        {
            PlayerId = playerId;
            EloValue = eloValue;
            DeviationValue = deviationValue;
            DataCreated = dataCreated;
        }
    }
}
