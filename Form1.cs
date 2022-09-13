namespace Pigeon
{
    public partial class Pigeon : Form
    {
        public Pigeon()
        {
            InitializeComponent();
            enabled_btnCheck();
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
            enabled_btnCheck();
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
            enabled_btnCheck();
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
            enabled_btnCheck();
        }

        private void rtbBankPathName_TextChanged(object sender, EventArgs e)
        {
            enabled_btnCheck();
        }

        private void rtbSAPPathName_TextChanged(object sender, EventArgs e)
        {
            enabled_btnCheck();
        }

        private void rtbStoreSlipPathName_TextChanged(object sender, EventArgs e)
        {
            enabled_btnCheck();
        }

        private void enabled_btnCheck()
        {
            if ((rtbBankPathName.Text != "" || rtbSAPPathName.Text != "" || rtbStoreSlipPathName.Text != "") || cbBankSameDir.Checked && cbSAPSameDir.Checked && cbStoreSlipSameDir.Checked)
            {
                btnCheck.Enabled = true;
            }
            else
            {
                btnCheck.Enabled = false;
            }
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
            Checking checking = new Checking(rtbStoreSlipPathName.Text);
            checking.ShowDialog();
        }
    }
}