using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RankingLibrary.DataAccess.MSSQL
{
    internal class SqlDataAccess
    {
        private string connectionString = "Data Source=DESKTOP-PNSFQ01;Initial Catalog=RANKING;Integrated Security=true;TrustServerCertificate=True;";// TODO Need to pass these in somehow

        internal SqlDataAccess()
        {

        }

        internal void ExecuteNonQuery(string strSqlStatement)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(strSqlStatement, conn);
                cmd.ExecuteNonQuery();

                conn.Close();
            }

        }

        internal void ExecuteCommand(SqlCommand incoming)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                incoming.Connection = conn;
                conn.Open();

                incoming.ExecuteNonQuery();
            }
        }

        internal DataSet ExecuteGetCommand(SqlCommand incoming)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                incoming.Connection = conn;
                conn.Open();

                SqlDataAdapter adp = new SqlDataAdapter(incoming);
                DataSet ds = new DataSet();
                adp.Fill(ds);

                return ds;
            }
        }

        internal DataTable GetData(string sql, SqlParameter singleParm = null)
        {
            DataTable response = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {

                SqlCommand oCmd = new SqlCommand(sql, conn);
                if (singleParm != null)
                {
                    oCmd.Parameters.Add(singleParm);
                }
                conn.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter(oCmd))
                {
                    sda.Fill(response);
                }
                conn.Close();

            }
            return response;
        }

        internal DataTable GetDataCom(SqlCommand com)
        {
            DataTable response = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {

                com.Connection = conn;
                conn.Open();
                using (SqlDataReader dr = com.ExecuteReader())
                {
                    response.Load(dr);
                }
                conn.Close();

            }
            return response;
        }
    }
}

