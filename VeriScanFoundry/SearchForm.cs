using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace VeriSignature
{
    public partial class SearchForm : Form
    {
        private bool isOK;
        public bool IsOK
        {
            get { return isOK; }
            set { isOK = value; }
        }
        private string msgBoxTitle = "";
        private TreeView treeViewSearch = null;

        public SearchForm(string _msgBoxTitle, TreeView _treeViewSearch)
        {
            isOK = false;
            InitializeComponent();
            msgBoxTitle = _msgBoxTitle;
            treeViewSearch = _treeViewSearch;
        }

        private void SearchForm_Load(object sender, EventArgs e)
        {
            cmbFiterElement.Items.Add("All");
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
            if (txtKeyword.Text.Length < 1)
            {
                MessageBox.Show("Please enter a keyword.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            isOK = true;
            // Removes previous search
            RemoveSearch(treeViewSearch.Nodes);
            treeViewSearch.CollapseAll();
            // Find
            if (IsValidRegex(txtKeyword.Text))
            {
                FindInTreeView(treeViewSearch.Nodes, txtKeyword.Text);
            }
            else
            {
                MessageBox.Show("The Regex pattern is invalid.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //
            this.Close();
        }

        private void FindInTreeView(TreeNodeCollection tncoll, string regexPattern)
        {
            foreach (TreeNode tnode in tncoll)
            {
                try
                {
                    string[] valueParts = tnode.Text.Split(':');

                    if (cmbFiterElement.SelectedItem.ToString().ToUpper() == "ALL" || valueParts[0].Trim().ToUpper() == cmbFiterElement.SelectedItem.ToString().ToUpper())
                    {
                        if (Regex.IsMatch(tnode.Text, regexPattern, RegexOptions.IgnoreCase))
                        {
                            tnode.NodeFont = new Font(treeViewSearch.Font, FontStyle.Underline);
                            tnode.TreeView.SelectedNode = tnode;
                        }
                    }
                   

                    //string[] valueParts = tnode.Text.Split(':');
                    ////if (tnode.Text.ToUpper() == strNode.ToUpper())
                    //if (chkNotInclude.Checked)
                    //{
                    //    if (valueParts[0].Trim().ToUpper() == cmbFiterElement.SelectedItem.ToString().ToUpper() && !valueParts[1].Trim().ToUpper().Contains(txtKeyword.Text.ToUpper()))
                    //    {
                    //        //tnode.ForeColor = Color.Peru;
                    //        tnode.NodeFont = new Font(treeViewSearch.Font, FontStyle.Underline);
                    //        tnode.TreeView.SelectedNode = tnode;
                    //    }
                    //    //else
                    //    //
                    //    //    tnode.BackColor = tnode.TreeView.BackColor;
                    //    //}
                    //}
                    //else
                    //{
                    //    if (valueParts[0].Trim().ToUpper() == cmbFiterElement.SelectedItem.ToString().ToUpper() && valueParts[1].Trim().ToUpper().Contains(txtKeyword.Text.ToUpper()))
                    //    {
                    //        //tnode.ForeColor = Color.Peru;
                    //        tnode.NodeFont = new Font(treeViewSearch.Font, FontStyle.Underline);
                    //        tnode.TreeView.SelectedNode = tnode;
                    //    }
                    //    //else
                    //    //{
                    //    //    tnode.BackColor = tnode.TreeView.BackColor;
                    //    //}
                    //}
                }
                catch { }
                FindInTreeView(tnode.Nodes, regexPattern);
            }
        }

        private static bool IsValidRegex(string pattern)
        {
            if (string.IsNullOrEmpty(pattern)) return false;

            try
            {
                Regex.Match("", pattern);
            }
            catch (ArgumentException)
            {
                return false;
            }

            return true;
        }

        private void RemoveSearch(TreeNodeCollection tncoll)
        {
            foreach (TreeNode tnode in tncoll)
            {
                try
                {
                    //if (tnode.ForeColor == Color.Peru)
                    //{
                    //    tnode.ForeColor = Color.Black;
                    //}
                    if (tnode.NodeFont != null && tnode.NodeFont.Underline)
                    {
                        //tnode.NodeFont = new Font(treeViewSearch.Font, FontStyle.Regular);
                        tnode.NodeFont = null;
                    }
                    //if (tnode.NodeFont == new Font(treeViewSearch.Font, FontStyle.Underline))
                    //{
                    //    tnode.NodeFont = new Font(treeViewSearch.Font, FontStyle.Regular);
                    //}
                }
                catch { }
                RemoveSearch(tnode.Nodes);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Removes previous search
            RemoveSearch(treeViewSearch.Nodes);
            treeViewSearch.CollapseAll();
            this.Close();
        }
    }
}
