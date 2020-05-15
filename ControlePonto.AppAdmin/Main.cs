using ControlePonto.Adapters.Gateways;
using ControlePonto.Adapters.Presentation;
using ControlePonto.Core.Contracts;
using ControlePonto.Core.Dto;
using ControlePonto.Core.UseCases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControlePonto.AppAdmin
{
    public partial class Main : Form
    {
        IADService _aDService;
        IExcelService _excelService;
        IPlanilhaRepository _planilhaRepository;
        IPontoRepository _pontoRepository;
        IEmailService _emailService;

        public Main()
        {
            InitializeComponent();
            InitializeServices();
        }

        private void ButtonEnviar_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(sender);
            DisplayLoadForm();
        }

        private void ButtonBaixar_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(sender);
            DisplayLoadForm();
            #region TODELETE
            //string destinoEmails = @"C:\Ponto\Recebidos";
            //_emailService = new EmailEngine(destinoEmails);
            //_planilhaRepository = new PlanilhaRepository(@"C:\Ponto", @"C:\Ponto\Recebidos");
            //var interactor = new BaixarPlanilhaInteractor(_emailService, _planilhaRepository);
            //var response = interactor.Handle();
            //var viewModel = new Presenter().Handle(response);

            //if (viewModel.Success)
            //{
            //    MessageBox.Show("Emails baixados com sucesso!", "Sucesso!", MessageBoxButtons.OK);
            //} 
            #endregion
        }

        private void ButtonCalculate_Click(object sender, EventArgs e)
        {
            string caminhoRelatorioHoras = @"..\..\Resources\relatorio de horas.xlsx";
            string nomeRelatorio = "Relatorio de horas.xlsx";
            int mesSelecionado = DisplayMonthSelector();
            Dictionary<int, Button> dic = new Dictionary<int, Button>();
            dic.Add(mesSelecionado, sender as Button);

            backgroundWorker2.RunWorkerAsync(dic);

            DisplayLoadForm();
            CopyFile(caminhoRelatorioHoras, GetDestineFolderPath(nomeRelatorio));

            #region TODELETE
            //string caminhoRelatorioHoras = @"..\..\Resources\relatorio de horas.xlsx";
            //string nomeRelatorio = "Relatorio de horas.xlsx";

            //_excelService = new ExcelService(caminhoRelatorioHoras);
            //_planilhaRepository = new PlanilhaRepository(@"C:\Ponto", @"C:\Ponto\Recebidos");

            //var extrairCalculointeractor = new ExtrairCalculoInteractor(_excelService, _pontoRepository, _planilhaRepository, _aDService);
            //var requestMessage = new ExtrairCalculoRequestMessage(caminhoRelatorioHoras, DisplayMonthSelector());
            //var responseMessage = extrairCalculointeractor.Handle(requestMessage);
            //var vm = new Presenter().Handle(responseMessage);

            //CopyFile(caminhoRelatorioHoras, GetDestineFolderPath(nomeRelatorio));
            #endregion
        }

        private void ButtonExtrairColaboradoresPendentes(object sender, EventArgs e)
        {
            int mesSelecionado = DisplayMonthSelector();
            Dictionary<int, Button> dic = new Dictionary<int, Button>();
            dic.Add(mesSelecionado, sender as Button);

            backgroundWorker2.RunWorkerAsync(dic);

            DisplayLoadForm();
            SaveNewExcelFile(_excelService, GetDestineFolderPath("Relatorio de colaboradores pendentes.xlsx"));
            #region TODELETE
            //_excelService = new ExcelService();
            //_planilhaRepository = new PlanilhaRepository(@"C:\Ponto", @"C:\Ponto\Recebidos");
            //var extrairColaboradoresPendentesInteractor = new ExtrairColaboradoresPendentesInteractor(_excelService, _planilhaRepository, _aDService);
            //var requestMessage = new ExtrairColaboradoresPendentesRequestMessage("Relatorio de colaboradores pendentes", "Colaboradores Pendentes");
            //var responseMessage = extrairColaboradoresPendentesInteractor.Handle(requestMessage);
            //var vm = new Presenter().Handle(responseMessage);

            //SaveNewExcelFile(_excelService, GetDestineFolderPath("Relatorio de colaboradores pendentes.xlsx")); 
            #endregion
        }

        private void EnviarPlanilhas()
        {
            var interactor = new EnviarPlanilhasInteractor(_aDService, _excelService, _planilhaRepository, _emailService);
            string sheetName = "Horários";
            var requestMessage = new EnviarPlanilhasRequestMessage(sheetName);
            var responseMessage = interactor.Handle(requestMessage);
            var viewModel = new Presenter().Handle(responseMessage);

            if (viewModel.Success)
            {
                MessageBox.Show(viewModel.ResultMessage, "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                MessageBox.Show(viewModel.ResultMessage, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BaixarPlanilhas()
        {
            string destinoEmails = @"C:\Ponto\Recebidos";
            _emailService = new EmailEngine(destinoEmails);
            _planilhaRepository = new PlanilhaRepository(@"C:\Ponto", @"C:\Ponto\Recebidos");
            var interactor = new BaixarPlanilhaInteractor(_emailService, _planilhaRepository);
            var response = interactor.Handle();
            var viewModel = new Presenter().Handle(response);
        }

        private void ExtrairRelatorioHoras(int numeroMesSelecionado)
        {
            string caminhoRelatorioHoras = @"..\..\Resources\relatorio de horas.xlsx";

            _excelService = new ExcelService(caminhoRelatorioHoras);
            _planilhaRepository = new PlanilhaRepository(@"C:\Ponto", @"C:\Ponto\Recebidos");

            var extrairCalculointeractor = new ExtrairCalculoInteractor(_excelService, _pontoRepository, _planilhaRepository, _aDService);
            var requestMessage = new ExtrairCalculoRequestMessage(caminhoRelatorioHoras, numeroMesSelecionado);
            var responseMessage = extrairCalculointeractor.Handle(requestMessage);
            var vm = new Presenter().Handle(responseMessage);

            //CopyFile(caminhoRelatorioHoras, GetDestineFolderPath(nomeRelatorio));
        }

        private void ExtrairColaboradoresPendentes(int numeroMesSelecionado)
        {
            _excelService = new ExcelService();
            _planilhaRepository = new PlanilhaRepository(@"C:\Ponto", @"C:\Ponto\Recebidos");
            var extrairColaboradoresPendentesInteractor = new ExtrairColaboradoresPendentesInteractor(_excelService, _planilhaRepository, _aDService);
            var requestMessage = new ExtrairColaboradoresPendentesRequestMessage("Relatorio de colaboradores pendentes", "Colaboradores Pendentes", numeroMesSelecionado);
            var responseMessage = extrairColaboradoresPendentesInteractor.Handle(requestMessage);
            var vm = new Presenter().Handle(responseMessage);

            if (vm.Success)
            {
                MessageBox.Show("Operação realizada com sucesso!", "Sucesso!", MessageBoxButtons.OK);
            }
            else
            {
                MessageBox.Show("Contate o suporte!", "ERRO!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //SaveNewExcelFile(_excelService, GetDestineFolderPath("Relatorio de colaboradores pendentes.xlsx"));
        }

        private void ExtrairColaboradoresPendentes()
        {
            _excelService = new ExcelService();
            _planilhaRepository = new PlanilhaRepository(@"C:\Ponto", @"C:\Ponto\Recebidos");
            var extrairColaboradoresPendentesInteractor = new ExtrairColaboradoresPendentesInteractor(_excelService, _planilhaRepository, _aDService);
            var requestMessage = new ExtrairColaboradoresPendentesRequestMessage("Relatorio de colaboradores pendentes", "Colaboradores Pendentes");
            var responseMessage = extrairColaboradoresPendentesInteractor.Handle(requestMessage);
            var vm = new Presenter().Handle(responseMessage);
            //SaveNewExcelFile(_excelService, GetDestineFolderPath("Relatorio de colaboradores pendentes.xlsx"));
        }

        private string GetDestineFolderPath(string reportName)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "Selecioe o diretório onde você deseja salvar o relatório";
            folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
            folderBrowserDialog.ShowDialog();
            return Path.Combine(folderBrowserDialog.SelectedPath, reportName);
        }

        private void SaveNewExcelFile(IExcelService excelService, string destFileName)
        {
            excelService.SaveAs(destFileName);
        }

        private void CopyFile(string sourceFileName, string destFileName)
        {
            try
            {
                
                File.Copy(sourceFileName, destFileName);
            }
            catch (IOException ex)
            {
                if (ex.Message.Contains("O arquivo") && ex.Message.Contains("já existe."))
                {
                    DialogResult dialog = MessageBox.Show("Já existe um relatório na pasta selecionada. Deseja substituí-lo?", "Arquivo já existe", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dialog == DialogResult.Yes)
                    {
                        File.Copy(sourceFileName, destFileName, true);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar arquivo. {ex.Message}, Contate o suporte", "Erro inesperado", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeServices()
        {
            _excelService = new ExcelService(@"..\..\Resources\controle_ponto.xlsm");
            IExcelService excelADService = new ExcelService(@"..\..\Resources\ad.xlsx");
            _aDService = new ADService(excelADService);
            _planilhaRepository = new PlanilhaRepository(@"C:\Ponto", @"C:\Ponto");
            _emailService = new EmailEngine();
            _pontoRepository = new PontoRepository();
            Directory.CreateDirectory(@"C:\Ponto\Recebidos");
        }

        private int DisplayMonthSelector()
        {
            Dictionary<int, string> months = new Dictionary<int, string>();
            months.Add(0, "Selecione o mês");
            months.Add(1, "Janeiro");
            months.Add(2, "Fevereiro");
            months.Add(3, "Março");
            months.Add(4, "Abril");
            months.Add(5, "Maio");
            months.Add(6, "Junho");
            months.Add(7, "Julho");
            months.Add(8, "Agosto");
            months.Add(9, "Setembro");
            months.Add(10, "Outubro");
            months.Add(11, "Novembro");
            months.Add(12, "Dezembro");

            Form form = new Form();
            form.Text = "Selecione o mÊs:";
            form.MaximizeBox = false;
            form.Size = new Size(200, 150);

            ComboBox comboBoxMonth = new ComboBox();
            comboBoxMonth.Items.AddRange(months.Select(x => x.Value).ToArray<string>());
            comboBoxMonth.Location = new Point((form.Width - comboBoxMonth.Width) / 2, (form.Height - comboBoxMonth.Height) / 5);
            comboBoxMonth.SelectedIndex = 0;

            Button buttonOk = new Button();
            buttonOk.Text = "OK";
            buttonOk.Location = new Point((form.Width - buttonOk.Width) / 2, (form.Height - buttonOk.Height) / 2);
            buttonOk.Click += this.FecharModalSelecionarMes;

            form.AcceptButton = buttonOk;
            form.Controls.Add(buttonOk);
            form.Controls.Add(comboBoxMonth);

            form.ShowDialog();

            return comboBoxMonth.SelectedIndex;
        }

        private void DisplayLoadForm()
        {
            Form form = new Form();
            form.Text = "Aguarde...";
            form.MaximizeBox = false;
            form.Size = new Size(200, 150);
            form.ControlBox = false;
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterParent;

            PictureBox picture = new PictureBox();
            picture.SizeMode = PictureBoxSizeMode.StretchImage;
            Image gifImage = Image.FromFile(@"..\..\Resources\loadAnim(2).gif");
            picture.ClientSize = new Size(form.Size.Width, form.Size.Height);
            picture.Left = (form.Width - picture.Width) / 2;
            picture.Top = (form.Height - picture.Width) / 2;
            picture.Image = gifImage;

            form.Controls.Add(picture);

            form.ShowDialog();
        }

        private void FecharModalSelecionarMes(object sender, EventArgs e)
        {
            var formToClose = Application.OpenForms[""];
            formToClose.Close();
        }

        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Button button = e.Argument as Button;
            if(button.Name == "buttonBaixar") BaixarPlanilhas();
            if (button.Name == "buttonEnviar") EnviarPlanilhas();
        }

        private void BackgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var formToClose = Application.OpenForms[""];
            var label = (Label) formToClose.Controls[0];
            StringBuilder stringBuilder = new StringBuilder();

            int i = 1;
            if (i > 3)
            {
                stringBuilder.Append(".");
            }

            stringBuilder.Append(".");
            label.Text = $"Baixando{stringBuilder} Aguarde um momento!";
        }

        private void BackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var formToClose = Application.OpenForms[""];
            formToClose.Close();
            MessageBox.Show("Emails baixados com sucesso!", "Sucesso!", MessageBoxButtons.OK);
        }

        private void BackgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            Dictionary<int, Button> dic = e.Argument as Dictionary<int, Button>;
            int mesSelecionado = dic.Select(x => x.Key).FirstOrDefault();
            Button button = dic.Select(x => x.Value).FirstOrDefault();

            if (button.Name == "buttonColabPendentes") ExtrairColaboradoresPendentes(mesSelecionado);
            if (button.Name == "buttonCalculate") ExtrairRelatorioHoras(mesSelecionado);
        }
        private void BackgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var formToClose = Application.OpenForms[""];
            formToClose.Close();
        }
    }
}
