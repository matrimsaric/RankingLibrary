using RankingLibrary.Security;
using RankingLibrary.SupportObjects.PlayerObjects;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Diagnostics.Contracts;

namespace RankingLibrary.DataAccess.MSSQL
{
    public class MsSqlDataAccess : DataAccessParent
    {
        private SqlDataAccess sqlClient = new SqlDataAccess();
        private TextValidator textValidator = new TextValidator();

        /// <summary>
        /// Dangerous. Once system active old players should never be deleted
        /// </summary>
        /// <param name="currentPlayer"></param>
        /// <returns></returns>
        public override Task<bool> DeleteBasePlayer(Player currentPlayer)
        {
            string sql = $"DELETE Player WHERE Id =  '{currentPlayer.Id}'";
            sqlClient.ExecuteNonQuery(sql);

            return Task.FromResult(true);
        }

        public override Task<bool> DeletePlayerFromId(int playerId)
        {
            string sql = $"DELETE Player WHERE Id =  '{playerId}'";
            sqlClient.ExecuteNonQuery(sql);

            return Task.FromResult(true);
        }

        public override Task<Player> LoadBasePlayer(int idToLoad)
        {
            string sql = $"SELECT * FROM Player WHERE Id =  '{idToLoad}'";

            DataTable response = sqlClient.GetData(sql);


            if (response.Rows.Count > 0)
            {
                // get first row and populate fields
                DataRow initial = response.Rows[0];
                string name = (string)initial["Name"];
                STATUS stat = (STATUS)initial["Status"];
                DateTime creDte = (DateTime)initial["CreatedDate"];

                DateTime? inactiveDate = null;
                if (stat != STATUS.ACTIVE && stat != STATUS.WATCH)
                {
                    inactiveDate = (DateTime)initial["InactiveDate"];
                }

                

                Player newPlayer = new Player(idToLoad, name, stat, creDte, inactiveDate);

                return Task.FromResult(newPlayer);

            }
            throw new Exception("Player Not Found");
        }


        public override Task<bool> SaveBasePlayer(Player currentPlayer, bool infoOnly)
        {
            SqlCommand cmd = new SqlCommand("SavePlayer");
            cmd.CommandType = CommandType.StoredProcedure;
            
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = currentPlayer.Id;
            cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 200).Value = currentPlayer.Name;
            cmd.Parameters.Add("@Status", SqlDbType.Int).Value = (int)currentPlayer.PlayerStatus;
            cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = currentPlayer.DateRegistered;


            try
            {
                sqlClient.ExecuteCommand(cmd);

                if(infoOnly == false)
                {
                    // Allowing save Player worked then also save Rating data
                    // this allows ratings data to be only saved with a new player,
                    // in all other cases it needs specific handling to prevent accidental
                    // duplication
                    SaveLiveRating(currentPlayer);
                }
                
            }
            catch (Exception exc)
            {
                throw new Exception("SavePlayer failed", exc);
            }

            return Task.FromResult(true);
        }

       private void SaveLiveRating(Player currentPlayer)
        {
            SqlCommand cmd = new SqlCommand("SaveRating");
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = currentPlayer.Id;
            cmd.Parameters.Add("@Rating", SqlDbType.Decimal).Value = currentPlayer.LiveRating;
            cmd.Parameters.Add("@Deviation", SqlDbType.Decimal).Value = (int)currentPlayer.LiveDeviation;
            cmd.Parameters.Add("@Volatility", SqlDbType.Decimal).Value = currentPlayer.LiveVolatility;


            try
            {
                sqlClient.ExecuteCommand(cmd);
            }
            catch (Exception exc)
            {
                throw new Exception("SaveRating failed", exc);
            }

        }

        public override int GetNewPlayerId()
        {
            string sql = "SELECT ISNULL(MAX(Id),0) FROM Player";

            DataTable response = sqlClient.GetData(sql);


            if (response.Rows.Count > 0)
            {
                int currentHighestId = Convert.ToInt32(response.Rows[0][0].ToString());

                currentHighestId += 1;

                return currentHighestId;
            }
            else
            {
                return 1;
            }
        }

        public override Task<DataTable> GetLiveRating(int playerId)
        {
            string sql = $"SELECT * FROM LiveRating WHERE PlayerId =  '{playerId}'";

            DataTable response = sqlClient.GetData(sql);

            return Task.FromResult(response);
        }

        public override Task<DataTable> GetHistoricalRatings(int playerId)
        {
            string sql = $"SELECT * FROM LiveRating WHERE PlayerId =  '{playerId}' ORDER BY DateChanged DESC ";

            DataTable response = sqlClient.GetData(sql);

            return Task.FromResult(response);
        }
    }
}
