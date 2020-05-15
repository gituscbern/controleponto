using ControlePonto.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Core.Entities
{
    public class Relatorio : EntityBase
    {
        public Usuario Usuario { get; set; }
        public Planilha Planilha { get; set; }
        public List<string> Erros { get; set; }

        public Relatorio()
        {

        }

        public Relatorio(Usuario usuario, Planilha planilha)
        {
            Usuario = usuario;
            Planilha = planilha;
        }

        public Relatorio(Usuario usuario, Planilha planilha, List<string> erros)
        {
            Usuario = usuario;
            Planilha = planilha;
            Erros = erros;
        }
    }
}
