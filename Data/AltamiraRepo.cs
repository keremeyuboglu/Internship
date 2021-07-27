using Altamira.Data.Entities;
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

        }

        // TODO many repo actions
    }
}
