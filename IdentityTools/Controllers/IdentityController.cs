using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IdentityTools.Controllers
{
    [Route("api/[controller]")]
    //[Authorize(Roles = "admin")]

    [ApiController]
    public class IdentityController : ControllerBase
    {
        // GET: api/<IdentityController>
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "admin")]
        public IEnumerable<string> Get()
        {
            string[] resultList = new string[] { "value1", "value2" };
            //User.Claims.ToList().ForEach(m=> m.Type)
            return resultList;
        }

        // GET api/<IdentityController>/5
        [HttpGet("{id}")]
        [Authorize(Roles = "normal,admin")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<IdentityController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<IdentityController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<IdentityController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
