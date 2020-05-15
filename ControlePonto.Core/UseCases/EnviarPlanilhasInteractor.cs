using ControlePonto.Core.Contracts;
using ControlePonto.Core.Dto;
using ControlePonto.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

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
                        DateTime data = new DateTime(DateTime.Now.Year, DateTime.Now./*AddMonths(-1).*/Month, 1);   //REMOVER MÊS DE ABRIL
                        _excelService.Sheet(message.SheetName).Write(usuario.Nome).OnRight("Nome:");
                        _excelService.Sheet(message.SheetName).Write(usuario.CentroCusto).OnRight("Área / Centro de Custo:");
                        _excelService.Sheet(message.SheetName).Write(usuario.CargaHoraria.ToString()).OnRight("CH:");
                        _excelService.Sheet(message.SheetName).Write(usuario.CargaHorariaSexta.ToString()).OnRight("CH Sexta:");
                        _excelService.Sheet(message.SheetName).Write(data.ToString("dd/MM/yyyy")).OnRight("Mês:");

                        Planilha planilha = new Planilha(usuario.Nome, usuario.Email, DateTime.Now./*AddMonths(-1).*/ToString("MMMM"), usuario.CentroCusto);    //REMOVER MêS DE ABRIL
                        planilha.CaminhoFonte = @"C:\Ponto\" + planilha.NomeArquivo + ".xlsm";
                        bool saved = _excelService.SaveAs(planilha.CaminhoFonte);

                        _emailService.SendEmail("Planilha Controle de ponto", planilha.NomeArquivo, new List<string>() { usuario.Email }, planilha, @"..\..\Resources\Fique atento_Planilha Ponto Maio.jpg");  //REMOVER MES DE ABRIL
                }
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }
            return new EnviarPlanilhasResponseMessage(!errors.Any(), errors);
        }
    }
}
