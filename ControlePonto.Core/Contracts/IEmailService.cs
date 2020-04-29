using ControlePonto.Core.Entities;

namespace ControlePonto.Core.Contracts
{
    public interface IEmailService
    {
        void SendEmail(Planilha planilha);
    }
}
