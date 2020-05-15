using ControlePonto.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Core.Dto
{
    public class ExtrairColaboradoresPendentesRequestMessage : IRequest<ExtrairColaboradoresPendentesResponseMessage>
    {
        public string WorkbookName { get; set; }
        public string SheetName { get; set; }
        public int NumeroMes { get; set; }

        public ExtrairColaboradoresPendentesRequestMessage(string workbookName, string sheetName)
        {
            WorkbookName = workbookName;
            SheetName = sheetName;
        }

        public ExtrairColaboradoresPendentesRequestMessage(string workbookName, string sheetName, int numeroMes)
        {
            WorkbookName = workbookName;
            SheetName = sheetName;
            NumeroMes = numeroMes;
        }
    }
}
