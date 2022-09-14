using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using Pigeon.Classes;

namespace Pigeon
{
    public partial class Compare : Form
    {
        Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();
        Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
        public Compare(Dictionary<string, List<string>> dict_param)
        {
            dict = dict_param;
            InitializeComponent();
            preparing();
        }

        private void preparing()
        {
            foreach (KeyValuePair<string, List<string>> entry in dict)
            {
                if (entry.Value.Count != 1) {
                    List<Slip> slips = new List<Slip>();
                    if (entry.Value.Contains("Bank"))
                    {

                    }
                    if (entry.Value.Contains("SAP"))
                    {

                    }
                    if (entry.Value.Contains("StoreSlip"))
                    {
                        slips = getSlips(entry.Key + "_StoreSlip.xlsx");
                    }
                }
            }
        }

        private void comparing()
        {

        }

        private List<Slip> getSlips(string Filename)
        {
            List<Slip> slips = new List<Slip>();
            string path = Environment.CurrentDirectory + "\\files\\" + Filename;
            Workbook wb = app.Workbooks.Open(path);
            Worksheet wsh = wb.Worksheets[1];
            Microsoft.Office.Interop.Excel.Range Cells = wsh.Cells;
            // Check column Name
            if (Cells[1, 1].value != "Date" && Cells[1,2].value != "Time" && Cells[1,3].value != "Amount")
            {
                wb.Close(0);
                app.Quit();
                throw new Exception("Columns are not correct.");
            }
            int running_row = 2;
            try
            {
                while (Cells[running_row, 1].value != null)
                {
                    Slip slip = new Slip();
                    slip.Store = "B30001";
                    slip.TrxDate = DateOnly.FromDateTime(Cells[running_row, 1].value);
                    var time = Convert.ToDateTime(Cells[running_row, 2].value);
                    slip.TrxTime = TimeOnly.FromDateTime(time);
                    slip.Amount = (decimal) Cells[running_row, 3].value;
                    slips.Add(slip);
                    running_row++;
                }
            }
            catch (Exception exc)
            {
                wb.Close(0);
                app.Quit();
                throw new Exception(exc.Message);
            }
            return slips;
        }
    }
}
