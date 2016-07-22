using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace VeriSignature
{
    class Group
    {
        private string errorMessage;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set { errorMessage = value; }
        }

        private List<string> groupElements = new List<string>();
        public List<string> GroupElements
        {
            get { return groupElements; }
            set { groupElements = value; }
        }

        //private List<string> valueRegExps = new List<string>();
        //public List<string> ValueRegExps
        //{
        //    get { return valueRegExps; }
        //    set { valueRegExps = value; }
        //}

        //private List<string> hives = new List<string>();
        //public List<string> Hives
        //{
        //    get { return hives; }
        //    set { hives = value; }
        //}

        private Dictionary<string, string> groupPairs = new Dictionary<string,string>();
        public Dictionary<string, string> GroupPairs
        {
            get { return groupPairs; }
            set { groupPairs = value; }
        }

        //
        public Group()
        {
            groupElements.Add("GroupId");
            groupElements.Add("GroupTitle");
            groupElements.Add("RuleId");
            groupElements.Add("Severity");
            groupElements.Add("RuleVersion");
            groupElements.Add("RuleTitle");
            groupElements.Add("Where");
            groupElements.Add("Applied");
            groupElements.Add("Type");
            groupElements.Add("Value");
            groupElements.Add("Ignore");
            groupElements.Add("IgnoreReason");
            //
            //valueRegExps.Add(@"\[Min\.\.\.\d+\],\d+");
            //valueRegExps.Add(@"\[Max\.\.\.\d+\],\d+");
            //valueRegExps.Add(@"\[\S+\|\S+\],\S+");
            //valueRegExps.Add(@"\[\S+\s\|\s\S+\],\S+");
            //valueRegExps.Add(@"\[\d+\.\.\.\d+\],\d+");
            //valueRegExps.Add(@"\[!\S+\],\S+");
            ////
            //hives.Add("HKLM");
            //hives.Add("HKCU");
            //hives.Add("HKU");
            //hives.Add("MACHINE");
            //
            foreach (string groupElement in groupElements)
            {
                if (groupElement != "GroupId")
                {
                    groupPairs.Add(groupElement, "");
                }
            }
        }

        public string SaveGroup(TreeView myTreeView, string myGroupId, bool isUpdate, XDocument xv, XDocument xb, TreeView treeViewSource, DataGridView dataGridViewGroupVanilla)
        {
            errorMessage = "";
            TreeNodeCollection newTreeNodeCollection = myTreeView.Nodes;
            TreeNodeCollection treeViewSourceCollection = treeViewSource.Nodes;
            if (isUpdate)
            {
                string nodeToDelete = "";
                bool swFound = false;

                foreach (TreeNode node in newTreeNodeCollection)
                {
                    try
                    {
                        nodeToDelete = Shared.GetElementValue(node.Text);
                        if (nodeToDelete.Trim() == myGroupId)
                        {
                            // Start "EditControls" ver: 1.0.8 date: 03-03-16
                            int nodeIndex = node.Index;
                            // End "EditControls" ver: 1.0.8 date: 03-03-16
                            // Remove old Group Id
                            newTreeNodeCollection.Remove(node);
                            swFound = true;
                            // Add the updated Group Id
                            XElement group = FillNewXelement(myGroupId, groupPairs);
                            TreeNode groupParentNode = FillNewTreeNode(myGroupId, groupPairs);
                            // Compares current group with base and source
                            string nodeTag = "";
                            if (xv != null)
                            {
                                // Compares with source
                                bool isDiffFromSource = CompareGroups(myGroupId, group, xb);
                                // Compares with base
                                bool isDiffFromBase = CompareGroups(myGroupId, group, xv);
                                if (isDiffFromSource && isDiffFromBase)
                                {
                                    nodeTag = "DiffFromSourceAndBase";
                                }
                                else
                                {
                                    if (isDiffFromSource)
                                    {
                                        nodeTag = "DiffFromSource";
                                    }
                                    else
                                    {
                                        if (isDiffFromBase)
                                        {
                                            nodeTag = "DiffFromBase";
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // Compares with source
                                if (CompareGroups(myGroupId, group, xb))
                                {
                                    nodeTag = "DiffFromSource";
                                }
                                else
                                {
                                    nodeTag = "NoDiff";
                                }
                            }
                            groupParentNode.Tag = nodeTag;
                            // Start "EditControls" ver: 1.0.8 date: 03-03-16
                            //newTreeNodeCollection.Add(groupParentNode);
                            newTreeNodeCollection.Insert(nodeIndex, groupParentNode);
                            // End "EditControls" ver: 1.0.8 date: 03-03-16
                            // Compare dataGridViewVanilla with current group to highlight the differences
                            //Shared.CompareVanillaGroupToCurrentGroupHighlightDifferences(groupParentNode, dataGridViewGroupVanilla);
                            break;
                        }
                    }
                    catch (Exception err)
                    {
                        errorMessage = "There was a problem when trying to update " + myGroupId + "." + err.Message;
                    }
                }
                if (!swFound)
                {
                    errorMessage = myGroupId + " was not updated because it was not found.";
                }
            }
            else
            {
                try
                {
                    bool doUpdate = true;
                    // Check if the group already exist on the target treeview
                    foreach (TreeNode node in newTreeNodeCollection)
                    {
                        if (Shared.GetElementValue(node.Text) == myGroupId)
                        {
                            errorMessage = "The group " + myGroupId + " already exist.";
                            doUpdate = false;
                            break;
                        }
                    }
                    // Check if the group already exist on the source treeview
                    foreach (TreeNode node in treeViewSourceCollection)
                    {
                        if (Shared.GetElementValue(node.Text) == myGroupId)
                        {
                            errorMessage = "The group " + myGroupId + " already exist on the source signature.";
                            doUpdate = false;
                            break;
                        }
                    }
                    if (doUpdate)
                    {
                        // Add the new Group Id
                        TreeNode groupParentNode = FillNewTreeNode(myGroupId, groupPairs);
                        if (xv != null)
                        {
                            XElement group = FillNewXelement(myGroupId, groupPairs);
                            // Compares with base
                            bool isDiffFromBase = CompareGroups(myGroupId, group, xv);
                            //
                            if (isDiffFromBase)
                            {
                                groupParentNode.Tag = "DiffFromSourceAndBase";
                            }
                            else
                            {
                                groupParentNode.Tag = "DiffFromSource";
                            }
                        }
                        else
                        {
                            groupParentNode.Tag = "DiffFromSource";
                        }
                        //
                        newTreeNodeCollection.Add(groupParentNode);
                    }
                }
                catch (Exception err)
                {
                    errorMessage = "The new group " + myGroupId + " was not added." + err.Message;
                }
            }
            return errorMessage;
        }

        private bool CompareGroups(string myGroupId, XElement group, XDocument xz)
        {
            bool isDifferent = false;
            try
            {
                XElement zGroup = Shared.FindGroupInDocument(xz, myGroupId);
                isDifferent = false;
                if (zGroup == null)
                {
                    isDifferent = true;
                }
                else
                {
                    isDifferent = Shared.CompareTwoElements(group, zGroup);
                }
            }
            catch
            {
            }
            return isDifferent;
        }

        //public string ValidateGroupInput(string category, string where, string type, string value, bool ignoreImport, bool ignoreExport, bool ignoreCase, string ignore, string ignoreReason)
        //{
        //    errorMessage = "";
        //    try
        //    {
        //        if (category.Length > 0)
        //        {
        //            if (category.ToUpper() != "NO VALIDATION")
        //            {
        //                switch (category.ToUpper())
        //                {
        //                    case "REGISTRY":
        //                        if (errorMessage.Length < 1)
        //                        {
        //                            // Validates where for registry path
        //                            if (where.Length > 0)
        //                            {
        //                                int countBackSlash = where.Split('\\').Length - 1;
        //                                bool countDoubleBackSlash = where.Contains(@"\\");
        //                                if (countBackSlash > 2 && where.EndsWith(@"\") && !where.Contains(@"\\"))
        //                                {
        //                                    string hive = where.Substring(0, where.IndexOf(@"\"));
        //                                    if (hive.Length > 0 && (hives.Contains(hive)))
        //                                    {
        //                                        // OK
        //                                    }
        //                                    else
        //                                    {
        //                                        errorMessage = "The registry path must start with one of the following: " + string.Join(",", hives) + ".";
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    errorMessage = "The format of the registry path on 'Where' is invalid.";
        //                                }
        //                            }
        //                            else
        //                            {
        //                                errorMessage = "Please enter a registry path on 'Where'.";
        //                            }
        //                        }
        //                        if (errorMessage.Length < 1)
        //                        {
        //                            // Validates value type
        //                            if (type.Length < 1)
        //                            {
        //                                errorMessage = "Please selecte a type.";
        //                            }
        //                        }
        //                        break;
        //                    default:

        //                        break;

        //                }
        //                // Validation for all categories
        //                if (errorMessage.Length < 1)
        //                {
        //                    // Validates value
        //                    if (value.Contains("["))
        //                    {
        //                        bool isValid = false;
        //                        foreach (string valueRegExp in valueRegExps)
        //                        {
        //                            Regex rx = new Regex(valueRegExp);
        //                            // Find matches.
        //                            MatchCollection matches = rx.Matches(value);
        //                            if (matches.Count > 0)
        //                            {
        //                                isValid = true;
        //                                break;
        //                            }
        //                        }
        //                        if (!isValid)
        //                        {
        //                            errorMessage = "The format of the value is not valid.";
        //                        }
        //                    }
        //                }
        //                // Validation of the ignore reason
        //                if (errorMessage.Length < 1)
        //                {
        //                    if (ignoreImport || ignoreExport || ignoreCase || ignore.Length > 0)
        //                    {
        //                        if (ignoreReason.Length < 1)
        //                        {
        //                            errorMessage = "Please enter a reason to ignore.";
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            errorMessage = "Please select a category.";
        //        }
        //    }
        //    catch
        //    {
        //    }
        //    return errorMessage;
        //}

        private TreeNode FillNewTreeNode(string groupId, Dictionary<string,string> groupPairs)
        {
            TreeNode groupParentNode = new TreeNode("GroupId: " + groupId);
            try
            {
                foreach (KeyValuePair<string, string> groupPair in groupPairs)
                {
                    groupParentNode.Nodes.Add(new TreeNode(groupPair.Key + ": " + groupPair.Value));
                }
            }
            catch
            {
            }
            return groupParentNode;
        }

        private XElement FillNewXelement(string myGroupId, Dictionary<string, string> groupPairs)
        {
            XElement group = new XElement("Group");
            XElement groupId = new XElement("GroupId", myGroupId);
            group.Add(groupId);
            try
            {
                foreach (KeyValuePair<string, string> groupPair in groupPairs)
                {
                    group.Add(new XElement(groupPair.Key, groupPair.Value));
                }
            }
            catch
            {
            }
            return group;
        }
    }
}
