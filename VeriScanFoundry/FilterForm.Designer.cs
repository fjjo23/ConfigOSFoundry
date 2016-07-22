namespace VeriSignature
{
    partial class FilterForm
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cmbKeyword = new System.Windows.Forms.ComboBox();
            this.txtKeyword = new System.Windows.Forms.TextBox();
            this.lblKeyword = new System.Windows.Forms.Label();
            this.cmbFiterElement = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkNotInclude = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(194, 113);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(278, 113);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cmbKeyword
            // 
            this.cmbKeyword.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbKeyword.FormattingEnabled = true;
            this.cmbKeyword.Location = new System.Drawing.Point(90, 64);
            this.cmbKeyword.Name = "cmbKeyword";
            this.cmbKeyword.Size = new System.Drawing.Size(263, 21);
            this.cmbKeyword.TabIndex = 11;
            this.cmbKeyword.Visible = false;
            // 
            // txtKeyword
            // 
            this.txtKeyword.Location = new System.Drawing.Point(90, 64);
            this.txtKeyword.Name = "txtKeyword";
            this.txtKeyword.Size = new System.Drawing.Size(263, 20);
            this.txtKeyword.TabIndex = 10;
            this.txtKeyword.Visible = false;
            // 
            // lblKeyword
            // 
            this.lblKeyword.AutoSize = true;
            this.lblKeyword.Location = new System.Drawing.Point(36, 67);
            this.lblKeyword.Name = "lblKeyword";
            this.lblKeyword.Size = new System.Drawing.Size(51, 13);
            this.lblKeyword.TabIndex = 9;
            this.lblKeyword.Text = "Keyword:";
            this.lblKeyword.Visible = false;
            // 
            // cmbFiterElement
            // 
            this.cmbFiterElement.FormattingEnabled = true;
            this.cmbFiterElement.Location = new System.Drawing.Point(90, 31);
            this.cmbFiterElement.Name = "cmbFiterElement";
            this.cmbFiterElement.Size = new System.Drawing.Size(263, 21);
            this.cmbFiterElement.TabIndex = 8;
            this.cmbFiterElement.SelectedIndexChanged += new System.EventHandler(this.cmbFiterElement_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Filter by:";
            // 
            // chkNotInclude
            // 
            this.chkNotInclude.AutoSize = true;
            this.chkNotInclude.Location = new System.Drawing.Point(90, 95);
            this.chkNotInclude.Name = "chkNotInclude";
            this.chkNotInclude.Size = new System.Drawing.Size(80, 17);
            this.chkNotInclude.TabIndex = 12;
            this.chkNotInclude.Text = "Not include";
            this.chkNotInclude.UseVisualStyleBackColor = true;
            this.chkNotInclude.Visible = false;
            // 
            // FilterForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(369, 141);
            this.Controls.Add(this.chkNotInclude);
            this.Controls.Add(this.cmbKeyword);
            this.Controls.Add(this.txtKeyword);
            this.Controls.Add(this.lblKeyword);
            this.Controls.Add(this.cmbFiterElement);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FilterForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Filter";
            this.Load += new System.EventHandler(this.FilterForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        internal System.Windows.Forms.ComboBox cmbKeyword;
        internal System.Windows.Forms.TextBox txtKeyword;
        private System.Windows.Forms.Label lblKeyword;
        internal System.Windows.Forms.ComboBox cmbFiterElement;
        private System.Windows.Forms.Label label1;
        internal System.Windows.Forms.CheckBox chkNotInclude;
    }
}