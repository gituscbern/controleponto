using ControlePonto.Core.Contracts;
using ControlePonto.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Adapters.Gateways
{
    public class ADService : IADService
    {
        private string _inputSource;
        private readonly IExcelService _excelService;

        public ADService()
        {

        }

        public ADService(string inputSource)
        {
            _inputSource = inputSource;
        }

        public ADService(IExcelService excelService)
        {
            _excelService = excelService;
        }

        public Usuario GetByEmail(string email)
        {
            int row = _excelService.Sheet("DrmjRIhEQ9Il3sn").GetLineNumberFrom(email);
            return GetUsuario(row);
        }

        public List<Usuario> GetAll()
        {
            List<Usuario> usuarios = new List<Usuario>();
            int rowsAmount  = _excelService.Sheet("DrmjRIhEQ9Il3sn").AmountRowsUsed();
            
            for (int row = 2; row <= rowsAmount; row++)
            {
                Usuario usuario = GetUsuario(row);
                usuarios.Add(usuario);
            }
            return usuarios;
        }

        private Usuario GetUsuario(int linePosition)
        {
            int nomeColumn = 5;
            int emailColumn = 13;
            int centroCustoColumn = 15;
            int cargaHorariaColumn = 9;

            return new Usuario()
            {
                CargaHoraria = _excelService.Get<string>(linePosition, cargaHorariaColumn) == "220 Horas" ? new TimeSpan(9, 0, 0) : new TimeSpan(8, 0, 0),
                CentroCusto = _excelService.Get<string>(linePosition, centroCustoColumn),
                CargaHorariaSexta = new TimeSpan(8, 0, 0),
                Email = _excelService.Get<string>(linePosition, emailColumn),
                Nome = _excelService.Get<string>(linePosition, nomeColumn)
            };
        }
    }
}
