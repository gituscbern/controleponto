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
    public class ExtrairCalculoInteractor : IRequestHandler<ExtrairCalculoRequestMessage, ExtrairCalculoResponseMessage>
    {
        IExcelService _excelService;
        IPontoRepository _pontoRepository;
        IPlanilhaRepository _planilhaRepository;
        IADService _aDService;
        
        public ExtrairCalculoInteractor(IExcelService excelService, IPontoRepository pontoRepository, IPlanilhaRepository planilhaRepository, IADService aDService)
        {
            _excelService = excelService;
            _pontoRepository = pontoRepository;
            _planilhaRepository = planilhaRepository;
            _aDService = aDService;
        }

        public ExtrairCalculoResponseMessage Handle(ExtrairCalculoRequestMessage message)
        {
            //List<Planilha> planilhas = _planilhaRepository.GetAll();
            List<Planilha> planilhas = _planilhaRepository.GetByMonth(message.Mes);
            List<Usuario> usuarios = new List<Usuario>();
            List<Relatorio> relatorios = new List<Relatorio>();

            var errors = new List<string>();

            foreach (var planilha in planilhas)
            {
                _excelService.SetSource(planilha.CaminhoFonte);

                string nomeUsuario = _excelService.Sheet("Horários").GetAfter("Nome:");
                Usuario usuario = _aDService.GetByName(nomeUsuario);
                errors = new List<string>();

                _pontoRepository.SetSource(_excelService);
                List<Ponto> pontos = new List<Ponto>();
                try
                {
                    pontos = _pontoRepository.GetAll();

                }
                catch (Exception ex)
                {

                }
                foreach (var ponto in pontos)
                {
                    if (!ponto.PontoValido())
                    {
                        errors.Add($"Há erros de validade com o ponto {ponto.Data}");
                        errors.AddRange(ponto.Events.Where(a => !a.Success).Select(x => x.Message));
                    }
                }
                usuario.RegistrarPonto(pontos);
                Relatorio relatorio = new Relatorio(usuario, planilha, errors);
                relatorios.Add(relatorio);

                //usuarios.Add(usuario);
            }
            _excelService.SetSource(message.CaminhoPlanilhaRelatorio);
            _excelService.Sheet("Banco de horas").CleanSheet(2);
            _excelService.Save();

            int row = 2;
            foreach (var relatorio in relatorios)
            {
                _excelService.Sheet("Banco de horas").Write(relatorio.Usuario.CentroCusto.ToString()).On(row, 1);
                _excelService.Sheet("Banco de horas").Write(relatorio.Usuario.Email.ToString()).On(row, 2);
                _excelService.Sheet("Banco de horas").Write(relatorio.Usuario.Nome.ToString()).On(row, 3);
                _excelService.Sheet("Banco de horas").Write(relatorio.Usuario.Saldo.Duration().ToString()).On(row, 4);

                string evento = relatorio.Usuario.Saldo < TimeSpan.Zero ? "Negativo" : "Positivo";
                _excelService.Sheet("Banco de horas").Write(evento).On(row, 5);

                var diasAbonado = relatorio.Usuario.Pontos.Where(x => x.Abono == Ponto.TipoAbono.Abono || x.Abono == Ponto.TipoAbono.Declaracao).Count();
                _excelService.Write(diasAbonado.ToString()).On(row, 6);

                _excelService.Sheet("Banco de horas").Write(relatorio.Planilha.CaminhoFonte).On(row, 7);

                string erros = string.Join(", ", relatorio.Erros);
                _excelService.Sheet("Banco de horas").Write(erros).On(row, 8);

                _excelService.Save();
                row++;
            }
            #region TODELETEAFTER
            //for (int row = 2; row <= relatorios.Count; row++)
            //{

            //_excelService.Sheet("Banco de horas").Write(relatorios.ElementAt(row - 2).Usuario.CentroCusto.ToString()).On(row, 1);
            //_excelService.Sheet("Banco de horas").Write(relatorios.ElementAt(row - 2).Usuario.Email.ToString()).On(row, 2);
            //_excelService.Sheet("Banco de horas").Write(relatorios.ElementAt(row - 2).Usuario.Nome.ToString()).On(row, 3);
            //_excelService.Sheet("Banco de horas").Write(relatorios.ElementAt(row - 2).Usuario.Saldo.Duration().ToString()).On(row, 4);

            //string evento = relatorios.ElementAt(row - 2).Usuario.Saldo < TimeSpan.Zero ? "Negativo" : "Positivo";
            //_excelService.Sheet("Banco de horas").Write(evento).On(row, 5);

            //var diasAbonado = relatorios.ElementAt(row - 2).Usuario.Pontos.Where(x => x.Abono == Ponto.TipoAbono.Abono || x.Abono == Ponto.TipoAbono.Declaracao).Count();
            //_excelService.Write(diasAbonado.ToString()).On(row, 6);

            //_excelService.Sheet("Banco de horas").Write(relatorios.ElementAt(row - 2).Planilha.CaminhoFonte).On(row, 7);

            //string erros = string.Join(", ", relatorios.ElementAt(row - 2).Erros);
            //_excelService.Sheet("Banco de horas").Write(erros).On(row, 8);

            //_excelService.Save();
            //} 
            #endregion

            return new ExtrairCalculoResponseMessage(!errors.Any(), errors);
        }

        #region TOBEREFACTORED
        //public ExtrairCalculoResponseMessage Handle(ExtrairCalculoRequestMessage message)
        //{
        //    List<Planilha> planilhas = _planilhaRepository.GetAll();
        //    List<Usuario> usuarios = _aDService.GetAll();
        //    List<Relatorio> relatorios = new List<Relatorio>();

        //    var errors = new List<string>();

        //    foreach (var planilha in planilhas)
        //    {
        //        _excelService.SetSource(planilha.CaminhoFonte);

        //        string nomeUsuario = _excelService.Sheet("Horários").GetAfter("Nome:");
        //        Usuario usuario = usuarios.Where(u => u.Nome == nomeUsuario).FirstOrDefault();
        //        errors = new List<string>();

        //        _pontoRepository.SetSource(_excelService);
        //        List<Ponto> pontos = new List<Ponto>();
        //        try
        //        {
        //             pontos = _pontoRepository.GetAll();

        //        }
        //        catch (Exception ex)
        //        {

        //        }
        //        foreach (var ponto in pontos)
        //        {
        //            if (!ponto.PontoValido())
        //            {
        //                errors.Add($"Há erros de validade com o ponto {ponto.Data}");
        //                errors.AddRange(ponto.Events.Where(a => !a.Success).Select(x => x.Message));
        //            }
        //        }
        //        usuario.RegistrarPonto(pontos);
        //        Relatorio relatorio = new Relatorio(usuario, planilha, errors);
        //        relatorios.Add(relatorio);

        //        //usuarios.Add(usuario);
        //    }

        //    #region TOBEREFACTORED
        //    /*  No foreach anterior eu carreguei os pontos para o usuário, no proximo irei add ao registro.
        //        *   Isso é necessário, por enquanto, porque pode haver mais de uma planilha para o mesmo usuario e
        //        *   estes dois foreachs tem por intenção atualizar o objeto Usuario.
        //        */
        //    //foreach (var planilha in planilhas)
        //    //{
        //    //    _excelService.SetSource(planilha.CaminhoFonte);

        //    //    string nomeUsuario = _excelService.Sheet("Horários").GetAfter("Nome:");
        //    //    Usuario usuario = usuarios.Where(u => u.Nome == nomeUsuario).FirstOrDefault();

        //    //    Relatorio relatorio = new Relatorio(usuario, planilha, errors);
        //    //    relatorios.Add(relatorio);
        //    //} 
        //    #endregion

        //    _excelService.SetSource(message.CaminhoPlanilhaRelatorio);
        //    _excelService.Sheet("Banco de horas").CleanSheet(2);
        //    for (int row = 2; row <= relatorios.Count; row++)
        //    {
        //        _excelService.Sheet("Banco de horas").Write(relatorios.ElementAt(row - 2).Usuario.CentroCusto.ToString()).On(row, 1);
        //        _excelService.Sheet("Banco de horas").Write(relatorios.ElementAt(row - 2).Usuario.Email.ToString()).On(row, 2);
        //        _excelService.Sheet("Banco de horas").Write(relatorios.ElementAt(row - 2).Usuario.Nome.ToString()).On(row, 3);
        //        _excelService.Sheet("Banco de horas").Write(relatorios.ElementAt(row - 2).Usuario.Saldo.Duration().ToString()).On(row, 4);

        //        string evento = relatorios.ElementAt(row - 2).Usuario.Saldo < TimeSpan.Zero ? "Negativo" : "Positivo";
        //        _excelService.Sheet("Banco de horas").Write(evento).On(row, 5);

        //        _excelService.Sheet("Banco de horas").Write(relatorios.ElementAt(row - 2).Planilha.CaminhoFonte).On(row, 6);

        //        string erros = string.Join(", ", relatorios.ElementAt(row - 2).Erros);
        //        _excelService.Sheet("Banco de horas").Write(erros).On(row, 7);

        //        _excelService.Save();
        //    }

        //    return new ExtrairCalculoResponseMessage(!errors.Any(), errors);
        //} 
        #endregion
    }
}
