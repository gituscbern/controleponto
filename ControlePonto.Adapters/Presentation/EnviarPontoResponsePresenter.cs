using ControlePonto.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Adapters.Presentation
{
    public class EnviarPontoResponsePresenter
    {
        public EnviarPontoResponseViewModel Handle(EnviarPontoResponseMessage responseMessage)
        {
            if (responseMessage.Success)
            {
                return new EnviarPontoResponseViewModel(true, "Course registration successful!");
            }

            var sb = new StringBuilder();
            sb.AppendLine("Failed to register course(s)");
            foreach (var e in responseMessage.Errors)
            {
                sb.AppendLine(e);
            }

            return new EnviarPontoResponseViewModel(false, sb.ToString());
        }
    }
}
