using System.Collections.Generic;
using Facemash.API.Business;
using Facemash.API.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Facemash.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatsController : ControllerBase
    {
        private readonly ICatManagement _catManegement;

        public CatsController(ICatManagement catManegement)
        {
            _catManegement = catManegement;
        }

        [HttpGet]
        [Route("GetMatch")]
        public ActionResult<IEnumerable<Match>> GetMatch()
        {
            return Ok(_catManegement.GetMatch());
        }

        [HttpPost]
        [Route("PostMatch")]
        public ActionResult PostMatch(Match match)
        {
            _catManegement.PostMatch(match);
            return Ok();
        }

        [HttpGet]
        [Route("GetResult")]
        public ActionResult<IEnumerable<Cat>> GetResult()
        {
            return Ok(_catManegement.GetResult());
        }
    }
}

