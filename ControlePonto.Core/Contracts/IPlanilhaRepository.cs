using ControlePonto.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Core.Contracts
{
    public interface IPlanilhaRepository
    {
        Planilha GetMe();
        void Save(Planilha planilha);
        bool IsSaved(Planilha planilha);
    }
}
