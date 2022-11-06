using RankingLibrary.DataAccess;
using RankingLibrary.Properties;
using RankingLibrary.Security;
using System;
using System.Collections.Generic;
using System.Data;
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

        public double LiveRating { get; set; }

        public double LiveDeviation { get; set; }

        public double LiveVolatility { get; set; }

        public DataTable HistoricalRatings { get; set; }

        public Player(string usName)
        {
            // TODO get highest ID from DB to use
            Id = dataAccess.GetLiveDataAccess().GetNewPlayerId();

            string sName = String.Empty;
            PlayerStatus = STATUS.ACTIVE;
            DateRegistered = DateTime.Now;
            LiveRating = 1500;// TODO from settings
            LiveDeviation = 350;// TODO from settings
            LiveVolatility = 0.06;// TODO from settings
            HistoricalRatings = new DataTable();// new player has no ratings

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
            if (loadedPlayer.Result != null)
            {
                Name = loadedPlayer.Result.Name;
                PlayerStatus = loadedPlayer.Result.PlayerStatus;
                DateRegistered = loadedPlayer.Result.DateRegistered;
                DateInactive = loadedPlayer.Result.DateInactive;
                HistoricalRatings = new DataTable();// new player has no ratings

                LoadLiveRatingData();               

               
            }
            else
            {
                // player not found throw a warning TODO TEST
                throw new InvalidDataException($"Error accessing Player Id {Id} .");

            }

        }

        public Player(int playerId, bool autoCreate)
        {
            if (playerId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(Id));
            }
            Id = playerId;

            if(autoCreate == true)
            {
                LiveRating = 1500;// TODO from settings
                LiveDeviation = 350;// TODO from settings
                LiveVolatility = 0.06;// TODO from settings
                PlayerStatus = STATUS.ACTIVE;
                DateRegistered = DateTime.Now;
                Name = "TEST USER";
                SavePlayer(false);
            }

            // auto save

            // load player
            Task<Player> loadedPlayer = dataAccess.GetLiveDataAccess().LoadBasePlayer(Id);
            if (loadedPlayer.Result != null)
            {
                Name = loadedPlayer.Result.Name;
                PlayerStatus = loadedPlayer.Result.PlayerStatus;
                DateRegistered = loadedPlayer.Result.DateRegistered;
                DateInactive = loadedPlayer.Result.DateInactive;
                HistoricalRatings = new DataTable();// new player has no ratings

                LoadLiveRatingData();


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
            HistoricalRatings = new DataTable();// new player has no ratings
            if (dateInactive != null)
            {
                DateInactive = dateInactive;
            }

            LoadLiveRatingData();

        }

        private void LoadLiveRatingData()
        {
            // load latest rating and if not found collect default
            Task<DataTable> liveRating = dataAccess.GetLiveDataAccess().GetLiveRating(Id);

            LiveRating = 1500;// TODO from settings (default here in case of load fail)
            LiveDeviation = 350;// TODO from settings(default here in case of load fail)
            LiveVolatility = 0.06;// TODO from settings(default here in case of load fail)

            if (liveRating.Result != null)
            {
                DataRow ratingRow = liveRating.Result.Rows[0];


                if (ratingRow["Rating"] != null)
                {
                    double rat1 = 0;

                    bool worked = Double.TryParse(ratingRow["Rating"].ToString(), out rat1);
                    if (worked)
                    {
                        LiveRating = rat1;
                    }
                }
                if (ratingRow["Deviation"] != null)
                {
                    double rat1 = 0;

                    bool worked = Double.TryParse(ratingRow["Deviation"].ToString(), out rat1);
                    if (worked)
                    {
                        LiveDeviation = rat1;
                    }
                }
                if (ratingRow["Volatility"] != null)
                {
                    double rat1 = 0;

                    bool worked = Double.TryParse(ratingRow["Volatility"].ToString(), out rat1);
                    if (worked)
                    {
                        LiveVolatility = rat1;
                    }
                }


            }
        }

        public void SavePlayer(bool infoOnly)
        {
            dataAccess.GetLiveDataAccess().SaveBasePlayer(this, infoOnly);
        }

        public void DeletePlayer(bool ifTestClear)
        {
            dataAccess.GetLiveDataAccess().DeleteBasePlayer(this, ifTestClear);
        }

        public DataTable GetHistoricalRatings()
        {
            // then try and load historical ratings
            Task<DataTable> historicalRatings = dataAccess.GetLiveDataAccess().GetHistoricalRatings(Id);

            if (historicalRatings.Result != null)
            {
                HistoricalRatings = historicalRatings.Result;
            }

            return HistoricalRatings;
        }
    }
}
