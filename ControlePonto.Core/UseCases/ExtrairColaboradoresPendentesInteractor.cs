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
    public class ExtrairColaboradoresPendentesInteractor : IRequestHandler<ExtrairColaboradoresPendentesRequestMessage, ExtrairColaboradoresPendentesResponseMessage>
    {
        IExcelService _excelService;
        IPlanilhaRepository _planilhaRepository;
        IADService _aDService;
        public ExtrairColaboradoresPendentesInteractor(IExcelService excelService, IPlanilhaRepository planilhaRepository, IADService aDService)
        {
            _excelService = excelService;
            _planilhaRepository = planilhaRepository;
            _aDService = aDService;
        }
        public ExtrairColaboradoresPendentesResponseMessage Handle(ExtrairColaboradoresPendentesRequestMessage message)
        {
            List<string> errors = new List<string>();
            try
            {
                //List<Planilha> planilhas = _planilhaRepository.GetAll();
                List<Planilha> planilhas = _planilhaRepository.GetByMonth(message.NumeroMes);
                List<Usuario> usuariosConcluidos = new List<Usuario>();
                foreach (var planilha in planilhas)
                {
                    _excelService.SetSource(planilha.CaminhoFonte);
                    string nomeUsuario = _excelService.Sheet("Horários").GetAfter("Nome:");
                    Usuario usuario = _aDService.GetByName(nomeUsuario);
                    usuariosConcluidos.Add(usuario);
                }
                List<Usuario> todosUsuarios = _aDService.GetAll();
                IEnumerable<Usuario> usuariosPendentes = todosUsuarios.Except(usuariosConcluidos);

                _excelService.Create(message.WorkbookName, message.SheetName);

                int row = 1;
                foreach (var usuarioPendente in usuariosPendentes)
                {
                    _excelService.Write(usuarioPendente.CentroCusto).On(row, 1);
                    _excelService.Write(usuarioPendente.Email).On(row, 2);
                    _excelService.Write(usuarioPendente.Nome).On(row, 3);
                    row++;
                }

                #region TODELETEAFTER
                //for (int row = 1; row <= usuariosPendentes.Count(); row++)
                //{
                //    _excelService.Write(usuariosPendentes.ElementAt(row - 1).CentroCusto).On(row, 1);
                //    _excelService.Write(usuariosPendentes.ElementAt(row - 1).Email).On(row, 2);
                //    _excelService.Write(usuariosPendentes.ElementAt(row - 1).Nome).On(row, 3);
                //} 
                #endregion
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }

            return new ExtrairColaboradoresPendentesResponseMessage(!errors.Any(), errors);
        }
    }
}
