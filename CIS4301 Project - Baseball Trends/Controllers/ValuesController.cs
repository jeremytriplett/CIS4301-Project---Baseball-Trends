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
        // GET: api/<controller>
        [HttpGet]
        public List<PitcherModel> Get()
        {
            List<PitcherModel> ret = DBLayer.GetRecentPitchers();

            return ret;

        }

        // POST api/<controller>
        [HttpPost]
        public void PostTest([FromBody]string value)
        {
        }

       
    }
}
