using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Collections;
using System.Drawing;

namespace VeriSignature
{
    class TreeViewMng
    {
        private string errorMessage;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set { errorMessage = value; }
        }

        private XElement groupsElementNew = null;
        public XElement GroupsElementNew
        {
            get { return groupsElementNew; }
            set { groupsElementNew = value; }
        }

        private List<TreeNode> remainingNodes = new List<TreeNode>();
        public List<TreeNode> RemainingNodes
        {
            get { return remainingNodes; }
            set { remainingNodes = value; }
        }
        private List<TreeNode> matchingNodes = new List<TreeNode>();
        public List<TreeNode> MatchingNodes
        {
            get { return matchingNodes; }
            set { matchingNodes = value; }
        }

        // Start modifications ver: 1.0.6 date: 02-18-15
        //public void SeparateFilteredNodes(TreeNodeCollection myTreeNodeCollection, string filterElement, string filterKeyword, bool mustMatch)
        public void SeparateFilteredNodes(TreeNodeCollection myTreeNodeCollection, string filterElement, string filterKeyword, bool mustMatch, bool notInclude)
        // End modifications ver: 1.0.6 date: 02-18-15
        {
            // Start modifications ver: 1.0.6 date: 02-20-15
            //remainingNodes.Clear();
            // End modifications ver: 1.0.6 date: 02-20-15
            matchingNodes.Clear();
            string ucFilterKeyword = filterKeyword.ToUpper();

            switch (filterElement.ToUpper())
            {
                case "GROUPID":
                    foreach (TreeNode parentNode in myTreeNodeCollection)
                    {
                        if (Shared.GetElementValue(parentNode.Text.ToUpper()).Contains(ucFilterKeyword))
                        {
                            // Start modifications ver: 1.0.6 date: 02-18-15
                            if (notInclude)
                            {
                                remainingNodes.Add(parentNode);
                            }
                            else
                            {
                                matchingNodes.Add(parentNode);
                            }
                            // End modifications ver: 1.0.6 date: 02-18-15
                        }
                        else
                        {
                            // Start modifications ver: 1.0.6 date: 02-18-15
                            if (notInclude)
                            {
                                matchingNodes.Add(parentNode);
                            }
                            else
                            {
                                remainingNodes.Add(parentNode);
                            }
                            // End modifications ver: 1.0.6 date: 02-18-15
                        }
                    }
                    break;
                // Start "FilterDifferences" ver: 1.0.8 date: 02-24-16
                case "DIFFERENCES":
                    foreach (TreeNode parentNode in myTreeNodeCollection)
                    {
                        if (parentNode.ForeColor == Color.DarkGreen)
                        {
                            if (notInclude)
                            {
                                remainingNodes.Add(parentNode);
                            }
                            else
                            {
                                matchingNodes.Add(parentNode);
                            }
                        }
                        else
                        {
                            if (notInclude)
                            {
                                matchingNodes.Add(parentNode);
                            }
                            else
                            {
                                remainingNodes.Add(parentNode);
                            }
                        }
                    }
                    break;
                // End "FilterDifferences" ver: 1.0.8 date: 02-24-16
                case "ALL":
                    foreach (TreeNode parentNode in myTreeNodeCollection)
                    {
                        bool swFound = false;
                        foreach (TreeNode childNode in parentNode.Nodes)
                        {
                            if (Shared.GetElementValue(childNode.Text.ToUpper()).Contains(ucFilterKeyword))
                            {
                                // Start modifications ver: 1.0.6 date: 02-18-15
                                if (notInclude)
                                {
                                    remainingNodes.Add(parentNode);
                                }
                                else
                                {
                                    matchingNodes.Add(parentNode);
                                }
                                // End modifications ver: 1.0.6 date: 02-18-15
                                swFound = true;
                                break;
                            }
                        }
                        if (!swFound)
                        {
                            // Start modifications ver: 1.0.6 date: 02-18-15
                            if (notInclude)
                            {
                                matchingNodes.Add(parentNode);
                            }
                            else
                            {
                                remainingNodes.Add(parentNode);
                            }
                            // End modifications ver: 1.0.6 date: 02-18-15
                        }
                    }
                    break;
                default:
                    foreach (TreeNode parentNode in myTreeNodeCollection)
                    {
                        bool swFound = false;
                        foreach (TreeNode childNode in parentNode.Nodes)
                        {
                            if (Shared.GetElementName(childNode.Text).ToUpper() == filterElement.ToUpper())
                            {
                                if (mustMatch)
                                {
                                    if (Shared.GetElementValue(childNode.Text).ToUpper().Equals(ucFilterKeyword))
                                    {
                                        // Start modifications ver: 1.0.6 date: 02-18-15
                                        if (notInclude)
                                        {
                                            remainingNodes.Add(parentNode);
                                        }
                                        else
                                        {
                                            matchingNodes.Add(parentNode);
                                        }
                                        // End modifications ver: 1.0.6 date: 02-18-15
                                        swFound = true;
                                    }
                                }
                                else
                                {
                                    if (Shared.GetElementValue(childNode.Text).ToUpper().Contains(ucFilterKeyword))
                                    {
                                        // Start modifications ver: 1.0.6 date: 02-18-15
                                        if (notInclude)
                                        {
                                            remainingNodes.Add(parentNode);
                                        }
                                        else
                                        {
                                            matchingNodes.Add(parentNode);
                                        }
                                        // End modifications ver: 1.0.6 date: 02-18-15
                                        swFound = true;
                                    }
                                }
                                break;
                            }
                        }
                        if (!swFound)
                        {
                            // Start modifications ver: 1.0.6 date: 02-18-15
                            if (notInclude)
                            {
                                matchingNodes.Add(parentNode);
                            }
                            else
                            {
                                remainingNodes.Add(parentNode);
                            }
                            // End modifications ver: 1.0.6 date: 02-18-15
                        }
                    }
                    break;
            }
        }

        public void CopyNodes(TreeNodeCollection myTreeNodeCollection, List<TreeNode> matchingNodes)
        {
            foreach (TreeNode matchNode in matchingNodes)
            {
                myTreeNodeCollection.Add(matchNode);
            }
        }

        public void MoveCheckedNodesFromToTreeViews(TreeNodeCollection fromTreeView, TreeNodeCollection toTreeView, XDocument xb, XDocument xv)
        {
            try
            {
                List<TreeNode> checkedNodesAdd = new List<TreeNode>();
                List<TreeNode> checkedNodesRemove = new List<TreeNode>();
                // Get checked nodes
                foreach (TreeNode node in fromTreeView)
                {
                    if (node.Checked)
                    {
                        checkedNodesRemove.Add(node);
                        if (node.Tag.ToString() == "DiffFromSourceAndBase" || node.Tag.ToString() == "DiffFromSource")
                        {
                            // Find group on source document
                            XElement bGroup = Shared.FindGroupInDocument(xb, Shared.GetElementValue(node.Text));
                            if (bGroup != null)
                            {
                                TreeNode groupParentNode = new TreeNode();
                                foreach (XElement bGroupChild in bGroup.Elements())
                                {
                                    if (bGroupChild.Name.ToString() == "GroupId")
                                    {
                                        groupParentNode.Text = "GroupId: " + bGroupChild.Value.Trim();
                                    }
                                    else
                                    {
                                        groupParentNode.Nodes.Add(new TreeNode(bGroupChild.Name.ToString() + ": " + bGroupChild.Value.Trim()));
                                    }
                                }
                                checkedNodesAdd.Add(groupParentNode);
                            }
                        }
                        else
                        {
                            checkedNodesAdd.Add(node);
                        }
                    }
                }
                // Remove nodes from
                foreach (TreeNode checkedNode in checkedNodesRemove)
                {
                    fromTreeView.Remove(checkedNode);
                }
                // Add nodes to
                foreach (TreeNode checkedNode in checkedNodesAdd)
                {
                    checkedNode.Checked = false;
                    if (xv != null)
                    {
                        // Find group on base document
                        XElement vGroup = Shared.FindGroupInDocument(xv, Shared.GetElementValue(checkedNode.Text));
                        // Find group on source document
                        XElement bGroup = Shared.FindGroupInDocument(xb, Shared.GetElementValue(checkedNode.Text));
                        // Compares wih base
                        bool isDiffFromBase = false;
                        if (vGroup == null || bGroup == null)
                        {
                            isDiffFromBase = true;
                        }
                        else
                        {
                            isDiffFromBase = Shared.CompareTwoElements(bGroup, vGroup);
                        }
                        if (isDiffFromBase)
                        {
                            checkedNode.Tag = "DiffFromBase";
                            // Start modifications ver: 1.0.6 date: 02-18-15
                            //checkedNode.ForeColor = Color.YellowGreen;
                            checkedNode.ForeColor = Color.DarkGreen;
                            // End modifications ver: 1.0.6 date: 02-18-15
                        }
                        else
                        {
                            checkedNode.Tag = "NoDiff";
                            checkedNode.ForeColor = Color.Black;
                        }
                    }
                    else
                    {
                        //
                        checkedNode.Tag = "NoDiff";
                        checkedNode.ForeColor = Color.Black;
                    }
                    //
                    toTreeView.Add(checkedNode);
                }
            }
            catch
            {
            }
        }

        public string LoadTreeview(TreeNodeCollection myTreeview, XDocument xb, XDocument xv)
        {
            errorMessage = "";
            try
            {
                // Create empty "Groups" element
                groupsElementNew = new XElement("Groups");
                // Add attributes to the empty "Groups" from the "Groups" of the loaded signature
                if (xb.Elements("Groups").Count() == 1)
                {
                    if (xb.Element("Groups").Attribute("SysArea").Value.ToString() == "SecurityPolicy")
                    {
                        foreach (XAttribute myAtt in xb.Elements("Groups").Attributes())
                        {
                            groupsElementNew.Add(myAtt);
                        }
                        //
                        myTreeview.Clear();
                        IEnumerable<XElement> bGroups =
                                               from bgp in xb.Elements("Groups").Elements("Items").Elements("Group")
                                               select bgp;
                        //
                        foreach (XElement bGroup in bGroups)
                        {
                            TreeNode groupParentNode = new TreeNode();
                            foreach (XElement bGroupChild in bGroup.Elements())
                            {
                                if (bGroupChild.Name.ToString() == "GroupId")
                                {
                                    groupParentNode.Text = "GroupId: " + bGroupChild.Value.Trim();
                                    groupParentNode.Tag = "NoDiff";
                                    // Check against base document
                                    if (xv != null)
                                    {
                                        // Find group on base document
                                        XElement vGroup = Shared.FindGroupInDocument(xv, bGroupChild.Value.Trim());
                                        // Compares wih base
                                        if (vGroup == null || Shared.CompareTwoElements(bGroup, vGroup))
                                        {
                                            groupParentNode.Tag = "DiffFromBase";
                                            // Start modifications ver: 1.0.6 date: 02-18-15
                                            //groupParentNode.ForeColor = Color.YellowGreen;
                                            groupParentNode.ForeColor = Color.DarkGreen;
                                            // End modifications ver: 1.0.6 date: 02-18-15
                                        }
                                    }
                                    //
                                    myTreeview.Add(groupParentNode);
                                }
                                else
                                {
                                    groupParentNode.Nodes.Add(new TreeNode(bGroupChild.Name.ToString() + ": " + bGroupChild.Value.Trim()));
                                }
                            }
                        }
                    }
                    else
                    {
                        errorMessage = "This is an invalid SteelCloud ConfigOS signature file.";
                    }
                }
                else
                {
                    errorMessage = "This is an invalid SteelCloud ConfigOS signature file.";
                }
            }
            catch(Exception err)
            {
                errorMessage = err.Message;
            }
            return errorMessage;
        }

        public void SaveTreeViewToFile(TreeNodeCollection treeNodeCollection, string fileToSave)
        {
            errorMessage = "";
            try
            {
                XDocument xc = new XDocument();
                XElement grpen = new XElement(groupsElementNew);
                grpen.Add(new XElement("Items"));
                xc.Add(new XElement(grpen));
                //
                foreach (TreeNode node in treeNodeCollection)
                {
                    XElement group = new XElement("Group");
                    XElement groupId = new XElement("GroupId", Shared.GetElementValue(node.Text));
                    group.Add(groupId);
                    foreach (TreeNode childNode in node.Nodes)
                    {
                        XElement groupChild = new XElement(Shared.GetElementName(childNode.Text), Shared.GetElementValue(childNode.Text));
                        group.Add(groupChild);
                    }
                    xc.Element("Groups").Element("Items").Add(new XElement(group));
                }
                xc.Save(fileToSave);
            }
            catch (Exception err)
            {
                errorMessage = "There was an error saving the file " + fileToSave + ". " + err.Message;
            }
        }

        public void SortTreeViewNodes(TreeView treeViewToSort)
        {
            CompareTreeNodes ctn = new CompareTreeNodes();
            ArrayList list = new ArrayList(treeViewToSort.Nodes.Count);
            foreach (TreeNode childNode in treeViewToSort.Nodes)
            {
                list.Add(childNode);
            }
            list.Sort(ctn);

            treeViewToSort.BeginUpdate();
            treeViewToSort.Nodes.Clear();
            foreach (TreeNode childNode in list)
            {
                treeViewToSort.Nodes.Add(childNode);
            }
            treeViewToSort.EndUpdate();
        }

        private class CompareTreeNodes : IComparer
        {
            // Compare the length of the strings, or the strings 
            // themselves, if they are the same length. 
            public int Compare(object x, object y)
            {
                TreeNode tx = x as TreeNode;
                TreeNode ty = y as TreeNode;

                // Compare the length of the strings, returning the difference. 
                //if (tx.Text.Length != ty.Text.Length)
                //    return tx.Text.Length - ty.Text.Length;

                // If they are the same length, call Compare. 
                return string.Compare(tx.Text, ty.Text);
            }
        }

        public void FillDataGridViewVanilla(DataGridView dataGridViewGroupVanilla, XDocument xv, string groupId)
        {
            try
            {
                XElement vGroup = (from van in xv.Elements("Groups").Elements("Items").Elements("Group")
                                   where van.Element("GroupId").Value.ToString().Trim() == groupId
                                   select van).FirstOrDefault();

                dataGridViewGroupVanilla.Rows.Clear();
                if (vGroup != null)
                {
                    foreach (XElement vGroupChild in vGroup.Elements())
                    {
                        if (vGroupChild.Name.ToString().Trim() != "GroupId")
                        {
                            dataGridViewGroupVanilla.Rows.Add(vGroupChild.Name.ToString(), vGroupChild.Value.ToString().Trim());
                        }
                    }
                }
            }
            catch
            {
            }
        }
    }
}
