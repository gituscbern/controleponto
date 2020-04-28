using ControlePonto.Core.Contracts;
using ControlePonto.Core.Dto;
using ControlePonto.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Core.UseCases
{
    public class ConfigurarPlanilhaInteractor : IRequestHandler<ConfigurarPlanilhaRequestMessage, ConfigurarPlanilhaResponseMessage>
    {
        private readonly IExcelService _excelService;
        private readonly IADService _aDService;

        public ConfigurarPlanilhaInteractor(IExcelService excelService, IADService aDService)
        {
            _excelService = excelService;
            _aDService = aDService;
        }
        public ConfigurarPlanilhaResponseMessage Handle(ConfigurarPlanilhaRequestMessage message)
        {
            var errors = new List<string>();

            try
            {
                Usuario usuario = _aDService.GetByEmail(message.Email);

                if (!PlanilhaConfigurada(message.SheetName))
                {
                    _excelService.Sheet(message.SheetName).Write(usuario.Nome).OnRight("Nome:");
                    _excelService.Sheet(message.SheetName).Write(usuario.CentroCusto).OnRight("Área / Centro de Custo:");
                    _excelService.Sheet(message.SheetName).Write(usuario.CargaHoraria.ToString()).OnRight("CH:");
                    _excelService.Sheet(message.SheetName).Write(usuario.CargaHorariaSexta.ToString()).OnRight("CH Sexta:");

                    DateTime data = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    _excelService.Sheet(message.SheetName).Write(data.ToString("dd/MM/yyyy")).OnRight("Mês:");

                    _excelService.Save(); 
                }
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }
            
            return new ConfigurarPlanilhaResponseMessage(!errors.Any(), errors);
        }
        private bool PlanilhaConfigurada(string sheetName)
        {
            if (string.IsNullOrEmpty(_excelService.Sheet(sheetName).GetAfter("Nome:")))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
