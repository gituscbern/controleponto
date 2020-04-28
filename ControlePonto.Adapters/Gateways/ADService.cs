using ControlePonto.Core.Contracts;
using ControlePonto.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Adapters.Gateways
{
    public class ADService : IADService
    {
        public Usuario GetByEmail(string email)
        {
            //DATA MOCK
            return new Usuario() {  CargaHoraria = new TimeSpan(8,0,0),
                                    CentroCusto = "USC",
                                    CargaHorariaSexta = new TimeSpan (9,0,0),
                                    Email = "jpsilva@bernhoeft.com.br",
                                    Nome = "José Thiago Pereira da Silva"
            };
        }
    }
}
