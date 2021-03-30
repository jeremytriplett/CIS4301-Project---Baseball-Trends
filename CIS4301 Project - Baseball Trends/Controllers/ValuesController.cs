using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CIS4301_Project___Baseball_Trends.DBAccess;
using CIS4301_Project___Baseball_Trends.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CIS4301_Project___Baseball_Trends.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        public List<PitcherModel> Get()
        {
            System.Diagnostics.Debug.WriteLine("Hello!");
            return DBLayer.GetRecentPitchers();
        }

        // POST api/<controller>
        [HttpPost]
        public void PostTest([FromBody]string value)
        {
        }

       
    }
}
