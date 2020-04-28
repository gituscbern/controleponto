using ControlePonto.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Core.Dto
{
    public class ConfigurarPlanilhaRequestMessage : IRequest<ConfigurarPlanilhaResponseMessage>
    {
        //public string NomeUsuario { get; private set; }
        public string Email { get; set; }
        public string SheetName { get; set; }
        //public DateTime Mes { get; set; }
        //public string CentroCusto { get; set; }
        //public int CargaHoraria { get; set; }
        //public int CargaHorariaSexta { get; set; }

        public ConfigurarPlanilhaRequestMessage(/*string nomeUsuario,*/ string email, string sheetName/*, DateTime mes, int cargaHoraria, int cargaHorariaSexta*/)
        {
            //NomeUsuario = nomeUsuario;
            Email = email;
            SheetName = sheetName;
            //Mes = mes;
            //CargaHoraria = cargaHoraria;
            //CargaHorariaSexta = cargaHorariaSexta;
        }
    }
}
