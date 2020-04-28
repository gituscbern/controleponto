﻿using ControlePonto.Core.Contracts;
using ControlePonto.Core.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Adapters.Gateways
{
    public class PlanilhaRepository : IPlanilhaRepository
    {
        private string _outputSource;
        private string _inputSource;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source">Este parâmetro espera o direotorio de onde o arquivo será pesquisado</param>
        public PlanilhaRepository(string outputSource, string inputSource)
        {
            _outputSource = outputSource;
            _inputSource = inputSource;
        }
        public Planilha GetMe()
        {
            throw new NotImplementedException();
        }

        public bool IsSaved(Planilha planilha)
        {
            string currentPathLevel = NavigateOnSource(planilha);
            string result = Path.Combine(currentPathLevel, planilha.NomeArquivo);
            if (File.Exists(result)) return true;
            return false;
        }

        public void Save(Planilha planilha)
        {
            string currentPathLevel = GetFinalLevel(planilha);
            string destFileName = Path.Combine(currentPathLevel, planilha.NomeArquivo + ".xlsx");
            Directory.CreateDirectory(currentPathLevel);
            File.Copy(_inputSource, destFileName,true);
        }

        /// <summary>
        /// As planilhas são armazenadas em dois níveis: Mês e Centro de Custo.
        /// Este método dá o caminho correto a ser acessado pelos demais métodos desta classe.
        /// </summary>
        /// <param name="planilha"></param>
        /// <returns>Caminho em que planilha está salva ou deve ser guardada.</returns>
        private string NavigateOnSource(Planilha planilha)
        {
            //string mesName = DateTime.Now.ToString("MMMM");
            //string CentroCustoName = Planilha.NomeCentroCusto(planilha);
            //string mesLevelPath = Path.Combine(_outputSource, mesName);
            string centroCustoLevelPath = GetFinalLevel(planilha);//Path.Combine(mesLevelPath, CentroCustoName);
            if (Directory.Exists(centroCustoLevelPath))
            {
                return centroCustoLevelPath;
            }
            else
            {
                return "";
            }
            throw new NotImplementedException();
        }
        /// <summary>
        /// Obtém o ultimo nível de armazenamento da planilha que é: Fonte/Mes/Centro de Custo
        ///
        /// </summary>
        /// <param name="planilha"></param>
        /// <returns>destino em que a planilha deve ser armazenada</returns>
        private string GetFinalLevel(Planilha planilha)
        {
            string mesName = DateTime.Now.ToString("MMMM");
            string CentroCustoName = Planilha.NomeCentroCusto(planilha);
            string mesLevelPath = Path.Combine(_outputSource, mesName);
            string centroCustoLevelPath = Path.Combine(mesLevelPath, CentroCustoName);
            return centroCustoLevelPath;
        }
    }
}