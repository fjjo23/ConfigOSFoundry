namespace VeriSignature
{
    partial class ProductLicenseForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProductLicenseForm));
            this.label1 = new System.Windows.Forms.Label();
            this.grpSelectKey = new System.Windows.Forms.GroupBox();
            this.txtProductLicense = new System.Windows.Forms.TextBox();
            this.btnProductLicense = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.grpSelectKey.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(15, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(380, 32);
            this.label1.TabIndex = 0;
            this.label1.Text = "The ConfigOS Foundry license was not found and is required to\r\nrun this applicati" +
    "on. Please select the license file below.";
            // 
            // grpSelectKey
            // 
            this.grpSelectKey.Controls.Add(this.txtProductLicense);
            this.grpSelectKey.Controls.Add(this.btnProductLicense);
            this.grpSelectKey.Controls.Add(this.label2);
            this.grpSelectKey.Location = new System.Drawing.Point(15, 58);
            this.grpSelectKey.Name = "grpSelectKey";
            this.grpSelectKey.Size = new System.Drawing.Size(389, 70);
            this.grpSelectKey.TabIndex = 6;
            this.grpSelectKey.TabStop = false;
            this.grpSelectKey.Text = "Product license";
            // 
            // txtProductLicense
            // 
            this.txtProductLicense.Location = new System.Drawing.Point(17, 40);
            this.txtProductLicense.Name = "txtProductLicense";
            this.txtProductLicense.Size = new System.Drawing.Size(326, 20);
            this.txtProductLicense.TabIndex = 3;
            // 
            // btnProductLicense
            // 
            this.btnProductLicense.Location = new System.Drawing.Point(349, 38);
            this.btnProductLicense.Name = "btnProductLicense";
            this.btnProductLicense.Size = new System.Drawing.Size(28, 23);
            this.btnProductLicense.TabIndex = 4;
            this.btnProductLicense.Text = "...";
            this.btnProductLicense.UseVisualStyleBackColor = true;
            this.btnProductLicense.Click += new System.EventHandler(this.btnProductLicense_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(259, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Select a file containig your ConfigOS Foundry license.";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(329, 134);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(248, 134);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 9;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // label4
            // 
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.label4.Location = new System.Drawing.Point(13, 167);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(392, 65);
            this.label4.TabIndex = 11;
            this.label4.Text = resources.GetString("label4.Text");
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label3.Location = new System.Drawing.Point(15, 164);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(388, 2);
            this.label3.TabIndex = 12;
            // 
            // ProductLicenseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 230);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.grpSelectKey);
            this.Controls.Add(this.label1);
            this.Name = "ProductLicenseForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ConfigOS Foundry Product License";
            this.grpSelectKey.ResumeLayout(false);
            this.grpSelectKey.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox grpSelectKey;
        private System.Windows.Forms.TextBox txtProductLicense;
        private System.Windows.Forms.Button btnProductLicense;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;

    }
}