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
            this.lblMain = new System.Windows.Forms.Label();
            this.lblDesc = new System.Windows.Forms.Label();
            this.lblCredit = new System.Windows.Forms.Label();
            this.gbCondition = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCheckFile = new System.Windows.Forms.Button();
            this.btnCompare = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            this.gbCondition.SuspendLayout();
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
            // lblMain
            // 
            this.lblMain.AutoSize = true;
            this.lblMain.Font = new System.Drawing.Font("Times New Roman", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblMain.Location = new System.Drawing.Point(136, 13);
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
            this.lblDesc.Location = new System.Drawing.Point(136, 55);
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
            this.lblCredit.Location = new System.Drawing.Point(524, 340);
            this.lblCredit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCredit.Name = "lblCredit";
            this.lblCredit.Size = new System.Drawing.Size(128, 15);
            this.lblCredit.TabIndex = 10;
            this.lblCredit.Text = "2022, Mesa IT Support";
            // 
            // gbCondition
            // 
            this.gbCondition.Controls.Add(this.label3);
            this.gbCondition.Controls.Add(this.label2);
            this.gbCondition.Controls.Add(this.label1);
            this.gbCondition.Location = new System.Drawing.Point(16, 143);
            this.gbCondition.Name = "gbCondition";
            this.gbCondition.Size = new System.Drawing.Size(636, 134);
            this.gbCondition.TabIndex = 11;
            this.gbCondition.TabStop = false;
            this.gbCondition.Text = "Place Files Path";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(6, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(303, 19);
            this.label3.TabIndex = 2;
            this.label3.Text = "Store Slip: \\files\\BXXXXX_StoreSlip.xlsx";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(6, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(229, 19);
            this.label2.TabIndex = 1;
            this.label2.Text = "SAP: \\files\\BXXXXX_SAP.xlsx";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(6, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(239, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "Bank: \\files\\BXXXXX_Bank.xlsx";
            // 
            // btnCheckFile
            // 
            this.btnCheckFile.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnCheckFile.Location = new System.Drawing.Point(16, 294);
            this.btnCheckFile.Name = "btnCheckFile";
            this.btnCheckFile.Size = new System.Drawing.Size(143, 42);
            this.btnCheckFile.TabIndex = 12;
            this.btnCheckFile.Text = "Check Files";
            this.btnCheckFile.UseVisualStyleBackColor = true;
            this.btnCheckFile.Click += new System.EventHandler(this.btnCheckFile_Click);
            // 
            // btnCompare
            // 
            this.btnCompare.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnCompare.Location = new System.Drawing.Point(509, 294);
            this.btnCompare.Name = "btnCompare";
            this.btnCompare.Size = new System.Drawing.Size(143, 42);
            this.btnCompare.TabIndex = 13;
            this.btnCompare.Text = "Compare";
            this.btnCompare.UseVisualStyleBackColor = true;
            this.btnCompare.Click += new System.EventHandler(this.btnCompare_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label4.Location = new System.Drawing.Point(136, 85);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(322, 19);
            this.label4.TabIndex = 14;
            this.label4.Text = "This application is used to check QR payment.";
            // 
            // Pigeon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(664, 364);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnCompare);
            this.Controls.Add(this.btnCheckFile);
            this.Controls.Add(this.gbCondition);
            this.Controls.Add(this.lblCredit);
            this.Controls.Add(this.lblDesc);
            this.Controls.Add(this.lblMain);
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
            this.gbCondition.ResumeLayout(false);
            this.gbCondition.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PictureBox pbLogo;
        private Label lblMain;
        private Label lblDesc;
        private Label lblCredit;
        private GroupBox gbCondition;
        private Label label3;
        private Label label2;
        private Label label1;
        private Button btnCheckFile;
        private Button btnCompare;
        private Label label4;
    }
}