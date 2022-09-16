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
            this.btnSaveDebug = new System.Windows.Forms.Button();
            this.btnSaveResult = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbDebug
            // 
            this.tbDebug.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tbDebug.ForeColor = System.Drawing.SystemColors.GrayText;
            this.tbDebug.Location = new System.Drawing.Point(12, 50);
            this.tbDebug.Multiline = true;
            this.tbDebug.Name = "tbDebug";
            this.tbDebug.ReadOnly = true;
            this.tbDebug.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbDebug.ShortcutsEnabled = false;
            this.tbDebug.Size = new System.Drawing.Size(628, 364);
            this.tbDebug.TabIndex = 0;
            // 
            // lblProcess
            // 
            this.lblProcess.AutoSize = true;
            this.lblProcess.Location = new System.Drawing.Point(12, 18);
            this.lblProcess.Name = "lblProcess";
            this.lblProcess.Size = new System.Drawing.Size(61, 20);
            this.lblProcess.TabIndex = 1;
            this.lblProcess.Text = "Process:";
            // 
            // lblProcessDesc
            // 
            this.lblProcessDesc.AutoSize = true;
            this.lblProcessDesc.Location = new System.Drawing.Point(79, 18);
            this.lblProcessDesc.Name = "lblProcessDesc";
            this.lblProcessDesc.Size = new System.Drawing.Size(18, 20);
            this.lblProcessDesc.TabIndex = 2;
            this.lblProcessDesc.Text = "...";
            // 
            // btnSaveDebug
            // 
            this.btnSaveDebug.Location = new System.Drawing.Point(546, 9);
            this.btnSaveDebug.Name = "btnSaveDebug";
            this.btnSaveDebug.Size = new System.Drawing.Size(94, 29);
            this.btnSaveDebug.TabIndex = 3;
            this.btnSaveDebug.Text = "Save Log";
            this.btnSaveDebug.UseVisualStyleBackColor = true;
            // 
            // btnSaveResult
            // 
            this.btnSaveResult.Location = new System.Drawing.Point(263, 423);
            this.btnSaveResult.Name = "btnSaveResult";
            this.btnSaveResult.Size = new System.Drawing.Size(120, 29);
            this.btnSaveResult.TabIndex = 4;
            this.btnSaveResult.Text = "Save Result";
            this.btnSaveResult.UseVisualStyleBackColor = true;
            // 
            // Compare
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(656, 464);
            this.Controls.Add(this.btnSaveResult);
            this.Controls.Add(this.btnSaveDebug);
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
        private Button btnSaveDebug;
        private Button btnSaveResult;
    }
}