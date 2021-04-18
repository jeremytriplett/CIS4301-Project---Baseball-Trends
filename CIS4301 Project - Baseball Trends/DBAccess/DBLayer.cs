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

        public static string connStr = "Data Source = (DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = oracle.cise.ufl.edu)(PORT = 1521))(CONNECT_DATA = (SID = orcl))); User Id = rporter; Password = goGators; ";

        public static List<Query1Tuple> GetQuery1(int dateFrom, int dateTo, double weight)
        {
            OracleConnection conn = new OracleConnection(connStr);
            conn.Open();

            string sql =
               @"
                SELECT 
                    a.yearid,
                    a.avg_hr_league,
                    b.avg_hr_weight
                FROM
                    (SELECT 
                        yearid, 
                        TRUNC(AVG(hr/g),8) as avg_hr_league
                    FROM
                        triplett.baseball_batting
                    GROUP BY
                        yearid)a
                INNER JOIN
                    (SELECT 
                        triplett.baseball_batting.yearid, 
                        TRUNC(AVG(hr/g),8) as avg_hr_weight
                    FROM
                        triplett.baseball_batting
                    INNER JOIN
                        triplett.baseball_people
                    ON
                        triplett.baseball_batting.playerid = triplett.baseball_people.playerid
                    WHERE
                        triplett.baseball_people.weight > :weight
                    GROUP BY
                        triplett.baseball_batting.yearid)b
                ON
                    a.yearid = b.yearid
                WHERE
                    a.yearid BETWEEN :dateFrom AND :dateTo
                ORDER BY
                    a.yearid ASC
               ";

            OracleCommand cmd = new OracleCommand(sql, conn);
            cmd.Parameters.Add(new OracleParameter("weight", Convert.ToInt32(weight)));
            cmd.Parameters.Add(new OracleParameter("dateFrom", dateFrom));
            cmd.Parameters.Add(new OracleParameter("dateTo", dateTo));

            OracleDataReader oraReader = cmd.ExecuteReader();

            List<Query1Tuple> ret = new List<Query1Tuple>();

            while (oraReader.Read())
            {
                Query1Tuple tuple = new Query1Tuple();

                tuple.yearId = Convert.ToInt32(oraReader["yearid"]);
                tuple.avgHrLeague = Convert.ToDouble(oraReader["avg_hr_league"]);
                tuple.avgHrWeight = Convert.ToDouble(oraReader["avg_hr_weight"]);


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
                    a.yearid,
                    a.avg_avg_attendance_league,
                    b.avg_attendance_ws_champs
                FROM
                    (SELECT
                        yearid,
                        TRUNC(AVG(attendance/ghome), 2) as avg_avg_attendance_league
                    FROM
                        triplett.baseball_teams 
                    GROUP BY
                        yearid) a
                INNER JOIN
                    (SELECT 
                        yearid, 
                        TRUNC(AVG(attendance/ghome), 2) as avg_attendance_ws_champs
                    FROM
                        triplett.baseball_teams
                    WHERE
                        wswin = 'Y'
                    GROUP BY
                        yearid) b       
                ON
                    a.yearid = b.yearid
                WHERE
                    a.yearid BETWEEN :dateFrom AND :dateTo
                ORDER BY 
                    a.yearid asc
";

            OracleCommand cmd = new OracleCommand(sql, conn);
            cmd.Parameters.Add(new OracleParameter("dateFrom", dateFrom));
            cmd.Parameters.Add(new OracleParameter("dateTo", dateTo));


            OracleDataReader oraReader = cmd.ExecuteReader();

            List<Query2Tuple> ret = new List<Query2Tuple>();

            while (oraReader.Read())
            {
                Query2Tuple tuple = new Query2Tuple();

                tuple.yearId = Convert.ToInt32(oraReader["yearid"]);
                tuple.avgAttendanceLeague = Convert.ToInt32(oraReader["avg_avg_attendance_league"]);
                tuple.attendanceWsChamps = Convert.ToInt32(oraReader["avg_attendance_ws_champs"]);

                ret.Add(tuple);
            }

            return ret;


        }

        public static List<Query3Tuple> GetQuery3(int dateFrom, int dateTo, double percentile)
        {
            OracleConnection conn = new OracleConnection(connStr);
            conn.Open();

            string sql =
               @"
                SELECT 
                    a.yearid,
                    a.avg_league_era,
                    c.avg_percentile_era
                FROM
                    (SELECT 
                        triplett.baseball_pitching.yearid,
                        TRUNC(AVG(era),8) AS avg_league_era
                    FROM
                        triplett.baseball_pitching
                    INNER JOIN
                       triplett.baseball_salaries
                    ON
                        triplett.baseball_pitching.playerid = triplett.baseball_salaries.playerid
                        AND triplett.baseball_pitching.yearid = triplett.baseball_salaries.yearid
                        AND triplett.baseball_pitching.teamid = triplett.baseball_salaries.teamid
                        AND triplett.baseball_pitching.lgid = triplett.baseball_salaries.lgid
                    GROUP BY
                        baseball_pitching.yearid)a
                INNER JOIN
                     (SELECT
                        b.yearid,
                        TRUNC(AVG(b.era),8) as avg_percentile_era
                    FROM
                        (SELECT 
                            triplett.baseball_pitching.yearid,
                            triplett.baseball_pitching.era,
                            triplett.baseball_salaries.salary,
                            PERCENT_RANK() OVER (PARTITION BY triplett.baseball_salaries.yearid ORDER BY triplett.baseball_salaries.salary ASC) * 100 as percentile
                        FROM
                            triplett.baseball_pitching
                        INNER JOIN
                            triplett.baseball_salaries
                        ON
                            triplett.baseball_pitching.playerid = triplett.baseball_salaries.playerid
                            AND triplett.baseball_pitching.yearid = triplett.baseball_salaries.yearid
                            AND triplett.baseball_pitching.teamid = triplett.baseball_salaries.teamid
                            AND triplett.baseball_pitching.lgid = triplett.baseball_salaries.lgid)b
                        WHERE
                            b.percentile >= :percentile
                        GROUP BY
                            b.yearid
                        ORDER BY
                            b.yearid)c
                ON
                    a.yearid = c.yearid
                WHERE
                    a.yearid BETWEEN :dateFrom AND :dateTo
                ORDER BY
                    a.yearid ASC
               ";

            OracleCommand cmd = new OracleCommand(sql, conn);
            cmd.Parameters.Add(new OracleParameter("percentile", percentile));
            cmd.Parameters.Add(new OracleParameter("dateFrom", dateFrom));
            cmd.Parameters.Add(new OracleParameter("dateTo", dateTo));


            OracleDataReader oraReader = cmd.ExecuteReader();

            List<Query3Tuple> ret = new List<Query3Tuple>();

            while (oraReader.Read())
            {
                Query3Tuple tuple = new Query3Tuple();

                tuple.yearId = Convert.ToInt32(oraReader["yearid"]);
                tuple.avgLeagueEra = Convert.ToDouble(oraReader["avg_league_era"]);
                tuple.avgPercentileEra = Convert.ToDouble(oraReader["avg_percentile_era"]);


                ret.Add(tuple);
            }

            return ret;

        }

        public static List<Query4Tuple> GetQuery4(int dateFrom, int dateTo)
        {
            OracleConnection conn = new OracleConnection(connStr);
            conn.Open();

            string sql =
               @"
                SELECT
                    a.yearid,
                    TRUNC(((b.player_managers_count/a.total_managers_count)*100),2) AS player_manager_percentage
                FROM 
                    (SELECT 
                        yearid, 
                        count(*) as total_managers_count 
                    FROM 
                        triplett.baseball_managers 
                    GROUP BY 
                        triplett.baseball_managers.yearid) a
                INNER JOIN 
                    (SELECT
                        yearid,
                        count(*) as player_managers_count 
                    FROM 
                        triplett.baseball_managers 
                    INNER JOIN 
                        (SELECT DISTINCT 
                            triplett.baseball_managers.playerid AS player_manager 
                        FROM 
                            triplett.baseball_managers 
                        INNER JOIN 
                            triplett.baseball_appearances 
                        ON 
                        triplett.baseball_managers.playerid = triplett.baseball_appearances.playerid) c 
                    ON 
                        c.player_manager = triplett.baseball_managers.playerid 
                    GROUP BY 
                       triplett.baseball_managers.yearid) b 
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

            List<Query4Tuple> ret = new List<Query4Tuple>();

            while (oraReader.Read())
            {
                Query4Tuple tuple = new Query4Tuple();

                tuple.yearId = Convert.ToInt32(oraReader["yearid"]);
                tuple.playerManagerPercentage = Convert.ToDouble(oraReader["player_manager_percentage"]);

                ret.Add(tuple);
            }

            return ret;

        }

        public static List<Query5Tuple> GetQuery5(int dateFrom, int dateTo)
        {
            OracleConnection conn = new OracleConnection(connStr);
            conn.Open();

            string sql =
               @"
                SELECT
                    a.yearid, 
                    a.avg_al_era, 
                    b.avg_nl_era
                FROM
                    (SELECT
                        yearid, 
                        trunc(AVG(era),8) as avg_al_era
                    FROM
                        triplett.baseball_pitching
                    WHERE
                        lgid = 'AL'
                    GROUP BY
                        yearid)a
                INNER JOIN
                    (SELECT
                        yearid, 
                        trunc(AVG(era),8) as avg_nl_era
                    FROM
                        triplett.baseball_pitching
                    WHERE
                        lgid = 'NL'
                    GROUP BY
                        yearid)b
                ON 
                    a.yearid = b.yearid
                WHERE
                    a.yearid BETWEEN :dateFrom AND :dateTo
                ORDER BY
                    a.yearid ASC               
               ";

            OracleCommand cmd = new OracleCommand(sql, conn);
            cmd.Parameters.Add(new OracleParameter("dateFrom", dateFrom));
            cmd.Parameters.Add(new OracleParameter("dateTo", dateTo));


            OracleDataReader oraReader = cmd.ExecuteReader();

            List<Query5Tuple> ret = new List<Query5Tuple>();

            while (oraReader.Read())
            {
                Query5Tuple tuple = new Query5Tuple();

                tuple.yearId = Convert.ToInt32(oraReader["yearid"]);
                tuple.avgAlEra = Convert.ToDouble(oraReader["avg_al_era"]);
                tuple.avgNlEra = Convert.ToDouble(oraReader["avg_nl_era"]);


                ret.Add(tuple);
            }

            return ret;

        }


        //OLD QUERY 3
        //public static List<Query3Tuple> GetQuery3(int dateFrom, int dateTo)
        //{
        //    OracleConnection conn = new OracleConnection(connStr);
        //    conn.Open();

        //    string sql =
        //       @"
        //        SELECT 
        //            a.yearid,
        //            a.median_pitcher_salary - b.median_batter_salary as difference_in_salary
        //        FROM
        //            (SELECT 
        //                triplett.baseball_salaries.yearid, MEDIAN(triplett.baseball_salaries.salary) as median_pitcher_salary
        //            FROM
        //                triplett.baseball_salaries
        //            INNER JOIN
        //                triplett.baseball_pitching
        //            ON
        //                triplett.baseball_pitching.yearid = triplett.baseball_salaries.yearid
        //                AND triplett.baseball_pitching.playerid = triplett.baseball_salaries.playerid
        //                AND triplett.baseball_pitching.teamid = triplett.baseball_salaries.teamid
        //                AND triplett.baseball_pitching.lgid = triplett.baseball_salaries.lgid
        //            WHERE
        //                triplett.baseball_salaries.lgid = 'AL'
        //            GROUP BY
        //                triplett.baseball_salaries.yearid
        //            ORDER BY
        //                triplett.baseball_salaries.yearid ASC) a
        //        INNER JOIN
        //            (SELECT 
        //                triplett.baseball_salaries.yearid, MEDIAN(triplett.baseball_salaries.salary) as median_batter_salary
        //            FROM
        //                triplett.baseball_batting
        //            INNER JOIN
        //                triplett.baseball_salaries
        //            ON
        //                triplett.baseball_batting.yearid = triplett.baseball_salaries.yearid
        //                AND triplett.baseball_batting.playerid = triplett.baseball_salaries.playerid
        //                AND triplett.baseball_batting.teamid = triplett.baseball_salaries.teamid
        //                AND triplett.baseball_batting.lgid = triplett.baseball_salaries.lgid
        //            WHERE
        //                triplett.baseball_salaries.lgid = 'AL'
        //            GROUP BY
        //                triplett.baseball_salaries.yearid
        //            ORDER BY
        //                triplett.baseball_salaries.yearid ASC) b
        //        ON
        //            a.yearid = b.yearid
        //        WHERE
        //            a.yearid BETWEEN :dateFrom AND :dateTo
        //       ";

        //    OracleCommand cmd = new OracleCommand(sql, conn);
        //    cmd.Parameters.Add(new OracleParameter("dateFrom", dateFrom));
        //    cmd.Parameters.Add(new OracleParameter("dateTo", dateTo));


        //    OracleDataReader oraReader = cmd.ExecuteReader();

        //    List<Query3Tuple> ret = new List<Query3Tuple>();

        //    while (oraReader.Read())
        //    {
        //        Query3Tuple tuple = new Query3Tuple();

        //        tuple.yearId = Convert.ToInt32(oraReader["yearid"]);
        //        tuple.medianDiff = Convert.ToDouble(oraReader["difference_in_salary"]);

        //        ret.Add(tuple);
        //    }

        //    return ret;

        //}


    }
}
