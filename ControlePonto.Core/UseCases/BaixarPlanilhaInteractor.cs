using ControlePonto.Core.Contracts;
using ControlePonto.Core.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Core.UseCases
{
    public class BaixarPlanilhaInteractor : IResponseHandler<BaixarPlanilhaResponseMessage>
    {
        IEmailService _emailService;
        IPlanilhaRepository _planilhaRepository;
        #region TOBEIMPLEMENTEDAFTER
        //IPlanilhaRepository _planilhaRepository; 
        #endregion

        public BaixarPlanilhaInteractor(IEmailService emailService)
        {
            _emailService = emailService;
            #region TOBEIMPLEMENTEDAFTER
            //_planilhaRepository = planilhaRepository; 
            #endregion
        }

        public BaixarPlanilhaInteractor(IEmailService emailService, IPlanilhaRepository planilhaRepository)
        {
            _emailService = emailService;
            _planilhaRepository = planilhaRepository;
        }

        public BaixarPlanilhaResponseMessage Handle()
        {
            List<string> erros = new List<string>();
            try
            {
                _planilhaRepository.DeleteAll();
                _emailService.GetEmail();
                return new BaixarPlanilhaResponseMessage(true, erros, "Planilhas baixadas com sucesso");
            }
            catch (Exception ex)
            {
                erros.Add(ex.Message);
                return new BaixarPlanilhaResponseMessage(false, erros, ex.Message);
            }
            #region TOBEIMPLEMENTEDAFTER
            //foreach (var file in fileStreams)
            //{
            //    _planilhaRepository.Save(file);
            //}
            #endregion  
        }
    }
}
