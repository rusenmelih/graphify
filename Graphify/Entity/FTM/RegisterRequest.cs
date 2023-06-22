using System;
using MelofyAPI;

namespace Entity.FTM
{
    public class RegisterRequest
    {
        public Component<string>? Email { get; set; }
        public Component<string>? Name { get; set; }
        public Component<string>? Password { get; set; }
        public Component<string>? PasswordRepeat { get; set; }
    }
}
