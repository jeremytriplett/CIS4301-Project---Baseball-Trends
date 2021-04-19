using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CIS4301_Project___Baseball_Trends.DBAccess;
using CIS4301_Project___Baseball_Trends.Models;

namespace CIS4301_Project___Baseball_Trends.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        [HttpGet("query1")]
        public List<Query1Tuple> Query1(int dateFrom, int dateTo, double weight)
        {
            List<Query1Tuple> ret = DBLayer.GetQuery1(dateFrom, dateTo, weight);

            return ret;
        }

        [HttpGet("query2")]
        public List<Query2Tuple> Query2(int dateFrom, int dateTo)
        {
            List<Query2Tuple> ret = DBLayer.GetQuery2(dateFrom, dateTo);

            return ret;
        }

        [HttpGet("query3")]
        public List<Query3Tuple> Query3(int dateFrom, int dateTo, double percentile)
        {
            List<Query3Tuple> ret = DBLayer.GetQuery3(dateFrom, dateTo, percentile);

            return ret;
        }

        [HttpGet("query4")]
        public List<Query4Tuple> Query4(int dateFrom, int dateTo)
        {
            List<Query4Tuple> ret = DBLayer.GetQuery4(dateFrom, dateTo);

            return ret;
        }


        [HttpGet("query5")]
        public List<Query5Tuple> Query5(int dateFrom, int dateTo)
        {
            List<Query5Tuple> ret = DBLayer.GetQuery5(dateFrom, dateTo);

            return ret;
        }

        [HttpGet("tuplescount")]
        public int GetTuplesCount()
        {
            int ret = DBLayer.GetTuplesCount();

            return ret;
        }
    }
}
