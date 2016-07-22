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
    public partial class NewFilterForm : Form
    {
        private bool isOK;
        public bool IsOK
        {
            get { return isOK; }
            set { isOK = value; }
        }
        private string msgBoxTitle = "";
        private string osType = "";
        private CheckBox vanillaChkbox;
        private List<FilterClass> filtersSelected;

        public NewFilterForm(string _msgBoxTitle, string _osType, CheckBox _vanillaChkbox, List<FilterClass> _filtersSelected)
        //public NewFilterForm(string _msgBoxTitle, string _osType, CheckBox _vanillaChkbox)
        {
            isOK = false;
            InitializeComponent();
            msgBoxTitle = _msgBoxTitle;
            osType = _osType;
            vanillaChkbox = _vanillaChkbox;
            filtersSelected = _filtersSelected;
        }

        private void NewFilterForm_Load(object sender, EventArgs e)
        {
            cmbFiterElement.Items.Add("All");
            if (vanillaChkbox.Checked)
            {
                cmbFiterElement.Items.Add("Differences");
            }
            Group myGroup = new Group();
            foreach (string groupElement in myGroup.GroupElements)
            {
                cmbFiterElement.Items.Add(groupElement);
            }
            // Load previous selected filters
            foreach (FilterClass filterSelected in filtersSelected)
            {
                listViewFilters.Items.Add(new ListViewItem(new string[] { filterSelected.FilterElement, filterSelected.FilterKeyword, filterSelected.NotInclude.ToString() }));
            }
        }

        private void btnAddFilter_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cmbFiterElement.Text.ToString()))
            {
                MessageBox.Show("Please make a selection.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //
            if (cmbFiterElement.Text.ToString().ToUpper() != "DIFFERENCES")
            {
                if (string.IsNullOrEmpty(cmbKeyword.Text.ToString()) && txtKeyword.Text.Length < 1)
                {
                    MessageBox.Show("Please enter a keyword.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            listViewFilters.Items.Add(new ListViewItem(new string[] { cmbFiterElement.Text.ToString(), cmbKeyword.Text.ToString().Length > 0 ? cmbKeyword.Text.ToString() : txtKeyword.Text, chkNotInclude.Checked.ToString() }));
            //
            cmbFiterElement.Text = "";
            cmbKeyword.Text = "";
            cmbKeyword.Visible = false;
            txtKeyword.Text = "";
            txtKeyword.Visible = false;
            chkNotInclude.Checked = false;
            chkNotInclude.Visible = false;
            lblKeyword.Visible = false;
        }

        private void btnRemoveFilter_Click(object sender, EventArgs e)
        {
            if (listViewFilters.SelectedItems.Count > 0)
            {
                listViewFilters.SelectedItems[0].Remove();
            }
        }

        private void btnRemoveAllFilters_Click(object sender, EventArgs e)
        {
            listViewFilters.Items.Clear();
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            isOK = true;
            this.Close();
        }
    }
}
