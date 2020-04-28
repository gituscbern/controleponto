using ControlePonto.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Core.Entities
{
    public class Usuario : EntityBase
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string CentroCusto { get; set; }
        public TimeSpan CargaHoraria { get; set; }
        public TimeSpan CargaHorariaSexta { get; set; }
    }
}
