using RankingLibrary.DataAccess;
using RankingLibrary.Properties;
using RankingLibrary.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RankingLibrary.SupportObjects.PlayerObjects
{
    public enum STATUS
    {
        ACTIVE = 0,
        INACTIVE = 1,
        SUSPENDED = 2,
        BANNED = 3,
        WATCH = 4
    }
    public class Player
    {
        private TextValidator textValidator = new TextValidator();
        private DataAccessProvider dataAccess = new DataAccessProvider();

        public int Id { get; set; }
        public string Name { get; set; }

        public STATUS PlayerStatus { get; set; }

        public DateTime DateRegistered { get; set; }

        public DateTime? DateInactive { get; set; }

        public Player(string usName)
        {
            // TODO get highest ID from DB to use
            Id = dataAccess.GetLiveDataAccess().GetNewPlayerId();

            string sName = String.Empty;
            PlayerStatus = STATUS.ACTIVE;
            DateRegistered = DateTime.Now;

            if (textValidator.ValidateText(usName, out sName))
            {
                Name = sName;
            }
            else
            {
                Name = Resources.DefaultPlayerName;
            }
        }

        public Player(int playerId)
        {
            if (playerId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(Id));
            }
            Id = playerId;

            // load player
            Task<Player> loadedPlayer = dataAccess.GetLiveDataAccess().LoadBasePlayer(Id);
            if(loadedPlayer.Result != null)
            {
                Name = loadedPlayer.Result.Name;
                PlayerStatus = loadedPlayer.Result.PlayerStatus;
                DateRegistered = loadedPlayer.Result.DateRegistered;
                DateInactive = loadedPlayer.Result.DateInactive;
            }
            else
            {
                // player not found throw a warning TODO TEST
                throw new InvalidDataException($"Error accessing Player Id {Id} .");

            }

        }

        public Player(int id, string usName, STATUS status, DateTime dateRegister, DateTime? dateInactive)
        {
            if (id < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }
            Id = id;

            string sName = String.Empty;
            if (textValidator.ValidateText(usName, out sName))
            {
                Name = sName;
            }
            else
            {
                Name = Resources.DefaultPlayerName;
            }

            PlayerStatus = status;
            DateRegistered = dateRegister;
            if(dateInactive != null)
            {
                DateInactive = dateInactive;
            }
            
        }

        public void SavePlayer()
        {
            dataAccess.GetLiveDataAccess().SaveBasePlayer(this);
        }
    }
}
