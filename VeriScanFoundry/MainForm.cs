using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Ionic.Zip;
using System.Security.Cryptography;
using SCGoodUtilsClass;
using Microsoft.Win32;
using System.Diagnostics;
using System.Xml;
using System.Xml.Linq;
using System.Runtime.InteropServices;
using ValidateXMLSchema;
// Start "ProductLicense" ver: 1.0.7 date: 04-20-15
using LogicNP.CryptoLicensing;
// End "ProductLicense" ver: 1.0.7 date: 04-20-15

namespace VeriSignature
{
    public partial class MainForm : Form
    {

        #region Variables
        Foundry fd = new Foundry();
        string msgBoxTitle = "SteelCloud ConfigOS Foundry";
        string workSignaturesFolder = "";
        string workSignaturesFolderP2 = "";
        string mainErrorMessage = "";
        string cosPreferencesFile = "";
        string signatureAesKey = "";
        string signatureAesKeyIV = "";
        string appPath = "";
        // Start modifications 07-30-14 ver 1.0.4 Signature Builder tab
        // Start "FilterBug" ver: 1.0.9 date: 04-26-16
        //TreeViewMng tvm;
        TreeViewMng tvm = new TreeViewMng();
        // End "FilterBug" ver: 1.0.9 date: 04-26-16
        XDocument xv;
        XDocument xb;
        bool swUpd = false;
        List<string> regHives = new List<string>();
        List<string> regTypes = new List<string>();
        List<string> cosCategories = new List<string>();
        string osTypeMaster = "";
        string osTypeSource = "";
        // End modifications 07-30-14 ver 1.0.4 Signature Builder tab

        // Start modifications ver: 1.0.6 date: 02-18-15
        string btnFilterToolTip = "";
        // End modifications ver: 1.0.6 date: 02-18-15
        // Start "BuilderEnhancements" ver: 1.0.8 date: 01-13-16
        bool newSignatureChanges = false;
        // End "BuilderEnhancements" ver: 1.0.8 date: 01-13-16

        // Start "NewFilterForm" ver: 1.0.9 date: 03-15-16
        List<FilterClass> filtersCls = new List<FilterClass>();
        // End "NewFilterForm" ver: 1.0.9 date: 03-15-16

        // Start "LinuxValidation" ver: 1.0.9 date: 05-09-16
        LinuxValidation linuxVal;
        // End "LinuxValidation" ver: 1.0.9 date: 05-09-16
        #endregion

        #region MainForm

        public MainForm()
        {
            // Enable FIPS compliance
            EnableFIPSCompliance();
            //
            InitializeComponent();
            // Start "ProductLicense" ver: 1.0.7 date: 05-18-15
            this.tabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;
            this.tabControl1.DrawItem += new DrawItemEventHandler(DisableTab_DrawItem);
            this.tabControl1.Selecting += new TabControlCancelEventHandler(DisableTab_Selecting);
            // End "ProductLicense" ver: 1.0.7 date: 05-18-15

            // Start "UnzipSCLDFile" ver: 1.0.9 date: 03-24-16
            tabControl1.TabPages.Remove(FoundryPreferences);
            // End "UnzipSCLDFile" ver: 1.0.9 date: 03-24-16
            newTextDocumentToolBarButton.Image = imageList.Images[0];
            openDocumentToolBarButton.Image = imageList.Images[1];
            saveDocumentToolBarButton.Image = imageList.Images[2];
            cutToolBarButton.Image = imageList.Images[5];
            copyToolBarButton.Image = imageList.Images[6];
            pasteToolBarButton.Image = imageList.Images[7];
            deleteToolBarButton.Image = imageList.Images[8];
            fileNewTextDocumentMenuItem.Image = imageList.Images[0];
            fileOpenDocumentMenuItem.Image = imageList.Images[1];
            fileSaveDocumentMenuItem.Image = imageList.Images[2];
            editCutMenuItem.Image = imageList.Images[5];
            editCopyMenuItem.Image = imageList.Images[6];
            editPasteMenuItem.Image = imageList.Images[7];
            editDeleteMenuItem.Image = imageList.Images[8];
            //
            this.Text = msgBoxTitle;
            try
            {
                ((ToolStripMenuItem)mainMenu.Items["helpToolStripMenuItem"]).DropDownItems["helpAboutMenuItem"].Text = "About " + msgBoxTitle;
            }
            catch
            {
            }
            // Start modifications 07-30-14 ver 1.0.4 Siagnature Admin tab
            treeViewBase.DrawMode = TreeViewDrawMode.OwnerDrawText;
            treeViewBase.DrawNode += new DrawTreeNodeEventHandler(tree_DrawNode);
            treeViewNew.DrawMode = TreeViewDrawMode.OwnerDrawText;
            treeViewNew.DrawNode += new DrawTreeNodeEventHandler(tree_DrawNode1);
            txtGroupId.Enabled = false;
            // End modifications 07-30-14 ver 1.0.4 Siagnature Admin tab

            // Start modifications 08-21-14 ver 1.0.4
            InitializeComboBoxes();
            txtBaseSignature.BackColor = Color.White;
            // End modifications 08-21-14 ver 1.0.4
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Start "ProductLicense" ver: 1.0.7 date: 04-20-15
            //
            // Validate license
            //
            // Check if product license file exists on the application installation folder
            string cosFoundryLicensePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Application.ExecutablePath), "COSFoundryLicense.lic");
            //
            //
            if (!File.Exists(cosFoundryLicensePath))
            {
                ProductLicenseForm plf = new ProductLicenseForm(msgBoxTitle, cosFoundryLicensePath);
                plf.ShowDialog();
            }
            // Re-check license existance
            if (File.Exists(cosFoundryLicensePath))
            {
                //
                // Validate the product license
                //
                CryptoLicense license = ProductLicense.Methods.CreateLicense();

                license.StorageMode = LicenseStorageMode.ToFile;
                license.FileStoragePath = cosFoundryLicensePath;

                // Load the license from file 
                if (license.Load())
                {
                    EvaluationInfoDialog dialog = new EvaluationInfoDialog(license);
                    dialog.ProductName = "ConfigOS Foundry";
                    dialog.PurchaseURL = "HTTP://www.steelcloud.com";
                    if (license.HasDateExpires && license.Status == LicenseStatus.Expired)
                    {
                        dialog.UseDateExpires = true;
                    }

                    if (dialog.ShowDialogInt()== false)
                    {
                        // license has expired, new license entered is also expired 
                        // or user choose the 'Exit Program' option 

                        // In your app, you may wish to exit app when eval license has expired
                        Application.Exit();
                    }
                    else
                    {
                        // The current license is valid or the new license entered is valid 
                        // or the user choose the 'Continue Evaluation' option 

                        // If the user enters a new valid license code, it replaces the existing code  
                        // and is automatically saved to the currently specified  
                        // storage medium (registry in this sample) using the CryptoLicense.Save method.  
                        // The new license code is thus available the next time your software runs. 
                        
                        if (license.Status != LicenseStatus.Valid)
                        {
                            MessageBox.Show("ConfigOS Foundry license validation failed.\nThe application will be closed.\n" + license.GetAllStatusExceptionsAsString() + "\nPlease select a correct license file and restart the application.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            license.Dispose();
                            Process.GetCurrentProcess().Kill();
                        }
                        else
                        {
                            ProductLicense.Runtime.CosFoundryFeature1 = license.IsFeaturePresentEx(1);
                            ProductLicense.Runtime.CosFoundryFeature2 = license.IsFeaturePresentEx(2);
                            ProductLicense.Runtime.CosFoundryFeature3 = license.IsFeaturePresentEx(3);
                            ProductLicense.Runtime.CosFoundryFeature4 = license.IsFeaturePresentEx(4);
                            ProductLicense.Runtime.CosFoundryFeature5 = license.IsFeaturePresentEx(5);
                            ProductLicense.Runtime.CosFoundryFeature6 = license.IsFeaturePresentEx(6);
                            ProductLicense.Runtime.CosFoundryFeature7 = license.IsFeaturePresentEx(7);
                            ProductLicense.Runtime.CosFoundryFeature8 = license.IsFeaturePresentEx(8);
                            ProductLicense.Runtime.CosFoundryFeature9 = license.IsFeaturePresentEx(9);
                            ProductLicense.Runtime.CosFoundryFeature10 = license.IsFeaturePresentEx(10);

                            ProductLicense.Runtime.LicenceExpitationDate = license.DateExpires.ToString("MM/dd/yyyy");
                            if (license.IsEvaluationLicense())
                            {
                                if (license.HasDateExpires)
                                {
                                    ProductLicense.Runtime.LicenceExpitationDate = license.DateExpires.ToString("MM/dd/yyyy") + ". " + license.RemainingUsageDays.ToString().Trim() + " Evaluation remaining usage days";
                                }
                                else
                                {
                                    ProductLicense.Runtime.LicenceExpitationDate = license.RemainingUsageDays.ToString() + " Remaining usage days";
                                }
                            }

                        }
                        
                    }

                    license.Dispose(); // Be sure to call Dispose during app exit or when done using the CryptoLicense object

                }
                else
                {
                    MessageBox.Show("ConfigOS Foundry license not found.\nThe application will be closed.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    license.Dispose();
                    Process.GetCurrentProcess().Kill();
                }
                //
            }
            else
            {
                MessageBox.Show("ConfigOS Foundry license not found.\nThe application will be closed.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Process.GetCurrentProcess().Kill();
            }
            //
            //
            //
            // End "ProductLicense" ver: 1.0.7 date: 04-20-15
            //
            ToolTip btnToolTip = new ToolTip();
            btnToolTip.SetToolTip(this.btnSelectFile, "Add files");
            txtSignaturesContainerName.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Signatures.zip";
             //
            // Get updates location information
            //
            // Check if COSPreferences.xml file exist
            appPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            cosPreferencesFile = appPath + @"\Resources\COSFoundryPreferences.xml";
            if (!File.Exists(cosPreferencesFile))
            {
                MessageBox.Show("The " + msgBoxTitle + " file '" + cosPreferencesFile + "' does not exist.\n" + msgBoxTitle + " updates will not be checked.");
            }
            else
            {
                // Read and update log directory info
                string cosUpdatesDirectory = COSPreferencesRead(cosPreferencesFile, "COSFoundryPreferences/Updates", "Directory");
                if (cosUpdatesDirectory.Length < 1)
                {
                    COSPreferencesUpdate(cosPreferencesFile, "COSFoundryPreferences/Updates", "Directory", appPath + @"\Updates");
                    cosUpdatesDirectory = appPath + @"\Updates";
                }
                if (cosUpdatesDirectory.Length > 0)
                {
                    updateController1.UpdateLocation = cosUpdatesDirectory;
                }
                // Checks for updates
                try
                {
                    updateController1.CheckForUpdate();
                    if (updateController1.CurrentUpdate != null)
                    {
                        // Disable FIPS compliance
                        DisableFIPSCompliance();
                        //
                        updateController1.UpdateInteractive();
                        // Update was cancel
                        // Enable FIPS compliance
                        EnableFIPSCompliance();
                    }
                }
                catch
                {
                    // Checks for updates failed
                    MessageBox.Show(msgBoxTitle + " was unable to contact updates at\n( " + updateController1.UpdateLocation + " )\nYou can manually download updates or changed the updates location on the Preferences tab.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            //string myAppName = "VeriScanFoundry";
            //// Saves deafult update location
            //string defaultUpdateLocation = updateController1.UpdateLocation;
            //string defaultUpdateLocationUserName = updateController1.UpdateLocationUserName;
            //string defaultUpdateLocationPassword = updateController1.UpdateLocationPassword;
            //// Reads new location from registry
            //string updatesLocation = "";
            //string registryPath = @"Software\SCLD\VeriScan\Updates\";
            //RegistryKey rkey = Registry.LocalMachine.OpenSubKey(registryPath + myAppName);
            //if (rkey != null)
            //{
            //    if (rkey.GetValue("Location") != null)
            //    {
            //        updatesLocation = rkey.GetValue("Location").ToString();
            //    }
            //}
            //if (updatesLocation.Length > 0)
            //{
            //    updateController1.UpdateLocation = updatesLocation;
            //    updateController1.UpdateLocationUserName = "";
            //    updateController1.UpdateLocationPassword = "";
            //}
            //// Checks for updates
            //try
            //{
            //    updateController1.CheckForUpdate();
            //    if (updateController1.CurrentUpdate != null)
            //    {
            //        updateController1.UpdateInteractive();
            //    }
            //}
            //catch
            //{
            //    // Checks for updates failed
            //    DialogResult dlgResult = MessageBox.Show("SteelCloud VeriScanFoundry  was unable to contact updates server at\n( " + updateController1.UpdateLocation + " )\nYou can manually download updates from support.steelcloud.com" + "\n\nWould you like to try a diferent location?", msgBoxTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //    if (dlgResult == DialogResult.Yes)
            //    {
            //        BaseUpdatesForm.BaseUpdatesForm buf = new BaseUpdatesForm.BaseUpdatesForm();
            //        buf.MyUpdateController = updateController1;
            //        buf.MyFormTitle = msgBoxTitle;
            //        buf.MyMessageBoxTitle = msgBoxTitle;
            //        buf.MyAppName = myAppName;
            //        buf.MyDefaultUpdateLocation = defaultUpdateLocation;
            //        buf.MyDefaultUpdateLocationUserName = defaultUpdateLocationUserName;
            //        buf.MyDefaultUpdateLocationPassword = defaultUpdateLocationPassword;
            //        buf.Text = msgBoxTitle;
            //        buf.MyRegistryPath = registryPath;
            //        //
            //        buf.ShowDialog();
            //    }
            //    else if (dlgResult == DialogResult.No)
            //    {
            //        // No, stop
            //    }
            //}

            //------- All this section need to be activated to work with keys generation -------------------------------//
            // Start modifications KEYS 05-22-13
            // Verify if the FoundryKey_yyyymmddHHMMSS.key exist
            // If the key exists verify if it is a good one by decrypting the file using the SteelCloud known key
            //
            //
            SCCryptography sccry1 = new SCCryptography();
            workSignaturesFolder = Environment.GetEnvironmentVariable("temp") + @"\signatures\";
            //
            string[] currentKeys = Directory.GetFiles(appPath + @"\", "FoundryKey_*.key");
            if (currentKeys.Length == 0)
            {
                KeysForm kf = new KeysForm(msgBoxTitle);
                kf.ShowDialog();
            }
            //
            try
            {
                string foundryKeyFile = "";
                // Verify if the FoundryKey_yyyymmddHHMMSS.key exist. It must be only one key
                currentKeys = Directory.GetFiles(appPath + @"\", "FoundryKey_*.key");
                if (currentKeys.Length > 0)
                {
                    if (currentKeys.Length > 1)
                    {
                        MessageBox.Show("There is more than one ConfigOS Foundry key to encrypt the signature files.\nThe process cannot continue.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Process.GetCurrentProcess().Kill();
                    }
                    foundryKeyFile = currentKeys[0];
                }
                else
                {
                    MessageBox.Show("There is no ConfigOS Foundry key to encrypt the signature files.\nThe process cannot continue.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Process.GetCurrentProcess().Kill();
                }
                //
                // Verify if this is a good FoundryKey by decrypting it using the SteelCloud known key
                //
                Key kcb = new Key();
                // Remove old Signatures folder from the user's temp folder
                fd.RemoveSignatureFolder(workSignaturesFolder);
                // Create new Signatures folder in the user's temp folder
                fd.CreateSignatureFolder(workSignaturesFolder);
                // Get Foundry encryption keys
                kcb.EncryptionKeysFoundry();
                string decryptedOutFile = workSignaturesFolder + Path.GetFileNameWithoutExtension(foundryKeyFile) + "Dec.key";
                // Decrypt the FoundryKey file
                bool retDecrypt = sccry1.DecryptFileAES(foundryKeyFile, decryptedOutFile, kcb.AesKey, kcb.AesKeyIV);
                if (retDecrypt)
                {
                    // Read the decrypted FoundryKey file and save the containig key to encrypt signatures
                    StreamReader re = new StreamReader(decryptedOutFile);
                    string input = null;
                    int ccLines = 0;
                    while ((input = re.ReadLine()) != null)
                    {
                        switch (ccLines)
                        {
                            case 0:
                                signatureAesKey = input.Trim();
                                break;
                            case 1:
                                signatureAesKeyIV = input.Trim();
                                break;
                        }
                        ccLines++;
                    }
                    re.Close();
                    fd.RemoveFile(decryptedOutFile);
                    if (signatureAesKey.Length != 32 || signatureAesKeyIV.Length != 16)
                    {
                        MessageBox.Show("The FoundryKey file '" + Path.GetFileName(foundryKeyFile) + "' does not contain the right information.\nPlease make sure you are using a valid FoundryKey file.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Unable to decrypt the FoundryKey file '" + Path.GetFileName(foundryKeyFile) + "'.\nPlease make sure you are using a valid FoundryKey file.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("Error decrypting the FoundryKey file.\n" + err.Message, msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // End modifications KEYS 05-22-13
            //
            //
            //---------------------------------------------------------------------------------------------------------//
            // Start "ProductLicense" ver: 1.0.7 date: 04-29-15
            // Enable/disable license features
            if (!ProductLicense.Runtime.CosFoundryFeature1) // Signature builder Tab
            {
                // Start "ProductLicense" ver: 1.0.7 date: 05-18-15
                //((Control)this.tabSignatureAdmin).Enabled = false;
                tabSignatureAdmin.Enabled = false;
                // End "ProductLicense" ver: 1.0.7 date: 05-18-15
            }
            //
            if (!ProductLicense.Runtime.CosFoundryFeature2) // Create signature container Tab
            {
                // Start "ProductLicense" ver: 1.0.7 date: 05-18-15
                //((Control)this.tabPage1).Enabled = false;
                tabPage1.Enabled = false;
                // End "ProductLicense" ver: 1.0.7 date: 05-18-15
            }
            // End "ProductLicense" ver: 1.0.7 date: 04-29-15

            // Start "ProductLicense" ver: 1.0.7 date: 05-18-15
            // Select the firts enabled tabpage
            int myTabIndex = 0;
            foreach (TabPage tp in tabControl1.TabPages)
            {
                if (tp.Enabled)
                {
                    myTabIndex = tabControl1.TabPages.IndexOf(tp);
                    break;
                }
            }
            tabControl1.SelectedIndex = myTabIndex;
            // End "ProductLicense" ver: 1.0.7 date: 05-18-15
            
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Remove work folder
            fd.RemoveSignatureFolder(workSignaturesFolder);
            fd.RemoveSignatureFolder(workSignaturesFolderP2);
        }

        #endregion

        #region Tab control

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Name == "FoundryPreferences")
            {
                // Get Updates folder
                txtUpdatesNewLocation.Text = COSPreferencesRead(cosPreferencesFile, "COSFoundryPreferences/Updates", "Directory");
            }
            if (tabControl1.SelectedTab.Name == "tabSignatureAdmin")
            {
                // Get Updates folder
                this.Size = new Size(950, 770);
                // Re-center the form after adjustment of form size
                int boundWidth = Screen.PrimaryScreen.Bounds.Width;
                int boundHeight = Screen.PrimaryScreen.Bounds.Height;
                int x = boundWidth - this.Width;
                int y = boundHeight - this.Height;
                // Position the form
                this.Location = new Point(x / 2, y / 2);
                //
                this.MaximizeBox = true;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
                this.Size = new Size(680, 540);
                // Re-center the form after adjustment of form size
                int boundWidth = Screen.PrimaryScreen.Bounds.Width;
                int boundHeight = Screen.PrimaryScreen.Bounds.Height;
                int x = boundWidth - this.Width;
                int y = boundHeight - this.Height;
                // Position the form
                this.Location = new Point(x / 2, y / 2);
                //
                this.MaximizeBox = false;
            }

        }

        #endregion

        # region Create Signatures Container tab

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            // Start modifications 06-11-14 ver 1.0.3
            //openFileDialog1.InitialDirectory = "C:\\";
            // End modifications 06-11-14 ver 1.0.3
            openFileDialog1.Title = "Select an xml file";
            openFileDialog1.Multiselect = true;
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                foreach (String file in openFileDialog1.FileNames)
                {
                    FileInfo fi = new FileInfo(file);
                    // Start modifications 08-21-14 ver 1.0.4
                    // Check if file is a ConfigOS signature
                    if(fd.IsXMLFile(file, "Groups", "ConfigOS"))
                    {
                        // Check the signature OS type (Windows or Linux)
                        XDocument xTmp = XDocument.Load(file);
                        string osTypeTmp = "";
                        if (xTmp.Element("Groups").Attribute("OSType") != null)
                        {
                            osTypeTmp = xTmp.Element("Groups").Attribute("OSType").Value.ToString().ToUpper();
                        }

                        #region check schema and value errors

                        // set error file name
                        string errorFile = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\Validation_" + Path.GetFileNameWithoutExtension(fi.Name) + ".txt";
                        if (File.Exists(errorFile))
                        {
                            try
                            {
                                File.Delete(errorFile);
                            }
                            catch
                            {
                            }
                        }
                        ConfigOSSignatureValidation cosv = new ConfigOSSignatureValidation();
                        cosv.Hives = regHives;
                        cosv.Categories = cosCategories;
                        cosv.RegTypes = regTypes;
                        cosv.ErrorFile = errorFile;
                        // Validate schema
                        string valErrorMessage = "";
                        if (File.Exists(appPath + @"\ConfigOS.xsd"))
                        {
                            valErrorMessage = cosv.ConfigOSSchemaValidation(appPath + @"\ConfigOS.xsd", file);
                            if (valErrorMessage.Length > 0)
                            {
                                DialogResult dr = MessageBox.Show(valErrorMessage + Environment.NewLine + Environment.NewLine + "Do you want to add it anyway?", msgBoxTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if (dr == DialogResult.No)
                                {
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("The schema file ConfigOS.xsd does not exist. Schema validation cannot be done.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        // Validate values
                        if (osTypeTmp == "LINUX")
                        {
                            // Validate linux signature
                        }
                        else
                        {
                            valErrorMessage = cosv.ConfigOSValuesValidation(file);
                            if (valErrorMessage.Length > 0)
                            {
                                DialogResult dr = MessageBox.Show(valErrorMessage + Environment.NewLine + Environment.NewLine + "Do you want to add it anyway?", msgBoxTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if (dr == DialogResult.No)
                                {
                                    continue;
                                }
                            }
                        }
                        #endregion
                    }
                    // End modifications 08-21-14 ver 1.0.4
                    //        //
                    ListViewItem lv = new ListViewItem(file);
                    lv.Tag = fi.Name;
                    lv.SubItems.Add(fi.Length.ToString());
                    lv.SubItems.Add(fi.LastAccessTime.ToString());
                    lstViewSelectedSignatures.Items.Add(lv);
                }
            }
        }

        private void btnSignaturesContainerName_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "zip files (*.zip)|*.zip";
            saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            saveFileDialog1.OverwritePrompt = false;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtSignaturesContainerName.Text = saveFileDialog1.FileName;
            }
        }

        private void btnUnselect_Click(object sender, EventArgs e)
        {
            try
            {
                // Start "UIChanges" ver: 1.0.9 date: 07-18-16
                //if (lstViewSelectedSignatures.SelectedItems.Count > 0)
                //{
                //    ListViewItem temp = lstViewSelectedSignatures.SelectedItems[0];
                //    lstViewSelectedSignatures.Items.Remove(temp);
                //}
                ListView listView = lstViewSelectedSignatures;
                foreach (ListViewItem item in listView.SelectedItems)
                {
                    item.Remove();
                }
                // End "UIChanges" ver: 1.0.9 date: 07-18-16
            }
            catch
            {
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SCCryptography sccry = new SCCryptography();
            workSignaturesFolder = Environment.GetEnvironmentVariable("temp") + @"\signatures\";
            //
            if (File.Exists(txtSignaturesContainerName.Text))
            {
                //MessageBox.Show("The Signatures Container file already exists.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult dr = MessageBox.Show(Path.GetFileName(txtSignaturesContainerName.Text) + " already exists.\nDo you want to replace it?", msgBoxTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.No)
                {
                    return;
                }
            }
            if (lstViewSelectedSignatures.Items.Count < 1)
            {
                MessageBox.Show("Please select available Signatures.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (Path.GetExtension(txtSignaturesContainerName.Text).ToUpper() != ".ZIP")
            {
                MessageBox.Show("The Signatures Container file extension must be ZIP.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!Directory.Exists(Path.GetDirectoryName(txtSignaturesContainerName.Text)))
            {
                MessageBox.Show("The Signatures Container directory does not exist", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Encrypt Signatures
            lstTasks.Items.Clear();
            lstTasks.Items.Add("Encrypting Signatures...");
            List<string> filesToZip = new List<string>();
            try
            {
                // Remove old Signatures folder from the user's temp folder
                fd.RemoveSignatureFolder(workSignaturesFolder);
                // Create new Signatures folder in the user's temp folder
                fd.CreateSignatureFolder(workSignaturesFolder);
                foreach (ListViewItem selectedSignature in lstViewSelectedSignatures.Items)
                {
                    if (Path.GetExtension(selectedSignature.Text.ToString()).ToUpper() == ".XML")
                    {
                        // Check if file is a VeriScan signature
                        bool isXMLFile = fd.IsXMLFile(selectedSignature.Text.ToString(), "Signature/Description", "VeriScan");
                        if (!isXMLFile)
                        {
                            // Check if file is a ConfigOS signature
                            //isXMLFile = fd.IsXMLFile(selectedSignature.Text.ToString(), "Groups/Items/Group");
                            isXMLFile = fd.IsXMLFile(selectedSignature.Text.ToString(), "Groups", "ConfigOS");
                        }
                        if (isXMLFile)
                        {
                            lstTasks.Items.Add("Encrypting '" + selectedSignature.Text.ToString() + "'");
                            // Get encryption keys
                            fd.GetEncryptionKeys();
                            string encryptedOutFile = workSignaturesFolder + selectedSignature.Tag.ToString();
                            // Remove the output encrypted file if exists
                            fd.RemoveFile(encryptedOutFile);
                            // Encrypt the file
                            // Start modifications KEYS 05-30-13
                            //bool encryp = sccry.EncryptFileAES(selectedSignature.Text.ToString(), encryptedOutFile, fd.AesKey, fd.AesKeyIV);
                            bool encryp = sccry.EncryptFileAES(selectedSignature.Text.ToString(), encryptedOutFile, signatureAesKey, signatureAesKeyIV); // with KEYS
                            // End modifications KEYS 05-30-13
                            if (encryp)
                            {
                                filesToZip.Add(encryptedOutFile);
                            }
                            else
                            {
                                lstTasks.Items.Add("Encryption failed.");
                            }
                        }
                        else
                        {
                            filesToZip.Add(selectedSignature.Text.ToString());
                        }
                    }
                    else
                    {
                        filesToZip.Add(selectedSignature.Text.ToString());
                    }

                }
            }
            catch (Exception err)
            {
                MessageBox.Show("Error encrypting signature files.\n" + err.Message, msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Zip signatures
            lstTasks.Items.Add("Creating Signatures Container...");
            string signaturesContainerFileName = txtSignaturesContainerName.Text.Substring(0, txtSignaturesContainerName.Text.LastIndexOf(".")) + "SHA1.zip";
            fd.RemoveFile(signaturesContainerFileName);
            if (filesToZip.Count > 0)
            {
                try
                {
                    using (ZipFile zip = new ZipFile())
                    {
                        foreach (string fileToZip in filesToZip)
                        {
                            ZipEntry ee = zip.AddFile(fileToZip, "");
                        }
                        zip.Save(signaturesContainerFileName);
                    }
                    // Remove Signatures folder from the user's temp folder
                    fd.RemoveSignatureFolder(workSignaturesFolder);
                }
                catch (Exception err)
                {
                    lstTasks.Items.Add(err.Message);
                }
            }
            else
            {
                lstTasks.Items.Add("There are no Signatures files to create the Signatures Container.");
                return;
            }
            // Calculate HASH for the zip file
            lstTasks.Items.Add("Calculating HASH for the Signatures Container file...");
            string signaturesContainerSHA1FileName = txtSignaturesContainerName.Text.Substring(0, txtSignaturesContainerName.Text.LastIndexOf(".")) + "SHA1.txt";
            byte[] sHA1Ret = sccry.GetSHA1(signaturesContainerFileName);
            if (sccry.ErrorMessage.Length > 0)
            {
                lstTasks.Items.Add("Unable to create SHA-1. The Signatures Container will not be created.");
                lstTasks.Items.Add(sccry.ErrorMessage);
                fd.RemoveFile(signaturesContainerFileName);
                return;
            }
            else
            {
                string bToS = fd.ByteToString(sHA1Ret);
                if (fd.ErrorMessage.Length > 0)
                {
                    lstTasks.Items.Add("Unable to convert SHA-1 to string. The Signatures Container will not be created.");
                    lstTasks.Items.Add(sccry.ErrorMessage);
                    fd.RemoveFile(signaturesContainerFileName);
                    return;
                }
                else
                {
                    try
                    {
                        StreamWriter sha1File = null;
                        sha1File = File.CreateText(signaturesContainerSHA1FileName);
                        sha1File.Write(bToS);
                        sha1File.Close();
                    }
                    catch
                    {
                        lstTasks.Items.Add("Unable to write the SHA-1 file. The Signatures Container will not be created.");
                        lstTasks.Items.Add(sccry.ErrorMessage);
                        fd.RemoveFile(signaturesContainerFileName);
                        return;
                    }
                }
            }
            // Create the Zip wrapper, including the Signatures Container plus the SHA1 files.
            if (File.Exists(signaturesContainerFileName) && File.Exists(signaturesContainerSHA1FileName))
            {
                try
                {
                    using (ZipFile zipf = new ZipFile())
                    {
                        zipf.AddFile(signaturesContainerFileName, "");
                        zipf.AddFile(signaturesContainerSHA1FileName, "");
                        zipf.Save(txtSignaturesContainerName.Text);
                    }
                }
                catch (Exception err)
                {
                    lstTasks.Items.Add(err.Message);
                    lstTasks.Items.Add("The Signatures Container will not be created.");
                }
            }
            else
            {
                lstTasks.Items.Add("The Signatures Container file or the SHA1 file is missing. The Signatures Container will not be created.");
            }
            fd.RemoveFile(signaturesContainerFileName);
            fd.RemoveFile(signaturesContainerSHA1FileName);
            // Done
            lstTasks.Items.Add("Done.");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        # region  View Signatures Container tab

        private void btnBrowseP2_Click(object sender, EventArgs e)
        {
            openFileDialog1.DefaultExt = "zip";
            // The Filter property requires a search string after the pipe ( | )
            openFileDialog1.Filter = "Signatures Container (*.zip)|" + "*.zip";
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtSingnaturesContainerP2.Text = openFileDialog1.FileName;
                //
                lstViewSignaturesContainerP2.Items.Clear();
            }
        }

        private void btnCancelP2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOKP2_Click(object sender, EventArgs e)
        {
            if (txtSingnaturesContainerP2.Text.Length < 1)
            {
                MessageBox.Show("Please select a Signatures container file.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (File.Exists(txtSingnaturesContainerP2.Text))
            {
                lstViewSignaturesContainerP2.Items.Clear();
                workSignaturesFolderP2 = Environment.GetEnvironmentVariable("temp") + @"\signaturesP2\";
                // Remove work folder
                fd.RemoveSignatureFolder(workSignaturesFolderP2);
                // Create work folder
                fd.CreateSignatureFolder(workSignaturesFolderP2);
                // Unzip Signatures Container wrapper
                // This wrapper contains the Signatures Container zip file and the 
                // and a text file containing the Signatures Container checksum
                bool unzipRet = MyExtract(workSignaturesFolderP2, txtSingnaturesContainerP2.Text);
                if (unzipRet)
                {
                    // Signatures Container zip file
                    string signaturesContainerSHA1 = workSignaturesFolderP2 + Path.GetFileNameWithoutExtension(txtSingnaturesContainerP2.Text) + "SHA1.zip";
                    if (File.Exists(signaturesContainerSHA1))
                    {
                        // unzip Signatures Container zip file
                        unzipRet = MyExtract(workSignaturesFolderP2, signaturesContainerSHA1);
                        if (unzipRet)
                        {
                            //string[] filePaths = Directory.GetFiles(workSignaturesFolder, "*.xml");
                            string[] filePaths = Directory.GetFiles(workSignaturesFolderP2);
                            if (filePaths.Count() > 0)
                            {
                                foreach (string filePath in filePaths)
                                {
                                    FileInfo fip2 = new FileInfo(filePath);
                                    if (fip2.Name.ToUpper().EndsWith("SHA1.TXT") || fip2.Name.ToUpper().EndsWith("SHA1.ZIP"))
                                    {
                                    }
                                    else
                                    {
                                        //
                                        ListViewItem lvp2 = new ListViewItem(fip2.Name);
                                        lvp2.SubItems.Add(fip2.Length.ToString());
                                        lvp2.SubItems.Add(fip2.LastAccessTime.ToString());
                                        lstViewSignaturesContainerP2.Items.Add(lvp2);
                                    }
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Unable to unzip the file '" + signaturesContainerSHA1 + "'.\nPlease select a different Signatures container file.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Unable to find the file '" + signaturesContainerSHA1 + "'.\nPlease select a different Signatures container file.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Unable to unzip the file '" + txtSingnaturesContainerP2.Text + "'.\nPlease select a different Signatures container file.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("The Signatures container file '" + txtSingnaturesContainerP2.Text + "' does not exist.\nPlease select a different Signatures container file.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Preferences tab

        private void btnBrowseUpdatesLocation_Click(object sender, EventArgs e)
        {
            btnVerify.ImageKey = "Connections.ico";
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtUpdatesNewLocation.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnVerify_Click(object sender, EventArgs e)
        {
            btnVerify.ImageKey = "Connections.ico";
            string retValidation = ValidateInput(txtUpdatesNewLocation.Text);
            if (retValidation.Length > 0)
            {
                MessageBox.Show(retValidation, msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnVerify.ImageKey = "Connections_Delete.ico";
                return;
            }
            updateController1.UpdateLocation = txtUpdatesNewLocation.Text;
            try
            {
                updateController1.CheckForUpdate();
                btnVerify.ImageKey = "Connections_Check.ico";
            }
            catch (Kjs.AppLife.Update.Controller.DownloadException err)
            {
                btnVerify.ImageKey = "Connections_Delete.ico";
                MessageBox.Show(err.Message, msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Kjs.AppLife.Update.Controller.ValidationException err)
            {
                btnVerify.ImageKey = "Connections_Delete.ico";
                MessageBox.Show(err.Message, msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception err)
            {
                btnVerify.ImageKey = "Connections_Delete.ico";
                MessageBox.Show("Unable to contact the specified update location. " + err.Message, msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelP3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOKP3_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(txtUpdatesNewLocation.Text))
            {
                MessageBox.Show("Please select an existing Updates folder.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            bool retUpdatesDirectory = COSPreferencesUpdate(cosPreferencesFile, "COSFoundryPreferences/Updates", "Directory", txtUpdatesNewLocation.Text);
            if (retUpdatesDirectory)
            {
                MessageBox.Show("The Preferences information was successfully updated.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("The " + msgBoxTitle + " Updates folder was NOT updated.\nPlease make sure that the COSFoundryPreferences.xml file has the right format.\n\nThe Preferences information was NOT successfully updated.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string ValidateInput(string input)
        {
            string strReturn = "";
            if (input.Length < 1 || !Directory.Exists(input))
            {
                strReturn = "Please enter an existing folder";
            }
            else
            {
                Uri testUri = null;
                if (!Uri.TryCreate(input, UriKind.Absolute, out testUri))
                {
                    strReturn = "The information entered is not in a valid format.";
                }
            }
            return strReturn;
        }

        private void btnRestoreDefaults_Click(object sender, EventArgs e)
        {
            btnVerify.ImageKey = "Connections.ico";
            txtUpdatesNewLocation.Text = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\Updates";
        }
        #endregion

        #region Menu Options

        private void fileExitMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void helpSteelCloudWebSiteMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.SteelCloud.com");
        }

        private void helpAboutMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm af = new AboutForm(msgBoxTitle);
            af.ShowDialog();
        }

        private void helpKeyMenuItem_Click(object sender, EventArgs e)
        {
            // Start modifications KEYS 06-05-13
            string msgToDisplay = "";
            string[] currentKeys = Directory.GetFiles(appPath + @"\", "FoundryKey_*.key");
            if (currentKeys.Length > 0)
            {
                if (currentKeys.Length > 1)
                {
                    msgToDisplay = "ERROR. There is more than one Foundry key";
                }
                else
                {
                    msgToDisplay = Path.GetFileName(currentKeys[0]);
                }
            }
            else
            {
                msgToDisplay = "ERROR. There is no Foundry key.";
            }
            MessageBox.Show("Your Key: " + msgToDisplay, msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            // End modifications KEYS 06-05-13
        }

        #endregion

        #region Auxiliar methods

        // Unzip signatures file (DotNetZip library)

        private bool MyExtract(string unpackDirectory, string zipToUnpack)
        {
            bool zipResult = false;
            try
            {
                using (ZipFile zip1 = ZipFile.Read(zipToUnpack))
                {
                    // here, we extract every entry, but we could extract conditionally
                    // based on entry name, size, date, checkbox status, etc.  
                    foreach (ZipEntry e in zip1)
                    {
                        e.Extract(unpackDirectory, ExtractExistingFileAction.OverwriteSilently);
                    }
                    zipResult = true;
                }
            }
            catch
            {
            }
            return zipResult;
        }

        private string COSPreferencesRead(string configOSPreferencesFile, string preferenceToRead, string nodeName)
        {
            mainErrorMessage = "";
            string retLogDirectory = "";
            try
            {
                XmlDocument xmlCOSPreferences = new XmlDocument();
                xmlCOSPreferences.Load(configOSPreferencesFile);
                // Get logs directory info
                XmlNodeList preferences = xmlCOSPreferences.SelectNodes(preferenceToRead);
                foreach (XmlNode preference in preferences)
                {
                    XmlNodeList preferenceChildren = preference.ChildNodes;
                    foreach (XmlNode preferenceChild in preferenceChildren)
                    {
                        if (preferenceChild.Name == nodeName)
                        {
                            retLogDirectory = preferenceChild.InnerText;
                        }
                    }
                }
            }
            catch (Exception err)
            {
                mainErrorMessage = "ConfigOS preferences file error. " + err.Message;
            }
            return retLogDirectory;
        }

        private bool COSPreferencesUpdate(string configOSPreferencesFile, string preferenceToRead, string nodeName, string nodeValue)
        {
            mainErrorMessage = "";
            bool updated = false;
            try
            {
                XmlDocument xmlCOSPreferences = new XmlDocument();
                xmlCOSPreferences.Load(configOSPreferencesFile);
                // Get logs directory info
                XmlNodeList preferences = xmlCOSPreferences.SelectNodes(preferenceToRead);
                foreach (XmlNode preference in preferences)
                {
                    XmlNodeList preferenceChildren = preference.ChildNodes;
                    foreach (XmlNode preferenceChild in preferenceChildren)
                    {
                        if (preferenceChild.Name == nodeName)
                        {
                            preferenceChild.InnerText = nodeValue;
                            xmlCOSPreferences.Save(configOSPreferencesFile);
                            updated = true;
                        }
                    }
                }
            }
            catch (Exception err)
            {
                mainErrorMessage = "ConfigOS preferences file error. " + err.Message;
            }
            return updated;
        }

        private void DisableFIPSCompliance()
        {
            int fipsKey;
            try
            {
                RegistryKey rkey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Lsa\FipsAlgorithmPolicy", true);
                if (rkey != null)
                {
                    if (rkey.GetValue("Enabled") != null)
                    {
                        fipsKey = Convert.ToInt32(rkey.GetValue("Enabled"));
                        if (fipsKey == 1)
                        {
                            rkey.SetValue("Enabled", 0);
                            // Save FIPS state on registry
                            RegistryKey regKeyPath = Registry.LocalMachine.OpenSubKey(@"Software\SCLD\ConfigOSFoundry\FipsAlgorithmPolicy");
                            if (regKeyPath == null)
                            {
                                regKeyPath = Registry.LocalMachine.CreateSubKey(@"Software\SCLD\ConfigOSFoundry\FipsAlgorithmPolicy");
                            }
                            regKeyPath = Registry.LocalMachine.OpenSubKey(@"Software\SCLD\ConfigOSFoundry\FipsAlgorithmPolicy", true);
                            regKeyPath.SetValue("Enabled", 1);
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void EnableFIPSCompliance()
        {
            try
            {
                RegistryKey regKeyPath = Registry.LocalMachine.OpenSubKey(@"Software\SCLD\ConfigOSFoundry\FipsAlgorithmPolicy");
                if (regKeyPath != null)
                {
                    if (regKeyPath.GetValue("Enabled") != null)
                    {
                        RegistryKey rkey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Lsa\FipsAlgorithmPolicy", true);
                        if (rkey != null)
                        {
                            if (rkey.GetValue("Enabled") != null)
                            {
                                rkey.SetValue("Enabled", 1);
                                Registry.LocalMachine.DeleteSubKeyTree(@"Software\SCLD\ConfigOSFoundry\FipsAlgorithmPolicy");
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }

        // Start modifications 08-21-14 ver 1.0.4
        private void InitializeComboBoxes()
        {
            try
            {
                regHives.Add("HKLM");
                regHives.Add("HKCU");
                regHives.Add("HKU");
                regHives.Add("MACHINE");
                //
                cosCategories.Add("Registry");
                cosCategories.Add("Local Security Policy");
                cosCategories.Add("Advanced Audit Policy Configuration");
                cosCategories.Add("Service General Setting");
                cosCategories.Add("File Security");
                cosCategories.Add("Registry Security");
                cosCategories.Add("No Validation");
                //
                regTypes.Add("REG_SZ");
                regTypes.Add("REG_DWORD");
                regTypes.Add("REG_BINARY");
                regTypes.Add("REG_MULTI_SZ");
                // Start "REG_MULTI_SZ" ver: 1.0.8 date: 10-20-15
                regTypes.Add("REG_EXPAND_SZ");
                // End "REG_MULTI_SZ" ver: 1.0.8 date: 10-20-15

                cmbCategory.Items.Clear();
                foreach (string cosCategory in cosCategories)
                {
                    cmbCategory.Items.Add(cosCategory);
                }
                cmbType.Items.Clear();
                foreach (string regType in regTypes)
                {
                    cmbType.Items.Add(regType);
                }
            }
            catch
            {
            }
        }
        // End modifications 08-21-14 ver 1.0.4

        // Start "ProductLicense" ver: 1.0.7 date: 05-18-15
        private void DisableTab_DrawItem(object sender, DrawItemEventArgs e)
        {
            TabControl tabControl = sender as TabControl;
            TabPage tabPage = tabControl.TabPages[e.Index];

            if (tabPage.Enabled == false)
            {
                using (SolidBrush brush =
                   new SolidBrush(SystemColors.GrayText))
                {
                    e.Graphics.DrawString(tabPage.Text, tabPage.Font, brush,
                       e.Bounds.X + 3, e.Bounds.Y + 3);
                }
            }
            else
            {
                using (SolidBrush brush = new SolidBrush(tabPage.ForeColor))
                {
                    e.Graphics.DrawString(tabPage.Text, tabPage.Font, brush,
                       e.Bounds.X + 3, e.Bounds.Y + 3);
                }
            }
        }

        private void DisableTab_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPage.Enabled == false)
            {
                e.Cancel = true;
            }
        }
        // End "ProductLicense" ver: 1.0.7 date: 05-18-15

        #endregion

        #region Signature Builder tab

        private void btnSignatureBrowse_Click(object sender, EventArgs e)
        {
            osTypeSource = "";
            tvm = new TreeViewMng();
            OpenFileDialog ofd1 = new OpenFileDialog();
            ofd1.Filter = "XML Files (.xml)|*.xml";
            string curBaseSignature = txtBaseSignature.Text;
            DialogResult result = ofd1.ShowDialog();
            // Start "LinuxValidation" ver: 1.0.9 date: 05-09-16
            linuxVal = new LinuxValidation();
            // End "LinuxValidation" ver: 1.0.9 date: 05-09-16
            if (result == DialogResult.OK)
            {
                txtBaseSignature.Text = ofd1.FileName;
                //txtBaseSignature.ReadOnly = true;
                //txtBaseSignature.BackColor = Color.White;
                // load treeview
                try
                {
                    xb = XDocument.Load(txtBaseSignature.Text);
                    // Check if the file is a ConfigOS signature
                    if (xb.Element("Groups").Attribute("SysArea").Value.ToString() == "SecurityPolicy")
                    {
                        // Check the signature OS type (Windows or Linux)
                        if (xb.Element("Groups").Attribute("OSType") != null)
                        {
                            osTypeSource = xb.Element("Groups").Attribute("OSType").Value.ToString().ToUpper();
                        }
                        #region Adjust user interface for linux
                        if (osTypeSource == "LINUX")
                        {
                            lblCategory.Visible = false;
                            cmbCategory.Visible = false;
                            // Start "LinuxValidation" ver: 1.0.9 date: 05-06-16
                            txtType.Visible = false;
                            cmbTypeLinux.Visible = true;
                            cmbTypeLinux.Text = "";
                            // End "LinuxValidation" ver: 1.0.9 date: 05-06-16
                        }
                        else
                        {
                            lblCategory.Visible = true;
                            cmbCategory.Visible = true;
                            // Start "LinuxValidation" ver: 1.0.9 date: 05-06-16
                            txtType.Visible = true;
                            cmbTypeLinux.Visible = false;
                            // End "LinuxValidation" ver: 1.0.9 date: 05-06-16
                        }
                        #endregion
                        //
                        #region check schema and value errors
                        // set error file name
                        FileInfo sfi = new FileInfo(txtBaseSignature.Text);
                        string errorFile = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\Validation_" + Path.GetFileNameWithoutExtension(sfi.Name) + ".txt";
                        if (File.Exists(errorFile))
                        {
                            try
                            {
                                File.Delete(errorFile);
                            }
                            catch
                            {
                            }
                        }
                        ConfigOSSignatureValidation cosv = new ConfigOSSignatureValidation();
                        cosv.Hives = regHives;
                        cosv.Categories = cosCategories;
                        cosv.RegTypes = regTypes;
                        cosv.ErrorFile = errorFile;
                        // Validate schema
                        string valErrorMessage = "";
                        if (File.Exists(appPath + @"\ConfigOS.xsd"))
                        {
                            valErrorMessage = cosv.ConfigOSSchemaValidation(appPath + @"\ConfigOS.xsd", txtBaseSignature.Text);
                            if (valErrorMessage.Length > 0)
                            {
                                DialogResult dr = MessageBox.Show(valErrorMessage + Environment.NewLine + Environment.NewLine + "Do you want to load it anyway?", msgBoxTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if (dr == DialogResult.No)
                                {
                                    txtBaseSignature.Text = curBaseSignature;
                                    return;
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("The schema file ConfigOS.xsd does not exist. Schema validation cannot be done.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        // Validate values
                        if (osTypeSource == "LINUX")
                        {
                            // Validate linux signature
                            // Start "LinuxValidation" ver: 1.0.9 date: 05-12-16
                            appPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
                            if (File.Exists(appPath + @"\Resources\LinuxValidation.xml"))
                            {
                                linuxVal.ErrorFile = errorFile;
                                valErrorMessage = linuxVal.ValidateLinuxSignature(txtBaseSignature.Text, appPath + @"\Resources\LinuxValidation.xml");
                                if (valErrorMessage.Length > 0)
                                {
                                    DialogResult dr = MessageBox.Show(valErrorMessage + Environment.NewLine + Environment.NewLine + "Do you want to load it anyway?", msgBoxTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                    if (dr == DialogResult.No)
                                    {
                                        txtBaseSignature.Text = curBaseSignature;
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                DialogResult dr = MessageBox.Show("The Linux validation file 'LinuxValidation.xml' does not exist on your installation folder." + Environment.NewLine  + "The signature file and the input data will not be validated." + Environment.NewLine + Environment.NewLine + "Do you want to continue anyway?", msgBoxTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if (dr == DialogResult.No)
                                {
                                    txtBaseSignature.Text = curBaseSignature;
                                    return;
                                }
                            }
                            // End "LinuxValidation" ver: 1.0.9 date: 05-12-16
                        }
                        else
                        {
                            // Validate windows signature
                            valErrorMessage = cosv.ConfigOSValuesValidation(txtBaseSignature.Text);
                            if (valErrorMessage.Length > 0)
                            {
                                DialogResult dr = MessageBox.Show(valErrorMessage + Environment.NewLine + Environment.NewLine + "Do you want to load it anyway?", msgBoxTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if (dr == DialogResult.No)
                                {
                                    txtBaseSignature.Text = curBaseSignature;
                                    return;
                                }
                            }
                        }
                        #endregion
                        //
                        if (txtVanillaSignature.Text.Length > 0 && File.Exists(txtVanillaSignature.Text))
                        {
                            tvm.LoadTreeview(treeViewBase.Nodes, xb, xv);
                        }
                        else
                        {
                            tvm.LoadTreeview(treeViewBase.Nodes, xb, null);
                        }
                        treeViewBase.Refresh();
                        if (tvm.ErrorMessage.Length > 0)
                        {
                            MessageBox.Show(tvm.ErrorMessage, msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtBaseSignature.Text = curBaseSignature;
                        }
                        else
                        {
                            // get "Groups" attributes
                            foreach (XAttribute groupAtt in tvm.GroupsElementNew.Attributes())
                            {
                                switch (groupAtt.Name.ToString().ToUpper())
                                {
                                    case "CREATEDATE":
                                        dtpCreationDate.Value = Convert.ToDateTime(groupAtt.Value.ToString());
                                        break;
                                    case "AUTHORNAME":
                                        txtAuthor.Text = groupAtt.Value.ToString();
                                        break;
                                    case "VERSION":
                                        txtVersion.Text = groupAtt.Value.ToString();
                                        break;
                                }
                            }
                            grpSignatureAtt.Visible = true;
                            // Start "BuilderDisableTreeviewSort" ver: 1.0.8 date: 08-31-15
                            //tvm.SortTreeViewNodes(treeViewBase);
                            // End "BuilderDisableTreeviewSort" ver: 1.0.8 date: 08-31-15
                            treeViewNew.Nodes.Clear();
                            //dataGridViewGroup.Rows.Clear();
                            SetNewGroupFields();
                            dataGridViewGroupVanilla.Rows.Clear();
                            txtGroupId.Text = "";
                            txtGroupId.Enabled = false;
                            // Start modifications ver: 1.0.6 date: 02-20-15
                            //chkFilter.Enabled = true;
                            //chkFilter.Checked = false;
                            // Remove filter labels
                            btnRemoveFilter.Enabled = false;
                            btnFilterToolTip = "";
                            toolTip1.SetToolTip(btnFilter, "");
                            // End modifications ver: 1.0.6 date: 02-20-15
                            lblTotalBase.Text = treeViewBase.Nodes.Count.ToString();
                            lblTotalNew.Text = "";
                            btnNewGroup.Enabled = true;
                            // Start "BuilderCopyGroup" ver: 1.0.8 date: 08-25-15
                            btnCopyGroup.Enabled = true;
                            // End "BuilderCopyGroup" ver: 1.0.8 date: 08-25-15
                        }
                    }
                    else
                    {
                        MessageBox.Show("This is an invalid SteelCloud ConfigOS signature file.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtBaseSignature.Text = curBaseSignature;
                    }
                }
                catch(Exception err)
                {
                    MessageBox.Show("File connot be read it. " + err.Message, msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtBaseSignature.Text = curBaseSignature;
                }
            }
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            if (treeViewBase.Nodes.Count > 0)
            {
                // Start "BuilderEnhancements" ver: 1.0.8 date: 01-13-16
                newSignatureChanges = true;
                // End "BuilderEnhancements" ver: 1.0.8 date: 01-13-16
                if (txtVanillaSignature.Text.Length > 0 && File.Exists(txtVanillaSignature.Text))
                {
                    tvm.MoveCheckedNodesFromToTreeViews(treeViewBase.Nodes, treeViewNew.Nodes, xb, xv);
                }
                else
                {
                    tvm.MoveCheckedNodesFromToTreeViews(treeViewBase.Nodes, treeViewNew.Nodes, xb, null);
                }
                // Start "BuilderDisableTreeviewSort" ver: 1.0.8 date: 08-31-15
                //tvm.SortTreeViewNodes(treeViewNew);
                // End "BuilderDisableTreeviewSort" ver: 1.0.8 date: 08-31-15
                if (chkSelectAll.Checked)
                {
                    chkSelectAll.Checked = false;
                }
                lblTotalBase.Text = treeViewBase.Nodes.Count.ToString();
                lblTotalNew.Text = treeViewNew.Nodes.Count.ToString();
            }
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            if (treeViewNew.Nodes.Count > 0)
            {
                // Start "BuilderEnhancements" ver: 1.0.8 date: 01-13-16
                newSignatureChanges = true;
                // End "BuilderEnhancements" ver: 1.0.8 date: 01-13-16
                if (txtVanillaSignature.Text.Length > 0 && File.Exists(txtVanillaSignature.Text))
                {
                    tvm.MoveCheckedNodesFromToTreeViews(treeViewNew.Nodes, treeViewBase.Nodes, xb, xv);
                }
                else
                {
                    tvm.MoveCheckedNodesFromToTreeViews(treeViewNew.Nodes, treeViewBase.Nodes, xb, null);
                }
                // Start "BuilderDisableTreeviewSort" ver: 1.0.8 date: 08-31-15
                //tvm.SortTreeViewNodes(treeViewBase);
                // End "BuilderDisableTreeviewSort" ver: 1.0.8 date: 08-31-15
            }
            lblTotalBase.Text = treeViewBase.Nodes.Count.ToString();
            lblTotalNew.Text = treeViewNew.Nodes.Count.ToString();
            // Start "BuilderCopyGroup" ver: 1.0.8 date: 08-25-15
            if (treeViewNew.Nodes.Count < 1)
            {
                SetNewGroupFields();
            }
            // End "BuilderCopyGroup" ver: 1.0.8 date: 08-25-15
        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSelectAll.Checked)
            {
                foreach (TreeNode node in treeViewBase.Nodes)
                {
                    node.Checked = true;
                }
            }
            else
            {
                foreach (TreeNode node in treeViewBase.Nodes)
                {
                    node.Checked = false;
                }
            }
        }

        private void btnSaveSignature_Click(object sender, EventArgs e)
        {
            if (treeViewNew.Nodes.Count > 0)
            {
                // Start "BuilderEnhancements" ver: 1.0.8 date: 01-13-16
                //// Configure save file dialog box
                //SaveFileDialog sdlg = new SaveFileDialog();
                //sdlg.Filter = "xml files (*.xml)|*.xml";
                //sdlg.RestoreDirectory = true;
                //if (sdlg.ShowDialog() == DialogResult.OK)
                //{
                //    // Save "Groups" attributes
                //    //tvm.GroupsElementNew.SetAttributeValue("CreateDate", txtCreationDate.Text);
                //    tvm.GroupsElementNew.SetAttributeValue("CreateDate", dtpCreationDate.Value.ToShortDateString());
                //    tvm.GroupsElementNew.SetAttributeValue("AuthorName", txtAuthor.Text);
                //    tvm.GroupsElementNew.SetAttributeValue("Version", txtVersion.Text);
                //    //
                //    tvm.SaveTreeViewToFile(treeViewNew.Nodes, sdlg.FileName);
                //    if (tvm.ErrorMessage.Length > 0)
                //    {
                //        MessageBox.Show(tvm.ErrorMessage, msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    }
                //}
                SaveSignature();
                newSignatureChanges = false;
                // End "BuilderEnhancements" ver: 1.0.8 date: 01-13-16
            }
        }

        private void btnNewGroup_Click(object sender, EventArgs e)
        {
            Group myGroup = new Group();
            //dataGridViewGroup.Rows.Clear();
            dataGridViewGroupVanilla.Rows.Clear();
            txtGroupId.Enabled = true;
            txtGroupId.ReadOnly = false;
            //
            SetNewGroupFields();
            //
            if (osTypeSource == "LINUX")
            {
                // Start "LinuxValidation" ver: 1.0.9 date: 05-06-16
                cmbTypeLinux.Text = "";
                // End "LinuxValidation" ver: 1.0.9 date: 05-06-16
            }
            else
            {
                grpScldInfo.Enabled = false;
            }
        }

        // Start "BuilderCopyGroup" ver: 1.0.8 date: 08-25-15
        private void btnCopyGroup_Click(object sender, EventArgs e)
        {
            if (txtGroupId.Text.Length < 1)
            {
                MessageBox.Show("Please select a group to copy from.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Group myGroup = new Group();
            dataGridViewGroupVanilla.Rows.Clear();
            txtGroupId.Enabled = true;
            txtGroupId.ReadOnly = false;
            txtGroupId.Text = txtGroupId.Text + "(COPY)";
        }
        // End "BuilderCopyGroup" ver: 1.0.8 date: 08-25-15

        private void btnSaveGroup_Click(object sender, EventArgs e)
        {
            if (txtGroupId.Text.Trim().Length > 0)
            {
                // Validate info
                ConfigOSSignatureValidation cosv = new ConfigOSSignatureValidation();
                cosv.Hives = regHives;
                cosv.Categories = cosCategories;
                cosv.RegTypes = regTypes;
                if (osTypeSource == "LINUX")
                {
                    // Linux validation
                    // Start "LinuxValidation" ver: 1.0.9 date: 05-09-16

                    // Verify if xml file containing linux validation exist
                    appPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
                    string linuxValidatingFile = appPath + @"\Resources\LinuxValidation.xml";
                    if (!File.Exists(linuxValidatingFile))
                    {
                        // No valitadion will be done
                    }
                    else
                    {
                        LinuxValidation linuxVal = new LinuxValidation();
                        linuxVal.GroupId = txtGroupId.Text;
                        linuxVal.Where = txtWhere.Text;
                        linuxVal.Applied = txtApplied.Text;
                        linuxVal.Type = cmbTypeLinux.Text;
                        linuxVal.Valuel = txtValue.Text;
                        linuxVal.Ignore = txtIgnore.Text;
                        linuxVal.IgnoreReason = txtIgnoreReason.Text;
                        if (linuxVal.Validate(linuxValidatingFile, true).Length > 0)
                        {
                            DialogResult dr = MessageBox.Show(linuxVal.ErrorMessage + Environment.NewLine + "Do you want to save the changes anyway?", msgBoxTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            if (dr == DialogResult.No)
                            {
                                return;
                            }
                        }
                    }

                    // End "LinuxValidation" ver: 1.0.9 date: 05-09-16
                }
                else
                {
                    // Windows validation
                    if (cmbCategory.Text != "No Validation")
                    {
                        if (cmbCategory.Text == "Registry")
                        {
                            if (cosv.ValidateRegistry(txtWhere.Text, cmbType.Text).Length > 0)
                            {
                                MessageBox.Show(cosv.ErrorMessage, msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                        if (cosv.ValidateValue(txtValue.Text).Length > 0)
                        {
                            MessageBox.Show(cosv.ErrorMessage, msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        string myIgnoreValue = GetIgnoreValue();
                        if (cosv.ValidateIgnoreReason(myIgnoreValue, txtIgnoreReason.Text).Length > 0)
                        {
                            MessageBox.Show(cosv.ErrorMessage, msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
                // Save info
                Group myGroup = new Group();
                SetValuesToSave(myGroup);
                string oldGroupId = txtGroupId.Text.Trim();
                if (txtVanillaSignature.Text.Length > 0 && File.Exists(txtVanillaSignature.Text))
                {
                    myGroup.SaveGroup(treeViewNew, txtGroupId.Text.Trim(), txtGroupId.ReadOnly, xv, xb, treeViewBase, dataGridViewGroupVanilla);
                }
                else
                {
                    myGroup.SaveGroup(treeViewNew, txtGroupId.Text.Trim(), txtGroupId.ReadOnly, null, xb, treeViewBase, dataGridViewGroupVanilla);
                }
                if (myGroup.ErrorMessage.Length > 0)
                {
                    MessageBox.Show(myGroup.ErrorMessage, msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    // Start "BuilderEnhancements" ver: 1.0.8 date: 01-13-16
                    return;
                    // End "BuilderEnhancements" ver: 1.0.8 date: 01-13-16
                }
                // Start "BuilderDisableTreeviewSort" ver: 1.0.8 date: 08-31-15
                //tvm.SortTreeViewNodes(treeViewNew);
                // End "BuilderDisableTreeviewSort" ver: 1.0.8 date: 08-31-15
                lblTotalNew.Text = treeViewNew.Nodes.Count.ToString();
                TreeNode treeNodeToSelect = Shared.FindNodeInTreeView(treeViewNew, oldGroupId);
                if (treeNodeToSelect != null)
                {
                    treeViewNew.SelectedNode = treeNodeToSelect;
                }
                // Start "BuilderEnhancements" ver: 1.0.8 date: 01-13-16
                newSignatureChanges = true;
                // End "BuilderEnhancements" ver: 1.0.8 date: 01-13-16
            }
        }

        private void treeViewNew_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //grpScldInfo.Enabled = true;
            //swUpd = true;
            //TreeNode selectedNode = treeViewNew.GetNodeAt(e.X, e.Y);
            //if (selectedNode.Level == 0)
            //{
            //    //dataGridViewGroup.Rows.Clear();
            //    lblGroupId.Text = Shared.GetElementName(selectedNode.Text);
            //    txtGroupId.Text = Shared.GetElementValue(selectedNode.Text);
            //    txtGroupId.ReadOnly = true;
            //    // Set values to update on the user interface
            //    SetValuesToUpdate(selectedNode);
            //    // Load values from vanilla signature
            //    if (txtVanillaSignature.Text.Length > 0 && File.Exists(txtVanillaSignature.Text))
            //    {
            //        dataGridViewGroupVanilla.Visible = true;
            //        //FillDataGridView(selectedNode);
            //        tvm.FillDataGridViewVanilla(dataGridViewGroupVanilla, xv, txtGroupId.Text);
            //        // Compare dataGridViewVanilla with current group to highlight the differences
            //        Shared.CompareVanillaGroupToCurrentGroupHighlightDifferences(selectedNode, dataGridViewGroupVanilla);
            //    }
            //}
            //swUpd = false;
        }

        // Start modifications ver: 1.0.6 date: 02-20-15

        //private void chkFilter_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (chkFilter.Checked)
        //    {
        //        DialogResult dr = new DialogResult();
        //        FilterForm ff = new FilterForm(msgBoxTitle, osTypeSource);
        //        dr = ff.ShowDialog();
        //        string filterKeyword = "";
        //        string selectedFilterElement = "";
        //        if (ff.IsOK)
        //        {
        //            bool mustMatch = false;
        //            if (ff.cmbFiterElement.SelectedIndex >= 0)
        //            {
        //                selectedFilterElement = ff.cmbFiterElement.SelectedItem.ToString().ToUpper();
        //                if (osTypeSource == "LINUX")
        //                {
        //                    if (selectedFilterElement == "SEVERITY")
        //                    {
        //                        filterKeyword = ff.cmbKeyword.SelectedItem.ToString().ToUpper();
        //                        mustMatch = true;
        //                    }
        //                    else
        //                    {
        //                        filterKeyword = ff.txtKeyword.Text;
        //                    }
        //                }
        //                else
        //                {
        //                    if (selectedFilterElement == "SEVERITY" || selectedFilterElement == "TYPE" || selectedFilterElement == "IGNORE")
        //                    {
        //                        filterKeyword = ff.cmbKeyword.SelectedItem.ToString().ToUpper();
        //                        if (selectedFilterElement == "IGNORE")
        //                        {
        //                            filterKeyword = filterKeyword.Substring(0, 1);
        //                        }
        //                        mustMatch = true;
        //                    }
        //                    else
        //                    {
        //                        filterKeyword = ff.txtKeyword.Text;
        //                    }
        //                }
        //            }
        //            if (filterKeyword.Length > 0)
        //            {
        //                //
        //                // Perform filter
        //                //
        //                // 1. Create a list with the nodes that match the filter criteria and a list of nodes that not match the filter criteria
        //                // Start modifications ver: 1.0.6 date: 02-18-15
        //                //tvm.SeparateFilteredNodes(treeViewBase.Nodes, selectedFilterElement, filterKeyword, mustMatch);
        //                tvm.SeparateFilteredNodes(treeViewBase.Nodes, selectedFilterElement, filterKeyword, mustMatch, ff.chkNotInclude.Checked);
        //                // End modifications ver: 1.0.6 date: 02-18-15
        //                // 2. Clear the current treeview
        //                treeViewBase.Nodes.Clear();
        //                // 3. Copy the nodes that match to the current treeview
        //                tvm.CopyNodes(treeViewBase.Nodes, tvm.MatchingNodes);
        //                // Add filter labels
        //                lblFilterElement.Text = selectedFilterElement;
        //                lblFilterKeyword.Text = filterKeyword;
        //                toolTip1.SetToolTip(lblFilterKeyword, filterKeyword);
        //                lblTotalBase.Text = treeViewBase.Nodes.Count.ToString();
        //            }
        //        }
        //        else
        //        {
        //            chkFilter.Checked = false;
        //        }
        //    }
        //    else
        //    {
        //        // 1. Copy the remaining nodes on current treeview
        //        tvm.CopyNodes(treeViewBase.Nodes, tvm.RemainingNodes);
        //        tvm.RemainingNodes.Clear();
        //        tvm.MatchingNodes.Clear();
        //        tvm.SortTreeViewNodes(treeViewBase);
        //        // Remove filter labels
        //        lblFilterElement.Text = "";
        //        lblFilterKeyword.Text = "";
        //        lblTotalBase.Text = treeViewBase.Nodes.Count.ToString();
        //    }
        //}

        // End modifications ver: 1.0.6 date: 02-20-15

        private void chkVanilla_CheckedChanged(object sender, EventArgs e)
        {
            if (chkVanilla.Checked)
            {
                txtVanillaSignature.Enabled = true;
                btnVanillaBrowse.Enabled = true;
                //
                txtVanillaSignature.BackColor = Color.White;
            }
            else
            {
                txtVanillaSignature.BackColor = SystemColors.Control;
                //
                txtVanillaSignature.Enabled = false;
                btnVanillaBrowse.Enabled = false;
                dataGridViewGroupVanilla.Rows.Clear();
                dataGridViewGroupVanilla.Visible = false;
                txtVanillaSignature.Text = "";

                if (txtBaseSignature.Text.Length > 0 && File.Exists(txtBaseSignature.Text))
                {
                    tvm.LoadTreeview(treeViewBase.Nodes, xb, null);
                    treeViewNew.Nodes.Clear();
                    SetNewGroupFields();
                    dataGridViewGroupVanilla.Rows.Clear();
                    txtGroupId.Text = "";
                    txtGroupId.Enabled = false;
                    lblTotalNew.Text = "";
                }
            }
            //txtVanillaSignature.ReadOnly = false;
        }

        private void btnVanillaBrowse_Click(object sender, EventArgs e)
        {
            osTypeMaster = "";
            OpenFileDialog ofd1 = new OpenFileDialog();
            ofd1.Filter = "XML Files (.xml)|*.xml";
            string curVanillaSignature = txtVanillaSignature.Text;
            DialogResult result = ofd1.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtVanillaSignature.Text = ofd1.FileName;
                try
                {
                    xv = XDocument.Load(txtVanillaSignature.Text);
                    if (xv.Element("Groups").Attribute("SysArea").Value.ToString() == "SecurityPolicy")
                    {
                        // Check the signature OS type (Windows or Linux)
                        if (xv.Element("Groups").Attribute("OSType") != null)
                        {
                            osTypeMaster = xv.Element("Groups").Attribute("OSType").Value.ToString().ToUpper();
                        }
                        if (osTypeSource == osTypeMaster)
                        {
                            // Check if the file has no schema or value error
                            #region check schema and value errors
                            // set error file name
                            FileInfo sfi = new FileInfo(txtVanillaSignature.Text);
                            string errorFile = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\Validation_" + Path.GetFileNameWithoutExtension(sfi.Name) + ".txt";
                            if (File.Exists(errorFile))
                            {
                                try
                                {
                                    File.Delete(errorFile);
                                }
                                catch
                                {
                                }
                            }
                            ConfigOSSignatureValidation cosv = new ConfigOSSignatureValidation();
                            cosv.Hives = regHives;
                            cosv.Categories = cosCategories;
                            cosv.RegTypes = regTypes;
                            cosv.ErrorFile = errorFile;
                            // Validate schema
                            string valErrorMessage = "";
                            if (File.Exists(appPath + @"\ConfigOS.xsd"))
                            {
                                valErrorMessage = cosv.ConfigOSSchemaValidation(appPath + @"\ConfigOS.xsd", txtVanillaSignature.Text);
                                if (valErrorMessage.Length > 0)
                                {
                                    DialogResult dr = MessageBox.Show(valErrorMessage + Environment.NewLine + Environment.NewLine + "Do you want to load it anyway?", msgBoxTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                    if (dr == DialogResult.No)
                                    {
                                        txtVanillaSignature.Text = curVanillaSignature;
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("The schema file ConfigOS.xsd does not exist. Schema validation cannot be done.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            // Validate values
                            if (osTypeMaster == "LINUX")
                            {
                                // Validate linux signature
                            }
                            else
                            {
                                valErrorMessage = cosv.ConfigOSValuesValidation(txtVanillaSignature.Text);
                                if (valErrorMessage.Length > 0)
                                {
                                    DialogResult dr = MessageBox.Show(valErrorMessage + Environment.NewLine + Environment.NewLine + "Do you want to load it anyway?", msgBoxTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                    if (dr == DialogResult.No)
                                    {
                                        txtVanillaSignature.Text = curVanillaSignature;
                                        return;
                                    }
                                }
                            }
                            #endregion
                            //
                            if (txtBaseSignature.Text.Length > 0 && File.Exists(txtBaseSignature.Text))
                            {
                                tvm.LoadTreeview(treeViewBase.Nodes, xb, xv);
                                treeViewNew.Nodes.Clear();
                                SetNewGroupFields();
                                dataGridViewGroupVanilla.Rows.Clear();
                                txtGroupId.Text = "";
                                txtGroupId.Enabled = false;
                                lblTotalNew.Text = "";
                            }
                        }
                        else
                        {
                            MessageBox.Show("This signature OS type does not match the Source signature OS type.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //txtVanillaSignature.Text = "";
                            txtVanillaSignature.Text = curVanillaSignature;
                        }
                    }
                    else
                    {
                        MessageBox.Show("This is an invalid SteelCloud ConfigOS signature file.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //txtVanillaSignature.Text = "";
                        txtVanillaSignature.Text = curVanillaSignature;
                    }
                }
                catch (Exception err)
                {
                    MessageBox.Show("File connot be read it. " + err.Message, msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //txtVanillaSignature.Text = "";
                    txtVanillaSignature.Text = curVanillaSignature;
                }
            }
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!swUpd)
            {
                //
                grpScldInfo.Enabled = true;
                txtType.Visible = false;
                txtIgnore.Visible = false;
                switch (cmbCategory.Text.ToUpper())
                {
                    case "REGISTRY":
                        txtWhere.Text = "";
                        txtWhere.Enabled = true;
                        cmbType.Enabled = true;
                        cmbType.Visible = true;
                        break;
                    case "NO VALIDATION":
                        txtWhere.Enabled = true;
                        txtWhere.Text = "";
                        cmbType.Visible = false;
                        chkIgnoreImport.Visible = false;
                        chkIgnoreExport.Visible = false;
                        chkIgnoreCase.Visible = false;
                        //
                        txtType.Visible = true;
                        txtIgnore.Visible = true;
                        //
                        txtType.Text = "";
                        txtIgnore.Text = "";
                        txtIgnoreReason.Text = "";
                        txtApplied.Text = "";
                        txtValue.Text = "";
                        break;
                    default:
                        txtWhere.Enabled = false;
                        cmbType.Visible = true;
                        cmbType.Enabled = false;
                        cmbType.Text = "";
                        cmbType.SelectedIndex = -1;
                        txtWhere.Text = "";
                        txtWhere.Text = cmbCategory.Text;
                        break;
                }
            }
        }

        private void treeViewNew_AfterSelect(object sender, TreeViewEventArgs e)
        {
            grpScldInfo.Enabled = true;
            swUpd = true;
            //TreeNode selectedNode = treeViewNew.GetNodeAt(e.X, e.Y);
            TreeNode selectedNode = treeViewNew.SelectedNode;
            if (selectedNode.Level == 0)
            {
                //dataGridViewGroup.Rows.Clear();
                lblGroupId.Text = Shared.GetElementName(selectedNode.Text);
                txtGroupId.Text = Shared.GetElementValue(selectedNode.Text);
                txtGroupId.ReadOnly = true;
                // Set values to update on the user interface
                SetValuesToUpdate(selectedNode);
                // Load values from vanilla signature
                if (txtVanillaSignature.Text.Length > 0 && File.Exists(txtVanillaSignature.Text))
                {
                    dataGridViewGroupVanilla.Visible = true;
                    //FillDataGridView(selectedNode);
                    tvm.FillDataGridViewVanilla(dataGridViewGroupVanilla, xv, txtGroupId.Text);
                    // Compare dataGridViewVanilla with current group to highlight the differences
                    Shared.CompareVanillaGroupToCurrentGroupHighlightDifferences(selectedNode, dataGridViewGroupVanilla);
                }
            }
            swUpd = false;
        }

        // Start modifications ver: 1.0.6 date: 02-20-15
        private void btnFilter_Click(object sender, EventArgs e)
        {
            // Start "NewFilterForm" ver: 1.0.9 date: 03-15-16
            //
            // Start Old code, using FilterForm
            //
            //if (treeViewBase.Nodes.Count > 0)
            //{
            //    DialogResult dr = new DialogResult();
            //    // Start "FilterDifferences" ver: 1.0.8 date: 02-24-16
            //    //FilterForm ff = new FilterForm(msgBoxTitle, osTypeSource);
            //    FilterForm ff = new FilterForm(msgBoxTitle, osTypeSource, chkVanilla);
            //    // End "FilterDifferences" ver: 1.0.8 date: 02-24-16
            //    dr = ff.ShowDialog();
            //    string filterKeyword = "";
            //    string selectedFilterElement = "";
            //    if (ff.IsOK)
            //    {
            //        bool mustMatch = false;
            //        if (ff.cmbFiterElement.SelectedIndex >= 0)
            //        {
            //            selectedFilterElement = ff.cmbFiterElement.SelectedItem.ToString().ToUpper();
            //            if (osTypeSource == "LINUX")
            //            {
            //                if (selectedFilterElement == "SEVERITY")
            //                {
            //                    filterKeyword = ff.cmbKeyword.SelectedItem.ToString().ToUpper();
            //                    mustMatch = true;
            //                }
            //                else
            //                {
            //                    filterKeyword = ff.txtKeyword.Text;
            //                }
            //            }
            //            else
            //            {
            //                if (selectedFilterElement == "SEVERITY" || selectedFilterElement == "TYPE" || selectedFilterElement == "IGNORE")
            //                {
            //                    filterKeyword = ff.cmbKeyword.SelectedItem.ToString().ToUpper();
            //                    if (selectedFilterElement == "IGNORE")
            //                    {
            //                        filterKeyword = filterKeyword.Substring(0, 1);
            //                    }
            //                    // Start "IgnoreFilter" ver: 1.0.7 date: 06-04-15
            //                    else
            //                    {
            //                        mustMatch = true;
            //                    }
            //                    // End "IgnoreFilter" ver: 1.0.7 date: 06-04-15
            //                }
            //                else
            //                {
            //                    filterKeyword = ff.txtKeyword.Text;
            //                }
            //            }
            //        }
            //        // Start "FilterDifferences" ver: 1.0.8 date: 02-24-16
            //        //if (filterKeyword.Length > 0)
            //        if (filterKeyword.Length > 0 || selectedFilterElement == "DIFFERENCES")
            //        // End "FilterDifferences" ver: 1.0.8 date: 02-24-16
            //        {
            //            //
            //            // Perform filter
            //            //
            //            // 1. Create a list with the nodes that match the filter criteria and a list of nodes that not match the filter criteria
            //            tvm.SeparateFilteredNodes(treeViewBase.Nodes, selectedFilterElement, filterKeyword, mustMatch, ff.chkNotInclude.Checked);
            //            // 2. Clear the current treeview
            //            treeViewBase.Nodes.Clear();
            //            // 3. Copy the nodes that match to the current treeview
            //            tvm.CopyNodes(treeViewBase.Nodes, tvm.MatchingNodes);
            //            // Add filter labels
            //            lblTotalBase.Text = treeViewBase.Nodes.Count.ToString();
            //            //
            //            btnRemoveFilter.Enabled = true;
            //            if (btnFilterToolTip.Length > 0)
            //            {
            //                btnFilterToolTip = btnFilterToolTip + " | ";
            //            }
            //            if (ff.chkNotInclude.Checked)
            //            {
            //                btnFilterToolTip = btnFilterToolTip + selectedFilterElement + ": NOT " + filterKeyword;
            //            }
            //            else
            //            {
            //                btnFilterToolTip = btnFilterToolTip + selectedFilterElement + ": " + filterKeyword;
            //            }
            //            toolTip1.SetToolTip(btnFilter, btnFilterToolTip);
            //        }
            //    }
            //}
            //
            // End old code, using FilterForm
            //

            //
            // New code, using NewFilterForm
            //
            if (treeViewBase.Nodes.Count > 0 || tvm.RemainingNodes.Count() > 0)
            {
                DialogResult dr = new DialogResult();
                NewFilterForm ff = new NewFilterForm(msgBoxTitle, osTypeSource, chkVanilla, filtersCls);
                //
                dr = ff.ShowDialog();
                string filterKeyword = "";
                string selectedFilterElement = "";
                if (ff.IsOK)
                {
                    filtersCls.Clear();
                    // Copy the remaining nodes on current treeview
                    if (tvm.RemainingNodes.Count() > 0)
                    {
                        tvm.CopyNodes(treeViewBase.Nodes, tvm.RemainingNodes);
                        tvm.RemainingNodes.Clear();
                        tvm.MatchingNodes.Clear();
                    }
                    btnFilterToolTip = "";
                    for (int i = 0; i < ff.listViewFilters.Items.Count; i++)
                    {
                        bool mustMatch = false;
                        selectedFilterElement = ff.listViewFilters.Items[i].SubItems[0].Text.ToUpper();
                        filterKeyword = ff.listViewFilters.Items[i].SubItems[1].Text.ToUpper();
                        if (osTypeSource == "LINUX")
                        {
                            if (selectedFilterElement == "SEVERITY")
                            {
                                mustMatch = true;
                            }
                        }
                        else
                        {
                            if (selectedFilterElement == "SEVERITY" || selectedFilterElement == "TYPE" || selectedFilterElement == "IGNORE")
                            {
                                if (selectedFilterElement == "IGNORE")
                                {
                                    filterKeyword = filterKeyword.Substring(0, 1);
                                }
                                else
                                {
                                    mustMatch = true;
                                }
                            }
                        }
                        //
                        if (filterKeyword.Length > 0 || selectedFilterElement == "DIFFERENCES")
                        {
                            //
                            // Perform filter
                            //
                            // 1. Create a list with the nodes that match the filter criteria and a list of nodes that not match the filter criteria
                            tvm.SeparateFilteredNodes(treeViewBase.Nodes, selectedFilterElement, filterKeyword, mustMatch, Convert.ToBoolean(ff.listViewFilters.Items[i].SubItems[2].Text));
                            // 2. Clear the current treeview
                            treeViewBase.Nodes.Clear();
                            // 3. Copy the nodes that match to the current treeview
                            tvm.CopyNodes(treeViewBase.Nodes, tvm.MatchingNodes);
                            //
                            btnRemoveFilter.Enabled = true;
                            if (btnFilterToolTip.Length > 0)
                            {
                                btnFilterToolTip = btnFilterToolTip + " | ";
                            }
                            if (Convert.ToBoolean(ff.listViewFilters.Items[i].SubItems[2].Text))
                            {
                                btnFilterToolTip = btnFilterToolTip + selectedFilterElement + ": NOT " + filterKeyword;
                            }
                            else
                            {
                                btnFilterToolTip = btnFilterToolTip + selectedFilterElement + ": " + filterKeyword;
                            }
                            filtersCls.Add(new FilterClass(selectedFilterElement, filterKeyword, mustMatch, Convert.ToBoolean(ff.listViewFilters.Items[i].SubItems[2].Text)));
                        }
                    }
                    // Add filter labels and filter tooltip
                    lblTotalBase.Text = treeViewBase.Nodes.Count.ToString();
                    toolTip1.SetToolTip(btnFilter, btnFilterToolTip);
                    // Start "ReAdd-RemoveAllFilters" ver: 1.0.9 date: 04-29-16
                    btnRemoveFilter.Enabled = false;
                    if (ff.listViewFilters.Items.Count > 0)
                    {
                        btnRemoveFilter.Enabled = true;
                    }
                    // End "ReAdd-RemoveAllFilters" ver: 1.0.9 date: 04-29-16
                }
            }
            // End "NewFilterForm" ver: 1.0.9 date: 03-15-16
        }

        // Start "ReAdd-RemoveAllFilters" ver: 1.0.9 date: 04-29-16
        private void btnRemoveFilter_Click(object sender, EventArgs e)
        {
            filtersCls.Clear();
            // Copy the remaining nodes on current treeview
            if (tvm.RemainingNodes.Count() > 0)
            {
                tvm.CopyNodes(treeViewBase.Nodes, tvm.RemainingNodes);
                tvm.RemainingNodes.Clear();
                tvm.MatchingNodes.Clear();
            }
            btnFilterToolTip = "";
            btnRemoveFilter.Enabled = false;
            // Add filter labels and filter tooltip
            lblTotalBase.Text = treeViewBase.Nodes.Count.ToString();
            toolTip1.SetToolTip(btnFilter, btnFilterToolTip);
        }
        // End "ReAdd-RemoveAllFilters" ver: 1.0.9 date: 04-29-16

        // Start "NewFilterForm" ver: 1.0.9 date: 03-15-16
        //private void btnRemoveFilter_Click(object sender, EventArgs e)
        //{
        //    // 1. Copy the remaining nodes on current treeview
        //    tvm.CopyNodes(treeViewBase.Nodes, tvm.RemainingNodes);
        //    tvm.RemainingNodes.Clear();
        //    tvm.MatchingNodes.Clear();
        //    // Start "BuilderDisableTreeviewSort" ver: 1.0.8 date: 08-31-15
        //    //tvm.SortTreeViewNodes(treeViewBase);
        //    // End "BuilderDisableTreeviewSort" ver: 1.0.8 date: 08-31-15
        //    lblTotalBase.Text = treeViewBase.Nodes.Count.ToString();
        //    // Remove filter labels
        //    btnRemoveFilter.Enabled = false;
        //    btnFilterToolTip = "";
        //    toolTip1.SetToolTip(btnFilter, "");
        //}
        // End "NewFilterForm" ver: 1.0.9 date: 03-15-16

        // End modifications ver: 1.0.6 date: 02-20-15

        #region Hide checkboxes from treeview

        public const int TVIF_STATE = 0x8;
        public const int TVIS_STATEIMAGEMASK = 0xF000;
        public const int TV_FIRST = 0x1100;
        public const int TVM_SETITEM = TV_FIRST + 63;

        [DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        // struct used to set node properties
        public struct TVITEM
        {
            public int mask;
            public IntPtr hItem;
            public int state;
            public int stateMask;
            [MarshalAs(UnmanagedType.LPTStr)]
            public String lpszText;
            public int cchTextMax;
            public int iImage;
            public int iSelectedImage;
            public int cChildren;
            public IntPtr lParam;

        }

        private void tree_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            if (e.Node.Level > 0)
                HideCheckBox(e.Node, treeViewBase);
            e.DrawDefault = true;
        }

        private void tree_DrawNode1(object sender, DrawTreeNodeEventArgs e)
        {
            if (e.Node.Level > 0)
                HideCheckBox(e.Node, treeViewNew);
            if (e.Node.Level == 0)
            {
                if (e.Node.Tag.ToString() == "DiffFromSourceAndBase")
                    e.Node.ForeColor = Color.Red;
                if (e.Node.Tag.ToString() == "DiffFromSource")
                    e.Node.ForeColor = Color.Blue;
                if (e.Node.Tag.ToString() == "DiffFromBase")
                    // Start modifications ver: 1.0.6 date: 02-18-15
                    //e.Node.ForeColor = Color.YellowGreen;
                    e.Node.ForeColor = Color.DarkGreen;
                    // End modifications ver: 1.0.6 date: 02-18-15
                if (e.Node.Tag.ToString() == "NoDiff")
                    e.Node.ForeColor = Color.Black;
            }
            e.DrawDefault = true;
        }

        private void HideCheckBox(TreeNode node, TreeView myTreeView)
        {
            TVITEM tvi = new TVITEM();
            tvi.hItem = node.Handle;
            tvi.mask = TVIF_STATE;
            tvi.stateMask = TVIS_STATEIMAGEMASK;
            tvi.state = 0;
            IntPtr lparam = Marshal.AllocHGlobal(Marshal.SizeOf(tvi));
            Marshal.StructureToPtr(tvi, lparam, false);
            SendMessage(myTreeView.Handle, TVM_SETITEM, IntPtr.Zero, lparam);
        }
        #endregion

        #region Auxiliar Methods

        private void SetValuesToSave(Group myGroup)
        {
            myGroup.GroupPairs["GroupTitle"] = txtGroupTitle.Text;
            myGroup.GroupPairs["RuleId"] = txtRuleId.Text;
            myGroup.GroupPairs["Severity"] = txtSeverity.Text;
            myGroup.GroupPairs["RuleVersion"] = txtRuleVersion.Text;
            myGroup.GroupPairs["RuleTitle"] = txtRuleTitle.Text;
            myGroup.GroupPairs["Applied"] = txtApplied.Text;
            myGroup.GroupPairs["IgnoreReason"] = txtIgnoreReason.Text;
            myGroup.GroupPairs["Value"] = txtValue.Text;
            if (osTypeSource == "LINUX")
            {
                myGroup.GroupPairs["Where"] = txtWhere.Text;
                // Start "LinuxValidation" ver: 1.0.9 date: 05-09-16
                //myGroup.GroupPairs["Type"] = txtType.Text;
                myGroup.GroupPairs["Type"] = cmbTypeLinux.Text;
                // End "LinuxValidation" ver: 1.0.9 date: 05-09-16
                myGroup.GroupPairs["Ignore"] = txtIgnore.Text;
            }
            else
            {
                //
                string ignoreValue = GetIgnoreValue();
                //
                myGroup.GroupPairs["Ignore"] = ignoreValue;
                switch (cmbCategory.Text.ToUpper())
                {
                    case "REGISTRY":
                        myGroup.GroupPairs["Where"] = txtWhere.Text;
                        myGroup.GroupPairs["Type"] = cmbType.Text;
                        break;
                    case "NO VALIDATION":
                        myGroup.GroupPairs["Where"] = txtWhere.Text;
                        myGroup.GroupPairs["Type"] = txtType.Text;
                        myGroup.GroupPairs["Ignore"] = txtIgnore.Text;
                        break;
                    default:
                        myGroup.GroupPairs["Where"] = cmbCategory.Text;
                        myGroup.GroupPairs["Type"] = "";
                        break;
                }
            }
        }

        private string GetIgnoreValue()
        {
            string ignoreValue = "";
            try
            {
                List<string> ignoreValues = new List<string>();
                if (chkIgnoreImport.Checked)
                {
                    ignoreValues.Add("1");
                }
                if (chkIgnoreExport.Checked)
                {
                    ignoreValues.Add("2");
                }
                if (chkIgnoreCase.Checked)
                {
                    ignoreValues.Add("3");
                }
                ignoreValue = "0";
                if (ignoreValues.Count > 0)
                {
                    ignoreValue = string.Join(",", ignoreValues);
                }
            }
            catch
            {
            }
            return ignoreValue;
        }

        private void SetValuesToUpdate(TreeNode selectedNode)
        {

            try
            {
                foreach (TreeNode node in selectedNode.Nodes)
                {
                    string elementValue = Shared.GetElementValue(node.Text);
                    //
                    switch (Shared.GetElementName(node.Text).ToUpper())
                    {
                        case "GROUPTITLE":
                            txtGroupTitle.Text = elementValue;
                            break;
                        case "RULEID":
                            txtRuleId.Text = elementValue;
                            break;
                        case "SEVERITY":
                            txtSeverity.Text = elementValue;
                            break;
                        case "RULEVERSION":
                            txtRuleVersion.Text = elementValue;
                            break;
                        case "RULETITLE":
                            txtRuleTitle.Text = elementValue;
                            break;
                        case "WHERE":
                            txtWhere.Text = elementValue;
                            if (osTypeSource == "LINUX")
                            {
                                txtWhere.Enabled = true;
                                cmbType.Visible = false;
                                chkIgnoreImport.Visible = false;
                                chkIgnoreExport.Visible = false;
                                chkIgnoreCase.Visible = false;
                                //
                                txtType.Visible = true;
                                txtIgnore.Visible = true;

                                // Start "LinuxValidation" ver: 1.0.9 date: 05-09-16
                                if (txtWhere.Text == "Auditing System")
                                {
                                    LoadCmbTypeLinux("AUDITING SYSTEM-SWITCH");
                                }
                                else
                                {
                                    LoadCmbTypeLinux("DIFFERENT OF AUDITING SYSTEM");
                                }
                                // End "LinuxValidation" ver: 1.0.9 date: 05-09-16
                            }
                            else
                            {
                                txtType.Visible = false;
                                txtIgnore.Visible = false;
                                chkIgnoreImport.Visible = true;
                                chkIgnoreExport.Visible = true;
                                chkIgnoreCase.Visible = true;
                                if (txtWhere.Text.IndexOf(@"\") > 1)
                                {
                                    cmbCategory.SelectedIndex = cmbCategory.FindString("Registry");
                                    //
                                    txtWhere.Enabled = true;
                                    cmbType.Enabled = true;
                                    cmbType.Visible = true;
                                    //
                                }
                                else
                                {
                                    if (txtWhere.Text.Length < 1 || cmbCategory.FindString(txtWhere.Text) == -1)
                                    {
                                        cmbCategory.SelectedIndex = cmbCategory.FindString("No Validation");
                                        txtWhere.Enabled = true;
                                        cmbType.Visible = false;
                                        chkIgnoreImport.Visible = false;
                                        chkIgnoreExport.Visible = false;
                                        chkIgnoreCase.Visible = false;
                                        //
                                        txtType.Visible = true;
                                        txtIgnore.Visible = true;
                                        //
                                    }
                                    else
                                    {
                                        txtWhere.Enabled = false;
                                        cmbType.Visible = true;
                                        cmbType.Enabled = false;
                                        cmbType.Text = "";
                                        //
                                        cmbCategory.SelectedIndex = cmbCategory.FindString(txtWhere.Text);
                                    }
                                }
                            }
                            break;
                        case "APPLIED":
                            txtApplied.Text = elementValue;
                            break;
                        case "TYPE":
                            if (osTypeSource == "LINUX")
                            {
                                // Start "LinuxValidation" ver: 1.0.9 date: 05-09-16
                                //txtType.Text = elementValue;
                                cmbTypeLinux.SelectedIndex = -1;
                                cmbTypeLinux.Text = "";
                                if (elementValue.Length > 0)
                                {
                                    int index = cmbTypeLinux.FindStringExact(elementValue);
                                }
                                if (cmbTypeLinux.Text == "")
                                {
                                    cmbTypeLinux.Text = elementValue;
                                }
                                // Start "LinuxValidation" ver: 1.0.9 date: 05-09-16
                            }
                            else
                            {
                                cmbType.SelectedIndex = -1;
                                cmbType.Text = "";
                                if (elementValue.Length > 0)
                                {
                                    cmbType.SelectedIndex = cmbType.FindString(elementValue);
                                }
                            }
                            break;
                        case "VALUE":
                            txtValue.Text = elementValue;
                            break;
                        case "IGNORE":
                            if (osTypeSource == "LINUX")
                            {
                                txtIgnore.Text = elementValue;
                            }
                            else
                            {
                                chkIgnoreImport.Checked = false;
                                chkIgnoreExport.Checked = false;
                                chkIgnoreCase.Checked = false;
                                if (elementValue.Length > 0)
                                {
                                    string[] ignores = elementValue.Split(',');
                                    foreach (string ignore in ignores)
                                    {
                                        switch (ignore)
                                        {
                                            case "1":
                                                chkIgnoreImport.Checked = true;
                                                break;
                                            case "2":
                                                chkIgnoreExport.Checked = true;
                                                break;
                                            case "3":
                                                chkIgnoreCase.Checked = true;
                                                break;
                                        }
                                    }
                                }
                            }
                            break;
                        case "IGNOREREASON":
                            txtIgnoreReason.Text = elementValue;
                            break;
                    }
                }
            }
            catch
            {
            }
        }

        private void SetNewGroupFields()
        {
            txtGroupId.Text = "";
            txtGroupTitle.Text = "";
            txtRuleId.Text = "";
            txtSeverity.Text = "";
            txtRuleVersion.Text = "";
            txtRuleTitle.Text = "";
            cmbCategory.Text = "";
            cmbCategory.SelectedIndex = -1;
            txtWhere.Text = "";
            txtApplied.Text = "";
            cmbType.Text = "";
            cmbType.SelectedIndex = -1;
            txtValue.Text = "";
            chkIgnoreImport.Checked = false;
            chkIgnoreExport.Checked = false;
            chkIgnoreCase.Checked = false;
            txtIgnoreReason.Text = "";
            txtType.Text = "";
            txtIgnore.Text = "";
        }

        #endregion

        // Start "BuilderEnhancements" ver: 1.0.8 date: 01-13-16
        private void SaveSignature()
        {
            // Configure save file dialog box
            SaveFileDialog sdlg = new SaveFileDialog();
            sdlg.Filter = "xml files (*.xml)|*.xml";
            sdlg.RestoreDirectory = true;
            if (sdlg.ShowDialog() == DialogResult.OK)
            {
                // Save "Groups" attributes
                //tvm.GroupsElementNew.SetAttributeValue("CreateDate", txtCreationDate.Text);
                tvm.GroupsElementNew.SetAttributeValue("CreateDate", dtpCreationDate.Value.ToShortDateString());
                tvm.GroupsElementNew.SetAttributeValue("AuthorName", txtAuthor.Text);
                tvm.GroupsElementNew.SetAttributeValue("Version", txtVersion.Text);
                //
                tvm.SaveTreeViewToFile(treeViewNew.Nodes, sdlg.FileName);
                if (tvm.ErrorMessage.Length > 0)
                {
                    MessageBox.Show(tvm.ErrorMessage, msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (treeViewNew.Nodes.Count > 0 && newSignatureChanges)
            {
                DialogResult result = MessageBox.Show("Do you want to save the new signature?", msgBoxTitle, MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    SaveSignature();
                }
            }
        }

        private TreeNode NodeToBeDeleted;
        private void treeViewNew_ItemDrag(object sender, ItemDragEventArgs e)
        {
            NodeToBeDeleted = (TreeNode)e.Item;
            string strItem = e.Item.ToString();
            DoDragDrop(strItem, DragDropEffects.Copy | DragDropEffects.Move);
        }

        private void treeViewNew_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        private void treeViewNew_DragDrop(object sender, DragEventArgs e)
        {
            Point Position = new Point(0, 0);
            Position.X = e.X;
            Position.Y = e.Y;
            Position = treeViewNew.PointToClient(Position);
            TreeNode DropNode = treeViewNew.GetNodeAt(Position);
            if (DropNode != null && DropNode.Parent == NodeToBeDeleted.Parent && NodeToBeDeleted.Level == 0)
            {
                TreeNode DragNode = NodeToBeDeleted;

                //DropNode.Parent.Nodes.Remove(this.NodeToBeDeleted);
                DropNode.Nodes.Remove(NodeToBeDeleted);

                //DropNode.Parent.Nodes.Insert(DropNode.Index + 1, DragNode);
                //treeViewNew.Nodes.Insert(DropNode.Index + 1, DragNode);
                treeViewNew.Nodes.Insert(DropNode.Index, DragNode);
                treeViewNew.SelectedNode = DragNode;
            }
            //
            if (DropNode == null && NodeToBeDeleted.Level == 0)
            {
                TreeNode DragNode = NodeToBeDeleted;

                treeViewNew.Nodes.Remove(NodeToBeDeleted);
                treeViewNew.Nodes.Add(DragNode);
                treeViewNew.SelectedNode = DragNode;
            }
        }

        private void treeViewNew_DragOver(object sender, DragEventArgs e)
        {
            treeViewNew.Scroll();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.F))
            {
                if (tabControl1.SelectedTab.Name == "tabSignatureAdmin" && treeViewNew.Nodes.Count > 1)
                {
                    SearchForm sf = new SearchForm(msgBoxTitle, treeViewNew);
                    sf.ShowDialog();
                    return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        // End "BuilderEnhancements" ver: 1.0.8 date: 01-13-16

        // Start "LinuxValidation" ver: 1.0.9 date: 05-09-16

        private void txtWhere_Leave(object sender, EventArgs e)
        {
            if (txtWhere.Text == "Auditing System")
            {
                LoadCmbTypeLinux("AUDITING SYSTEM-SWITCH");
            }
            else
            {
                LoadCmbTypeLinux("DIFFERENT OF AUDITING SYSTEM");
            }
        }
        private void LoadCmbTypeLinux(string validationGroupName)
        {
            cmbTypeLinux.Items.Clear();
            appPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            linuxVal.GetValidValues(appPath + @"\Resources\LinuxValidation.xml", validationGroupName);
            if (linuxVal.RetValues.Count > 0)
            {
                foreach (string retValue in linuxVal.RetValues)
                {
                    cmbTypeLinux.Items.Add(retValue);
                }
            }
        }

        // End "LinuxValidation" ver: 1.0.9 date: 05-09-16
       
        #endregion
        
        #region Signature combine & split tab
        // Start "BuilderEnhancements" ver: 1.0.8 date: 01-25-16
        private void rbSignatureCombine_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSignatureCombine.Checked)
            {
                grpSignatureSplit.Visible = false;
                grpSignatureCombine.Size = new Size(640, 354);
                grpSignatureCombine.Location = new Point(8, 32);
                grpSignatureCombine.Visible = true;
            }
            if (rbSignatureSplit.Checked)
            {
                grpSignatureCombine.Visible = false;
                grpSignatureSplit.Size = new Size(640,354);
                grpSignatureSplit.Location = new Point(8,32);
                grpSignatureSplit.Visible = true;
                cmbParts.SelectedIndex = 0;
            }
        }

        private void btnBrowseSigToCombine_Click(object sender, EventArgs e)
        {
            openFileDialog1.DefaultExt = "xml";
            // The Filter property requires a search string after the pipe ( | )
            openFileDialog1.Filter = "Signature (*.xml)|" + "*.xml";
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtSignatureToCombine.Text = openFileDialog1.FileName;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (File.Exists(txtSignatureToCombine.Text))
            {
                if (!fd.IsXMLFile(txtSignatureToCombine.Text, "Groups", "ConfigOS"))
                {
                    MessageBox.Show("The selected file is not a valid signature file.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                for(int i=0;i<listViewSignaturesToCombine.Items.Count;i++)
                {
                    if (listViewSignaturesToCombine.Items[i].SubItems[0].Text == txtSignatureToCombine.Text)
                    {
                        MessageBox.Show("The selected file has been already added.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (listViewSignaturesToCombine.Items[i].SubItems[1].Text != "" && listViewSignaturesToCombine.Items[i].SubItems[1].Text == txtSignaturePrefix.Text)
                    {
                        MessageBox.Show("The prefix " + txtSignaturePrefix.Text + " has been used.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                listViewSignaturesToCombine.Items.Add(new ListViewItem(new string[] { txtSignatureToCombine.Text, txtSignaturePrefix.Text }));
                txtSignatureToCombine.Text = "";
                txtSignaturePrefix.Text = "";
            }
            else
            {
                MessageBox.Show("The selected file does not exist.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBrowseCombineSignatureName_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "xml files (*.xml)|*.xml";
            saveFileDialog1.OverwritePrompt = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtCombinedSignatureName.Text = saveFileDialog1.FileName;
            }
        }

        private void btnCancelCandS_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOKCandS_Click(object sender, EventArgs e)
        {
            #region Signature combine
            //
            // Combine Signature
            //
            if (rbSignatureCombine.Checked)
            {

                if (listViewSignaturesToCombine.Items.Count < 2)
                {
                    MessageBox.Show("It must be more than one file to combine.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (txtCombinedSignatureName.Text.Length < 1)
                {
                    MessageBox.Show("Please enter a file name for the combined signature.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!Directory.Exists(Path.GetDirectoryName(txtCombinedSignatureName.Text)))
                {
                    MessageBox.Show("Please enter a valid path for the combined signature.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                for (int i = 0; i < listViewSignaturesToCombine.Items.Count; i++)
                {
                    if (listViewSignaturesToCombine.Items[i].SubItems[0].Text == txtCombinedSignatureName.Text)
                    {
                        MessageBox.Show("The combined signature file cannot be one of the signatures to combine.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                //
                lstErrors.Items.Clear();
                lstErrors.Items.Add("Processing...");
                this.Refresh();
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    // Copy the first file selected to be the base file
                    File.Copy(listViewSignaturesToCombine.Items[0].SubItems[0].Text, txtCombinedSignatureName.Text, true);
                    XDocument baseFile = XDocument.Load(txtCombinedSignatureName.Text);
                    baseFile.Descendants("Group").Remove();
                    for (int i = 0; i < listViewSignaturesToCombine.Items.Count; i++)
                    {
                        XDocument fileToAdd = XDocument.Load(listViewSignaturesToCombine.Items[i].SubItems[0].Text);
                        foreach (XElement group in fileToAdd.Descendants("Group"))
                        {
                            // Check if group name already exists
                            try
                            {
                                XElement elementToChange = baseFile.Descendants("Group")
                                                            .Single(x => x.Element("GroupId").Value.ToUpper().Trim() == listViewSignaturesToCombine.Items[i].SubItems[1].Text.Trim() + group.Element("GroupId").Value.ToUpper().Trim());
                                lstErrors.Items.Add(listViewSignaturesToCombine.Items[i].SubItems[0].Text + ". The group " + group.Element("GroupId").Value + " was not added. Duplicate.");
                            }
                            catch
                            {
                                // Add the group the group does not exist
                                group.Element("GroupId").Value = listViewSignaturesToCombine.Items[i].SubItems[1].Text.Trim() + group.Element("GroupId").Value.Trim();
                                baseFile.Element("Groups").Element("Items").Add(group);
                            }
                        }
                    }
                    // Save the new combine signatures file
                    baseFile.Save(txtCombinedSignatureName.Text);
                    lstErrors.Items.Add("Done");
                    Cursor.Current = Cursors.Default;
                }
                catch (Exception err)
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Unable to process the request. " + err.Message, msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            #endregion

            #region Signature split
            //
            // Split Signature
            //
            if (rbSignatureSplit.Checked)
            {
                if (!File.Exists(txtSignatureToSplit.Text))
                {
                    MessageBox.Show("PLease select an existing signature.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!fd.IsXMLFile(txtSignatureToSplit.Text, "Groups", "ConfigOS"))
                {
                    MessageBox.Show("The selected file is not a valid signature file.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                XDocument baseDocument = XDocument.Load(txtSignatureToSplit.Text);
                int totalGroups = baseDocument.Descendants("Group").Count();
                int groupsPerFile = totalGroups / Convert.ToInt32(cmbParts.SelectedItem.ToString());
                if (groupsPerFile < 1)
                {
                    MessageBox.Show("This file cannot be split in " + Convert.ToInt32(cmbParts.SelectedItem.ToString()) + " parts.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // Start "SplitKeepPartsTogether" ver: 1.0.9 date: 03-14-16
                //SplitSignature(txtSignatureToSplit.Text, groupsPerFile, Convert.ToInt32(cmbParts.SelectedItem.ToString()));
                string messageToDisplay = "The signature split finished successfully.";
                string errMessage = SplitSignature_KeepPartsTogether(txtSignatureToSplit.Text, groupsPerFile, Convert.ToInt32(cmbParts.SelectedItem.ToString()));
                if (errMessage.Length > 0)
                {
                    messageToDisplay = errMessage;
                }
                //MessageBox.Show("The signature split finished successfully.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                MessageBox.Show(messageToDisplay, msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                // End "SplitKeepPartsTogether" ver: 1.0.9 date: 03-14-16
            }

            #endregion
        }

        private void btnRemoveSignature_Click(object sender, EventArgs e)
        {
            try
            {
                // Start "UIChanges" ver: 1.0.9 date: 07-18-16
                ListView listView = listViewSignaturesToCombine;
                foreach (ListViewItem item in listView.SelectedItems)
                {
                    item.Remove();
                }
                //if (listViewSignaturesToCombine.SelectedItems.Count > 0)
                //{
                //    ListViewItem temp = listViewSignaturesToCombine.SelectedItems[0];
                //    listViewSignaturesToCombine.Items.Remove(temp);
                //}
                // End "UIChanges" ver: 1.0.9 date: 07-18-16
            }
            catch
            {
            }

        }

        private void btnBrowseSplit_Click(object sender, EventArgs e)
        {
            openFileDialog1.DefaultExt = "Signature";
            openFileDialog1.Filter = "Signature file (*.xml)|" + "*.xml";
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtSignatureToSplit.Text = openFileDialog1.FileName;
                //
            }
        }

        private void SplitSignature(string inputFile, int groupsPerSplit, int numberOfSplits)
        {
            string outputFile = Path.GetDirectoryName(inputFile) + @"\" + Path.GetFileNameWithoutExtension(inputFile);
            XDocument baseDocument = XDocument.Load(inputFile);

            XDocument emptyDocument = XDocument.Load(inputFile);
            emptyDocument.Descendants("Group").Remove();

            int ccBreaks = 1;
            int addedGroups = 0;
            foreach (XElement group in baseDocument.Descendants("Group"))
            {
                if (ccBreaks < numberOfSplits)
                {
                    emptyDocument.Element("Groups").Element("Items").Add(group);
                    addedGroups++;
                    if (addedGroups == groupsPerSplit)
                    {
                        emptyDocument.Save(outputFile + ccBreaks.ToString() + ".xml");
                        emptyDocument.Descendants("Group").Remove();
                        ccBreaks++;
                        addedGroups = 0;
                    }
                }
                else
                {
                    emptyDocument.Element("Groups").Element("Items").Add(group);
                }
            }
            emptyDocument.Save(outputFile + ccBreaks.ToString() + ".xml");

        }

        // Start "SplitKeepPartsTogether" ver: 1.0.9 date: 03-14-16
        private string SplitSignature_KeepPartsTogether(string inputFile, int groupsPerSplit, int numberOfSplits)
        {
            string errorMessage = "";
            try
            {
                string outputFile = Path.GetDirectoryName(inputFile) + @"\" + Path.GetFileNameWithoutExtension(inputFile);
                XDocument baseDocument = XDocument.Load(inputFile);

                XDocument emptyDocument = XDocument.Load(inputFile);
                emptyDocument.Descendants("Group").Remove();

                int ccBreaks = 1;
                int addedGroups = 0;
                List<XElement> groups = baseDocument.Descendants("Group").ToList();
                for (int i = 0; i < groups.Count(); i++)
                {
                    XElement group = groups[i];
                    if (ccBreaks < numberOfSplits)
                    {
                        emptyDocument.Element("Groups").Element("Items").Add(group);
                        addedGroups++;
                        if (addedGroups == groupsPerSplit)
                        {
                            if (i + 1 < groups.Count())
                            {
                                XElement nextGroup = groups[i + 1];
                                //
                                string groupIdCurrent = group.Element("GroupId").Value.Trim().ToUpper();
                                string groupIdNext = nextGroup.Element("GroupId").Value.Trim().ToUpper();
                                string[] groupIdCurrentParts = groupIdCurrent.Split(new string[] { "PART" }, StringSplitOptions.None);
                                string[] groupIdNextParts = groupIdNext.Split(new string[] { "PART" }, StringSplitOptions.None);
                                //
                                if (groupIdNext.Contains("PART") && groupIdCurrentParts[0].Trim() == groupIdNextParts[0].Trim())
                                {
                                    i++;
                                    while (i < groups.Count())
                                    {
                                        group = groups[i];
                                        string groupId = group.Element("GroupId").Value.Trim().ToUpper();
                                        string[] groupIdParts = groupId.Split(new string[] { "PART" }, StringSplitOptions.None);
                                        if (groupId.Contains("PART") && groupIdParts[0].Trim() == groupIdCurrentParts[0].Trim())
                                        {
                                            emptyDocument.Element("Groups").Element("Items").Add(group);
                                            i++;
                                        }
                                        else
                                        {
                                            i--;
                                            break;
                                        }
                                    }
                                }
                            }
                            emptyDocument.Save(outputFile + ccBreaks.ToString() + ".xml");
                            emptyDocument.Descendants("Group").Remove();
                            ccBreaks++;
                            addedGroups = 0;
                        }
                    }
                    else
                    {
                        emptyDocument.Element("Groups").Element("Items").Add(group);
                    }
                }
                //
                if (emptyDocument.Descendants("Group").Count() > 0)
                {
                    emptyDocument.Save(outputFile + ccBreaks.ToString() + ".xml");
                }
            }
            catch (Exception err)
            {
                errorMessage = "The signature file was not successfully split. " + err.Message;
            }
            return errorMessage;
        }
        // End "SplitKeepPartsTogether" ver: 1.0.9 date: 03-14-16

        // End "BuilderEnhancements" ver: 1.0.8 date: 01-25-16
        #endregion

        #region Unzip SCLD file tab

        // Start "UnzipSCLDFile" ver: 1.0.9 date: 03-22-16
        private void btnBrowseSCLDZipFile_Click(object sender, EventArgs e)
        {
            openFileDialog1.DefaultExt = "zip";
            openFileDialog1.Filter = "SCLD zip file (*.zip)|" + "*.zip";
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtSCLDZipFile.Text = openFileDialog1.FileName;
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

        private void btnZipCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnZipOK_Click(object sender, EventArgs e)
        {
            if (!File.Exists(txtSCLDZipFile.Text))
            {
                MessageBox.Show("PLease select an existing SCLD zip file.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!Directory.Exists(txtNewKeyFolder.Text))
            {
                MessageBox.Show("Please select an existing folder.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string zipToUnpack = txtSCLDZipFile.Text;
            string unpackDirectory = txtNewKeyFolder.Text;
            Cursor.Current = Cursors.WaitCursor;
            Key kunzip = new Key();
            kunzip.EncryptionKeysFoundry();
            try
            {
                ZipFile zipFile = ZipFile.Read(zipToUnpack);
                zipFile.Password = kunzip.AesKey;
                foreach (ZipEntry zipEntry in zipFile)
                {
                    // If the zip entry is a file and the UncompressedSize is > 0, it has to be password protected (DOTNetZip encrypts at file level)
                    if (!zipEntry.IsDirectory)
                    {
                        if (zipEntry.UncompressedSize > 0 && !zipEntry.UsesEncryption)
                        {
                            MessageBox.Show("The unzip proccess failed.\nPlease make sure you are selecting a SCLD zip file.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    //
                    zipEntry.Extract(unpackDirectory, ExtractExistingFileAction.OverwriteSilently);
                }
                MessageBox.Show("The file was successfully unzipped.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception err)
            {
                MessageBox.Show("The unzip proccess failed.\n" + err.Message + "\nPlease make sure you are selecting a SCLD zip file.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Cursor.Current = Cursors.Default;
            }
        }

        // End "UnzipSCLDFile" ver: 1.0.9 date: 03-22-16

        #endregion
    }
    
    #region Treeview methods
    // Start "BuilderEnhancements" ver: 1.0.8 date: 02-11-16
    public static class TreeViewMethods
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        public static void Scroll(this Control control)
        {
            var pt = control.PointToClient(Cursor.Position);

            if ((pt.Y + 20) > control.Height)
            {
                // scroll down
                SendMessage(control.Handle, 277, (IntPtr)1, (IntPtr)0);
            }
            else if (pt.Y < 20)
            {
                // scroll up
                SendMessage(control.Handle, 277, (IntPtr)0, (IntPtr)0);
            }
        }
    }
    // End "BuilderEnhancements" ver: 1.0.8 date: 02-11-16
    #endregion
    
}
