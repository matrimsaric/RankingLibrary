using RankingLibrary.Security;
using RankingLibrary.SupportObjects.PlayerObjects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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

                Player newPlayer = new Player(idToLoad, name);

                return Task.FromResult(newPlayer);

            }
            throw new Exception("Player Not Found");
        }

        public override Task<bool> SaveBasePlayer(Player currentPlayer)
        {
            SqlCommand cmd = new SqlCommand("SavePlayer");
            cmd.CommandType = CommandType.StoredProcedure;
            
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = currentPlayer.Id;
            cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 200).Value = currentPlayer.Name;

            try
            {
                sqlClient.ExecuteCommand(cmd);
            }
            catch (Exception exc)
            {
                throw new Exception("SavePlayer failed", exc);
            }

            return Task.FromResult(true);
        }
    }
}
