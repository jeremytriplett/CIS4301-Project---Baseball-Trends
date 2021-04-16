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

        public static string connStr = "Data Source = (DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = oracle.cise.ufl.edu)(PORT = 1521))(CONNECT_DATA = (SID = orcl))); User Id = username; Password = password; ";

        public static List<Query1Tuple> GetQuery1(int dateFrom, int dateTo)
        {
            OracleConnection conn = new OracleConnection(connStr);
            conn.Open();

            string sql =
               @"
                SELECT 
                    a.yearid, a.avg_attendance_league, b.attendance_ws_champs
                FROM
                    (SELECT
                        yearid, TRUNC(AVG(attendance), 2) as avg_attendance_league
                    FROM
                        triplett.baseball_teams 
                    GROUP BY
                        yearid) a
                INNER JOIN
                    (SELECT 
                        yearid, attendance as attendance_ws_champs
                    FROM
                        triplett.baseball_teams
                    WHERE
                        wswin = 'Y'
                    ) b       
                ON
                    a.yearid = b.yearid
                WHERE
                    a.yearid BETWEEN :dateFrom AND :dateTo
                ORDER BY 
                    a.yearid asc";

            OracleCommand cmd = new OracleCommand(sql, conn);
            cmd.Parameters.Add(new OracleParameter("dateFrom", dateFrom));
            cmd.Parameters.Add(new OracleParameter("dateTo", dateTo));


            OracleDataReader oraReader = cmd.ExecuteReader();

            List<Query1Tuple> ret = new List<Query1Tuple>();

            while (oraReader.Read())
            {
                Query1Tuple tuple = new Query1Tuple();

                tuple.yearId = Convert.ToString(oraReader["yearid"]);
                tuple.avgAttendanceLeague = Convert.ToInt32(oraReader["avg_attendance_league"]);
                tuple.attendanceWsChamps = Convert.ToInt32(oraReader["attendance_ws_champs"]);

                ret.Add(tuple);
            }

            return ret;


        }

        public static List<Query2Tuple> GetQuery2(int dateFrom, int dateTo)
        {
            OracleConnection conn = new OracleConnection(connStr);
            conn.Open();

            string sql =
               @"
                SELECT
                    a.yearid, TRUNC(((b.player_managers_count/a.total_managers_count)*100),2) AS player_manager_percentage
                FROM 
                    (SELECT 
                        yearid, count(*) as total_managers_count 
                    FROM 
                        triplett.baseball_managers 
                    GROUP BY 
                        baseball_managers.yearid) a
                INNER JOIN 
                    (SELECT
                        yearid, count(*) as player_managers_count 
                    FROM 
                        triplett.baseball_managers 
                    INNER JOIN 
                        (SELECT DISTINCT 
                            baseball_managers.playerid AS player_manager 
                        FROM 
                            triplett.baseball_managers 
                        INNER JOIN 
                            baseball_appearances 
                        ON 
                        baseball_managers.playerid = baseball_appearances.playerid) c 
                    ON 
                        c.player_manager = baseball_managers.playerid 
                    GROUP BY 
                        baseball_managers. yearid) b 
                ON
                    a.yearid = b.yearid
                WHERE
                    a.yearid BETWEEN :dateFrom AND :dateTo
                ORDER BY
                    yearid ASC";

            OracleCommand cmd = new OracleCommand(sql, conn);
            cmd.Parameters.Add(new OracleParameter("dateFrom", dateFrom));
            cmd.Parameters.Add(new OracleParameter("dateTo", dateTo));


            OracleDataReader oraReader = cmd.ExecuteReader();

            List<Query2Tuple> ret = new List<Query2Tuple>();

            while (oraReader.Read())
            {
                Query2Tuple tuple = new Query2Tuple();

                tuple.yearId = Convert.ToString(oraReader["yearid"]);
                tuple.playerManagerPercentage = Convert.ToDouble(oraReader["player_manager_percentage"]);

                ret.Add(tuple);
            }

            return ret;


        }

    }
}
