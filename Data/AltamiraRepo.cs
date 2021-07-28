using Altamira.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altamira.Data
{
    public class AltamiraRepo : IAltamiraRepo
    {
        private readonly AltamiraContext _ctx;

        public AltamiraRepo(AltamiraContext ctx)
        {
            _ctx = ctx;
        }

        public void AddUser(User user)
        {
            _ctx.Users.Add(user);

        }

        public void DeleteUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            _ctx.Users.Remove(user);
        }

        public User GetUserById(int id)
        {
            var temp = _ctx.Users.Where(c => c.Id == id)
                .Include(c => c.Company)
                .Include(u => u.Address)
                .ThenInclude(a => a.Coordinate)
                .FirstOrDefault();
            return temp;
        }

        public IEnumerable<User> GetUsers()
        {
            return _ctx.Users.Include(c => c.Company)
                .Include(u => u.Address)
                .ThenInclude(a => a.Coordinate)
                .ToList();
        }

        public bool SaveChanges()
        {
            return (_ctx.SaveChanges() > 0);
        }



        // TODO DTOs for POST actions probably
        // TODO validation of inputs and database properties(adding max length required etc.)
    }
}
