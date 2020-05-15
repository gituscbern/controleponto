using ControlePonto.Core.Contracts;
using ControlePonto.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Core.Entities
{
    public class Ponto : EntityBase
    {
        public DateTime Data { get; set; }
        public TimeSpan Entrada { get; set; }
        public TimeSpan EntradaIntervalo { get; set; }
        public TimeSpan SaidaIntervalo { get; set; }
        public TimeSpan Saida { get; set; }
        public TimeSpan HoraSaidaPrevista { get; set; }
        public TimeSpan HorasTrabalhadas { get; set; }
        public TimeSpan Saldo { get; set; }
        public TimeSpan CargaHoraria { get; set; }
        public TipoSaldo Tipo { get; set; }
        public TipoAbono Abono { get; set; }
        public enum TipoSaldo { Positivo = 1, Negativo = 2 }
        public enum TipoAbono {None = 0, Atestado = 1, Declaracao = 2, Abono = 3, Feriado = 4}

        public bool PontoValido()
        {
            List<ValidationResult> errorsEvent = new List<ValidationResult>();

            if (DataForadoMes()) return true;
            if (EhDomingo()) return true;
            if (FimDeSemana() && ValidarSaldoFimDeSemana()) return true;
            if (NaoTrabalhou()) return true;
            if (EhAbonado()) return true;

            errorsEvent.Add(ValidarEntrada());
            errorsEvent.Add(ValidarEntradaIntervalo());
            errorsEvent.Add(ValidarSaidaIntervalo());
            errorsEvent.Add(ValidarHoraSaidaPrevista());
            errorsEvent.Add(ValidarSaida());
            errorsEvent.Add(ValidarHorasTrabalhadas());
            errorsEvent.Add(ValidarSaldo());
            Events.AddRange(errorsEvent);

            return !errorsEvent.Any(x => !x.Success);
        }
        
        private ValidationResult ValidarEntrada()
        {
            TimeSpan entradaMinima = new TimeSpan(0, 0, 0);
            //TimeSpan entradaMaxima = new TimeSpan(9, 0, 0);
            if (Entrada >= entradaMinima && Entrada <= EntradaIntervalo/*entradaMaxima*/)
            {
                return new ValidationResult() { Message = "Horário de Entrada válido", Success = true };
            }
            else
            {
                return new ValidationResult() { Message = "Horário de Entrada inválido", Success = false };
            }
        }

        private ValidationResult ValidarEntradaIntervalo()
        {
            if (EntradaIntervalo == new TimeSpan(12, 0, 0))
            {
                return new ValidationResult() { Message = "Horário de Entrada intervalo valido", Success = true };
            }
            return new ValidationResult() { Message = "Horário de Entrada intervalo inválido", Success = false };
        }
        private ValidationResult ValidarSaidaIntervalo()
        {
            if (SaidaIntervalo == new TimeSpan(13, 0, 0))
            {
                return new ValidationResult() { Message = "Horário de Saída intervalo valido", Success = true };
            }
            return new ValidationResult() { Message = "Horário de Saída intervalo inválido", Success = false }; 
        }
        private ValidationResult ValidarHoraSaidaPrevista()
        {
            var cargaHorariaSexta = AjustarCargaHorariaSexta();
            TimeSpan horaSaidaPrevistaCorreta = (cargaHorariaSexta - (EntradaIntervalo - Entrada))+SaidaIntervalo;
            if (HoraSaidaPrevista == horaSaidaPrevistaCorreta)
            {
                return new ValidationResult() { Message = "Horário de Saída Prevista valido", Success = true };
            }
                
            return new ValidationResult() { Message = "Horário de Saída Prevista inválido", Success = false }; ; ;
        }
        private ValidationResult ValidarSaida()
        {
            var limiteHorasTrabalhadas = new TimeSpan(10, 0, 0);
            var limiteSaida = new TimeSpan(22, 0, 0);
            var horasTrabalhadasCorreta = (EntradaIntervalo - Entrada) + (Saida - SaidaIntervalo);
            if (Saida > SaidaIntervalo && horasTrabalhadasCorreta <= limiteHorasTrabalhadas && Saida <= limiteSaida /*Saida <= HoraSaidaPrevista + limiteHorasTrabalhadas*/)
            {
                return new ValidationResult() { Message = "Horário de Saída valido", Success = true };
            }
            return new ValidationResult() { Message = "Horário de Saída inválido", Success = false }; ;
        }
        private ValidationResult ValidarHorasTrabalhadas()
        {
            var horasTrabalhadasCorreta = (EntradaIntervalo - Entrada) + (Saida - SaidaIntervalo);
            if (HorasTrabalhadas == horasTrabalhadasCorreta)
            {
                return new ValidationResult() { Message = "Horas trabalhadas valida", Success = true };
            }
            return new ValidationResult() { Message = "Horas trabalhadas inválidas", Success = false };
        }
        private ValidationResult ValidarSaldo()
        {
            var cargaHorariaSexta = AjustarCargaHorariaSexta();
            
            var saldoCorreto = HorasTrabalhadas - cargaHorariaSexta;
            if (Saldo == saldoCorreto)
            {
                return new ValidationResult() { Message = "Saldo valido", Success = true };
            }
            return new ValidationResult() { Message = "Saldo inválido", Success = false };
        }

        /// <summary>
        /// Para meses com 30 dias, a ultima linha da planilha retornará um DateTime default. Não é preciso computar esta data.
        /// </summary>
        /// <returns></returns>
        private bool DataForadoMes()
        {
            if(Data == default(DateTime))
            {
                return true;
            }
            return false;
        }

        private bool FimDeSemana()
        {
            if (Data.DayOfWeek == DayOfWeek.Saturday) return true;
            return false;
        }

        private bool EhDomingo()
        {
            if (Data.DayOfWeek == DayOfWeek.Sunday) return true;
            return false;
        }

        private bool ValidarSaldoFimDeSemana()
        {
            var saldoEsperado = Entrada != TimeSpan.Zero && Saida != TimeSpan.Zero ? (EntradaIntervalo - Entrada) + (Saida - SaidaIntervalo) : TimeSpan.Zero;
            if (Saldo == saldoEsperado) return true;
            return false;
        }

        private bool EhSexta()
        {
            if (Data.DayOfWeek == DayOfWeek.Friday) return true;
            return false;
        }

        private bool NaoTrabalhou()
        {
            if (Entrada == TimeSpan.Zero && Saida == TimeSpan.Zero && Saldo == TimeSpan.Zero && Abono == TipoAbono.None)
            {
                Saldo = new TimeSpan(-9, 0, 0);
                Tipo = TipoSaldo.Negativo;
                return true;
            }

            return false;
        }



        private bool EhAbonado()
        {
            if (Entrada == TimeSpan.Zero && Saida == TimeSpan.Zero && Saldo == TimeSpan.Zero && Abono != TipoAbono.None)
            {
                Saldo = new TimeSpan(0, 0, 0);
                Tipo = TipoSaldo.Positivo;
                return true;
            }

            return false;
        }

        private TimeSpan AjustarCargaHorariaSexta()
        {
            var cargaHorariaSexta = CargaHoraria;
            if (EhSexta() && CargaHoraria == new TimeSpan(9, 0, 0))
            {
                cargaHorariaSexta = new TimeSpan(8, 0, 0);
            }
            return cargaHorariaSexta;
        }

        //private DomainEventBase NaoTrabalhou()
        //{
        //    if (Entrada == TimeSpan.Zero && Saida == TimeSpan.Zero && Saldo == TimeSpan.Zero && Abono == TipoAbono.None)
        //    {
        //        Saldo = new TimeSpan(-9, 0, 0);
        //        Tipo = TipoSaldo.Negativo;
        //        //return true;
        //        return new ValidationResult() { Message = "Colaborador não trabalhou", Success = true };
        //    }

        //    return false;
        //}

        //private bool ValidarEntrada()
        //{
        //    TimeSpan entradaMinima = new TimeSpan(0, 0, 0);
        //    //TimeSpan entradaMaxima = new TimeSpan(9, 0, 0);
        //    if (Entrada >= entradaMinima && Entrada <= EntradaIntervalo/*entradaMaxima*/)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
    }
}
