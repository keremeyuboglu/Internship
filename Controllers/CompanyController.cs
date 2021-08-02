using Altamira.Data;
using Altamira.Data.DTOs.Update;
using Altamira.Data.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
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

        public CompanyController(IMapper mapper, IAltamiraRepo repo, IDistributedCache cache)
        {
            _mapper = mapper;
            _repo = repo;
            _cache = cache;
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> UpdateCompanyOfUser(int id, [FromBody] CompanyUpdateDTO companyDTO)
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

                return Ok();
            }
            return BadRequest("Invalid model.");
        }
    }
}
