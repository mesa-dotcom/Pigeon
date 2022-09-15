namespace Pigeon
{
    partial class Compare
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Compare));
            this.tbDebug = new System.Windows.Forms.TextBox();
            this.lblProcess = new System.Windows.Forms.Label();
            this.lblProcessDesc = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tbDebug
            // 
            this.tbDebug.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tbDebug.ForeColor = System.Drawing.SystemColors.GrayText;
            this.tbDebug.Location = new System.Drawing.Point(12, 32);
            this.tbDebug.Multiline = true;
            this.tbDebug.Name = "tbDebug";
            this.tbDebug.ReadOnly = true;
            this.tbDebug.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbDebug.Size = new System.Drawing.Size(628, 391);
            this.tbDebug.TabIndex = 0;
            // 
            // lblProcess
            // 
            this.lblProcess.AutoSize = true;
            this.lblProcess.Location = new System.Drawing.Point(12, 9);
            this.lblProcess.Name = "lblProcess";
            this.lblProcess.Size = new System.Drawing.Size(61, 20);
            this.lblProcess.TabIndex = 1;
            this.lblProcess.Text = "Process:";
            // 
            // lblProcessDesc
            // 
            this.lblProcessDesc.AutoSize = true;
            this.lblProcessDesc.Location = new System.Drawing.Point(79, 9);
            this.lblProcessDesc.Name = "lblProcessDesc";
            this.lblProcessDesc.Size = new System.Drawing.Size(18, 20);
            this.lblProcessDesc.TabIndex = 2;
            this.lblProcessDesc.Text = "...";
            // 
            // Compare
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(656, 450);
            this.Controls.Add(this.lblProcessDesc);
            this.Controls.Add(this.lblProcess);
            this.Controls.Add(this.tbDebug);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Compare";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Comparing Form";
            this.Shown += new System.EventHandler(this.Compare_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox tbDebug;
        private Label lblProcess;
        private Label lblProcessDesc;
    }
}