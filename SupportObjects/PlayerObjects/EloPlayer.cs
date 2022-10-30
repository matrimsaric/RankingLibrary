using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RankingLibrary.SupportObjects.PlayerObjects
{
    public class EloPlayer : Player
    {
        public Elo CurrentElo { get; set; }
        public List<Elo> HistoricalElo { get; set; }

        public EloPlayer(int id, Elo liveElo) : base(id)
        {
            CurrentElo = liveElo;
            HistoricalElo = new List<Elo>();
        }

    }
}
