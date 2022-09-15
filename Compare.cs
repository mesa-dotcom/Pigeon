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
        public int cells_ignore = 5;
        public Compare(Dictionary<string, List<string>> dict_param)
        {
            dict = dict_param;
            InitializeComponent();
            tbDebug.Text = "****** Program is starting " + DateTime.Now.ToString() + " ******";
        }

        private void starting()
        {
            foreach (KeyValuePair<string, List<string>> entry in dict)
            {
                // Getting Data
                addTextToDebug(entry.Key);
                addTextToDebug(" + Getting data from the files of the store.");
                List<TnxBank> tnxBanks = new List<TnxBank>();
                List<Slip> slips = new List<Slip>();
                List<SAP> SAPs = new List<SAP>();
                var tnxBankSumByDate = "";
                var tnxBankSumByDateByBank = "">;
                var tnxSapSum = "";
                var tnxSlipSum = "";
                if (entry.Value.Count != 1) {
                    if (entry.Value.Contains("Bank"))
                    {
                        addTextToDebug("  - Reading file bank...");
                        tnxBanks = getTnxBank(entry.Key + "_Bank");
                    }
                    if (entry.Value.Contains("SAP"))
                    {
                        addTextToDebug("  - Reading file SAP...");
                        SAPs = getSAPs(entry.Key + "_SAP");

                    }
                    if (entry.Value.Contains("StoreSlip"))
                    {
                        addTextToDebug("  - Reading file store slip...");
                        slips = getSlips(entry.Key + "_StoreSlip");
                    }
                }
                // Calucate sum group by
                addTextToDebug(" + Calcute total from the files of the store.");
                if (tnxBanks.Count != 0)
                {
                    addTextToDebug("  - Get total of the bank file...");
                    // filter ACLEDA Bank Plc.
                    var sss = tnxBanks.OrderBy(tb => tb.CutoffDate).GroupBy(tb => new { tb.CutoffDate, tb.InterXBank }).Select(i => new
                    {
                        CutoffDate = i.Key.CutoffDate,
                        InterXBank = i.Key.InterXBank,
                        Total = i.Sum(x => x.PaymentAmount)
                    }).ToList();
                    var bankSumByDate = tnxBanks.OrderBy(tb => tb.CutoffDate).GroupBy(tb => tb.CutoffDate).Select(i => new Object[]
                    {
                            i.Key,
                            i.Sum(x => x.PaymentAmount)
                    }).ToList();
                }
                if (SAPs.Count != 0)
                {
                    addTextToDebug("  - Get total of the sap file...");
                    var sapSum = SAPs.OrderBy(s => s.Assignment).GroupBy(s => s.Assignment).Select(i => new object[] {
                        i.Key,
                        i.Sum(x => x.AmountInLocalCur)
                    }).ToList();
                }
                if (slips.Count != 0)
                {
                    addTextToDebug("  - Get total of the slip file...");
                    var slipSum = slips.OrderBy(s => s.CutoffDate).GroupBy(s => s.CutoffDate).Select(i => new object[] {
                            i.Key.ToString(),
                            i.Sum(x => x.Amount)
                        }).ToList();
                }
            }
        }

        private List<TnxBank> getTnxBank(string filename)
        {
            List<TnxBank> tnxBanks = new List<TnxBank>();
            string path = Environment.CurrentDirectory + "\\files\\" + filename;
            Workbook wb = app.Workbooks.Open(path);
            Worksheet wsh = wb.Worksheets[1];
            Microsoft.Office.Interop.Excel.Range cells = wsh.Cells;
            int running_row = cells_ignore + 2;
            try
            {
                while (cells[running_row, 1].value != null)
                {
                    TnxBank tnxBank = new TnxBank();
                    var dt = DateTime.Parse(cells[running_row, 1].value);
                    var donly = DateOnly.FromDateTime(dt);
                    var tonly = TimeOnly.FromDateTime(dt);
                    var amt = cells[running_row, 12].value;
                    tnxBank.TnxDateTime = dt;
                    tnxBank.CutoffDate = tonly.CompareTo(TimeOnly.Parse("05:00 PM")) < 0 ? donly : donly.AddDays(1);
                    tnxBank.PaymentAmount = Decimal.Parse(amt);
                    tnxBank.TnxCCY = cells[running_row, 14].value;
                    tnxBank.RefPrimary = cells[running_row, 16].value == "" ? cells[running_row, 16].value : cells[running_row, 17].value;
                    tnxBank.SettleStatus = cells[running_row, 22].value;
                    tnxBank.SRCBank = cells[running_row, 19].value;
                    tnxBank.InterXBank = cells[running_row, 19].value == "ACLEDA Bank Plc." ? "InnerBank" : "Other"; 
                    tnxBanks.Add(tnxBank);
                    changeLblProcessDesc($"reading row {running_row - cells_ignore - 1} from file {filename}.");
                    running_row++;
                }
            }
            catch (Exception exc)
            {
                wb.Close(0);
                app.Quit();
                throw new Exception(exc.Message + " in " + filename);
            }
            wb.Close(0);
            app.Quit();
            return tnxBanks;

        }

        private List<SAP> getSAPs(string filename)
        {
            List<SAP> SAPs = new List<SAP>();
            string path = Environment.CurrentDirectory + "\\files\\" + filename;
            Workbook wb = app.Workbooks.Open(path);
            Worksheet wsh = wb.Worksheets[1];
            Microsoft.Office.Interop.Excel.Range cells = wsh.Cells;
            var ccn = checkColumnName(cells, new List<string> { "Assignment", "DocumentNo", "BusA", "Type", "Doc. Date", "PK", "Amount in local cur.", "LCurr", "Text" });
            if ( ccn != "")
            {
                wb.Close(0);
                app.Quit();
                throw new Exception(filename + " column (" + ccn +") is not correctly set.");
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
                    var amount = cells[running_row, 7].Value;
                    sap.AmountInLocalCur = (decimal) Math.Abs(amount);
                    sap.LCurr = cells[running_row, 8].Text;
                    sap.Text = cells[running_row, 9].Text;
                    sap.InterXBank = cells[running_row, 9].Text.Contains("KHQR") ? "Other" : "InnerBank";
                    SAPs.Add(sap);
                    changeLblProcessDesc($"reading row {running_row - 1} from file {filename}.");
                    running_row++;
                }
            }
            catch (Exception exc)
            {
                wb.Close(0);
                app.Quit();
                throw new Exception(exc.Message + " in " + filename);
            }
            wb.Close(0);
            app.Quit();
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
            var ccn = checkColumnName(cells, new List<string> { "Date", "Time", "Amount" });
            if (ccn != "")
            {
                wb.Close(0);
                app.Quit();
                throw new Exception(filename + " column (" + ccn + ") is not correctly set.");
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
                    changeLblProcessDesc($"reading row {running_row - 1} from file {filename}.");
                    running_row++;
                }
            }
            catch (Exception exc)
            {
                wb.Close(0);
                app.Quit();
                throw new Exception(exc.Message + " in " + filename);
            }
            wb.Close(0);
            app.Quit();
            return slips;
        }

        private string checkColumnName(Microsoft.Office.Interop.Excel.Range cells, List<string> columns)
        {
            for (int i = 1; i < columns.Count + 1; i++)
            {
                if (cells[1, i].text != columns[i - 1])
                {
                    return cells[1, i].text;
                }
            }
            return "";
        }

        private void addTextToDebug(string txt)
        {
            tbDebug.AppendText("\r\n" + txt);
        }

        private void changeLblProcessDesc(string message)
        {
            lblProcessDesc.Text = message;
        }

        private void Compare_Shown(object sender, EventArgs e)
        {
            System.Threading.Thread.Sleep(100);
            starting();
        }
    }
}
