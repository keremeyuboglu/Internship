using Altamira.Data;
using Altamira.Data.DTOs.Update;
using Altamira.Data.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altamira.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IAltamiraRepo _repo;
        private readonly IDistributedCache _cache;
        private readonly ILogger<CompanyController> _logger;

        public CompanyController(IMapper mapper, IAltamiraRepo repo, IDistributedCache cache, ILogger<CompanyController> logger)
        {
            _mapper = mapper;
            _repo = repo;
            _cache = cache;
            _logger = logger;
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> UpdateCompanyOfUser(int id, [FromBody] CompanyUpdateDTO companyDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string json;
                    User user;
                    Company company;
                    var userFromCache = await _cache.GetAsync(id.ToString());
                    if (userFromCache != null)
                    {
                        await _cache.RemoveAsync(typeof(Company).Name + id.ToString());
                    }
                    user = await Task.Run(() => _repo.GetUserById(id));
                    if (user == null)
                    {
                        return BadRequest("That user doesn't exist.");
                    }
                    company = user.Company;
                    _mapper.Map(companyDTO, company);

                    _repo.SaveChanges();

                    json = JsonConvert.SerializeObject(company);
                    userFromCache = Encoding.UTF8.GetBytes(json);
                    var options = new DistributedCacheEntryOptions()
                            .SetAbsoluteExpiration(TimeSpan.FromSeconds(600));
                    await _cache.SetAsync(typeof(Company).Name + id.ToString(), userFromCache, options);
                    _logger.LogInformation($"{userFromCache}"); // Elastic search logger trying out 
                    return Ok();
                }
                return BadRequest("Invalid model.");
            }

            catch (System.Exception ex)
            {
                _logger.LogError($"Failed to grant request: {ex}");
            }
            return BadRequest("Failed to grant request");
        }


        [SwaggerOperation(Summary = "This endpoint ic created to demonstrate Elasticsearch implementation")]
        [HttpGet]
        public ActionResult GetCompanyById(int id)
        {
            var compnay = _repo.GetCompanyById(id);
            _logger.LogInformation($"The company with ID {compnay.Id} has been requested");
            return Ok(compnay);
        }
    }
}
