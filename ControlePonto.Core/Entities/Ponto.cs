using ControlePonto.Core.Contracts;
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
        public enum TipoSaldo { Positivo = 1, Negativo = 2 }
        public enum TipoAbono { Atestado = 1, Declaraçã = 2, Abono = 3 }

        public bool PontoValido()
        {
            if (EhDomingo()) return true;
            if (FimDeSemana() && ValidarSaldoFimDeSemana()) return true;
            if (!ValidarEntrada()) return false;
            if (!ValidarEntradaIntervalo()) return false;
            if (!ValidarSaidaIntervalo()) return false;
            if (!ValidarHoraSaidaPrevista()) return false;
            if (!ValidarSaida()) return false;
            if (!ValidarHorasTrabalhadas()) return false;
            if (!ValidarSaldo()) return false;

            return true;
        }

        private bool ValidarEntrada()
        {
            TimeSpan entradaMinima = new TimeSpan(7, 0, 0);
            TimeSpan entradaMaxima = new TimeSpan(9, 0, 0);
            if(Entrada >= entradaMinima && Entrada <= entradaMaxima)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool ValidarEntradaIntervalo()
        {
            if (EntradaIntervalo == new TimeSpan(12, 0, 0)) return true;
            return false;
        }
        private bool ValidarSaidaIntervalo()
        {
            if (SaidaIntervalo == new TimeSpan(13, 0, 0)) return true;
            return false;
        }
        private bool ValidarHoraSaidaPrevista()
        {
            var cargaHorariaSexta = AjustarCargaHorariaSexta();
            TimeSpan horaSaidaPrevistaCorreta = (cargaHorariaSexta - (EntradaIntervalo - Entrada))+SaidaIntervalo;
            if (HoraSaidaPrevista == horaSaidaPrevistaCorreta) return true;
            return false;
        }
        private bool ValidarSaida()
        {
            var limiteHorasTrabalhadas = new TimeSpan(10, 0, 0);
            var horasTrabalhadasCorreta = (EntradaIntervalo - Entrada) + (Saida - SaidaIntervalo);
            if (Saida > SaidaIntervalo && horasTrabalhadasCorreta <= limiteHorasTrabalhadas/*Saida <= HoraSaidaPrevista + limiteHorasTrabalhadas*/) return true;
            return false;
        }
        private bool ValidarHorasTrabalhadas()
        {
            var horasTrabalhadasCorreta = (EntradaIntervalo - Entrada) + (Saida - SaidaIntervalo);
            if (HorasTrabalhadas == horasTrabalhadasCorreta) return true;
            return false;
        }
        private bool ValidarSaldo()
        {
            var cargaHorariaSexta = AjustarCargaHorariaSexta();
            //if(EhSexta() && CargaHoraria == new TimeSpan(9,0,0))
            //{
            //    cargaHorariaSexta = new TimeSpan(8, 0, 0);
            //}
            var saldoCorreto = HorasTrabalhadas - cargaHorariaSexta;
            if (Saldo == saldoCorreto.Duration()) return true;
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
            var saldoEsperado = (EntradaIntervalo - Entrada) + (Saida - SaidaIntervalo);
            if (Saldo == saldoEsperado) return true;
            return false;
        }

        private bool EhSexta()
        {
            if (Data.DayOfWeek == DayOfWeek.Friday) return true;
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
    }
}
