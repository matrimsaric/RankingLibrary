using RankingLibrary.SupportObjects.PlayerObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RankingLibrary.DataAccess
{
    public abstract class DataAccessParent
    {
        public abstract Task<bool> SaveBasePlayer(Player currentPlayer);

        public abstract Task<Player> LoadBasePlayer(int idToLoad);

        public abstract Task<bool> DeleteBasePlayer(Player currentPlayer);

        public abstract Task<bool> DeletePlayerFromId(int playerId);
    }
}
