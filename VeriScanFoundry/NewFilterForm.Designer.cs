namespace VeriSignature
{
    partial class NewFilterForm
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
            this.chkNotInclude = new System.Windows.Forms.CheckBox();
            this.cmbKeyword = new System.Windows.Forms.ComboBox();
            this.txtKeyword = new System.Windows.Forms.TextBox();
            this.lblKeyword = new System.Windows.Forms.Label();
            this.cmbFiterElement = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.listViewFilters = new System.Windows.Forms.ListView();
            this.colElement = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colKeyword = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colNotInclude = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnAddFilter = new System.Windows.Forms.Button();
            this.btnRemoveFilter = new System.Windows.Forms.Button();
            this.btnRemoveAllFilters = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // chkNotInclude
            // 
            this.chkNotInclude.AutoSize = true;
            this.chkNotInclude.Location = new System.Drawing.Point(77, 82);
            this.chkNotInclude.Name = "chkNotInclude";
            this.chkNotInclude.Size = new System.Drawing.Size(80, 17);
            this.chkNotInclude.TabIndex = 20;
            this.chkNotInclude.Text = "Not include";
            this.chkNotInclude.UseVisualStyleBackColor = true;
            this.chkNotInclude.Visible = false;
            // 
            // cmbKeyword
            // 
            this.cmbKeyword.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbKeyword.FormattingEnabled = true;
            this.cmbKeyword.Location = new System.Drawing.Point(77, 51);
            this.cmbKeyword.Name = "cmbKeyword";
            this.cmbKeyword.Size = new System.Drawing.Size(263, 21);
            this.cmbKeyword.TabIndex = 19;
            this.cmbKeyword.Visible = false;
            // 
            // txtKeyword
            // 
            this.txtKeyword.Location = new System.Drawing.Point(77, 51);
            this.txtKeyword.Name = "txtKeyword";
            this.txtKeyword.Size = new System.Drawing.Size(263, 20);
            this.txtKeyword.TabIndex = 18;
            this.txtKeyword.Visible = false;
            // 
            // lblKeyword
            // 
            this.lblKeyword.AutoSize = true;
            this.lblKeyword.Location = new System.Drawing.Point(23, 54);
            this.lblKeyword.Name = "lblKeyword";
            this.lblKeyword.Size = new System.Drawing.Size(51, 13);
            this.lblKeyword.TabIndex = 17;
            this.lblKeyword.Text = "Keyword:";
            this.lblKeyword.Visible = false;
            // 
            // cmbFiterElement
            // 
            this.cmbFiterElement.FormattingEnabled = true;
            this.cmbFiterElement.Location = new System.Drawing.Point(77, 18);
            this.cmbFiterElement.Name = "cmbFiterElement";
            this.cmbFiterElement.Size = new System.Drawing.Size(263, 21);
            this.cmbFiterElement.TabIndex = 16;
            this.cmbFiterElement.SelectedIndexChanged += new System.EventHandler(this.cmbFiterElement_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Filter by:";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(352, 253);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(270, 253);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 13;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // listViewFilters
            // 
            this.listViewFilters.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listViewFilters.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colElement,
            this.colKeyword,
            this.colNotInclude});
            this.listViewFilters.Location = new System.Drawing.Point(28, 105);
            this.listViewFilters.Name = "listViewFilters";
            this.listViewFilters.Size = new System.Drawing.Size(399, 109);
            this.listViewFilters.TabIndex = 21;
            this.listViewFilters.UseCompatibleStateImageBehavior = false;
            this.listViewFilters.View = System.Windows.Forms.View.Details;
            // 
            // colElement
            // 
            this.colElement.Text = "Filter by";
            this.colElement.Width = 150;
            // 
            // colKeyword
            // 
            this.colKeyword.Text = "Keyword";
            this.colKeyword.Width = 180;
            // 
            // colNotInclude
            // 
            this.colNotInclude.Text = "Not include";
            this.colNotInclude.Width = 67;
            // 
            // btnAddFilter
            // 
            this.btnAddFilter.Location = new System.Drawing.Point(352, 18);
            this.btnAddFilter.Name = "btnAddFilter";
            this.btnAddFilter.Size = new System.Drawing.Size(75, 23);
            this.btnAddFilter.TabIndex = 22;
            this.btnAddFilter.Text = "Add filter";
            this.btnAddFilter.UseVisualStyleBackColor = true;
            this.btnAddFilter.Click += new System.EventHandler(this.btnAddFilter_Click);
            // 
            // btnRemoveFilter
            // 
            this.btnRemoveFilter.Location = new System.Drawing.Point(28, 220);
            this.btnRemoveFilter.Name = "btnRemoveFilter";
            this.btnRemoveFilter.Size = new System.Drawing.Size(80, 23);
            this.btnRemoveFilter.TabIndex = 23;
            this.btnRemoveFilter.Text = "Remove filter";
            this.btnRemoveFilter.UseVisualStyleBackColor = true;
            this.btnRemoveFilter.Click += new System.EventHandler(this.btnRemoveFilter_Click);
            // 
            // btnRemoveAllFilters
            // 
            this.btnRemoveAllFilters.Location = new System.Drawing.Point(114, 220);
            this.btnRemoveAllFilters.Name = "btnRemoveAllFilters";
            this.btnRemoveAllFilters.Size = new System.Drawing.Size(101, 23);
            this.btnRemoveAllFilters.TabIndex = 24;
            this.btnRemoveAllFilters.Text = "Remove all filters";
            this.btnRemoveAllFilters.UseVisualStyleBackColor = true;
            this.btnRemoveAllFilters.Click += new System.EventHandler(this.btnRemoveAllFilters_Click);
            // 
            // NewFilterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(451, 283);
            this.Controls.Add(this.btnRemoveAllFilters);
            this.Controls.Add(this.btnRemoveFilter);
            this.Controls.Add(this.btnAddFilter);
            this.Controls.Add(this.listViewFilters);
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
            this.Name = "NewFilterForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Filter";
            this.Load += new System.EventHandler(this.NewFilterForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.CheckBox chkNotInclude;
        internal System.Windows.Forms.ComboBox cmbKeyword;
        internal System.Windows.Forms.TextBox txtKeyword;
        private System.Windows.Forms.Label lblKeyword;
        internal System.Windows.Forms.ComboBox cmbFiterElement;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ColumnHeader colElement;
        private System.Windows.Forms.ColumnHeader colKeyword;
        private System.Windows.Forms.ColumnHeader colNotInclude;
        private System.Windows.Forms.Button btnAddFilter;
        private System.Windows.Forms.Button btnRemoveFilter;
        private System.Windows.Forms.Button btnRemoveAllFilters;
        internal System.Windows.Forms.ListView listViewFilters;
    }
}