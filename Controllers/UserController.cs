using Altamira.Data;
using Altamira.Data.DTOs;
using Altamira.Data.DTOs.Post;
using Altamira.Data.DTOs.Update;
using Altamira.Data.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
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
    public class UserController : Controller
    {
        private readonly IAltamiraRepo _repo;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;
        private readonly ILogger<UserController> _logger;

        public UserController(IAltamiraRepo repo, IMapper mapper, IDistributedCache cache, ILogger<UserController> logger) // Connection to the repo should be stored to manipulate data
        {
            _repo = repo;
            _mapper = mapper;
            _cache = cache;
            _logger = logger;
        }

        [Authorize]
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAllUsers()
        {
            try
            {
                IEnumerable<User> users = _repo.GetUsers();
                if (!users.Any())
                {
                    return NotFound();
                }
                return Ok(users);
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Failed to grant request: {ex}");
            }
            return BadRequest("Failed to grant request");
        }

   //     [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetUser(int id)
        {
            try
            {
                string json;
                User user;
                var userFromCache = await _cache.GetAsync((typeof(User).Name + id.ToString()));
                if (userFromCache != null)
                {
                    json = Encoding.UTF8.GetString(userFromCache);
                    user = JsonConvert.DeserializeObject<User>(json);
                }
                else
                {
                    user = await Task.Run(() => _repo.GetUserById(id));
                    if (user == null)
                    {
                        return NotFound();
                    }
                    json = JsonConvert.SerializeObject(user);
                    userFromCache = Encoding.UTF8.GetBytes(json);
                    var options = new DistributedCacheEntryOptions()
                            .SetAbsoluteExpiration(TimeSpan.FromSeconds(600));
                    await _cache.SetAsync((typeof(User).Name + id.ToString()), userFromCache, options);
                }
                return Ok(user);
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Failed to grant request: {ex}");
            }
            return BadRequest("Failed to grant request");
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddUserAsync([FromBody] UserPostDTO userDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User usr = _mapper.Map<UserPostDTO, User>(userDTO);
                    var options = new DistributedCacheEntryOptions()
                            .SetAbsoluteExpiration(TimeSpan.FromSeconds(600));
                    _repo.AddUser(usr);
                    _repo.SaveChanges();
                    string json = JsonConvert.SerializeObject(usr);
                    var userFromCache = Encoding.UTF8.GetBytes(json);
                    var id = usr.Id;
                    await _cache.SetAsync((typeof(User).Name + id.ToString()), userFromCache, options);
                    return Ok();
                }
                return BadRequest("Invalid Model");
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Failed to grant request: {ex}");
            }
            return BadRequest("Failed to grant request");
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUser(int id, [FromBody] UserUpdateDTO userDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string json;
                    User user;
                    var userFromCache = await _cache.GetAsync((typeof(User).Name + id.ToString()));
                    if (userFromCache != null)
                    {
                        await _cache.RemoveAsync(typeof(User).Name + id.ToString());
                    }
                    user = await Task.Run(() => _repo.GetUserById(id));
                    if (user == null)
                    {
                        return BadRequest("That user doesn't exist.");
                    }
                    _mapper.Map(userDTO, user);
                    _repo.SaveChanges();

                    json = JsonConvert.SerializeObject(user);
                    userFromCache = Encoding.UTF8.GetBytes(json);
                    var options = new DistributedCacheEntryOptions()
                            .SetAbsoluteExpiration(TimeSpan.FromSeconds(600));
                    await _cache.SetAsync((typeof(User).Name + id.ToString()), userFromCache, options);

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

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            try
            {
                string json;
                User user;
                // DELETE IF EXIST IN CACHE
                var userFromCache = await _cache.GetAsync((typeof(User).Name + id.ToString()));
                if (userFromCache != null)
                {
                    json = Encoding.UTF8.GetString(userFromCache);
                    user = JsonConvert.DeserializeObject<User>(json);
                    await _cache.RemoveAsync((typeof(User).Name + id.ToString()));
                }
                // DELETE FROM DATABASE
                user = await Task.Run(() => _repo.GetUserById(id));
                if (user == null)
                {
                    return NotFound();
                }
                _repo.DeleteUser(user);
                _repo.SaveChanges();
                return Ok();
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Failed to grant request: {ex}");
            }
            return BadRequest("Failed to grant request");
        }
    }
}
