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
    public class EnviarPontoInteractor : IRequestHandler<EnviarPontoRequestMessage, EnviarPontoResponseMessage>
    {
        private readonly IPontoRepository _pontoRepository;
        private IExcelService _excelService;
        private readonly IPlanilhaRepository _planilhaRepository;
        private readonly IADService _aDService;

        public EnviarPontoInteractor(IPontoRepository pontoRepository, IExcelService excelService, IPlanilhaRepository planilhaRepository, IADService aDService)
        {
            _pontoRepository = pontoRepository;
            _excelService = excelService;
            _planilhaRepository = planilhaRepository;
            _aDService = aDService;
        }

        public EnviarPontoResponseMessage Handle(EnviarPontoRequestMessage message)
        {
            Usuario usuario = _aDService.GetByEmail(message.Email);
            List<Ponto> pontos = _pontoRepository.GetAll();
            Planilha planilha = new Planilha(usuario.Nome, usuario.Email, DateTime.Now.ToString("MMMM"), usuario.CentroCusto);
            planilha.CaminhoFonte = @"..\..\Resources\controle_ponto4.xlsx";

            var erros = new List<string>();

            foreach (var ponto in pontos)
            {
                if (!ponto.PontoValido())
                {
                    _excelService.Sheet(message.SheetName).Paint(ponto.Id);
                    erros.Add("O ponto está fora das regras");
                }
                else
                {
                    _excelService.Sheet(message.SheetName).Discolor(ponto.Id);
                }
            }
            _excelService.Save();

            if (!erros.Any())
            {
                _planilhaRepository.Save(planilha);
            }


            return new EnviarPontoResponseMessage(!erros.Any(), erros);
        }
    }
}
