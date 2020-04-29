using ControlePonto.Adapters.Gateways;
using ControlePonto.Adapters.Presentation;
using ControlePonto.Core.Contracts;
using ControlePonto.Core.Dto;
using ControlePonto.Core.UseCases;
using System;
using System.IO;
using System.Windows.Forms;

namespace ControlePonto.ConsoleApp
{
    public partial class main : Form
    {
        private string _caminhoPlanilha = @"..\..\Resources\controle_ponto4.xlsx";
        private string _caminhoDestino = @"C:\Ponto";

        private IExcelService _excelService;
        private IPontoRepository _pontoRepository;
        private IPlanilhaRepository _planilhaRepository;
        private IADService _ADService = new ADService();
        private IAuthService _authService = new AuthService();
        
        public main()
        {
            InitializeComponent();
            ConfigurarAplicativo();
            var vm = ConfigurarPlanilha(_caminhoPlanilha);
            if (!vm.Success)
            {
                MessageBox.Show(vm.ResultMessage, "Erro ao inicializar", MessageBoxButtons.OK);
                this.Close();
            }
        }

        /// <summary>
        /// Este método configura o aplicativo. Aqui ele habilita o botão de envio somente
        /// no ultimo dia do mês vigente.
        /// </summary>
        private void ConfigurarAplicativo()
        {
            int diaAtual = DateTime.Now.Day;
            int ultimoDiaDesteMes = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            int diaEnvioPlanilha = 29;//ultimoDiaDesteMes;
            if (diaAtual == diaEnvioPlanilha)
            {
                btnEnviarPonto.Enabled = true;
            }
        }

        private ConfigurarPlanilhaResponseViewModel ConfigurarPlanilha(string caminhoPlanilha)
        {
            ConfigurarPlanilhaResponseViewModel vm;
            try
            {
                _excelService = new ExcelService(caminhoPlanilha);
                IExcelService _excelServiceAD = new ExcelService(@"..\..\Resources\ad.xlsx");

                _ADService = new ADService(_excelServiceAD);
                var email = _authService.GetLoggedUser();
                var configurarPlanilhaRequestUseCase = new ConfigurarPlanilhaInteractor(_excelService, _ADService);
                var ConfigurarPlanilhaRequestMessage = new ConfigurarPlanilhaRequestMessage(email, "Horários");
                var responseMessage = configurarPlanilhaRequestUseCase.Handle(ConfigurarPlanilhaRequestMessage);
                var configurarPlanilhaResponsePresenter = new ConfigurarPlanilhaResponsePresenter();
                vm = configurarPlanilhaResponsePresenter.Handle(responseMessage);
            }
            catch(IOException ex)
            {
                vm = new ConfigurarPlanilhaResponseViewModel(false, "Feche a planilha de controle de ponto antes de iniciar o programa");
            }
            catch (Exception ex)
            {
                vm = new ConfigurarPlanilhaResponseViewModel(false, ex.Message);
            }

            return vm;
        } 

        private void BtnAddPonto_Click(object sender, EventArgs e)
        {
            if (ExibirMensagemInicial())
            {
                var responseViewModel = AbrirPlanilha();
                if (!responseViewModel.Success)
                {
                    MessageBox.Show(responseViewModel.ResultMessage, "Erro ao abrir planilha");
                }
            }
        }


        private void BtnEnviarPonto_Click(object sender, EventArgs e)
        {
            EnviarPontoResponseViewModel vm;
            try
            {
                _excelService = new ExcelService(_caminhoPlanilha);
                _pontoRepository = new PontoRepository(_excelService);
                _planilhaRepository = new PlanilhaRepository(_caminhoDestino, _caminhoPlanilha);
                var email = _authService.GetLoggedUser();
                var enviarPontoRequestUseCase = new EnviarPontoInteractor(_pontoRepository, _excelService, _planilhaRepository, _ADService);
                var enviarPontoRequestMessage = new EnviarPontoRequestMessage("Horários", email);
                var enviarPontoResponseMessage = enviarPontoRequestUseCase.Handle(enviarPontoRequestMessage);
                var enviarPontoResponsePresenter = new EnviarPontoResponsePresenter();
                vm = enviarPontoResponsePresenter.Handle(enviarPontoResponseMessage);

                if (!vm.Success)
                {
                    MessageBox.Show("Sua planilha contém erros de validação, corrija-as antes do envio", "Erros na planilha", MessageBoxButtons.OK);
                    AbrirPlanilha();
                }
                else
                {
                    MessageBox.Show("Planilha Enviada com sucesso!", "Sucesso!", MessageBoxButtons.OK);
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("Salve e feche a planilha de controle de pontos antes de enviá-la!","Erro ao enviar", MessageBoxButtons.OK);
            }
        }

        private AdicionarPontoResponseViewModel AbrirPlanilha()
        {
            try
            {
                System.Diagnostics.Process.Start(_caminhoPlanilha);
                return new AdicionarPontoResponseViewModel(true, "Arquivo aberto com sucesso");
            }
            catch (Exception ex)
            {
                return new AdicionarPontoResponseViewModel(false, ex.Message);
            }
        }

        private bool ExibirMensagemInicial()
        {
            string mensagem = "• Salve as alterações (Ctrl + B) realizadas na planilha antes de fechá-la.";
            string titulo = "Aviso";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            DialogResult resultado;

            resultado = MessageBox.Show(mensagem, titulo, buttons);
            if (resultado == DialogResult.OK)
            {
                return true;
            }

            return false;
        }
    }
}
