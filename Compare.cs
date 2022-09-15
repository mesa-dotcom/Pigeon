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
            tbDebug.Text = "--- Program is starting " + DateTime.Now.ToString() + "---";
        }

        private void Compare_Load(object sender, EventArgs e)
        {
            preparing();
        }

        private void preparing()
        {
            foreach (KeyValuePair<string, List<string>> entry in dict)
            {
                addTextToDebug("Getting data from the files of the store " + entry.Key + ".");
                if (entry.Value.Count != 1) {
                    List<Slip> slips = new List<Slip>();
                    List<SAP> SAPs = new List<SAP>();
                    if (entry.Value.Contains("Bank"))
                    {
                        addTextToDebug(entry.Key + " bank file is read");
                    }
                    if (entry.Value.Contains("SAP"))
                    {
                        SAPs = getSAPs(entry.Key + "_SAP.xlsx");
                        addTextToDebug(entry.Key + " sap file is read");
                    }
                    if (entry.Value.Contains("StoreSlip"))
                    {
                        slips = getSlips(entry.Key + "_StoreSlip.xlsx");
                        addTextToDebug(entry.Key + " store slip file is read");
                    }
                }
            }
        }

        private void compareSAPandSlip(List<Slip> slips, List<SAP> SAPs)
        {
            List<Object[]> slipSum = slips.OrderBy(s => s.CutoffDate).GroupBy(s => s.CutoffDate).Select(i => new object[]
            {
                i.Key.ToString(),
                i.Sum(x => x.Amount)
            }).ToList();
            List<Object[]> sapSum = SAPs.OrderBy(s => s.Assignment).GroupBy(s => s.Assignment).Select(i => new object[]
            {
                i.Key,
                i.Sum(x => x.AmountInLocalCur)
            }).ToList();
            System.Windows.Forms.Label lbl = new System.Windows.Forms.Label();
        }

        private List<SAP> getSAPs(string filename)
        {
            List<SAP> SAPs = new List<SAP>();
            string path = Environment.CurrentDirectory + "\\files\\" + filename;
            Workbook wb = app.Workbooks.Open(path);
            Worksheet wsh = wb.Worksheets[1];
            Microsoft.Office.Interop.Excel.Range cells = wsh.Cells;
            if (checkColumnName(cells, new List<string> { "Assignment", "DocumentNo", "BusA", "Type", "Doc. Date", "PK", "Amount in local cur.", "LCurr", "Text" }))
            {
                wb.Close(0);
                app.Quit();
                throw new Exception(filename + "'s columns are not correctly set.");
            }
            int running_row = 2;
            try
            {
                while (cells[running_row, 1].value != null)
                {
                    SAP sap = new SAP();
                    sap.Assignment = (string) cells[running_row, 1].Text;
                    sap.DocumentNo = cells[running_row, 2].Text;
                    sap.BusA = cells[running_row, 3].Text;
                    sap.Type = cells[running_row, 4].Text;
                    sap.DocDate = cells[running_row, 5].Text;
                    sap.PK = cells[running_row, 6].Text;
                    sap.AmountInLocalCur = (decimal) Math.Abs(cells[running_row, 7].Value);
                    sap.LCurr = cells[running_row, 8].Text;
                    sap.Text = cells[running_row, 9].Text;
                    sap.InterXBank = cells[running_row, 9].Text.Contains("KHQR") ? "Other" : "Same";
                    SAPs.Add(sap);
                    running_row++;
                }
            }
            catch (Exception exc)
            {
                wb.Close(0);
                app.Quit();
                throw new Exception(exc.Message);
            }

            return SAPs;
        }

        private List<Slip> getSlips(string filename)
        {
            List<Slip> slips = new List<Slip>();
            string path = Environment.CurrentDirectory + "\\files\\" + filename;
            Workbook wb = app.Workbooks.Open(path);
            Worksheet wsh = wb.Worksheets[1];
            Microsoft.Office.Interop.Excel.Range cells = wsh.Cells;
            // Check column Name
            if (checkColumnName(cells, new List<string> { "Date", "Time", "Amount"}))
            {
                wb.Close(0);
                app.Quit();
                throw new Exception(filename + "'s columns are not correctly set.");
            }
            int running_row = 2;
            try
            {
                while (cells[running_row, 1].value != null)
                {
                    Slip slip = new Slip();
                    slip.Store = filename.Substring(0, 6);
                    slip.TrxDate = DateOnly.FromDateTime(cells[running_row, 1].value);
                    var time = Convert.ToDateTime(cells[running_row, 2].value);
                    slip.TrxTime = TimeOnly.FromDateTime(time);
                    slip.CutoffDate = slip.TrxTime.CompareTo(TimeOnly.Parse("05:00 PM")) < 0 ? slip.TrxDate : slip.TrxDate.AddDays(1);
                    slip.Amount = (decimal) cells[running_row, 3].value;
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

        private bool checkColumnName(Microsoft.Office.Interop.Excel.Range cells, List<string> columns)
        {
            for (int i = 1; i < columns.Count + 1; i++)
            {
                if (cells[1, i].value != columns[i])
                {
                    return false;
                }
            }
            return true;
        }

        private void addTextToDebug(string txt)
        {
            tbDebug.AppendText("\r\n" + txt);
        }
    }
}
