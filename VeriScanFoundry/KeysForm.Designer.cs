namespace VeriSignature
{
    partial class KeysForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KeysForm));
            this.rbtBrowseKey = new System.Windows.Forms.RadioButton();
            this.rbtGenerateKey = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.txtExistingKey = new System.Windows.Forms.TextBox();
            this.btnBrowseKey = new System.Windows.Forms.Button();
            this.grpSelectKey = new System.Windows.Forms.GroupBox();
            this.grpGenerateKey = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnBrowseNewKey = new System.Windows.Forms.Button();
            this.txtNewKeyFolder = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.lblKey = new System.Windows.Forms.Label();
            this.lblCurrentKey = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.grpSelectKey.SuspendLayout();
            this.grpGenerateKey.SuspendLayout();
            this.SuspendLayout();
            // 
            // rbtBrowseKey
            // 
            this.rbtBrowseKey.AutoSize = true;
            this.rbtBrowseKey.Checked = true;
            this.rbtBrowseKey.Location = new System.Drawing.Point(16, 13);
            this.rbtBrowseKey.Name = "rbtBrowseKey";
            this.rbtBrowseKey.Size = new System.Drawing.Size(162, 17);
            this.rbtBrowseKey.TabIndex = 0;
            this.rbtBrowseKey.TabStop = true;
            this.rbtBrowseKey.Text = "Browse for an encryption key";
            this.rbtBrowseKey.UseVisualStyleBackColor = true;
            this.rbtBrowseKey.CheckedChanged += new System.EventHandler(this.rbtBrowseKey_CheckedChanged);
            // 
            // rbtGenerateKey
            // 
            this.rbtGenerateKey.AutoSize = true;
            this.rbtGenerateKey.Location = new System.Drawing.Point(223, 13);
            this.rbtGenerateKey.Name = "rbtGenerateKey";
            this.rbtGenerateKey.Size = new System.Drawing.Size(184, 17);
            this.rbtGenerateKey.TabIndex = 1;
            this.rbtGenerateKey.Text = "Generate new encryption key pair";
            this.rbtGenerateKey.UseVisualStyleBackColor = true;
            this.rbtGenerateKey.CheckedChanged += new System.EventHandler(this.rbtGenerateKey_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(244, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Select a file containig your existing encryption key:";
            // 
            // txtExistingKey
            // 
            this.txtExistingKey.Location = new System.Drawing.Point(17, 40);
            this.txtExistingKey.Name = "txtExistingKey";
            this.txtExistingKey.Size = new System.Drawing.Size(326, 20);
            this.txtExistingKey.TabIndex = 3;
            // 
            // btnBrowseKey
            // 
            this.btnBrowseKey.Location = new System.Drawing.Point(349, 38);
            this.btnBrowseKey.Name = "btnBrowseKey";
            this.btnBrowseKey.Size = new System.Drawing.Size(28, 23);
            this.btnBrowseKey.TabIndex = 4;
            this.btnBrowseKey.Text = "...";
            this.btnBrowseKey.UseVisualStyleBackColor = true;
            this.btnBrowseKey.Click += new System.EventHandler(this.btnBrowseKey_Click);
            // 
            // grpSelectKey
            // 
            this.grpSelectKey.Controls.Add(this.txtExistingKey);
            this.grpSelectKey.Controls.Add(this.btnBrowseKey);
            this.grpSelectKey.Controls.Add(this.label1);
            this.grpSelectKey.Location = new System.Drawing.Point(16, 61);
            this.grpSelectKey.Name = "grpSelectKey";
            this.grpSelectKey.Size = new System.Drawing.Size(389, 70);
            this.grpSelectKey.TabIndex = 5;
            this.grpSelectKey.TabStop = false;
            this.grpSelectKey.Text = "Select an existing encryption key";
            // 
            // grpGenerateKey
            // 
            this.grpGenerateKey.Controls.Add(this.label2);
            this.grpGenerateKey.Controls.Add(this.btnBrowseNewKey);
            this.grpGenerateKey.Controls.Add(this.txtNewKeyFolder);
            this.grpGenerateKey.Enabled = false;
            this.grpGenerateKey.Location = new System.Drawing.Point(16, 143);
            this.grpGenerateKey.Name = "grpGenerateKey";
            this.grpGenerateKey.Size = new System.Drawing.Size(389, 70);
            this.grpGenerateKey.TabIndex = 6;
            this.grpGenerateKey.TabStop = false;
            this.grpGenerateKey.Text = "Generate a new encryption key pair";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(341, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Select the folder where you want to save your new encryption key pair:";
            // 
            // btnBrowseNewKey
            // 
            this.btnBrowseNewKey.Location = new System.Drawing.Point(349, 39);
            this.btnBrowseNewKey.Name = "btnBrowseNewKey";
            this.btnBrowseNewKey.Size = new System.Drawing.Size(27, 23);
            this.btnBrowseNewKey.TabIndex = 1;
            this.btnBrowseNewKey.Text = "...";
            this.btnBrowseNewKey.UseVisualStyleBackColor = true;
            this.btnBrowseNewKey.Click += new System.EventHandler(this.btnBrowseNewKey_Click);
            // 
            // txtNewKeyFolder
            // 
            this.txtNewKeyFolder.Location = new System.Drawing.Point(17, 39);
            this.txtNewKeyFolder.Name = "txtNewKeyFolder";
            this.txtNewKeyFolder.Size = new System.Drawing.Size(326, 20);
            this.txtNewKeyFolder.TabIndex = 0;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(249, 219);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(330, 219);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // lblKey
            // 
            this.lblKey.AutoSize = true;
            this.lblKey.Location = new System.Drawing.Point(60, 39);
            this.lblKey.Name = "lblKey";
            this.lblKey.Size = new System.Drawing.Size(104, 13);
            this.lblKey.TabIndex = 9;
            this.lblKey.Text = "Your encryption key:";
            this.lblKey.Visible = false;
            // 
            // lblCurrentKey
            // 
            this.lblCurrentKey.AutoSize = true;
            this.lblCurrentKey.Location = new System.Drawing.Point(167, 39);
            this.lblCurrentKey.Name = "lblCurrentKey";
            this.lblCurrentKey.Size = new System.Drawing.Size(0, 13);
            this.lblCurrentKey.TabIndex = 10;
            this.lblCurrentKey.Visible = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(50, 219);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "test";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // KeysForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(423, 245);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lblCurrentKey);
            this.Controls.Add(this.lblKey);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.grpGenerateKey);
            this.Controls.Add(this.grpSelectKey);
            this.Controls.Add(this.rbtGenerateKey);
            this.Controls.Add(this.rbtBrowseKey);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "KeysForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ConfigOS Keys";
            this.Load += new System.EventHandler(this.KeysForm_Load);
            this.grpSelectKey.ResumeLayout(false);
            this.grpSelectKey.PerformLayout();
            this.grpGenerateKey.ResumeLayout(false);
            this.grpGenerateKey.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rbtBrowseKey;
        private System.Windows.Forms.RadioButton rbtGenerateKey;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtExistingKey;
        private System.Windows.Forms.Button btnBrowseKey;
        private System.Windows.Forms.GroupBox grpSelectKey;
        private System.Windows.Forms.GroupBox grpGenerateKey;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnBrowseNewKey;
        private System.Windows.Forms.TextBox txtNewKeyFolder;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label lblKey;
        private System.Windows.Forms.Label lblCurrentKey;
        private System.Windows.Forms.Button button1;
    }
}