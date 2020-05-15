using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Core.Contracts
{
    public interface IExcelService
    {
        IExcelService Sheet(string sheetName);
        IExcelService Write(string value);
        void OnRight(string cellLocation);
        void On(int row, int column);
        bool Save();
        bool SaveAs(string destPath);
        string GetAfter(string cellLocation);
        void Paint(int lineNumber);
        void Discolor(int lineNumber);
        T Get<T>(int row, int column);
        TimeSpan GetFromHours(int row, int column);
        int GetLineNumberFrom(string value);
        int AmountRowsUsed();
        void SetSource(string caminhoFonte);
        void CleanSheet(int from);
        void Create(string workbookName, string sheetName);
    }
}
