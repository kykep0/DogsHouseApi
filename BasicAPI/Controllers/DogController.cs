using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BasicAPI.Models;
using BasicAPI.Helpers;

namespace BasicAPI.Controllers
{
    [Route("")]
    [ApiController]
    
    public class DogController : ControllerBase
    {
        private readonly DogRepository _repository;

        public DogController(DogRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("/ping")]
        public ActionResult<string> Ping()
        {
            return "Dogs house service. Version 1.0.1";
        }

        [HttpGet("/dogs")]
        public async Task<ActionResult<List<Dog>>> GetDogs([FromQuery] string attribute, [FromQuery] string order,
            [FromQuery] int? pageNumber, [FromQuery] int? limit)
        {
            if (String.IsNullOrEmpty(attribute) && String.IsNullOrEmpty(order) && !pageNumber.HasValue && !limit.HasValue)
            {
                return await _repository.GetAllDogs();
            }
            else if (pageNumber.HasValue && limit.HasValue && !String.IsNullOrEmpty(attribute))
            {
                if (pageNumber < 1 || limit < 1)
                {
                    return BadRequest("Invalid pagenumber or limit");
                }
                if (String.IsNullOrEmpty(order))
                {
                    order = "asc";
                }
                if (order != "asc" && order != "desc")
                {
                    return BadRequest("Invalid order");
                }

                return await _repository.GetDogs(attribute, order, (int)pageNumber, (int)limit);
            }
            else if (String.IsNullOrEmpty(attribute) && String.IsNullOrEmpty(order) && pageNumber.HasValue && limit.HasValue)
            {
                if (pageNumber < 1 || limit < 1)
                {
                    return BadRequest("Invalid pagenumber or limit");
                }

                return await _repository.GetDogsByPage((int)pageNumber, (int)limit);
            }
            else if (!pageNumber.HasValue && !limit.HasValue && !String.IsNullOrEmpty(attribute))
            {
                if (String.IsNullOrEmpty(order))
                {
                    order = "asc";
                }

                if (order == "asc" || order == "desc")
                {
                    return await _repository.GetDogsSorted(attribute, order);
                }
                else
                {
                    return BadRequest("Invalid order");
                }
            }
            else
            {
                return BadRequest("Invalid parameters");
            }
        }

        [HttpPost("/dog")]
        public async Task<IActionResult> CreateDog([FromBody] Dog dog)
        {
            if (dog == null)
            {
                return BadRequest("Invalid JSON");
            }

            if (dog.TailLength <= 0 || dog.Weight <= 0)
            {
                return BadRequest("Invalid TailLength or Weight");
            }

            if (await _repository.DoesDogExist(dog.Name))
            {
                return Conflict("A dog with the same name already exists");
            }

            await _repository.CreateDog(dog);

            return Ok();
        }
    }
}
