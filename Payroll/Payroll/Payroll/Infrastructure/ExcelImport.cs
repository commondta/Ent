using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;

namespace Payroll_HCC.Infrastructure
{
    /// <summary>
    /// Reads uploaded .xlsx files via EPPlus (no Excel installation required on the server).
    /// Replaces the former Microsoft.Office.Interop.Excel usage, which is unsupported
    /// in server processes and required Office on the machine.
    /// </summary>
    public static class ExcelImport
    {
        /// <summary>
        /// Reads the first worksheet into rows of trimmed cell strings.
        /// Empty rows are skipped; missing cells come back as "".
        /// </summary>
        /// <param name="stream">The uploaded file stream (.xlsx).</param>
        /// <param name="skipHeaderRow">True to skip the first row (column captions).</param>
        public static List<string[]> ReadFirstSheet(Stream stream, bool skipHeaderRow)
        {
            var rows = new List<string[]>();
            using (var package = new ExcelPackage(stream))
            {
                if (package.Workbook.Worksheets.Count == 0) return rows;
                ExcelWorksheet ws = package.Workbook.Worksheets[1]; // EPPlus 4.x is 1-based
                if (ws.Dimension == null) return rows;              // completely empty sheet

                int firstRow = ws.Dimension.Start.Row + (skipHeaderRow ? 1 : 0);
                int lastRow = ws.Dimension.End.Row;
                int firstCol = ws.Dimension.Start.Column;
                int lastCol = ws.Dimension.End.Column;

                for (int r = firstRow; r <= lastRow; r++)
                {
                    var cells = new string[lastCol - firstCol + 1];
                    bool any = false;
                    for (int c = firstCol; c <= lastCol; c++)
                    {
                        object v = ws.Cells[r, c].Value;
                        string s = v == null ? "" : v.ToString().Trim();
                        cells[c - firstCol] = s;
                        if (s.Length > 0) any = true;
                    }
                    if (any) rows.Add(cells);
                }
            }
            return rows;
        }
    }
}
