using Pigeon.Classes;
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

namespace Pigeon
{
    public partial class Checking : Form
    {
        string storeSlipExcelPath = "";
        public Checking(string ssep)
        {
            storeSlipExcelPath = ssep;
            InitializeComponent();
            readStoreSlipExcel();
        }

        private void readStoreSlipExcel()
        {
            Microsoft.Office.Interop.Excel.Application application = new Microsoft.Office.Interop.Excel.Application();
            Workbook wb = application.Workbooks.Open(storeSlipExcelPath);
            Dictionary<string, Worksheet> dict = new Dictionary<string, Worksheet>();
            foreach (Worksheet worksheet in wb.Worksheets)
            {
                string store = worksheet.Name;
                foreach (var row in worksheet.Rows)
                {

                }
            }
        }
    }
}
