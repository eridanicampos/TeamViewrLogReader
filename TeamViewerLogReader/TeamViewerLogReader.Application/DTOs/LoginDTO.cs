using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamViewerLogReader.Service.DTOs
{
    public class LoginDTO
    {
        public string Login { get; set; }
        public string Password { get; set; }


        public string HashPassword()
        {
            return PasswordHasher.HashPassword(this.Password);
        }
    }


}
