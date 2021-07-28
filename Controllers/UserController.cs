﻿using Altamira.Data;
using Altamira.Data.Entities;
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

        public UserController(IAltamiraRepo repo) // Connection to the repo should be stored to manipulate data
        {
            _repo = repo;
        }

        // BOILERPLATE CODE 

        // GET: api/<ValuesController>
        [HttpGet]
        public IEnumerable<User> Get()
        {
            IEnumerable<User> users = _repo.GetUsers();
            return users;
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public User Get(int id)
        {
            User user = _repo.GetUserById(id);
            return user;
        }

        // POST 
        // Since the User object has different name from seed json, some json keys should be changed via DTOs probably
        [HttpPost]
        public ActionResult Post([FromBody] User usr)
        {
            _repo.AddUser(usr);
            _repo.SaveChanges();
            
            return Ok();
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
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
