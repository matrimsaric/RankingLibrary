using RankingLibrary.DataAccess.MSSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RankingLibrary.DataAccess
{
    public enum ACCESS_CHOICE
    {
        MS_SQL = 0,
        DATA_IO = 1
    }
    public class DataAccessProvider
    {
        public DataAccessParent GetLiveDataAccess(ACCESS_CHOICE userChoice = ACCESS_CHOICE.MS_SQL)
        {
            switch (userChoice)
            {
                case ACCESS_CHOICE.MS_SQL:
                    return new MsSqlDataAccess();
                    //case ACCESS_CHOICE.DATA_IO:
                    //    return new DataIoAccess();
            }
            return new MsSqlDataAccess();// default to MS sql

        }
    }
}
