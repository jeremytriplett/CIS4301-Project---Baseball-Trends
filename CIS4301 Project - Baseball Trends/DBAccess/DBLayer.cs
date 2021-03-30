using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using CIS4301_Project___Baseball_Trends.Models;

namespace CIS4301_Project___Baseball_Trends.DBAccess
{
    public class DBLayer
    {
        public static List<PitcherModel> GetRecentPitchers()
        {
            string connStr = "Data Source = (DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = localhost)(PORT = 1521))(CONNECT_DATA = (SERVICE_NAME = orcl.home))); User Id = system; Password = password; ";

            OracleConnection conn = new OracleConnection(connStr);
            conn.Open();

            string sql =
                "SELECT " +
                "   playerid," +
                "   yearid," +
                "   stint," +
                "   teamid," +
                "   w," +
                "   l," +
                "   ipouts," +
                "   era " +
                "FROM" +
                "   baseball_pitching " +
                "WHERE" +
                "   yearid = :mostRecentYear";

            OracleCommand cmd = new OracleCommand(sql, conn);
            cmd.Parameters.Add(new OracleParameter("mostRecentYear", 2020));

            OracleDataReader oraReader = cmd.ExecuteReader();

            List<PitcherModel> ret = new List<PitcherModel>();

            while (oraReader.Read())
            {
                PitcherModel pitcher = new PitcherModel();

                pitcher.playerId = Convert.ToString(oraReader["playerid"]);
                pitcher.yearId = Convert.ToString(oraReader["yearId"]);
                pitcher.stint = Convert.ToInt32(oraReader["stint"]);
                pitcher.teamId = Convert.ToString(oraReader["teamId"]);
                pitcher.wins = Convert.ToInt32(oraReader["w"]);
                pitcher.losses = Convert.ToInt32(oraReader["l"]);
                pitcher.ipOuts = Convert.ToInt32(oraReader["ipOuts"]);
                pitcher.era = Convert.ToDouble(oraReader["era"]);

                ret.Add(pitcher);
            }


            return ret;


        }
    }
}
