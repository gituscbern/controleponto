using ClosedXML.Excel;
using ControlePonto.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Adapters.Gateways
{
    public class ExcelService : IExcelService
    {
        private XLWorkbook _xLWorkbook;

        private string _currentText;
        private string _currentSheet;

        public ExcelService(string workbookPath)
        {
            _xLWorkbook = new XLWorkbook(workbookPath);
        }

        public IXLCell GetCellFrom(string value, string sheet)
        {
            var ws = _xLWorkbook.Worksheet(sheet);

            int rowsUsed = ws.RowsUsed().Count();
            int columnsUsed = ws.ColumnsUsed().Count();

            for (int row = 1; row < rowsUsed; row++)
            {
                for (int column = 1; column <= columnsUsed; column++)
                {
                    string cellValue = ws.Cell(row, column).Value.ToString();
                    if (cellValue.Replace(" ", "") == value.Replace(" ", ""))
                    {
                        return ws.Cell(row, column);
                    }
                }
            }
            return null;
        }

        public IXLCell GetCellFrom(string value, int sheet)
        {
            var ws = _xLWorkbook.Worksheet(sheet);

            int rowsUsed = ws.RowsUsed().Count();
            int columnsUsed = ws.ColumnsUsed().Count();

            for (int row = 1; row < rowsUsed; row++)
            {
                for (int column = 1; column <= columnsUsed; column++)
                {
                    string cellValue = ws.Cell(row, column).Value.ToString();
                    if (cellValue.Replace(" ", "") == value.Replace(" ", ""))
                    {
                        return ws.Cell(row, column);
                    }
                }
            }
            return null;
        }
        public IExcelService Write(string value)
        {
            _currentText = value;
            return this;
        }
        public void OnRight(string cellLocation)
        {
            var ws = _xLWorkbook.Worksheet(_currentSheet);
            IXLCell cell = GetCellFrom(cellLocation, _currentSheet );
            int row = cell.Address.RowNumber;
            int column = cell.Address.ColumnNumber+1;

            ws.Cell(row, column).Value = _currentText;
        }
        public IExcelService Sheet(string nameSheet)
        {
            _currentSheet = nameSheet;
            return this;
        }

        public string GetAfter(string cellLocation)
        {
            var ws = _xLWorkbook.Worksheet(_currentSheet);
            IXLCell cell = GetCellFrom(cellLocation, _currentSheet);
            int row = cell.Address.RowNumber;
            int column = cell.Address.ColumnNumber + 1;

            return ws.Cell(row, column).Value.ToString();
        }

        public T Get<T>(int row, int column)
        {
            var ws = _xLWorkbook.Worksheet(_currentSheet);
            return ws.Cell(row, column).GetValue<T>();
        }

        public TimeSpan GetFromHours(int row, int column)
        {
            var ws = _xLWorkbook.Worksheet(_currentSheet);

            if (ws.Cell(row, column).Value.ToString() == "") return TimeSpan.Zero;

            return TimeSpan.FromDays(Convert.ToDouble(ws.Cell(row, column).Value));
        }

        public void Paint(int lineNumber)
        {
            var ws = _xLWorkbook.Worksheet(_currentSheet);
            ws.Row(lineNumber).Style.Fill.BackgroundColor = XLColor.PastelRed;
        }
        public void Discolor(int lineNumber)
        {
            var ws = _xLWorkbook.Worksheet(_currentSheet);
            ws.Row(lineNumber).Style.Fill.BackgroundColor = XLColor.White;
        }
        public bool Save()
        {
            try
            {
                _xLWorkbook.Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool SaveAs(string destPath)
        {
            try
            {
                _xLWorkbook.SaveAs(destPath);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public int GetLineNumberFrom(string value)
        {
            var row = GetCellFrom(value, _currentSheet);
            return row.Address.RowNumber;
        }

        public int AmountRowsUsed()
        {
            var ws = _xLWorkbook.Worksheet(_currentSheet);
            return ws.RowsUsed().Count();
        }
    }
}
