using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditCardProgram
{
    public class ClsMember
    {
        //public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Identity { get; set; }

        public ClsMember(string email, string password, string identity)
        {
            //this.Name = name;
            this.Email = email;
            this.Password = password;
            this.Identity = identity;
        }

    }
}
