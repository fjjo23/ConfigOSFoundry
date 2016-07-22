using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace VeriSignature
{
    public partial class AboutForm : Form
    {
        string msgBoxTitle;
        public AboutForm(string _msgBoxTitle)
        {
            msgBoxTitle = _msgBoxTitle;
            InitializeComponent();
            this.Text = msgBoxTitle;
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            label1.Text = msgBoxTitle;
            Version ver = new Version(System.Windows.Forms.Application.ProductVersion);
            lblVersion.Text = "Version  " + ver;
            // Gets 
            RegistryKey regKeyPath = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows NT\CurrentVersion");
            if (regKeyPath != null)
            {
                if (regKeyPath.GetValue("RegisteredOrganization") != null)
                {
                    txtRegisteredOrg.Text = regKeyPath.GetValue("RegisteredOrganization").ToString();
                }
                if (regKeyPath.GetValue("RegisteredOwner") != null)
                {
                    txtRegisteredOwner.Text = regKeyPath.GetValue("RegisteredOwner").ToString();
                }
            }
            // Start "ProductLicense" ver: 1.0.7 date: 05-21-15
            lblLicenseExpiration.Text = ProductLicense.Runtime.LicenceExpitationDate;
            // End "ProductLicense" ver: 1.0.7 date: 05-21-15
        }
    }
}