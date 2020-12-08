using IceCat.Assets.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace IceCat.Assets.Classes.Configs
{
    class icecatCredentials : ICredentials
    {
        private string username = "alphaslo";
        private string password = "KJ6j1c9y8c2YwMq8GTjc";

        public string getPassword()
        {
            return password;
        }

        public string getUsername() {
            return username;
        }

        public string getAuthenticationString() {
            return username + ":" + password;
        }
    }
}
