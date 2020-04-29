using ControlePonto.Core.Entities;
using System.Collections.Generic;

namespace ControlePonto.Core.Contracts
{
    public interface IPlanilhaRepository
    {
        Planilha GetMe();
        void Save(Planilha planilha);
        bool IsSaved(Planilha planilha);
        List<Planilha> GetAll();
    }
}
