using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using LogicNP.CryptoLicensing;
using System.Diagnostics;

namespace VeriSignature
{
    public partial class ProductLicenseForm : Form
    {
        string msgBoxTitle;
        string prouctLicensePath;
        public ProductLicenseForm(string _msgBoxTitle, string _prouctLicensePath)
        {
            msgBoxTitle = _msgBoxTitle;
            prouctLicensePath = _prouctLicensePath;
            InitializeComponent();
        }

        private void btnProductLicense_Click(object sender, EventArgs e)
        {
            openFileDialog1.DefaultExt = "Fkey";
            openFileDialog1.Filter = "ConfigOS FoundyKey license (*.lic)|" + "*.*";
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtProductLicense.Text = openFileDialog1.FileName;
                //
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!File.Exists(txtProductLicense.Text))
            {
                MessageBox.Show("Please select an existing file.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //
            // Validate the product license
            //
            CryptoLicense license = ProductLicense.Methods.CreateLicense();

            license.StorageMode = LicenseStorageMode.ToFile;
            license.FileStoragePath = txtProductLicense.Text;

            // Load the license from file 
            if (license.Load())
            {
                if (license.Status != LicenseStatus.Valid)
                {
                    MessageBox.Show("ConfigOS Foundry license validation failed.\nThe application will be closed.\n" + license.GetAllStatusExceptionsAsString() + "\nPlease select a correct license file and restart the application.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    license.Dispose();
                    Process.GetCurrentProcess().Kill();
                }
                else
                {
                }
            }
            else
            {
                MessageBox.Show("ConfigOS Foundry license not found.\nThe application will be closed.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                license.Dispose();
                Process.GetCurrentProcess().Kill();
            }
            //
            //
            // Copy the existing Foundrykey to the application folder
            try
            {
                File.Copy(txtProductLicense.Text, prouctLicensePath, true);
            }
            catch (Exception err)
            {
                MessageBox.Show("The ConfigOS Foundry license was not successfully copied to the ConfigOS Foundry application folder.\nThe application will be closed." + err.Message, msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                Process.GetCurrentProcess().Kill();
            }
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The ConfigOS Foundry license file has not been successfully selected.\nThe application will be closed.", msgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            Process.GetCurrentProcess().Kill();
            this.Close();
        }

    }
}
