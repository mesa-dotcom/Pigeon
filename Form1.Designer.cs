namespace Pigeon
{
    partial class Pigeon
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Pigeon));
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.gbBank = new System.Windows.Forms.GroupBox();
            this.rtbBankPathName = new System.Windows.Forms.RichTextBox();
            this.btnBankBrowse = new System.Windows.Forms.Button();
            this.cbBankSameDir = new System.Windows.Forms.CheckBox();
            this.gbSAP = new System.Windows.Forms.GroupBox();
            this.rtbSAPPathName = new System.Windows.Forms.RichTextBox();
            this.btnSAPBrowse = new System.Windows.Forms.Button();
            this.cbSAPSameDir = new System.Windows.Forms.CheckBox();
            this.gbStoreSlip = new System.Windows.Forms.GroupBox();
            this.rtbStoreSlipPathName = new System.Windows.Forms.RichTextBox();
            this.btnStoreSlipBrowse = new System.Windows.Forms.Button();
            this.cbStoreSlipSameDir = new System.Windows.Forms.CheckBox();
            this.btnCheck = new System.Windows.Forms.Button();
            this.lblMain = new System.Windows.Forms.Label();
            this.lblDesc = new System.Windows.Forms.Label();
            this.lblCredit = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            this.gbBank.SuspendLayout();
            this.gbSAP.SuspendLayout();
            this.gbStoreSlip.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbLogo
            // 
            this.pbLogo.Image = global::Pigeon.Properties.Resources.carrier_pigeon;
            this.pbLogo.Location = new System.Drawing.Point(16, 13);
            this.pbLogo.Margin = new System.Windows.Forms.Padding(4);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(112, 112);
            this.pbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbLogo.TabIndex = 0;
            this.pbLogo.TabStop = false;
            // 
            // gbBank
            // 
            this.gbBank.Controls.Add(this.rtbBankPathName);
            this.gbBank.Controls.Add(this.btnBankBrowse);
            this.gbBank.Controls.Add(this.cbBankSameDir);
            this.gbBank.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.gbBank.Location = new System.Drawing.Point(16, 132);
            this.gbBank.Margin = new System.Windows.Forms.Padding(4);
            this.gbBank.Name = "gbBank";
            this.gbBank.Padding = new System.Windows.Forms.Padding(4);
            this.gbBank.Size = new System.Drawing.Size(630, 106);
            this.gbBank.TabIndex = 1;
            this.gbBank.TabStop = false;
            this.gbBank.Text = "Bank Excel";
            // 
            // rtbBankPathName
            // 
            this.rtbBankPathName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.rtbBankPathName.Enabled = false;
            this.rtbBankPathName.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.rtbBankPathName.Location = new System.Drawing.Point(8, 62);
            this.rtbBankPathName.Margin = new System.Windows.Forms.Padding(4);
            this.rtbBankPathName.Name = "rtbBankPathName";
            this.rtbBankPathName.Size = new System.Drawing.Size(480, 30);
            this.rtbBankPathName.TabIndex = 4;
            this.rtbBankPathName.Text = "";
            this.rtbBankPathName.TextChanged += new System.EventHandler(this.rtbBankPathName_TextChanged);
            // 
            // btnBankBrowse
            // 
            this.btnBankBrowse.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnBankBrowse.Location = new System.Drawing.Point(513, 62);
            this.btnBankBrowse.Margin = new System.Windows.Forms.Padding(4);
            this.btnBankBrowse.Name = "btnBankBrowse";
            this.btnBankBrowse.Size = new System.Drawing.Size(97, 30);
            this.btnBankBrowse.TabIndex = 3;
            this.btnBankBrowse.Text = "Browse";
            this.btnBankBrowse.UseVisualStyleBackColor = true;
            this.btnBankBrowse.Click += new System.EventHandler(this.btnBankBrowse_Click);
            // 
            // cbBankSameDir
            // 
            this.cbBankSameDir.AutoSize = true;
            this.cbBankSameDir.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cbBankSameDir.Location = new System.Drawing.Point(8, 29);
            this.cbBankSameDir.Margin = new System.Windows.Forms.Padding(4);
            this.cbBankSameDir.Name = "cbBankSameDir";
            this.cbBankSameDir.Size = new System.Drawing.Size(262, 23);
            this.cbBankSameDir.TabIndex = 2;
            this.cbBankSameDir.Text = "Same Directory (\\files\\Bank.xlsx)";
            this.cbBankSameDir.UseVisualStyleBackColor = true;
            this.cbBankSameDir.CheckedChanged += new System.EventHandler(this.cbBankSameDir_CheckedChanged);
            // 
            // gbSAP
            // 
            this.gbSAP.Controls.Add(this.rtbSAPPathName);
            this.gbSAP.Controls.Add(this.btnSAPBrowse);
            this.gbSAP.Controls.Add(this.cbSAPSameDir);
            this.gbSAP.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.gbSAP.Location = new System.Drawing.Point(16, 244);
            this.gbSAP.Margin = new System.Windows.Forms.Padding(4);
            this.gbSAP.Name = "gbSAP";
            this.gbSAP.Padding = new System.Windows.Forms.Padding(4);
            this.gbSAP.Size = new System.Drawing.Size(630, 106);
            this.gbSAP.TabIndex = 5;
            this.gbSAP.TabStop = false;
            this.gbSAP.Text = "SAP Excel";
            // 
            // rtbSAPPathName
            // 
            this.rtbSAPPathName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.rtbSAPPathName.Enabled = false;
            this.rtbSAPPathName.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.rtbSAPPathName.Location = new System.Drawing.Point(8, 62);
            this.rtbSAPPathName.Margin = new System.Windows.Forms.Padding(4);
            this.rtbSAPPathName.Name = "rtbSAPPathName";
            this.rtbSAPPathName.Size = new System.Drawing.Size(480, 30);
            this.rtbSAPPathName.TabIndex = 4;
            this.rtbSAPPathName.Text = "";
            this.rtbSAPPathName.TextChanged += new System.EventHandler(this.rtbSAPPathName_TextChanged);
            // 
            // btnSAPBrowse
            // 
            this.btnSAPBrowse.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnSAPBrowse.Location = new System.Drawing.Point(513, 62);
            this.btnSAPBrowse.Margin = new System.Windows.Forms.Padding(4);
            this.btnSAPBrowse.Name = "btnSAPBrowse";
            this.btnSAPBrowse.Size = new System.Drawing.Size(97, 30);
            this.btnSAPBrowse.TabIndex = 3;
            this.btnSAPBrowse.Text = "Browse";
            this.btnSAPBrowse.UseVisualStyleBackColor = true;
            this.btnSAPBrowse.Click += new System.EventHandler(this.btnSAPBrowse_Click);
            // 
            // cbSAPSameDir
            // 
            this.cbSAPSameDir.AutoSize = true;
            this.cbSAPSameDir.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cbSAPSameDir.Location = new System.Drawing.Point(8, 29);
            this.cbSAPSameDir.Margin = new System.Windows.Forms.Padding(4);
            this.cbSAPSameDir.Name = "cbSAPSameDir";
            this.cbSAPSameDir.Size = new System.Drawing.Size(256, 23);
            this.cbSAPSameDir.TabIndex = 2;
            this.cbSAPSameDir.Text = "Same Directory (\\files\\SAP.xlsx)";
            this.cbSAPSameDir.UseVisualStyleBackColor = true;
            this.cbSAPSameDir.CheckedChanged += new System.EventHandler(this.cbSAPSameDir_CheckedChanged);
            // 
            // gbStoreSlip
            // 
            this.gbStoreSlip.Controls.Add(this.rtbStoreSlipPathName);
            this.gbStoreSlip.Controls.Add(this.btnStoreSlipBrowse);
            this.gbStoreSlip.Controls.Add(this.cbStoreSlipSameDir);
            this.gbStoreSlip.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.gbStoreSlip.Location = new System.Drawing.Point(16, 356);
            this.gbStoreSlip.Margin = new System.Windows.Forms.Padding(4);
            this.gbStoreSlip.Name = "gbStoreSlip";
            this.gbStoreSlip.Padding = new System.Windows.Forms.Padding(4);
            this.gbStoreSlip.Size = new System.Drawing.Size(630, 106);
            this.gbStoreSlip.TabIndex = 6;
            this.gbStoreSlip.TabStop = false;
            this.gbStoreSlip.Text = "Store Slip Excel";
            // 
            // rtbStoreSlipPathName
            // 
            this.rtbStoreSlipPathName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.rtbStoreSlipPathName.Enabled = false;
            this.rtbStoreSlipPathName.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.rtbStoreSlipPathName.Location = new System.Drawing.Point(8, 62);
            this.rtbStoreSlipPathName.Margin = new System.Windows.Forms.Padding(4);
            this.rtbStoreSlipPathName.Name = "rtbStoreSlipPathName";
            this.rtbStoreSlipPathName.Size = new System.Drawing.Size(480, 30);
            this.rtbStoreSlipPathName.TabIndex = 4;
            this.rtbStoreSlipPathName.Text = "";
            this.rtbStoreSlipPathName.TextChanged += new System.EventHandler(this.rtbStoreSlipPathName_TextChanged);
            // 
            // btnStoreSlipBrowse
            // 
            this.btnStoreSlipBrowse.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnStoreSlipBrowse.Location = new System.Drawing.Point(513, 62);
            this.btnStoreSlipBrowse.Margin = new System.Windows.Forms.Padding(4);
            this.btnStoreSlipBrowse.Name = "btnStoreSlipBrowse";
            this.btnStoreSlipBrowse.Size = new System.Drawing.Size(97, 30);
            this.btnStoreSlipBrowse.TabIndex = 3;
            this.btnStoreSlipBrowse.Text = "Browse";
            this.btnStoreSlipBrowse.UseVisualStyleBackColor = true;
            this.btnStoreSlipBrowse.Click += new System.EventHandler(this.btnStoreSlipBrowse_Click);
            // 
            // cbStoreSlipSameDir
            // 
            this.cbStoreSlipSameDir.AutoSize = true;
            this.cbStoreSlipSameDir.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cbStoreSlipSameDir.Location = new System.Drawing.Point(8, 29);
            this.cbStoreSlipSameDir.Margin = new System.Windows.Forms.Padding(4);
            this.cbStoreSlipSameDir.Name = "cbStoreSlipSameDir";
            this.cbStoreSlipSameDir.Size = new System.Drawing.Size(292, 23);
            this.cbStoreSlipSameDir.TabIndex = 2;
            this.cbStoreSlipSameDir.Text = "Same Directory (\\files\\StoreSlip.xlsx)";
            this.cbStoreSlipSameDir.UseVisualStyleBackColor = true;
            this.cbStoreSlipSameDir.CheckedChanged += new System.EventHandler(this.cbStoreSlipSameDir_CheckedChanged);
            // 
            // btnCheck
            // 
            this.btnCheck.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnCheck.Location = new System.Drawing.Point(239, 469);
            this.btnCheck.Margin = new System.Windows.Forms.Padding(4);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(129, 42);
            this.btnCheck.TabIndex = 7;
            this.btnCheck.Text = "Check";
            this.btnCheck.UseVisualStyleBackColor = true;
            this.btnCheck.Click += new System.EventHandler(this.btnCheck_Click);
            // 
            // lblMain
            // 
            this.lblMain.AutoSize = true;
            this.lblMain.Font = new System.Drawing.Font("Times New Roman", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblMain.Location = new System.Drawing.Point(136, 36);
            this.lblMain.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMain.Name = "lblMain";
            this.lblMain.Size = new System.Drawing.Size(96, 32);
            this.lblMain.TabIndex = 8;
            this.lblMain.Text = "Pigeon";
            // 
            // lblDesc
            // 
            this.lblDesc.AutoSize = true;
            this.lblDesc.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblDesc.Location = new System.Drawing.Point(136, 77);
            this.lblDesc.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(368, 19);
            this.lblDesc.TabIndex = 9;
            this.lblDesc.Text = "Pigeon, the sign of fortune, luck, and transformation.";
            // 
            // lblCredit
            // 
            this.lblCredit.AutoSize = true;
            this.lblCredit.Font = new System.Drawing.Font("Times New Roman", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblCredit.Location = new System.Drawing.Point(526, 490);
            this.lblCredit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCredit.Name = "lblCredit";
            this.lblCredit.Size = new System.Drawing.Size(128, 15);
            this.lblCredit.TabIndex = 10;
            this.lblCredit.Text = "2022, Mesa IT Support";
            // 
            // Pigeon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(664, 518);
            this.Controls.Add(this.lblCredit);
            this.Controls.Add(this.lblDesc);
            this.Controls.Add(this.lblMain);
            this.Controls.Add(this.btnCheck);
            this.Controls.Add(this.gbStoreSlip);
            this.Controls.Add(this.gbSAP);
            this.Controls.Add(this.gbBank);
            this.Controls.Add(this.pbLogo);
            this.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "Pigeon";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pigeon";
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            this.gbBank.ResumeLayout(false);
            this.gbBank.PerformLayout();
            this.gbSAP.ResumeLayout(false);
            this.gbSAP.PerformLayout();
            this.gbStoreSlip.ResumeLayout(false);
            this.gbStoreSlip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PictureBox pbLogo;
        private GroupBox gbBank;
        private CheckBox cbBankSameDir;
        private Button btnBankBrowse;
        private RichTextBox rtbBankPathName;
        private GroupBox gbSAP;
        private RichTextBox rtbSAPPathName;
        private Button btnSAPBrowse;
        private CheckBox cbSAPSameDir;
        private GroupBox gbStoreSlip;
        private RichTextBox rtbStoreSlipPathName;
        private Button btnStoreSlipBrowse;
        private CheckBox cbStoreSlipSameDir;
        private Button btnCheck;
        private Label lblMain;
        private Label lblDesc;
        private Label lblCredit;
    }
}