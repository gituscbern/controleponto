using ControlePonto.Core.Contracts;
using ControlePonto.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Adapters.Gateways
{
    public class PontoRepository : IPontoRepository
    {
        private IExcelService _excelService;

        public PontoRepository(IExcelService excelService)
        {
            _excelService = excelService;
        }

        public List<Ponto> GetAll()
        {
            List<Ponto> pontos = new List<Ponto>();

            int initRow = 10;
            int endRow = 39;

            for (int row = initRow; row <= endRow; row++)
            {
                Ponto ponto = new Ponto()
                {
                    //Data = _excelService.Sheet("Horários").Get<TimeSpan>(row,2),
                    Data = TratarData(_excelService.Sheet("Horários").Get<object>(row,2)),
                    Entrada = _excelService.Sheet("Horários").GetFromHours(row, 3),
                    EntradaIntervalo = _excelService.Sheet("Horários").GetFromHours(row, 4),
                    SaidaIntervalo = _excelService.Sheet("Horários").GetFromHours(row, 5),
                    Saida = _excelService.Sheet("Horários").GetFromHours(row, 6),
                    HoraSaidaPrevista = _excelService.Sheet("Horários").GetFromHours(row, 7),
                    HorasTrabalhadas = _excelService.Sheet("Horários").GetFromHours(row, 8),
                    Saldo = _excelService.Sheet("Horários").GetFromHours(row, 9),
                    Id = row,
                    CargaHoraria = _excelService.Sheet("Horários").Get<DateTime>(5, 3).TimeOfDay
                };
                pontos.Add(ponto);
            }
            return pontos;
        }

        public void Save(Ponto ponto)
        {
            throw new NotImplementedException();
        }

        private TimeSpan CellValueToTimeSpan(object toConvert)
        {
            if(toConvert.ToString() == "")
            {
                return TimeSpan.Zero;
            }
            return (TimeSpan)toConvert;
        }

        private DateTime TratarData(object cellObject)
        {
            
            if(cellObject.GetType().Name == "Double")
            {
                DateTime dt = new DateTime(1899, 12, 30);
                TimeSpan ts = TimeSpan.FromDays((Double)cellObject);
                dt = dt + ts;
                return dt;
            }
            else if (cellObject.GetType().Name == "String" && string.IsNullOrEmpty(cellObject.ToString()))
            {
                return new DateTime();
            }
            else
            {
                return (DateTime)cellObject;
            }
        }
    }
}
