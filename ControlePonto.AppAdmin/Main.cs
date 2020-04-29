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
        IEmailService _emailService;

        public Main()
        {
            InitializeComponent();
            InitializeServices();
        }

        private void ButtonEnviar_Click(object sender, EventArgs e)
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

        private void InitializeServices()
        {
            _excelService = new ExcelService(@"..\..\Resources\controle_ponto.xlsm");
            IExcelService excelADService = new ExcelService(@"..\..\Resources\ad.xlsx");
            _aDService = new ADService(excelADService);
            _planilhaRepository = new PlanilhaRepository(@"C:\Ponto\ToSend", @"C:\Ponto");
            _emailService = new EmailService();
        }
    }
}
