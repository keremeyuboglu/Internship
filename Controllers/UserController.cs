using Altamira.Data;
using Altamira.Data.DTOs;
using Altamira.Data.DTOs.Post;
using Altamira.Data.DTOs.Update;
using Altamira.Data.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public UserController(IAltamiraRepo repo, IMapper mapper) // Connection to the repo should be stored to manipulate data
        {
            _repo = repo;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAllUsers()
        {
            IEnumerable<User> users = _repo.GetUsers();
            if (!users.Any())
            {
                return NotFound();
            }
            return Ok(users);
        }

        [Authorize]
        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public ActionResult GetUser(int id)
        {
            User user = _repo.GetUserById(id);
            if(user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [Authorize]
        [HttpPost]
        public ActionResult PostUser([FromBody] UserPostDTO userDTO)
        {
            if (ModelState.IsValid)
            {
                User usr = _mapper.Map<UserPostDTO, User>(userDTO);

                _repo.AddUser(usr);

                _repo.SaveChanges();

                return Ok();
            }
            return BadRequest();
        }

        [Authorize]
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] UserUpdateDTO userDTO)
        {
            if (ModelState.IsValid)
            {
                User usr = _repo.GetUserById(id);
                if (usr == null)
                {
                    return BadRequest();
                }
                _mapper.Map(userDTO, usr);

                _repo.UpdateUser(usr);

                _repo.SaveChanges();

                return Ok();
            }
            return BadRequest();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            User user = _repo.GetUserById(id);
            if(user == null)
            {
                return NotFound();
            }
            _repo.DeleteUser(user);
            _repo.SaveChanges();
            return Ok();
        }
    }
}
