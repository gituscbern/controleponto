using ControlePonto.Core.Contracts;
using ControlePonto.Core.Dto;
using ControlePonto.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ControlePonto.Core.UseCases
{
    public class EnviarPlanilhasInteractor : IRequestHandler<EnviarPlanilhasRequestMessage, EnviarPlanilhasResponseMessage>
    {
        IADService _aDService;
        IExcelService _excelService;
        IPlanilhaRepository _planilhaRepository;
        IEmailService _emailService;

        public EnviarPlanilhasInteractor(IADService aDService, IExcelService excelService, IPlanilhaRepository planilhaRepository, IEmailService emailservice)
        {
            _aDService = aDService;
            _excelService = excelService;
            _planilhaRepository = planilhaRepository;
            _emailService = emailservice;
        }
        public EnviarPlanilhasResponseMessage Handle(EnviarPlanilhasRequestMessage message)
        {
            List<Usuario> usuarios = _aDService.GetAll();
            
            List<string> errors = new List<string>();
            try
            {
                foreach (var usuario in usuarios)
                {
                    DateTime data = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    _excelService.Sheet(message.SheetName).Write(usuario.Nome).OnRight("Nome:");
                    _excelService.Sheet(message.SheetName).Write(usuario.CentroCusto).OnRight("Área / Centro de Custo:");
                    _excelService.Sheet(message.SheetName).Write(usuario.CargaHoraria.ToString()).OnRight("CH:");
                    _excelService.Sheet(message.SheetName).Write(usuario.CargaHorariaSexta.ToString()).OnRight("CH Sexta:");
                    _excelService.Sheet(message.SheetName).Write(data.ToString("dd/MM/yyyy")).OnRight("Mês:");

                    Planilha planilha = new Planilha(usuario.Nome, usuario.Email, DateTime.Now.ToString("MMMM"), usuario.CentroCusto);
                    //planilha.CaminhoFonte = @"C:\Ponto\" + planilha.NomeArquivo + ".xlsm";
                    planilha.CaminhoFonte = @"..\..\Resources\controle_ponto.xlsm";
                    bool saved = _excelService.Save();
                    //_planilhaRepository.Save(planilha);
                    _emailService.SendEmail(planilha);
                }

                //List<Planilha> planilhas = _planilhaRepository.GetAll();
                //foreach (var planilha in planilhas)
                //{
                //    _emailService.SendEmail(planilha);
                //}
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }
            return new EnviarPlanilhasResponseMessage(!errors.Any(), errors);
        }
    }
}
