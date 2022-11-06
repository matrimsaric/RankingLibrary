using RankingLibrary.SupportObjects.PlayerObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RankingLibrary.DataAccess
{
    public abstract class DataAccessParent
    {
        public abstract Task<bool> SaveBasePlayer(Player currentPlayer, bool infoOnly);

        public abstract Task<Player> LoadBasePlayer(int idToLoad);

        public abstract Task<bool> DeleteBasePlayer(Player currentPlayer, bool ifTestClear);

        public abstract Task<bool> DeletePlayer(int playerId, bool ifTestClear);

        public abstract int GetNewPlayerId();

        public abstract Task<DataTable> GetLiveRating(int playerId);

        public abstract Task<DataTable> GetHistoricalRatings(int playerId);




    }
}
