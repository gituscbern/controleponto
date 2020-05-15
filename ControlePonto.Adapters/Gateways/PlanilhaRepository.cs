using ControlePonto.Core.Contracts;
using ControlePonto.Core.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            string destFileName = Path.Combine(currentPathLevel, planilha.NomeArquivo + ".xlsm");
            Directory.CreateDirectory(currentPathLevel);
            File.Copy(planilha.CaminhoFonte/*_inputSource*/, destFileName,true);
        }

        public List<Planilha> GetAll()
        {
            return ProcessDirectory(_inputSource);
        }

        public void DeleteAll()
        {
            DirectoryInfo di = new DirectoryInfo(_inputSource);
            FileInfo[] files = di.GetFiles().Where(x => !x.Name.Contains("~$")).ToArray();
            foreach (FileInfo file in files)
            {
                file.Delete();
            }
        }

        public List<Planilha> GetByMonth(int monthNumber)
        {
            var month = new DateTime(2020,monthNumber,1).ToString("MMMM");

            List<Planilha> planilhas = new List<Planilha>();

            //Process the list of files found in the directory.
            string[] files = Directory.GetFiles(_inputSource).Where(x => !x.Contains("~$")).ToArray();
            foreach (var file in files)
            {
                string fileName = Path.GetFileName(file);
                Planilha planilha = new Planilha() { CaminhoFonte = file, NomeArquivo = fileName };
                try
                {
                    if (monthNumber.ToString() == Planilha.NomeMes(planilha) || month == Planilha.NomeMes(planilha))
                    {
                        planilhas.Add(new Planilha() { CaminhoFonte = file });
                    }
                }
                catch (Exception ex)
                {
                    /*IGNORA TODAS AS PLANILHAS QUE DESOBEDECEM OS FORMATOS:
                    *   nome_login_mes_negocio => MICHELLEGOMESOLIVEIRADOSANJOS_manjos_abril_Contabilidade
                    *   usuarioPC_negocio_mes => micke_Perícia_4
                    */
                }
            }

            return planilhas;
        }

        private List<Planilha> ProcessDirectory(string targetDirectory)
        {
            List<Planilha> planilhas = new List<Planilha>();

            //Process the list of files found in the directory.
            string[] files = Directory.GetFiles(targetDirectory).Where(x => !x.Contains("~$")).ToArray();
            foreach (var file in files)
                planilhas.Add(new Planilha() { CaminhoFonte = file });

            // Recurse into subdirectories of this directory.
            string[] subdirectories = Directory.GetDirectories(targetDirectory);
            foreach (var subdirectory in subdirectories)
                ProcessDirectory(subdirectory);

            return planilhas;
        }

        #region TOBEIMPLEMENTEDAFTER
        //public void Save(FileStream file)
        //{
        //    string currentPathLevel = GetFinalLevel(planilha);
        //    string destFileName = Path.Combine(currentPathLevel, planilha.NomeArquivo + ".xlsm");
        //    Directory.CreateDirectory(currentPathLevel);
        //    File.Copy(planilha.CaminhoFonte/*_inputSource*/, destFileName, true);
        //} 
        #endregion

        #region TOBEUSEDAFTER
        //public List<Planilha> GetAll()
        //{
        //    List<Planilha> planilhas = new List<Planilha>();

        //    string[] dirsMonths = Directory.GetDirectories(_inputSource);
        //    foreach (var directory in dirsMonths)
        //    {
        //        string[] dirsCC = Directory.GetDirectories(directory);
        //        foreach (var ccDirectory in dirsCC)
        //        {
        //            string[] sheetFiles = Directory.GetFiles(ccDirectory);
        //            foreach (var sheet in sheetFiles)
        //            {
        //                Planilha planilha = new Planilha() { CaminhoFonte = sheet };
        //                planilhas.Add(planilha);
        //            }
        //        }
        //    }

        //    return planilhas;
        //} 
        #endregion

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
