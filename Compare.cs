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
        Dictionary<string, List<string>> dict = new();
        Microsoft.Office.Interop.Excel.Application app = new();
        public DateTime runningTime = DateTime.MinValue;
        public string CurrentDirectory = Environment.CurrentDirectory;
        public bool HasSAP = false;
        public Compare(Dictionary<string, List<string>> dict_param, bool hs)
        {
            dict = dict_param;
            HasSAP = hs;
            runningTime = DateTime.Now;
            InitializeComponent();
            tbDebug.Enter += (s, e) => { tbDebug.Parent.Focus(); };
            tbDebug.Text = "****** Program is starting " + runningTime.ToString() + " ******";
        }

        private void Starting()
        {
            try
            {
                List<SAP> SAPs = new(); // sap is already seperated by Store ID and InterXBank
                List<TnxBank> allTnxBanks = new();
                List<Result> resAll = new();
                AddTextToDebug("First compare store and bank");
                foreach (KeyValuePair<string, List<string>> entry in dict)
                {
                    AddTextToDebug(" + " + entry.Key);
                    List<TnxBank> tnxBanks = new();
                    if (entry.Value.Count != 1)
                    {
                        AddTextToDebug("  - Getting data from the bank and store files of the store.");
                        List<Slip> slips = new();
                        List<CommonSum> bankSums = new();
                        List<CommonSum> slipSums = new();
                        AddTextToDebug("  -- reading file bank...");
                        tnxBanks = GetTnxBank(entry.Key);
                        AddTextToDebug("  -- reading file store slip...");
                        slips = GetSlips(entry.Key + "_StoreSlip");

                        // Calucate sum group by
                        AddTextToDebug("  - Calculate total from the files");
                        if (tnxBanks.Count != 0)
                        {
                            allTnxBanks.AddRange(tnxBanks);
                            AddTextToDebug("  -- get total of the bank file...");
                            bankSums = tnxBanks.OrderBy(tb => tb.CutoffDate).GroupBy(tb => tb.CutoffDate).Select(i => new CommonSum
                            {
                                CutoffDate = i.Key,
                                Total = i.Sum(x => x.PaymentAmount)
                            }).ToList();
                        }
                        if (slips.Count != 0)
                        {
                            AddTextToDebug("  -- get total of the slip file...");
                            slipSums = slips.OrderBy(s => s.CutoffDate).GroupBy(s => s.CutoffDate).Select(i => new CommonSum
                            {
                                CutoffDate = i.Key,
                                Total = i.Sum(x => x.Amount)
                            }).ToList();
                        }

                        // start comparing
                        AddTextToDebug("  - Compare Bank and Store Slip (Bank - Slip)");
                        resAll.AddRange(CompareBankStoreSlip(bankSums, slipSums, entry.Key));
                    } else if (entry.Value.Contains("Bank") && HasSAP)
                    {
                        AddTextToDebug($"  - no store slip file.");
                        AddTextToDebug($"  - reading file bank to compare with SAP...");
                        tnxBanks = GetTnxBank(entry.Key);
                        allTnxBanks.AddRange(tnxBanks);
                    } else if (entry.Value.Contains("Bank"))
                    {
                        AddTextToDebug($"  - has bank file, but store slip file.");
                    }
                    else if (entry.Value.Contains("StoreSlip"))
                    {
                        AddTextToDebug($"  - has store slip file, but no bank file.");
                    }
                }
                AddTextToDebug("Compare bank and SAP by store, cutoff date, interaction bank");
                if (allTnxBanks.Count != 0 && HasSAP)
                {
                    AddTextToDebug("  - get data from SAP file");
                    SAPs = GetSAPs("SAP");
                    AddTextToDebug("  - calculate sum of transaction bank by store, cutoff date, interaction bank");
                    List<SumByInterX> sumTnxBanksByStore = allTnxBanks.GroupBy(tb => new { tb.Store, tb.CutoffDate, tb.InterXBank }).Select(i => new SumByInterX
                    {
                        Store = i.Key.Store,
                        CutoffDate = i.Key.CutoffDate,
                        InterXBank = i.Key.InterXBank,
                        Total = i.Sum(x => x.PaymentAmount)
                    }).ToList();
                    resAll.AddRange(CompareBankSAPByStore(sumTnxBanksByStore, SAPs));
                } else if (HasSAP)
                {
                    AddTextToDebug("  - there is file SAP, no file bank.");
                }
                if (resAll.Count != 0)
                {
                    CreateExcelResult(resAll.OrderBy(r => r.CutoffDate).ThenBy(r => r.Comparer1).ToList());
                }
                btnSaveDebug.Enabled = true;
                lblProcessDesc.Text = "...";
                AddTextToDebug($"****** Program is finishing {DateTime.Now} ******");
            }
            catch (Exception exc)
            {
                if (MessageBox.Show(exc.Message, "Error occurs.", MessageBoxButtons.OK) == DialogResult.OK)
                {
                    createLogFile("debug");
                    Close();
                }
            }
        }

        private void CreateExcelResult(List<Result> res)
        {
            Workbook wb = null;
            object misValue = System.Reflection.Missing.Value;
            wb = app.Workbooks.Add(misValue);
            AddTextToDebug("  - creating file info sheet");
            Worksheet fileInfo = wb.ActiveSheet as Worksheet;
            fileInfo.Name = "File Info";
            fileInfo.Cells[1, 1] = "File SAP";
            fileInfo.Cells[1, 2] = HasSAP ? "Has" : "No Has";
            fileInfo.Cells[3, 1] = "Store";
            fileInfo.Cells[3, 2] = "Bank File";
            fileInfo.Cells[3, 3] = "Store Slip File";
            foreach (var x in dict.Select((Entry, Index) => new { Entry, Index }))
            {
                fileInfo.Cells[x.Index + 4, 1] = x.Entry.Key;
                fileInfo.Cells[x.Index + 4, 2] = x.Entry.Value.Contains("Bank") ? "Has" : "No Has";
                fileInfo.Cells[x.Index + 4, 3] = x.Entry.Value.Contains("StoreSlip") ? "Has" : "No Has";
            }
            fileInfo.Cells.HorizontalAlignment = HorizontalAlignment.Center;
            fileInfo.Columns.AutoFit();
            fileInfo.Rows.AutoFit();

            List<Result> noComparerRes = (from r in res
                                          where r.Comparer2 == null 
                                          orderby r.Store, r.CutoffDate, r.Comparer1
                                          select r).ToList();
            AddTextToDebug("  - creating no commparer sheet");
            Worksheet noComp = wb.Sheets.Add(misValue, misValue, 1, misValue) as Worksheet;
            noComp.Name = "NoComparer";
            noComp.Cells[1, 1] = "Store";
            noComp.Cells[1, 2] = "Cutoff Date";
            noComp.Cells[1, 3] = "File Type";
            noComp.Cells[1, 4] = "Note";
            for (int i = 0; i < noComparerRes.Count; i++)
            {
                noComp.Cells[i + 2, 1] = noComparerRes[i].Store;
                noComp.Cells[i + 2, 2] = noComparerRes[i].CutoffDate.ToString("dd-MMM-yyyy");
                noComp.Cells[i + 2, 3] = noComparerRes[i].Comparer1;
                if (noComparerRes[i].Comparer1 == "Bank")
                {
                    noComp.Cells[i + 2, 4] = noComparerRes[i].SRCBank == null ? "No StoreSlip" : $"No SAP ({noComparerRes[i].SRCBank})";
                } else if (noComparerRes[i].Comparer1 == "SAP")
                {
                    noComp.Cells[i + 2, 4] = noComparerRes[i].SRCBank == null ? $"No Bank ({noComparerRes[i].SRCBank})" : "No Bank";
                } else
                {
                    noComp.Cells[i + 2, 4] = "No Bank ";
                }
                noComp.Cells[i + 2, 4].Font.Color = ColorTranslator.ToOle(Color.Red);
            }
            noComp.Cells.HorizontalAlignment = HorizontalAlignment.Center;
            noComp.Columns.AutoFit();
            noComp.Rows.AutoFit();
            Dictionary<string, List<Result>> groupped = res.OrderBy(r => r.Store).GroupBy(r => r.Store).ToDictionary(g => g.Key, g => g.ToList());
            foreach (KeyValuePair<string, List<Result>> g in groupped)
            {
                AddTextToDebug($"  - creating result {g.Key} sheet");
                Worksheet awsh = wb.Sheets.Add(misValue, misValue, 1, misValue) as Worksheet;
                awsh.Name = g.Key;
                awsh.Cells[1, 1] = "Date";
                awsh.Cells[1, 2] = "SRC Bank";
                awsh.Cells[1, 3] = "Comparer 1 (Amount)";
                awsh.Cells[1, 4] = "Comparer 2 (Amount)";
                awsh.Cells[1, 5] = "Diff.";
                for (int i = 0; i < g.Value.Count; i++)
                {
                    awsh.Cells[i + 2, 1] = g.Value[i].CutoffDate.ToString("dd-MMM-yyyy");
                    awsh.Cells[i + 2, 3] = $"{g.Value[i].Comparer1} ({g.Value[i].Comparer1Amount})";
                    if (g.Value[i].Comparer1 == "Bank")
                    {
                        if (g.Value[i].SRCBank != null) // bank compare with SAP
                        {
                            awsh.Cells[i + 2, 2] = g.Value[i].SRCBank == "InnerBank" ? "ACLEDA Bank Plc. (TC)" : "Other Bank (KHQR)";
                            if (g.Value[i].Comparer2 == null)
                            {
                                awsh.Cells[i + 2, 4] = "No SAP";
                                awsh.Cells[i + 2, 4].Font.Color = ColorTranslator.ToOle(Color.Red);
                                awsh.Cells[i + 2, 5] = "N/A";
                                awsh.Cells[i + 2, 5].Font.Color = ColorTranslator.ToOle(Color.Red);
                            } else
                            {
                                awsh.Cells[i + 2, 4] = $"{g.Value[i].Comparer2} ({g.Value[i].Comparer2Amount})";
                                awsh.Cells[i + 2, 5].FormulaR1C1 = $"={g.Value[i].Comparer1Amount}-{g.Value[i].Comparer2Amount}";
                            }
                        } else // bank Compare with Store Slip
                        {
                            awsh.Cells[i + 2, 2] = "All Bank";
                            if (g.Value[i].Comparer2 == null)
                            {
                                awsh.Cells[i + 2, 4] = "No StoreSlip";
                                awsh.Cells[i + 2, 4].Font.Color = ColorTranslator.ToOle(Color.Red);
                                awsh.Cells[i + 2, 5] = "N/A";
                                awsh.Cells[i + 2, 5].Font.Color = ColorTranslator.ToOle(Color.Red);
                            }
                            else
                            {
                                awsh.Cells[i + 2, 4] = $"{g.Value[i].Comparer2} ({g.Value[i].Comparer2Amount})";
                                awsh.Cells[i + 2, 5].FormulaR1C1 = $"={g.Value[i].Comparer1Amount}-{g.Value[i].Comparer2Amount}";
                            }
                        }
                    } else
                    {
                        if (g.Value[i].SRCBank != null) // SAP with Bank
                        {
                            awsh.Cells[i + 2, 2] = g.Value[i].SRCBank == "InnerBank" ? "ACLEDA Bank Plc. (TC)" : "Other Bank (KHQR)";
                        } else
                        {
                            awsh.Cells[i + 2, 2] = "All Bank";
                        }
                        awsh.Cells[i + 2, 4] = "No Bank";
                        awsh.Cells[i + 2, 4].Font.Color = ColorTranslator.ToOle(Color.Red);
                        awsh.Cells[i + 2, 5] = "N/A";
                        awsh.Cells[i + 2, 5].Font.Color = ColorTranslator.ToOle(Color.Red);
                    }
                }
                awsh.Cells.HorizontalAlignment = HorizontalAlignment.Center;
                awsh.Columns.AutoFit();
                awsh.Rows.AutoFit();
            }
            wb.SaveAs(CurrentDirectory + $"\\results\\result_{runningTime:yyyyMMdd_HHmm}.xlsx", XlFileFormat.xlWorkbookDefault, misValue, misValue, misValue, misValue, XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            wb.Close(true, misValue, misValue);
            app.Quit();
        }

        private List<Result> CompareBankSAPByStore(List<SumByInterX> sbixs, List<SAP> saps)
        {
            List<Result> res = new();
            var res1 = from sbix in sbixs
                       join sap in saps
                       on new { X = sbix.CutoffDate, Y = sbix.InterXBank, Z = sbix.Store } equals new { X = sap.DocDate, Y = sap.InterXBank, Z = sap.Store }
                       into JoinedList
                       from sap in JoinedList.DefaultIfEmpty()
                       select new
                       {
                           CutoffDate = sbix.CutoffDate,
                           InterXBank = sbix.InterXBank,
                           BankSum = sbix?.Total,
                           Sap = sap?.AmountInLocalCur,
                           Store = sbix.Store
                       };
            var res2 = from sap in saps
                       join sbix in sbixs
                       on new { X = sap.DocDate, Y = sap.InterXBank, Z = sap.Store } equals new { X = sbix.CutoffDate, Y = sbix.InterXBank, Z = sbix.Store }
                       into JoinedList
                       from sbix in JoinedList.DefaultIfEmpty()
                       select new
                       {
                           CutoffDate = sap.DocDate,
                           InterXBank = sap.InterXBank,
                           BankSum = sbix?.Total,
                           Sap = sap?.AmountInLocalCur,
                           Store = sap.Store
                       };
            var res3 = res1.Union(res2).OrderBy(r => r.Store).ThenBy(r => r.CutoffDate).ThenBy(r => r.InterXBank).ToList();
            res3.ForEach(r =>
            {
                var donly = r.CutoffDate.ToString("dd-MMM-yyyy");
                if (r.BankSum != null && r.Sap != null)
                {
                    AddTextToDebug($"    > {r.Store} {donly} {r.InterXBank}: Bank - SAP = {r.BankSum} - {r.Sap} = {r.BankSum - r.Sap} ");
                    res.Add(new Result
                    {
                        CutoffDate = r.CutoffDate,
                        SRCBank = r.InterXBank,
                        Comparer1 = "Bank",
                        Comparer2 = "SAP",
                        Comparer1Amount = r.BankSum,
                        Comparer2Amount = r.Sap,
                        Store = r.Store
                    });
                }
                else if (r.BankSum != null)
                {
                    AddTextToDebug($"    > {r.Store} {donly} {r.InterXBank}: Bank ({r.BankSum}), no SAP");
                    res.Add(new Result
                    {
                        CutoffDate = r.CutoffDate,
                        SRCBank = r.InterXBank,
                        Comparer1 = "Bank",
                        Comparer2 = null,
                        Comparer1Amount = r.BankSum,
                        Store = r.Store
                    });
                }
                else
                {
                    AddTextToDebug($"    > {r.Store} {donly} {r.InterXBank}: SAP ({r.Sap}), no Bank");
                    res.Add(new Result
                    {
                        CutoffDate = r.CutoffDate,
                        SRCBank = r.InterXBank,
                        Comparer1 = "SAP",
                        Comparer2 = null,
                        Comparer1Amount = r.Sap,
                        Store = r.Store
                    });
                }
            });
            return res;
        }

        private List<Result> CompareBankStoreSlip(List<CommonSum> bankSums, List<CommonSum> slipSums, string store)
        {
            List<Result> res = new();
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
                        Comparer1 = "StoreSlip",
                        Comparer1Amount = r.SlipSum,
                        Comparer2Amount = null
                    });
                }
            });
            return res;
        }

        private List<TnxBank> GetTnxBank(string filename)
        {
            List<TnxBank> tnxBanks = new();
            string path = CurrentDirectory + "\\files\\" + filename + "_Bank";
            Workbook wb = app.Workbooks.Open(path);
            Worksheet wsh = wb.Worksheets[1];
            Microsoft.Office.Interop.Excel.Range cells = wsh.Cells;
            int running_row = 2;
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
                    tnxBank.Store = filename;
                    tnxBanks.Add(tnxBank);
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
            return tnxBanks;

        }

        private List<SAP> GetSAPs(string filename)
        {
            List<SAP> SAPs = new();
            string path = CurrentDirectory + "\\files\\" + filename;
            Workbook wb = app.Workbooks.Open(path);
            Worksheet wsh = wb.Worksheets[1];
            Microsoft.Office.Interop.Excel.Range cells = wsh.Cells;
            var ccn = CheckColumnName(cells, new List<string> { "Assignment", "DocumentNo", "BusA", "Type", "Doc. Date", "PK", "Amount in local cur.", "LCurr", "Text" });
            if ( ccn != "")
            {
                wb.Close(0);
                app.Quit();
                throw new Exception(filename + " column name is incorrect (" + ccn +").");
            }
            int running_row = 2;
            try
            {
                while (cells[running_row, 1].value != null)
                {
                    if (cells[running_row, 9].Text.Contains("(KHQR") || cells[running_row, 9].Text.Contains("(TC"))
                    {
                        SAP sap = new();
                        sap.Assignment = (string)cells[running_row, 1].Text;
                        sap.DocumentNo = cells[running_row, 2].Text;
                        sap.BusA = cells[running_row, 3].Text;
                        sap.Type = cells[running_row, 4].Text;
                        var txt = cells[running_row, 5].Text.Split(".");
                        DateTime dt = new (Int32.Parse(txt[2]), Int32.Parse(txt[1]), Int32.Parse(txt[0]));
                        sap.DocDate = DateOnly.FromDateTime(dt);
                        sap.PK = cells[running_row, 6].Text;
                        var amount = cells[running_row, 7].Value;
                        sap.AmountInLocalCur = (decimal)Math.Abs(amount);
                        sap.LCurr = cells[running_row, 8].Text;
                        string text = cells[running_row, 9].Text;
                        sap.Text = text;
                        sap.InterXBank = text.Contains("KHQR") ? "OtherBank" : "InnerBank";
                        sap.Store = "B" + text[^5..];
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
            List<Slip> slips = new();
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
                throw new Exception(filename + " column name is incorrect (" + ccn + ").");
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
            var wrong_col = "";
            for (int i = 1; i < columns.Count + 1; i++)
            {
                if (cells[1, i].text != columns[i - 1])
                {
                    wrong_col = columns[i - 1];
                    break;
                }
            }
            return wrong_col;
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
                createLogFile("log");
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            if (MessageBox.Show("Log file is saved in directory results.",  "Saving Log" , MessageBoxButtons.OK) == DialogResult.OK)
            {
                btnSaveDebug.Enabled = false;
            }
        }

        private void createLogFile(string name)
        {
            // create log text file
            using (FileStream fs = File.Create(CurrentDirectory + $"\\results\\{name}_{runningTime:yyyyMMdd_HHmm}.txt"))
            {
                string str = tbDebug.Text;
                Byte[] data = new UTF8Encoding(true).GetBytes(str);
                fs.Write(data, 0, data.Length);
            }
        }
    }
}
