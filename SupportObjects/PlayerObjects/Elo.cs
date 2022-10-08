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
        public int EloValue { get; set; }
        public double DeviationValue { get; set; }

        public DateTime DataCreated { get; set; }


    }
}
