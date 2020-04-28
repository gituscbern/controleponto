using ControlePonto.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Core.Entities
{
    public class Planilha : EntityBase
    {
        public string NomeArquivo { get; set; }
        public string LocalDestino { get; set; }
        public string CaminhoFonte { get; set; }


        public Planilha(string nome, string dominio, string mes, string centroCusto)
        {
            NomeArquivo = nome.Replace(" ", string.Empty) + "_" + dominio + "_" + mes + "_" + centroCusto;
        }
        public static string NomeCentroCusto(Planilha planilha)
        {
            string[] vs = planilha.NomeArquivo.Split('_');
            return vs[3];
        }
    }
}
