using ControlePonto.Core.Contracts;
using ControlePonto.Core.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        public PontoRepository()
        {

        }

        public List<Ponto> GetAll()
        {
            List<Ponto> pontos = new List<Ponto>();
            Ponto ponto = new Ponto();
            int initRow = 10;
            int endRow = 40;

            for (int row = initRow; row <= endRow; row++)
            {
                Ponto.TipoSaldo tipo = HandleTipo(row, 10);
                TimeSpan saldo = HandleSaldo(row, 9);
                
                try
                {
                    ponto = new Ponto()
                    {
                        //Data = _excelService.Sheet("Horários").Get<TimeSpan>(row,2),
                        Data = HandleData(_excelService.Sheet("Horários").Get<object>(row, 2)),
                        Entrada = _excelService.Sheet("Horários").GetFromHours(row, 3),
                        EntradaIntervalo = _excelService.Sheet("Horários").GetFromHours(row, 4),
                        SaidaIntervalo = _excelService.Sheet("Horários").GetFromHours(row, 5),
                        Saida = _excelService.Sheet("Horários").GetFromHours(row, 6),
                        HoraSaidaPrevista = _excelService.Sheet("Horários").GetFromHours(row, 7),
                        HorasTrabalhadas = _excelService.Sheet("Horários").GetFromHours(row, 8),
                        Saldo = saldo,
                        Id = row,
                        CargaHoraria = _excelService.Sheet("Horários").Get<DateTime>(5, 3).TimeOfDay,
                        Tipo = tipo,
                        Abono = HandleAbono(row, 11)
                    };
                }
                catch (Exception ex)
                {
                    throw;
                }
                pontos.Add(ponto);
            }
            return pontos;
        }

        private Ponto.TipoAbono HandleAbono(int row, int column)
        {
            string tipoAbono = _excelService.Sheet("Horários").Get<string>(row, column);
            if (!string.IsNullOrEmpty(tipoAbono))
            {
                return (Ponto.TipoAbono) Enum.Parse(typeof(Ponto.TipoAbono), RemoveDiacritics(tipoAbono));
            }
            {
                return Ponto.TipoAbono.None;
            }
        }

        private Ponto.TipoSaldo HandleTipo(int row, int column)
        {
            string tipo = _excelService.Sheet("Horários").Get<string>(row, column);
            if (!string.IsNullOrEmpty(tipo))
            {
                return (Ponto.TipoSaldo) Enum.Parse(typeof(Ponto.TipoSaldo), tipo);
            }
            else
            {
                return Ponto.TipoSaldo.Positivo;
            }
        }
        private DateTime HandleData(object cellObject)
        {

            if (cellObject.GetType().Name == "Double")
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
        private TimeSpan HandleSaldo(int row, int column)
        {
            TimeSpan saldo = _excelService.Sheet("Horários").GetFromHours(row, column);
            if (HandleTipo(row, 10) == Ponto.TipoSaldo.Negativo)
            {
                saldo = saldo.Negate();
            }
            return saldo;
        }

        private string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public void Save(Ponto ponto)
        {
            throw new NotImplementedException();
        }
        public void SetSource(IExcelService excelService)
        {
            _excelService = excelService;
        }
        private TimeSpan CellValueToTimeSpan(object toConvert)
        {
            if(toConvert.ToString() == "")
            {
                return TimeSpan.Zero;
            }
            return (TimeSpan)toConvert;
        }

        
    }
}
