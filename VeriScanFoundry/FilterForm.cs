using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VeriSignature
{
    public partial class FilterForm : Form
    {
        private bool isOK;
        public bool IsOK
        {
            get { return isOK; }
            set { isOK = value; }
        }
        private string msgBoxTitle = "";
        private string osType = "";

        // Start "FilterDifferences" ver: 1.0.8 date: 02-24-16
        private CheckBox vanillaChkbox;
        //public FilterForm(string _msgBoxTitle, string _osType)
        public FilterForm(string _msgBoxTitle, string _osType, CheckBox _vanillaChkbox)
        // End "FilterDifferences" ver: 1.0.8 date: 02-24-16
        {
            isOK = false;
            InitializeComponent();
            msgBoxTitle = _msgBoxTitle;
            osType = _osType;
            // Start "FilterDifferences" ver: 1.0.8 date: 02-24-16
            vanillaChkbox = _vanillaChkbox;
            // End "FilterDifferences" ver: 1.0.8 date: 02-24-16
        }

        private void FilterForm_Load(object sender, EventArgs e)
        {
            cmbFiterElement.Items.Add("All");
            // Start "FilterDifferences" ver: 1.0.8 date: 02-24-16
            if (vanillaChkbox.Checked)
            {
                cmbFiterElement.Items.Add("Differences");
            }
            // End "FilterDifferences" ver: 1.0.8 date: 02-24-16
            Group myGroup = new Group();
            foreach (string groupElement in myGroup.GroupElements)
            {
                cmbFiterElement.Items.Add(groupElement);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cmbFiterElement.Text.ToString()))
            {
                MessageBox.Show("Please make a selection.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Start "FilterDifferences" ver: 1.0.8 date: 02-24-16
            if (cmbFiterElement.Text.ToString().ToUpper() != "DIFFERENCES")
            {
            // End "FilterDifferences" ver: 1.0.8 date: 02-24-16
                if (string.IsNullOrEmpty(cmbKeyword.Text.ToString()) && txtKeyword.Text.Length < 1)
                {
                    MessageBox.Show("Please enter a keyword.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            isOK = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbFiterElement_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtKeyword.Text = "";
            cmbKeyword.Items.Clear();
            if (cmbFiterElement.SelectedIndex >= 0)
            {
                lblKeyword.Visible = true;
                chkNotInclude.Visible = true;
                string caseSwitch = cmbFiterElement.SelectedItem.ToString().ToUpper();
                switch (caseSwitch)
                {
                    case "SEVERITY":
                        cmbKeyword.Items.Add("CAT I");
                        cmbKeyword.Items.Add("CAT II");
                        cmbKeyword.Items.Add("CAT III");
                        txtKeyword.Visible = false;
                        cmbKeyword.Visible = true;
                        break;
                    case "TYPE":
                        if (osType == "LINUX")
                        {
                            txtKeyword.Visible = true;
                            cmbKeyword.Visible = false;
                        }
                        else
                        {
                            cmbKeyword.Items.Add("REG_SZ");
                            cmbKeyword.Items.Add("REG_DWORD");
                            cmbKeyword.Items.Add("REG_BINARY");
                            cmbKeyword.Items.Add("REG_MULTI_SZ");
                            txtKeyword.Visible = false;
                            cmbKeyword.Visible = true;
                        }
                        break;
                    case "IGNORE":
                        if (osType == "LINUX")
                        {
                            txtKeyword.Visible = true;
                            cmbKeyword.Visible = false;
                        }
                        else
                        {
                            cmbKeyword.Items.Add("0 - Disable");
                            cmbKeyword.Items.Add("1 - Import");
                            cmbKeyword.Items.Add("2 - Export");
                            cmbKeyword.Items.Add("3 - Case");
                            txtKeyword.Visible = false;
                            cmbKeyword.Visible = true;
                        }
                        break;
                    // Start "FilterDifferences" ver: 1.0.8 date: 02-24-16
                    case "DIFFERENCES":
                        txtKeyword.Visible = false;
                        cmbKeyword.Visible = false;
                        lblKeyword.Visible = false;
                        break;
                    // End "FilterDifferences" ver: 1.0.8 date: 02-24-16
                    default:
                        txtKeyword.Visible = true;
                        cmbKeyword.Visible = false;
                        break;
                }
            }
        }
    }
}
