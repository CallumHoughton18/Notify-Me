using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace notifyme.shared.Models
{
    public class User
    {
        public User(string userName)
        {
            UserName = userName;
        }
        public string UserName { get; private set; }
    }
}
