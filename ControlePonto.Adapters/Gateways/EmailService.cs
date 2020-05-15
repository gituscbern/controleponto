using ControlePonto.Core.Contracts;
using ControlePonto.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Adapters.Gateways
{
    public class EmailService : IEmailService
    {
        public void GetEmail()
        {
            throw new NotImplementedException();
        }

        public void SendEmail(Planilha planilha)
        {
            
        }

        public void SendEmail(string assunto, string corpo, List<string> emails, Planilha planilha, string imgPath)
        {
            throw new NotImplementedException();
        }
    }
}
