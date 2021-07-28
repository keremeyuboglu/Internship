using Altamira.Data.Entities;
using System.Collections.Generic;

namespace Altamira.Data
{
    public interface IAltamiraRepo
    {
        void AddUser(User user);
        public IEnumerable<User> GetUsers();
        public User GetUserById(int id);
        public void DeleteUser(User user);

        bool SaveChanges();
    }
}