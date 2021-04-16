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
        public List<Query1Tuple> Query1(int dateFrom, int dateTo)
        {
            List<Query1Tuple> ret = DBLayer.GetQuery1(dateFrom, dateTo);

            return ret;
        }

        [HttpGet("query2")]
        public List<Query2Tuple> Query2(int dateFrom, int dateTo)
        {
            List<Query2Tuple> ret = DBLayer.GetQuery2(dateFrom, dateTo);

            return ret;
        }

        [HttpGet("query3")]
        public List<PitcherModel> Query3(int dateFrom, int dateTo)
        {
            return null;
        }

        [HttpGet("query4")]
        public List<PitcherModel> Query4(int dateFrom, int dateTo)
        {
            return null;
        }

        [HttpGet("query5")]
        public List<PitcherModel> Query5(int dateFrom, int dateTo)
        {
            return null;
        }
       
    }
}
