using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using SCGoodUtilsClass;

namespace VeriSignature
{
    public partial class KeysForm : Form
    {
        string msgBoxTitle = "";

        public KeysForm(string _msgBoxTitle)
        {
            msgBoxTitle = _msgBoxTitle;
            InitializeComponent();
        }

        private void KeysForm_Load(object sender, EventArgs e)
        {
            //string[] currentKeys = Directory.GetFiles(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\", "FoundryKey_*.key");
            //if (currentKeys.Length > 0)
            //{
            //    rbtBrowseKey.Enabled = false;
            //    rbtGenerateKey.Enabled = false;
            //    grpGenerateKey.Enabled = false;
            //    grpSelectKey.Enabled = false;
            //    btnOK.Enabled = false;
            //    lblCurrentKey.Visible = true;
            //    lblKey.Visible = true;
            //    if (currentKeys.Length > 1)
            //    {
            //        lblCurrentKey.Text = "ERROR. There is more than one key";
            //    }
            //    else
            //    {
            //        lblCurrentKey.Text = Path.GetFileName(currentKeys[0]);
            //    }
            //}
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rbtBrowseKey_CheckedChanged(object sender, EventArgs e)
        {
            grpSelectKey.Enabled = true;
            grpGenerateKey.Enabled = false;
        }

        private void rbtGenerateKey_CheckedChanged(object sender, EventArgs e)
        {
            grpGenerateKey.Enabled = true;
            grpSelectKey.Enabled = false;
        }

        private void btnBrowseKey_Click(object sender, EventArgs e)
        {
            openFileDialog1.DefaultExt = "Fkey";
            openFileDialog1.Filter = "FoundyKey file (FoundryKey_*.key)|" + "FoundryKey_*.key";
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtExistingKey.Text = openFileDialog1.FileName;
                //
            }
        }

        private void btnBrowseNewKey_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtNewKeyFolder.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            #region Browse for a key

            if (rbtBrowseKey.Checked)
            {
                if (!File.Exists(txtExistingKey.Text))
                {
                    MessageBox.Show("Please select an existing file.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // Verify Foundry key by decrypting it using SCLD key for Foundry
                //
                
                Key kcb = new Key();
                SCCryptography sccryb = new SCCryptography();
                Foundry fdb = new Foundry();
                string signaturesFolder = Environment.GetEnvironmentVariable("temp") + @"\signatures\";
                // Remove old Signatures folder from the user's temp folder
                fdb.RemoveSignatureFolder(signaturesFolder);
                // Create new Signatures folder in the user's temp folder
                fdb.CreateSignatureFolder(signaturesFolder);
                // Get SC encryption keys
                kcb.EncryptionKeysFoundry();
                // Dencrypt the file
                string decryptedOutFile = signaturesFolder + Path.GetFileNameWithoutExtension(txtExistingKey.Text) + "Dec.key";
                bool decrypt = sccryb.DecryptFileAES(txtExistingKey.Text, decryptedOutFile, kcb.AesKey, kcb.AesKeyIV);
                if (decrypt)
                {
                    // This is a valid key
                    // Remove the non encrypted file if exists
                    fdb.RemoveFile(decryptedOutFile);
                }
                else
                {
                    MessageBox.Show("The selected key is not a valid ConfigOS Foundry key.\nPlease make sure to select a valid ConfigOS Foundry key.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // Copy the existing Foundrykey to the application folder
                try
                {
                    // Remove any existing FoundryKeys from the application folder
                    string[] foundryKeys = Directory.GetFiles(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\", "FoundryKey_*.key");
                    foreach (string foundryKey in foundryKeys)
                    {
                        fdb.RemoveFile(foundryKey);
                    }
                    File.Copy(txtExistingKey.Text, Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\" + Path.GetFileName(txtExistingKey.Text), true);
                }
                catch(Exception err)
                {
                    MessageBox.Show("The Foundry Key was not successfully copied to the Foundry application folder.\n" + err.Message, msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                MessageBox.Show("The Foundry Key was successfully selected.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }

            #endregion

            #region Generate new key

            if (rbtGenerateKey.Checked)
            {
                if (!Directory.Exists(txtNewKeyFolder.Text))
                {
                    MessageBox.Show("Please select an existing folder.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // Generate the key pair (Foundry and client key)
                Foundry fd = new Foundry();
                SCCryptography sccry = new SCCryptography();
                Key kc = new Key();
                // Remove the key files if exist
                fd.RemoveFile(txtNewKeyFolder.Text + @"\FoundryKeyDec.txt");
                fd.RemoveFile(txtNewKeyFolder.Text + @"\ClientKeyDec.txt");
                // Generate new Foundry key
                bool retGenerateKey = kc.GenerateKey(txtNewKeyFolder.Text + @"\FoundryKeyDec.txt", "16", "8");
                if (!retGenerateKey)
                {
                    MessageBox.Show(fd.ErrorMessage, msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // Client key 
                try
                {
                    File.Copy(txtNewKeyFolder.Text + @"\FoundryKeyDec.txt", txtNewKeyFolder.Text + @"\ClientKeyDec.txt", true);
                }
                catch(Exception err)
                {
                    MessageBox.Show("The Client key was not successfully created.\n" + err.Message , msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // Encrypt the key pair using SCLD keys for Foundry and client
                //
                // Foundry key
                // Get SC encryption keys
                kc.EncryptionKeysFoundry();
                string currentDateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                // Encrypt the file
                bool encryp = sccry.EncryptFileAES(txtNewKeyFolder.Text + @"\FoundryKeyDec.txt", txtNewKeyFolder.Text + @"\FoundryKey_" + currentDateTime + ".key", kc.AesKey, kc.AesKeyIV);
                if (encryp)
                {
                    // Remove the non encrypted file if exists
                    fd.RemoveFile(txtNewKeyFolder.Text + @"\FoundryKeyDec.txt");
                }
                else
                {
                    MessageBox.Show("The encryption of the Foundry Key failed.\n" + sccry.ErrorMessage, msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // Client key
                // Get encryption keys
                kc.EncryptionKeysClient();
                // Encrypt the file
                encryp = sccry.EncryptFileAES(txtNewKeyFolder.Text + @"\ClientKeyDec.txt", txtNewKeyFolder.Text + @"\ClientKey_" + currentDateTime + ".key", kc.AesKey, kc.AesKeyIV);
                if (encryp)
                {
                    // Remove the non encrypted file if exists
                    fd.RemoveFile(txtNewKeyFolder.Text + @"\ClientKeyDec.txt");
                }
                else
                {
                    MessageBox.Show("The encryption of the Client Key failed.\n" + sccry.ErrorMessage, msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // Copy the generated FoundryKey to the application folder
                try
                {
                    // Remove any existing FoundryKeys from the application folder
                    string[] foundryKeys = Directory.GetFiles(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\", "FoundryKey_*.key");
                    foreach (string foundryKey in foundryKeys)
                    {
                        fd.RemoveFile(foundryKey);
                    }
                    File.Copy(txtNewKeyFolder.Text + @"\FoundryKey_" + currentDateTime + ".key", Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\FoundryKey_" + currentDateTime + ".key", true);
                }
                catch(Exception err)
                {
                    MessageBox.Show("The Foundry key was not successfully copied to the Foundry application folder.\n" + err.Message, msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                MessageBox.Show("The ConfigOS Key pair was successfully created.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }

            #endregion
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Hello");
        }
     }
}
