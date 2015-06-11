using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;

namespace ExcelGraphs
{
    class Program
    {
        static void Main(string[] args)
        {
            Application app = new Application();
            Workbook book = null;
            Range range = null;
            app.Visible = false;
            //app.ActiveWindow.Visible = false;
            app.ScreenUpdating = false;
            app.DisplayAlerts = false;

            string execPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);

            book = app.Workbooks.Open(@"C:\Users\DCG\Desktop\Calibration_curve_data.xlsx");


            foreach (Worksheet sheet in book.Worksheets)
            {
                // get a range to work with
                range = sheet.Range["A1", Missing.Value];
                // get the end of values to the right (will stop at the first empty cell)
                range = range.End[XlDirection.xlToRight];
                // get the end of values toward the bottom, looking in the last column (will stop at first empty cell)
                range = range.End[XlDirection.xlDown];

                // get the address of the bottom, right cell
                string downAddress = range.Address[false, false, XlReferenceStyle.xlA1, Type.Missing, Type.Missing];

                // Get the range, then values from a1
                range = sheet.Range["A1", downAddress];
                object[,] values = (object[,])range.Value2;

                // View the values
                Console.Write("\t");
                Console.WriteLine();
                for (int i = 1; i <= values.GetLength(0); i++)
                {
                    for (int j = 1; j <= values.GetLength(1); j++)
                    {
                        Console.Write("{0}\t", values[i, j]);
                    }
                    Console.WriteLine();
                }
                Console.ReadLine();
            }
        }
    }
}
