using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
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
        public int cellsIgnore = 5;
        public List<string> possiblePair = new List<string> { "BankSAP", "BankStoreSlip", "SAPStoreSlip" };
        public Compare(Dictionary<string, List<string>> dict_param)
        {
            dict = dict_param;
            InitializeComponent();
            tbDebug.Enter += (s, e) => { tbDebug.Parent.Focus(); };
            tbDebug.Text = "****** Program is starting " + DateTime.Now.ToString() + " ******";
        }

        private void starting()
        {
            foreach (KeyValuePair<string, List<string>> entry in dict)
            {
                List<string> eachPossiblePair = new List<string>(possiblePair);
                // Getting Data
                List<TnxBank> tnxBanks = new List<TnxBank>();
                List<Slip> slips = new List<Slip>();
                List<SAP> SAPs = new List<SAP>();
                List<CommonSum> bankSums = new List<CommonSum>();
                List<SumByInterX> bankSumsByInterX = new List<SumByInterX>();
                List<CommonSum> sapSums = new List<CommonSum>();
                List<CommonSum> slipSums = new List<CommonSum>();
                if (entry.Value.Count != 1) {
                    addTextToDebug(entry.Key);
                    addTextToDebug(" + Getting data from the files of the store.");
                    if (entry.Value.Contains("Bank"))
                    {
                        addTextToDebug("  - reading file bank...");
                        tnxBanks = getTnxBank(entry.Key + "_Bank");
                    } else
                    {
                        eachPossiblePair.Remove("BankSAP");
                        eachPossiblePair.Remove("BankStoreSlip");
                    }
                    if (entry.Value.Contains("SAP"))
                    {
                        addTextToDebug("  - reading file sap...");
                        SAPs = getSAPs(entry.Key + "_SAP");

                    } else
                    {
                        eachPossiblePair.Remove("BankSAP");
                        eachPossiblePair.Remove("SAPStoreSlip");
                    }
                    if (entry.Value.Contains("StoreSlip"))
                    {
                        addTextToDebug("  - reading file store slip...");
                        slips = getSlips(entry.Key + "_StoreSlip");
                    } else
                    {
                        eachPossiblePair.Remove("BankStoreSlip");
                        eachPossiblePair.Remove("SAPStoreSlip");
                    }

                    // Calucate sum group by
                    addTextToDebug(" + Calculate total from the files");
                    if (tnxBanks.Count != 0)
                    {
                        addTextToDebug("  - get total of the bank file...");
                        // filter ACLEDA Bank Plc.
                        bankSumsByInterX = tnxBanks.OrderBy(tb => tb.CutoffDate).GroupBy(tb => new { tb.CutoffDate, tb.InterXBank }).Select(i => new SumByInterX
                        {
                            CutoffDate = i.Key.CutoffDate,
                            InterXBank = i.Key.InterXBank,
                            Total = i.Sum(x => x.PaymentAmount)
                        }).ToList();
                        bankSums = tnxBanks.OrderBy(tb => tb.CutoffDate).GroupBy(tb => tb.CutoffDate).Select(i => new CommonSum
                        {
                            CutoffDate = i.Key,
                            Total = i.Sum(x => x.PaymentAmount)
                        }).ToList();
                    }
                    if (SAPs.Count != 0)
                    {
                        addTextToDebug("  - get total of the sap file...");
                        sapSums = SAPs.OrderBy(s => s.DocDate).GroupBy(s => s.DocDate).Select(i => new CommonSum {
                        CutoffDate = i.Key,
                        Total = i.Sum(x => x.AmountInLocalCur)
                    }).ToList();
                    }
                    if (slips.Count != 0)
                    {
                        addTextToDebug("  - get total of the slip file...");
                        slipSums = slips.OrderBy(s => s.CutoffDate).GroupBy(s => s.CutoffDate).Select(i => new CommonSum {
                            CutoffDate = i.Key,
                            Total = i.Sum(x => x.Amount)
                        }).ToList();
                    }

                    // start comparing
                    addTextToDebug(" + Compare the possible pair");
                    eachPossiblePair.ForEach(pair =>
                    {
                        if (pair == "BankSAP")
                        {
                            addTextToDebug("  - between Bank and SAP (Bank - SAP)");
                            compareBankSAP(bankSumsByInterX, SAPs);
                        } else if ( pair == "BankStoreSlip")
                        {
                            addTextToDebug("  - between Bank and Store Slip (Bank - Slip)");
                            compareBankStoreSlip(bankSums, slipSums);
                        } else
                        {
                            addTextToDebug("  - between SAP and Store Slip (SAP - StoreSlip)");
                            compareSAPStoreSlip(sapSums, slipSums);
                        } 
                    });
                } else if (entry.Value.Count == 1)
                {
                    addTextToDebug($"There is only one file, {entry.Key} {entry.Value[0]}, cannot compare to anything.");
                }
                lblProcessDesc.Text = "...";
                addTextToDebug($"****** Program is finishing {DateTime.Now.ToString()} ******");
            }
        }

        private void compareBankSAP(List<SumByInterX> sbixs, List<SAP> saps)
        {
            lblProcessDesc.Text = "comparing bank and sap by cutoff date and bank type";
            var res1 = from sbix in sbixs
                       join sap in saps
                       on new { X = sbix.CutoffDate, Y = sbix.InterXBank } equals new { X = sap.DocDate, Y = sap.InterXBank }
                       into JoinedList
                       from sap in JoinedList.DefaultIfEmpty()
                       select new
                       {
                           CutoffDate = sbix.CutoffDate,
                           InterXBank = sbix.InterXBank,
                           BankSum = sbix?.Total,
                           Sap = sap?.AmountInLocalCur
                       };
            var res2 = from sap in saps
                       join sbix in sbixs
                       on new { X = sap.DocDate, Y = sap.InterXBank } equals new { X = sbix.CutoffDate, Y = sbix.InterXBank }
                       into JoinedList
                       from sbix in JoinedList.DefaultIfEmpty()
                       select new
                       {
                           CutoffDate = sap.DocDate,
                           InterXBank = sap.InterXBank,
                           BankSum = sbix?.Total,
                           Sap = sap?.AmountInLocalCur
                       };
            var res3 = res1.Union(res2).OrderBy(r => r.CutoffDate).ToList();
            res3.ForEach(r =>
            {
                var donly = r.CutoffDate.ToString("dd-MMM-yyyy");
                if (r.BankSum != null && r.Sap != null)
                {
                    addTextToDebug($"    > {donly} {r.InterXBank}: Bank - SAP = {r.BankSum} - {r.Sap} = {r.BankSum - r.Sap} ");
                }
                else if (r.BankSum != null)
                {
                    addTextToDebug($"    > {donly} {r.InterXBank}: Bank ({r.BankSum}), no SAP");
                }
                else
                {
                    addTextToDebug($"    > {donly} {r.InterXBank}: SAP ({r.Sap}), no Bank");
                }
            });
        }

        private void compareBankStoreSlip(List<CommonSum> bankSums, List<CommonSum> slipSums)
        {
            lblProcessDesc.Text = "comparing bank and store slip by cutoff date...";
            var res1 = from bs in bankSums
                      join sls in slipSums
                      on bs.CutoffDate equals sls.CutoffDate
                      into JoinedList
                      from sls in JoinedList.DefaultIfEmpty()
                      select new
                      {
                          CutoffDate = bs.CutoffDate,
                          BankSum = bs?.Total,
                          SlipSum = sls?.Total
                      };
            var res2 = from sls in slipSums
                       join bs in bankSums
                       on sls.CutoffDate equals bs.CutoffDate
                       into JoinedList
                       from bs in JoinedList.DefaultIfEmpty()
                       select new
                       {
                           CutoffDate = sls.CutoffDate,
                           BankSum = bs?.Total,
                           SlipSum = sls?.Total
                       };
            var res3 = res1.Union(res2).OrderBy(r => r.CutoffDate).ToList();
            res3.ForEach(r =>
            {
                var donly = r.CutoffDate.ToString("dd-MMM-yyyy");
                if (r.BankSum != null && r.SlipSum != null)
                {
                    addTextToDebug($"    > {donly}: Bank - Store Slip = {r.BankSum} - {r.SlipSum} = {r.BankSum - r.SlipSum} ");
                }
                else if (r.BankSum != null)
                {
                    addTextToDebug($"    > {donly}: Bank ({r.BankSum}), no Store Slip");
                }
                else
                {
                    addTextToDebug($"    > {donly}: Store Slip ({r.SlipSum}), no Bank");
                }
            });
        }

        private void compareSAPStoreSlip(List<CommonSum> sapSums, List<CommonSum> slipSums)
        {
            lblProcessDesc.Text = "comparing sap and store slip by cutoff date...";
            var res1 = from sas in sapSums
                       join sls in slipSums
                       on sas.CutoffDate equals sls.CutoffDate
                       into JoinedList from sls in JoinedList.DefaultIfEmpty()
                       select new {
                           CutoffDate = sas.CutoffDate,
                           SapSum = sas?.Total,
                           SlipSum = sls?.Total
                       };
            var res2 = from sls in slipSums
                       join sas in sapSums
                       on sls.CutoffDate equals sas.CutoffDate
                       into JoinedList
                       from sas in JoinedList.DefaultIfEmpty()
                       select new 
                       {
                           CutoffDate = sls.CutoffDate,
                           SapSum = sas?.Total,
                           SlipSum = sls?.Total
                       };
            var res3 = res1.Union(res2).OrderBy(r => r.CutoffDate).ToList();
            res3.ForEach(r =>
            {
                var donly = r.CutoffDate.ToString("dd-MMM-yyyy");
                if (r.SapSum != null && r.SlipSum != null)
                {
                    addTextToDebug($"    > {donly}: SAP - Store Slip = {r.SapSum} - {r.SlipSum} = {r.SapSum - r.SlipSum} ");
                } else if (r.SapSum != null)
                {
                    addTextToDebug($"    > {donly}: SAP ({r.SapSum}), no Store Slip");
                } else
                {
                    addTextToDebug($"    > {donly}: Store Slip ({r.SlipSum}), no SAP");
                }
            });
        }

        private List<TnxBank> getTnxBank(string filename)
        {
            List<TnxBank> tnxBanks = new List<TnxBank>();
            string path = Environment.CurrentDirectory + "\\files\\" + filename;
            Workbook wb = app.Workbooks.Open(path);
            Worksheet wsh = wb.Worksheets[1];
            Microsoft.Office.Interop.Excel.Range cells = wsh.Cells;
            int running_row = cellsIgnore + 2;
            try
            {
                while (cells[running_row, 1].value != null)
                {
                    TnxBank tnxBank = new TnxBank();
                    var dt = DateTime.Parse(cells[running_row, 1].value);
                    var donly = DateOnly.FromDateTime(dt);
                    var tonly = TimeOnly.FromDateTime(dt);
                    var amt = cells[running_row, 12].Text;
                    tnxBank.TnxDateTime = dt;
                    tnxBank.CutoffDate = tonly.CompareTo(TimeOnly.Parse("05:00 PM")) < 0 ? donly : donly.AddDays(1);
                    tnxBank.PaymentAmount = Decimal.Parse(amt);
                    tnxBank.TnxCCY = cells[running_row, 14].value;
                    tnxBank.RefPrimary = cells[running_row, 16].value == "" ? cells[running_row, 16].value : cells[running_row, 17].value;
                    tnxBank.SettleStatus = cells[running_row, 22].value;
                    tnxBank.SRCBank = cells[running_row, 19].value;
                    tnxBank.InterXBank = cells[running_row, 19].value == "ACLEDA Bank Plc." ? "InnerBank" : "OtherBank"; 
                    tnxBanks.Add(tnxBank);
                    changeLblProcessDesc($"reading row {running_row - cellsIgnore - 1} from file {filename}.");
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
                    if (cells[running_row, 9].Text.Contains("(KHQR") || cells[running_row, 9].Text.Contains("(TC"))
                    {
                        SAP sap = new SAP();
                        sap.Assignment = (string)cells[running_row, 1].Text;
                        sap.DocumentNo = cells[running_row, 2].Text;
                        sap.BusA = cells[running_row, 3].Text;
                        sap.Type = cells[running_row, 4].Text;
                        var txt = cells[running_row, 5].Text.Split(".");
                        DateTime dt = new DateTime(Int32.Parse(txt[2]), Int32.Parse(txt[1]), Int32.Parse(txt[0]));
                        sap.DocDate = DateOnly.FromDateTime(dt);
                        sap.PK = cells[running_row, 6].Text;
                        var amount = cells[running_row, 7].Value;
                        sap.AmountInLocalCur = (decimal)Math.Abs(amount);
                        sap.LCurr = cells[running_row, 8].Text;
                        sap.Text = cells[running_row, 9].Text;
                        sap.InterXBank = cells[running_row, 9].Text.Contains("KHQR") ? "OtherBank" : "InnerBank";
                        SAPs.Add(sap);
                    }
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
                    var dt = cells[running_row, 1].Text;
                    slip.TrxDate = DateOnly.FromDateTime(DateTime.Parse(cells[running_row, 1].Text));
                    var time = Convert.ToDateTime(cells[running_row, 2].Text);
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
            System.Threading.Thread.Sleep(1);
            starting();
        }
    }
}
