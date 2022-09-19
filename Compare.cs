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
using Microsoft.Office.Interop;
using Pigeon.Classes;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Pigeon
{
    public partial class Compare : Form
    {
        Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();
        Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
        public int cellsIgnore = 0;
        public List<string> possiblePair = new List<string> { "BankSAP", "BankStoreSlip", "SAPStoreSlip" };
        public DateTime runningTime = DateTime.MinValue;
        public string CurrentDirectory = Environment.CurrentDirectory;
        public Compare(Dictionary<string, List<string>> dict_param)
        {
            dict = dict_param;
            runningTime = DateTime.Now;
            InitializeComponent();
            tbDebug.Enter += (s, e) => { tbDebug.Parent.Focus(); };
            tbDebug.Text = "****** Program is starting " + runningTime.ToString() + " ******";
        }

        private void Starting()
        {
            Dictionary<string, List<Result>> grouppedResults = new Dictionary<string, List<Result>>();
            foreach (KeyValuePair<string, List<string>> entry in dict)
            {
                List<string> eachPossiblePair = new List<string>(possiblePair);
                List<Result> results = new List<Result>();
                // Getting Data
                if (entry.Value.Count != 1) {
                    AddTextToDebug(entry.Key);
                    AddTextToDebug(" + Getting data from the files of the store.");
                    List<TnxBank> tnxBanks = new List<TnxBank>();
                    List<Slip> slips = new List<Slip>();
                    List<SAP> SAPs = new List<SAP>();
                    List<CommonSum> bankSums = new List<CommonSum>();
                    List<SumByInterX> bankSumsByInterX = new List<SumByInterX>();
                    List<CommonSum> sapSums = new List<CommonSum>();
                    List<CommonSum> slipSums = new List<CommonSum>();

                    if (entry.Value.Contains("Bank"))
                    {
                        AddTextToDebug("  - reading file bank...");
                        tnxBanks = GetTnxBank(entry.Key + "_Bank");
                    } else
                    {
                        eachPossiblePair.Remove("BankSAP");
                        eachPossiblePair.Remove("BankStoreSlip");
                    }
                    if (entry.Value.Contains("SAP"))
                    {
                        AddTextToDebug("  - reading file sap...");
                        SAPs = GetSAPs(entry.Key + "_SAP");

                    } else
                    {
                        eachPossiblePair.Remove("BankSAP");
                        eachPossiblePair.Remove("SAPStoreSlip");
                    }
                    if (entry.Value.Contains("StoreSlip"))
                    {
                        AddTextToDebug("  - reading file store slip...");
                        slips = GetSlips(entry.Key + "_StoreSlip");
                    } else
                    {
                        eachPossiblePair.Remove("BankStoreSlip");
                        eachPossiblePair.Remove("SAPStoreSlip");
                    }

                    // Calucate sum group by
                    AddTextToDebug(" + Calculate total from the files");
                    if (tnxBanks.Count != 0)
                    {
                        AddTextToDebug("  - get total of the bank file...");
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
                        AddTextToDebug("  - get total of the sap file...");
                        sapSums = SAPs.OrderBy(s => s.DocDate).GroupBy(s => s.DocDate).Select(i => new CommonSum {
                        CutoffDate = i.Key,
                        Total = i.Sum(x => x.AmountInLocalCur)
                    }).ToList();
                    }
                    if (slips.Count != 0)
                    {
                        AddTextToDebug("  - get total of the slip file...");
                        slipSums = slips.OrderBy(s => s.CutoffDate).GroupBy(s => s.CutoffDate).Select(i => new CommonSum {
                            CutoffDate = i.Key,
                            Total = i.Sum(x => x.Amount)
                        }).ToList();
                    }

                    // start comparing
                    AddTextToDebug(" + Compare the possible pair");
                    eachPossiblePair.ForEach(pair =>
                    {
                        if (pair == "BankSAP")
                        {
                            AddTextToDebug("  - between Bank and SAP (Bank - SAP)");
                            results.AddRange(CompareBankSAP(bankSumsByInterX, SAPs, entry.Key));
                        } else if ( pair == "BankStoreSlip")
                        {
                            AddTextToDebug("  - between Bank and Store Slip (Bank - Slip)");
                            results.AddRange(CompareBankStoreSlip(bankSums, slipSums, entry.Key));
                        } else
                        {
                            AddTextToDebug("  - between SAP and Store Slip (SAP - StoreSlip)");
                            results.Concat(CompareSAPStoreSlip(sapSums, slipSums, entry.Key));
                        } 
                    });
                    grouppedResults.Add(entry.Key, results.OrderBy(r => r.CutoffDate).ThenBy(r => r.SRCBank).ThenBy(r => r.Comparer1).ToList()); 
                } else if (entry.Value.Count == 1)
                {
                    AddTextToDebug($"There is only one file, {entry.Key} {entry.Value[0]}, cannot compare to anything.");
                }
            }
            btnSaveDebug.Enabled = true;
            if (grouppedResults.Count != 0 || dict.Count != 0)
            {
                CreateExcelResult(grouppedResults);
            }
            lblProcessDesc.Text = "...";
            AddTextToDebug($"****** Program is finishing {DateTime.Now.ToString()} ******");
        }

        private void CreateExcelResult(Dictionary<string, List<Result>> gr)
        {
            Workbook wb = null;
            object misValue = System.Reflection.Missing.Value;
            wb = app.Workbooks.Add(misValue);
            Worksheet firstSheet = wb.ActiveSheet as Worksheet;
            firstSheet.Name = "File Info";
            firstSheet.Cells[1, 1] = "Store";
            firstSheet.Cells[1, 2] = "Bank File";
            firstSheet.Cells[1, 3] = "SAP File";
            firstSheet.Cells[1, 4] = "Store Slip File";
            foreach (var x in dict.Select((Entry, Index) => new { Entry, Index }))
            {
                firstSheet.Cells[x.Index + 2, 1] = x.Entry.Key;
                firstSheet.Cells[x.Index + 2, 2] = x.Entry.Value.Contains("Bank") ? "Has" : "-";
                firstSheet.Cells[x.Index + 2, 3] = x.Entry.Value.Contains("SAP") ? "Has" : "-";
                firstSheet.Cells[x.Index + 2, 4] = x.Entry.Value.Contains("StoreSlip") ? "Has" : "-";
            }
            foreach (KeyValuePair<string, List<Result>> g in gr)
            {
                Worksheet awsh = wb.Sheets.Add(misValue, misValue, 1, misValue)
                        as Worksheet;
                awsh.Name = g.Key;
                awsh.Cells[1, 1] = "Date";
                awsh.Cells[1, 2] = "SRC_BANK";
                awsh.Cells[1, 3] = "Comparer 1";
                awsh.Cells[1, 4] = "Comparer 2";
                awsh.Cells[1, 5] = "Diff. (Comparer 1 - Comparer 2)";
                for (int i = 0; i < g.Value.Count; i++)
                {
                    awsh.Cells[i + 2, 1] = g.Value[i].CutoffDate.ToString("dd-MMM-yyyy");
                    if (g.Value[i].SRCBank != null)
                    {
                        awsh.Cells[i + 2, 2] = g.Value[i].SRCBank == "InnerBank" ? "ACLEDA Bank Plc. (TC)" : "Other Bank (KHQR)";
                    } else
                    {
                        awsh.Cells[i + 2, 2] = "-";
                    }
                    awsh.Cells[i + 2, 3] = $"{g.Value[i].Comparer1} ({g.Value[i].Comparer1Amount})";
                    if (g.Value[i].Comparer2 != null)
                    {
                        awsh.Cells[i + 2, 4] = $"{g.Value[i].Comparer2} ({g.Value[i].Comparer2Amount})";
                    } else
                    {
                        awsh.Cells[i + 2, 4] = "No Comparer";
                        awsh.Cells[i + 2, 5] = "-";
                    }
                    if (g.Value[i].Comparer2 != null)
                    {
                        awsh.Cells[i + 2, 5].FormulaR1C1 = $"={g.Value[i].Comparer1Amount} - {g.Value[i].Comparer2Amount}";
                    }
                    awsh.Cells.HorizontalAlignment = HorizontalAlignment.Center;
                    awsh.Columns.AutoFit();
                    awsh.Rows.AutoFit();
                }
            }

            wb.SaveAs(CurrentDirectory + $"\\results\\result_{runningTime.ToString("yyyyMMdd_HHmm")}.xlsx", XlFileFormat.xlWorkbookDefault, misValue, misValue, misValue, misValue, XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            wb.Close(true, misValue, misValue);
            app.Quit();
        }

        private List<Result> CompareBankSAP(List<SumByInterX> sbixs, List<SAP> saps, string store)
        {
            List<Result> res = new List<Result>();
            lblProcessDesc.Text = "comparing bank and sap by cutoff date and bank type";
            // full outer join by left join and union
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
                    AddTextToDebug($"    > {donly} {r.InterXBank}: Bank - SAP = {r.BankSum} - {r.Sap} = {r.BankSum - r.Sap} ");
                    res.Add(new Result
                    { 
                        Store = store,
                        CutoffDate = r.CutoffDate,
                        SRCBank = r.InterXBank,
                        Comparer1 = "Bank",
                        Comparer2 = "SAP",
                        Comparer1Amount = r.BankSum,
                        Comparer2Amount = r.Sap
                    });
                }
                else if (r.BankSum != null)
                {
                    AddTextToDebug($"    > {donly} {r.InterXBank}: Bank ({r.BankSum}), no SAP");
                    res.Add(new Result
                    {
                        Store = store,
                        CutoffDate = r.CutoffDate,
                        SRCBank = r.InterXBank,
                        Comparer1 = "Bank",
                        Comparer2 = null,
                        Comparer1Amount = r.BankSum
                    });
                }
                else
                {
                    AddTextToDebug($"    > {donly} {r.InterXBank}: SAP ({r.Sap}), no Bank");
                    res.Add(new Result
                    {
                        Store = store,
                        CutoffDate = r.CutoffDate,
                        SRCBank = r.InterXBank,
                        Comparer1 = "SAP",
                        Comparer2 = null,
                        Comparer1Amount = r.Sap
                    });
                }
            });
            return res;
        }

        private List<Result> CompareBankStoreSlip(List<CommonSum> bankSums, List<CommonSum> slipSums, string store)
        {
            List<Result> res = new List<Result>();
            lblProcessDesc.Text = "comparing bank and store slip by cutoff date...";
            // full outer join by left join and union
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
                    AddTextToDebug($"    > {donly}: Bank - Store Slip = {r.BankSum} - {r.SlipSum} = {r.BankSum - r.SlipSum} ");
                    res.Add(new Result
                    {
                        Store = store,
                        CutoffDate = r.CutoffDate,
                        SRCBank = null,
                        Comparer1 = "Bank",
                        Comparer2 = "Store Slip",
                        Comparer1Amount = r.BankSum,
                        Comparer2Amount = r.SlipSum
                    });
                }
                else if (r.BankSum != null)
                {
                    AddTextToDebug($"    > {donly}: Bank ({r.BankSum}), no Store Slip");
                    res.Add(new Result
                    {
                        Store = store,
                        CutoffDate = r.CutoffDate,
                        SRCBank = null,
                        Comparer1 = "Bank",
                        Comparer2 = null,
                        Comparer1Amount = r.BankSum,
                        Comparer2Amount = null
                    });
                }
                else
                {
                    AddTextToDebug($"    > {donly}: Store Slip ({r.SlipSum}), no Bank");
                    res.Add(new Result
                    {
                        Store = store,
                        CutoffDate = r.CutoffDate,
                        SRCBank = null,
                        Comparer1 = "Store Slip",
                        Comparer1Amount = r.SlipSum,
                        Comparer2Amount = null
                    });
                }
            });
            return res;
        }

        private List<Result> CompareSAPStoreSlip(List<CommonSum> sapSums, List<CommonSum> slipSums, string store)
        {
            List<Result> res = new List<Result>();
            lblProcessDesc.Text = "comparing sap and store slip by cutoff date...";
            // full outer join by left join and union
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
                    AddTextToDebug($"    > {donly}: SAP - Store Slip = {r.SapSum} - {r.SlipSum} = {r.SapSum - r.SlipSum} ");
                    res.Add(new Result
                    {
                        Store = store,
                        CutoffDate = r.CutoffDate,
                        SRCBank = null,
                        Comparer1 = "SAP",
                        Comparer1Amount = r.SapSum,
                        Comparer2 = "Store Slip",
                        Comparer2Amount = r.SlipSum
                    });
                } else if (r.SapSum != null)
                {
                    AddTextToDebug($"    > {donly}: SAP ({r.SapSum}), no Store Slip");
                    res.Add(new Result
                    {
                        Store = store,
                        CutoffDate = r.CutoffDate,
                        SRCBank = null,
                        Comparer1 = "SAP",
                        Comparer1Amount = r.SapSum,
                        Comparer2 = null,
                        Comparer2Amount = null
                    });
                } else
                {
                    AddTextToDebug($"    > {donly}: Store Slip ({r.SlipSum}), no SAP");
                    res.Add(new Result
                    {
                        Store = store,
                        CutoffDate = r.CutoffDate,
                        SRCBank = null,
                        Comparer1 = "Store Slip",
                        Comparer1Amount = r.SlipSum,
                        Comparer2 = null,
                        Comparer2Amount = null
                    });
                }
            });
            return res;
        }

        private List<TnxBank> GetTnxBank(string filename)
        {
            List<TnxBank> tnxBanks = new List<TnxBank>();
            string path = CurrentDirectory + "\\files\\" + filename;
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
                    ChangeLblProcessDesc($"reading row {running_row - cellsIgnore - 1} from file {filename}.");
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

        private List<SAP> GetSAPs(string filename)
        {
            List<SAP> SAPs = new List<SAP>();
            string path = CurrentDirectory + "\\files\\" + filename;
            Workbook wb = app.Workbooks.Open(path);
            Worksheet wsh = wb.Worksheets[1];
            Microsoft.Office.Interop.Excel.Range cells = wsh.Cells;
            var ccn = CheckColumnName(cells, new List<string> { "Assignment", "DocumentNo", "BusA", "Type", "Doc. Date", "PK", "Amount in local cur.", "LCurr", "Text" });
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
                    ChangeLblProcessDesc($"reading row {running_row - 1} from file {filename}.");
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

        private List<Slip> GetSlips(string filename)
        {
            List<Slip> slips = new List<Slip>();
            string path = CurrentDirectory + "\\files\\" + filename;
            Workbook wb = app.Workbooks.Open(path);
            Worksheet wsh = wb.Worksheets[1];
            Microsoft.Office.Interop.Excel.Range cells = wsh.Cells;
            // Check column Name
            var ccn = CheckColumnName(cells, new List<string> { "Date", "Time", "Amount" });
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
                    ChangeLblProcessDesc($"reading row {running_row - 1} from file {filename}.");
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

        private string CheckColumnName(Microsoft.Office.Interop.Excel.Range cells, List<string> columns)
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

        private void AddTextToDebug(string txt)
        {
            tbDebug.AppendText("\r\n" + txt);
        }

        private void ChangeLblProcessDesc(string message)
        {
            lblProcessDesc.Text = message;
        }

        private void Compare_Shown(object sender, EventArgs e)
        {
            System.Threading.Thread.Sleep(1);
            Starting();
        }

        private void btnSaveDebug_Click(object sender, EventArgs e)
        {
            try
            {
                // create log text file
                using (FileStream fs = File.Create(CurrentDirectory + $"\\results\\log_{runningTime.ToString("yyyyMMdd_HHmm")}.txt"))
                {
                    string str = tbDebug.Text;
                    Byte[] data = new UTF8Encoding(true).GetBytes(str);
                    fs.Write(data, 0, data.Length);
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            MessageBox.Show("Log file is saved in directory results.");
        }
    }
}
