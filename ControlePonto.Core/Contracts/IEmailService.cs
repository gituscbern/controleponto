using ControlePonto.Core.Entities;
using System.Collections.Generic;
using System.IO;

namespace ControlePonto.Core.Contracts
{
    public interface IEmailService
    {
        void SendEmail(string assunto, string corpo, List<string> emails,Planilha planilha, string imgPath);
        void GetEmail();
    }
}
