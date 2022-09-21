using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pigeon
{
    public partial class CheckFiles : Form
    {
        Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();
        public CheckFiles(Dictionary<string, List<string>> dict_param, bool hasSAP)
        {
            dict = dict_param;
            InitializeComponent();
            checkFileSAP(hasSAP);
            FillingDatatable();
        }

        private void checkFileSAP(bool hasSAP)
        {
            if (hasSAP)
            {
                cbSAPFile.Checked = true;
            } else
            {
                cbSAPFile.Checked = false;
            }
        }

        private void FillingDatatable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("No");
            dt.Columns.Add("Store");
            dt.Columns.Add("Bank");
            dt.Columns.Add("Store Slip");
            var i = 1;
            foreach (KeyValuePair<string, List<string>> entry in dict)
            {
                List<string> files = entry.Value;
                DataRow dr = dt.NewRow();
                dr["No"] = i;
                dr["Store"] = entry.Key;
                dr["Bank"] = files.Contains("Bank") ? "/" : "-";
                dr["Store Slip"] = files.Contains("StoreSlip") ? "/" : "-";
                dt.Rows.Add(dr);
                i++;
            }
            dtgvFilesList.DataSource = dt;
            dtgvFilesList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
        }
    }
}
