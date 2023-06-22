using MelofyAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.FTM
{
    public class LoginRequest
    {
        public Component<string>? Email { get; set; }
        public Component<string>? Password { get; set; }
    }
}
