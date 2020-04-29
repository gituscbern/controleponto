using ControlePonto.Core.Contracts;
using ControlePonto.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Adapters.Gateways
{
    public class AuthService : IAuthService
    {
        public string GetLoggedUser()
        {
            return Environment.UserName+"@bernhoeft.com.br";
        }
    }
}
