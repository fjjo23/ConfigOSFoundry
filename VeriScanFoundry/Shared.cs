using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Windows.Forms;
using System.Drawing;

namespace VeriSignature
{
    public static class Shared
    {
        public static string GetElementName(string element)
        {
            return element.Substring(0, element.IndexOf(":"));
        }

        public static string GetElementValue(string element)
        {
            return element.Substring(element.IndexOf(":") + 2, element.Length - element.IndexOf(": ") - 2);
        }

        public static bool CompareTwoElements(XElement elementA, XElement elementB)
        {
            bool isDifferent = false;
            try
            {
                foreach (XElement alementAchild in elementA.Elements())
                {
                    XElement elementBchild = elementB.Element(alementAchild.Name);
                    if (elementBchild != null)
                    {
                        if (alementAchild.Value.ToString().Trim() != elementBchild.Value.ToString().Trim())
                        {
                            isDifferent = true;
                        }
                    }
                }
            }
            catch
            {
            }
            return isDifferent;
        }

        public static XElement FindGroupInDocument(XDocument xb, string goupId)
        {
            XElement retElement = null;
            try
            {
                retElement = (from ban in xb.Elements("Groups").Elements("Items").Elements("Group")
                             where ban.Element("GroupId").Value.ToString().Trim() == goupId
                             select ban).FirstOrDefault();
            }
            catch
            {
            }
            return retElement;
        }

        public static TreeNode FindNodeInTreeView(TreeView treeView, string groupId)
        {
            TreeNode retNode = null;
            try
            {
                foreach (TreeNode node in treeView.Nodes)
                {
                    if (GetElementValue(node.Text.ToString()).Trim() == groupId)
                    {
                        retNode = node;
                        break;
                    }
                }
            }
            catch
            {
            }
            return retNode;
        }

        public static void CompareVanillaGroupToCurrentGroupHighlightDifferences(TreeNode selectedNode, DataGridView dataGridViewGroupVanilla)
        {
            try
            {
                foreach (TreeNode node in selectedNode.Nodes)
                {
                    foreach (DataGridViewRow rowv in dataGridViewGroupVanilla.Rows)
                    {
                        if (Shared.GetElementName(node.Text) == rowv.Cells[0].Value.ToString().Trim())
                        {
                            if (Shared.GetElementValue(node.Text) != rowv.Cells[1].Value.ToString().Trim())
                            {
                                if (Shared.GetElementName(node.Text).ToUpper() == "IGNORE")
                                {
                                    if ((Shared.GetElementValue(node.Text) == "" || Shared.GetElementValue(node.Text) == "0") && (rowv.Cells[1].Value.ToString().Trim() == "" || rowv.Cells[1].Value.ToString().Trim() == "0"))
                                    {
                                        // OK
                                    }
                                    else
                                    {
                                        rowv.Cells[1].Style.BackColor = Color.Yellow;
                                    }

                                }
                                else
                                {
                                    rowv.Cells[1].Style.BackColor = Color.Yellow;
                                }
                            }
                            break;
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
