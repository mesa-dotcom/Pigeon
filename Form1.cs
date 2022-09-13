namespace Pigeon
{
    public partial class Pigeon : Form
    {
        public Pigeon()
        {
            InitializeComponent();
        }

        private void btnBankBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select Bank file";
            ofd.InitialDirectory = @"C:\";
            ofd.Filter = "Excel Files| *.xls; *.xlsx; *.xlsm";
            ofd.FilterIndex = 1;
            ofd.ShowDialog();
            if (ofd.FileName != "")
            {
                rtbBankPathName.Text = ofd.FileName;
            }
        }

        private void cbBankSameDir_CheckedChanged(object sender, EventArgs e)
        {
            if (cbBankSameDir.Checked)
            {
                rtbBankPathName.Text = "";
                btnBankBrowse.Enabled = false;
            }
            else
            {
                btnBankBrowse.Enabled = true;
            }
        }

        private void btnSAPBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select SAP file";
            ofd.InitialDirectory = @"C:\";
            ofd.Filter = "Excel Files| *.xls; *.xlsx; *.xlsm";
            ofd.FilterIndex = 1;
            ofd.ShowDialog();
            if (ofd.FileName != "")
            {
                rtbSAPPathName.Text = ofd.FileName;
            }
        }

        private void btnStoreSlipBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select Store Slip file";
            ofd.InitialDirectory = @"C:\";
            ofd.Filter = "Excel Files| *.xls; *.xlsx; *.xlsm";
            ofd.FilterIndex = 1;
            ofd.ShowDialog();
            if (ofd.FileName != "")
            {
                rtbStoreSlipPathName.Text = ofd.FileName;
            }
        }

        private void cbSAPSameDir_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSAPSameDir.Checked)
            {
                rtbSAPPathName.Text = "";
                btnSAPBrowse.Enabled = false;
            }
            else
            {
                btnSAPBrowse.Enabled = true;
            }
        }

        private void cbStoreSlipSameDir_CheckedChanged(object sender, EventArgs e)
        {
            if (cbStoreSlipSameDir.Checked)
            {
                rtbStoreSlipPathName.Text = "";
                btnStoreSlipBrowse.Enabled = false;
            }
            else
            {
                btnStoreSlipBrowse.Enabled = true;
            }
        }
    }
}