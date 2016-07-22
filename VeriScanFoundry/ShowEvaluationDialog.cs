//////////////////////////////////////////////////////////////////////////
//                         LogicNP Software, Inc.                      
//              Copyright (c) 2009-2010 All Rights reserved.
//
// This file and its contents are protected by International copyright 
// laws. Unauthorized reproduction and/or distribution of all or any 
// portion of the code contained herein is strictly prohibited and will 
// result in severe civil and criminal penalties. Any violations of this 
// copyright will be prosecuted to the fullest extent possible under law.
//                                                                        
// THE SOURCE CODE CONTAINED HEREIN AND IN RELATED FILES IS THE PROPERTY  
// OF LOGICNP SOFTWARE. THIS SOURCE CODE CAN ONLY BE USED UNDER THE TERMS 
// AND CONDITIONS OUTLINED IN THE SOURCE CODE LICENSE AGREEMENT EXECUTED 
// BETWEEN LOGICNP SOFTWARE AND YOU.
//  
// UNDER NO CIRCUMSTANCES MAY THE SOURCE CODE BE USED IN WHOLE OR IN     
// PART, AS THE BASIS FOR CREATING A PRODUCT THAT PROVIDES THE SAME, OR  
// SUBSTANTIALLY THE SAME, FUNCTIONALITY AS ANY LOGICNP SOFTWARE PRODUCT.
//
// UNDER NO CIRCUMSTANCES MAY ANY PORTION OF THE SOURCE CODE BE
// DISTRIBUTED, DISCLOSED OR OTHERWISE MADE AVAILABLE TO ANY THIRD PARTY 
// WITHOUT THE EXPRESS WRITTEN CONSENT OF LOGICNP SOFTWARE.
//
// THE REGISTERED DEVELOPER(S) ACKNOWLEDGES THAT THIS SOURCE CODE
// CONTAINS VALUABLE AND PROPRIETARY TRADE SECRETS OF LOGICNP SOFTWARE.
// THE REGISTERED DEVELOPER(S) AGREES TO MAKE EVERY EFFORT TO INSURE
// ITS CONFIDENTIALITY.
//
// THIS COPYRIGHT NOTICE MAY NOT BE REMOVED FROM THIS FILE.
//////////////////////////////////////////////////////////////////////////


using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;

using LogicNP.CryptoLicensing;

/*
Add this file to your Visual Studio Project and use as follows:

  Usage:
  CryptoLicense lic = new CryptoLicense();
  lic.ValidationKey=...
  lic.LicenseCode = .... OR lic.Load()
  ...
  ...
  EvaluationInfoDialog dialog = new EvaluationInfoDialog(lic);
  dialog.ProductName = productName;
  dialog.PurchaseURL = purchaseURL;
  dialog.StartPane = startPane;
  if(dialog.ShowDialogInt()==....)
  ....
 
*/

// ***REPLACE NAMESPACE below with your own to avoid conflict with the EvaluationInfoDialog class from LogicNP.CryptoLicensing.dll
//namespace LogicNP.CryptoLicensing
namespace VeriSignature
{

    public class EvaluationInfoDialog : Form
    {

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnEnterLicense = new System.Windows.Forms.Button();
            this.btnPurchase = new System.Windows.Forms.Button();
            this.btnContinueEval = new System.Windows.Forms.Button();
            this.lblThankYou = new System.Windows.Forms.Label();
            this.lblEnterLicense = new System.Windows.Forms.Label();
            this.lbPurchase = new System.Windows.Forms.Label();
            this.lblContinueEval = new System.Windows.Forms.Label();
            this.pnlShowEvaluationInfo = new System.Windows.Forms.Panel();
            this.lblExit = new System.Windows.Forms.Label();
            this.btnExitProgram = new System.Windows.Forms.Button();
            this.pnlEnterLicense = new System.Windows.Forms.Panel();
            this.btnOfflineActivation = new System.Windows.Forms.Button();
            this.lblInvalidLicenseEntered = new System.Windows.Forms.Label();
            this.btnEnterLicenseGoBack = new System.Windows.Forms.Button();
            this.btnUnlock = new System.Windows.Forms.Button();
            this.btnPasteFromClipboard = new System.Windows.Forms.Button();
            this.btnLoadFromFile = new System.Windows.Forms.Button();
            this.txtLicenseCode = new System.Windows.Forms.TextBox();
            this.lblHeader = new System.Windows.Forms.Label();
            this.pnlLicenseInfo = new System.Windows.Forms.Panel();
            this.btnStartProgram = new System.Windows.Forms.Button();
            this.lblLienseInfo = new System.Windows.Forms.Label();
            this.lblThankYou_pnlLicenseInfo = new System.Windows.Forms.Label();
            this.pnlOfflineActivation = new System.Windows.Forms.Panel();
            this.btnBack_pnlOfflineActivation = new System.Windows.Forms.Button();
            this.btnCopyComputerID = new System.Windows.Forms.Button();
            this.txtComputerID = new System.Windows.Forms.TextBox();
            this.lblOfflineActivation = new System.Windows.Forms.Label();
            this.pnlProgressBars = new System.Windows.Forms.Panel();
            this.pnlShowEvaluationInfo.SuspendLayout();
            this.pnlEnterLicense.SuspendLayout();
            this.pnlLicenseInfo.SuspendLayout();
            this.pnlOfflineActivation.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnEnterLicense
            // 
            this.btnEnterLicense.Location = new System.Drawing.Point(19, 178);
            this.btnEnterLicense.Name = "btnEnterLicense";
            this.btnEnterLicense.Size = new System.Drawing.Size(150, 29);
            this.btnEnterLicense.TabIndex = 1;
            this.btnEnterLicense.Text = "Enter License...";
            this.btnEnterLicense.Click += new System.EventHandler(this.btnEnterLicense_Click);
            // 
            // btnPurchase
            // 
            this.btnPurchase.Location = new System.Drawing.Point(19, 265);
            this.btnPurchase.Name = "btnPurchase";
            this.btnPurchase.Size = new System.Drawing.Size(150, 29);
            this.btnPurchase.TabIndex = 2;
            this.btnPurchase.Text = "Purchase Information...";
            this.btnPurchase.Click += new System.EventHandler(this.btnPurchase_Click);
            // 
            // btnContinueEval
            // 
            this.btnContinueEval.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnContinueEval.Location = new System.Drawing.Point(19, 99);
            this.btnContinueEval.Name = "btnContinueEval";
            this.btnContinueEval.Size = new System.Drawing.Size(150, 29);
            this.btnContinueEval.TabIndex = 0;
            this.btnContinueEval.Text = "Continue";
            // 
            // lblThankYou
            // 
            this.lblThankYou.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblThankYou.Location = new System.Drawing.Point(15, 10);
            this.lblThankYou.Name = "lblThankYou";
            this.lblThankYou.Size = new System.Drawing.Size(414, 36);
            this.lblThankYou.TabIndex = 2;
            this.lblThankYou.Text = "Thank you";
            // 
            // lblEnterLicense
            // 
            this.lblEnterLicense.Location = new System.Drawing.Point(15, 133);
            this.lblEnterLicense.Name = "lblEnterLicense";
            this.lblEnterLicense.Size = new System.Drawing.Size(411, 39);
            this.lblEnterLicense.TabIndex = 4;
            this.lblEnterLicense.Text = "enter license description";
            this.lblEnterLicense.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // lbPurchase
            // 
            this.lbPurchase.Location = new System.Drawing.Point(15, 220);
            this.lbPurchase.Name = "lbPurchase";
            this.lbPurchase.Size = new System.Drawing.Size(411, 42);
            this.lbPurchase.TabIndex = 4;
            this.lbPurchase.Text = "enter license description";
            this.lbPurchase.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // lblContinueEval
            // 
            this.lblContinueEval.Location = new System.Drawing.Point(15, 45);
            this.lblContinueEval.Name = "lblContinueEval";
            this.lblContinueEval.Size = new System.Drawing.Size(414, 52);
            this.lblContinueEval.TabIndex = 4;
            this.lblContinueEval.Text = "Continue evaluation description";
            this.lblContinueEval.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // pnlShowEvaluationInfo
            // 
            this.pnlShowEvaluationInfo.Controls.Add(this.lblThankYou);
            this.pnlShowEvaluationInfo.Controls.Add(this.lblContinueEval);
            this.pnlShowEvaluationInfo.Controls.Add(this.btnEnterLicense);
            this.pnlShowEvaluationInfo.Controls.Add(this.lblExit);
            this.pnlShowEvaluationInfo.Controls.Add(this.lbPurchase);
            this.pnlShowEvaluationInfo.Controls.Add(this.btnExitProgram);
            this.pnlShowEvaluationInfo.Controls.Add(this.btnPurchase);
            this.pnlShowEvaluationInfo.Controls.Add(this.lblEnterLicense);
            this.pnlShowEvaluationInfo.Controls.Add(this.btnContinueEval);
            this.pnlShowEvaluationInfo.Location = new System.Drawing.Point(230, 27);
            this.pnlShowEvaluationInfo.Name = "pnlShowEvaluationInfo";
            this.pnlShowEvaluationInfo.Size = new System.Drawing.Size(446, 390);
            this.pnlShowEvaluationInfo.TabIndex = 5;
            // 
            // lblExit
            // 
            this.lblExit.Location = new System.Drawing.Point(15, 299);
            this.lblExit.Name = "lblExit";
            this.lblExit.Size = new System.Drawing.Size(411, 42);
            this.lblExit.TabIndex = 4;
            this.lblExit.Text = "enter license description";
            this.lblExit.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // btnExitProgram
            // 
            this.btnExitProgram.Location = new System.Drawing.Point(19, 344);
            this.btnExitProgram.Name = "btnExitProgram";
            this.btnExitProgram.Size = new System.Drawing.Size(150, 29);
            this.btnExitProgram.TabIndex = 3;
            this.btnExitProgram.Text = "Exit";
            this.btnExitProgram.Click += new System.EventHandler(this.btnExitProgram_Click);
            // 
            // pnlEnterLicense
            // 
            this.pnlEnterLicense.Controls.Add(this.btnOfflineActivation);
            this.pnlEnterLicense.Controls.Add(this.lblInvalidLicenseEntered);
            this.pnlEnterLicense.Controls.Add(this.btnEnterLicenseGoBack);
            this.pnlEnterLicense.Controls.Add(this.btnUnlock);
            this.pnlEnterLicense.Controls.Add(this.btnPasteFromClipboard);
            this.pnlEnterLicense.Controls.Add(this.btnLoadFromFile);
            this.pnlEnterLicense.Controls.Add(this.txtLicenseCode);
            this.pnlEnterLicense.Controls.Add(this.lblHeader);
            this.pnlEnterLicense.Location = new System.Drawing.Point(203, 157);
            this.pnlEnterLicense.Name = "pnlEnterLicense";
            this.pnlEnterLicense.Size = new System.Drawing.Size(446, 389);
            this.pnlEnterLicense.TabIndex = 7;
            // 
            // btnOfflineActivation
            // 
            this.btnOfflineActivation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOfflineActivation.Location = new System.Drawing.Point(273, 196);
            this.btnOfflineActivation.Name = "btnOfflineActivation";
            this.btnOfflineActivation.Size = new System.Drawing.Size(158, 30);
            this.btnOfflineActivation.TabIndex = 13;
            this.btnOfflineActivation.Text = "Offline Activation...";
            this.btnOfflineActivation.Click += new System.EventHandler(this.btnOfflineActivation_Click);
            // 
            // lblInvalidLicenseEntered
            // 
            this.lblInvalidLicenseEntered.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInvalidLicenseEntered.ForeColor = System.Drawing.Color.Red;
            this.lblInvalidLicenseEntered.Location = new System.Drawing.Point(12, 229);
            this.lblInvalidLicenseEntered.Name = "lblInvalidLicenseEntered";
            this.lblInvalidLicenseEntered.Size = new System.Drawing.Size(423, 90);
            this.lblInvalidLicenseEntered.TabIndex = 10;
            this.lblInvalidLicenseEntered.Text = "label1";
            // 
            // btnEnterLicenseGoBack
            // 
            this.btnEnterLicenseGoBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEnterLicenseGoBack.Location = new System.Drawing.Point(287, 350);
            this.btnEnterLicenseGoBack.Name = "btnEnterLicenseGoBack";
            this.btnEnterLicenseGoBack.Size = new System.Drawing.Size(148, 27);
            this.btnEnterLicenseGoBack.TabIndex = 5;
            this.btnEnterLicenseGoBack.Text = "<< Back";
            this.btnEnterLicenseGoBack.Click += new System.EventHandler(this.btnEnterLicenseGoBack_Click);
            // 
            // btnUnlock
            // 
            this.btnUnlock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnUnlock.Location = new System.Drawing.Point(11, 197);
            this.btnUnlock.Name = "btnUnlock";
            this.btnUnlock.Size = new System.Drawing.Size(149, 29);
            this.btnUnlock.TabIndex = 3;
            this.btnUnlock.Text = "Apply License";
            this.btnUnlock.Click += new System.EventHandler(this.btnUnlock_Click);
            // 
            // btnPasteFromClipboard
            // 
            this.btnPasteFromClipboard.Location = new System.Drawing.Point(13, 40);
            this.btnPasteFromClipboard.Name = "btnPasteFromClipboard";
            this.btnPasteFromClipboard.Size = new System.Drawing.Size(156, 33);
            this.btnPasteFromClipboard.TabIndex = 1;
            this.btnPasteFromClipboard.Text = "Paste From Clipboard";
            this.btnPasteFromClipboard.Click += new System.EventHandler(this.btnPasteFromClipboard_Click);
            // 
            // btnLoadFromFile
            // 
            this.btnLoadFromFile.Location = new System.Drawing.Point(175, 40);
            this.btnLoadFromFile.Name = "btnLoadFromFile";
            this.btnLoadFromFile.Size = new System.Drawing.Size(132, 33);
            this.btnLoadFromFile.TabIndex = 0;
            this.btnLoadFromFile.Text = "Load From File...";
            this.btnLoadFromFile.Click += new System.EventHandler(this.btnLoadFromFile_Click);
            // 
            // txtLicenseCode
            // 
            this.txtLicenseCode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLicenseCode.Location = new System.Drawing.Point(10, 79);
            this.txtLicenseCode.Multiline = true;
            this.txtLicenseCode.Name = "txtLicenseCode";
            this.txtLicenseCode.Size = new System.Drawing.Size(421, 112);
            this.txtLicenseCode.TabIndex = 2;
            // 
            // lblHeader
            // 
            this.lblHeader.Location = new System.Drawing.Point(12, 8);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(429, 29);
            this.lblHeader.TabIndex = 4;
            this.lblHeader.Text = "Enter license";
            // 
            // pnlLicenseInfo
            // 
            this.pnlLicenseInfo.Controls.Add(this.btnStartProgram);
            this.pnlLicenseInfo.Controls.Add(this.lblLienseInfo);
            this.pnlLicenseInfo.Controls.Add(this.lblThankYou_pnlLicenseInfo);
            this.pnlLicenseInfo.Location = new System.Drawing.Point(214, 117);
            this.pnlLicenseInfo.Name = "pnlLicenseInfo";
            this.pnlLicenseInfo.Size = new System.Drawing.Size(464, 299);
            this.pnlLicenseInfo.TabIndex = 8;
            // 
            // btnStartProgram
            // 
            this.btnStartProgram.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnStartProgram.Location = new System.Drawing.Point(22, 50);
            this.btnStartProgram.Name = "btnStartProgram";
            this.btnStartProgram.Size = new System.Drawing.Size(150, 29);
            this.btnStartProgram.TabIndex = 5;
            this.btnStartProgram.Text = "Start Program";
            this.btnStartProgram.Click += new System.EventHandler(this.btnStartProgram_Click);
            // 
            // lblLienseInfo
            // 
            this.lblLienseInfo.Location = new System.Drawing.Point(19, 90);
            this.lblLienseInfo.Name = "lblLienseInfo";
            this.lblLienseInfo.Size = new System.Drawing.Size(409, 190);
            this.lblLienseInfo.TabIndex = 4;
            this.lblLienseInfo.Text = "label2";
            // 
            // lblThankYou_pnlLicenseInfo
            // 
            this.lblThankYou_pnlLicenseInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblThankYou_pnlLicenseInfo.Location = new System.Drawing.Point(19, 11);
            this.lblThankYou_pnlLicenseInfo.Name = "lblThankYou_pnlLicenseInfo";
            this.lblThankYou_pnlLicenseInfo.Size = new System.Drawing.Size(411, 36);
            this.lblThankYou_pnlLicenseInfo.TabIndex = 3;
            this.lblThankYou_pnlLicenseInfo.Text = "Thank you";
            this.lblThankYou_pnlLicenseInfo.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // pnlOfflineActivation
            // 
            this.pnlOfflineActivation.Controls.Add(this.btnBack_pnlOfflineActivation);
            this.pnlOfflineActivation.Controls.Add(this.btnCopyComputerID);
            this.pnlOfflineActivation.Controls.Add(this.txtComputerID);
            this.pnlOfflineActivation.Controls.Add(this.lblOfflineActivation);
            this.pnlOfflineActivation.Location = new System.Drawing.Point(295, 10);
            this.pnlOfflineActivation.Name = "pnlOfflineActivation";
            this.pnlOfflineActivation.Size = new System.Drawing.Size(448, 379);
            this.pnlOfflineActivation.TabIndex = 9;
            // 
            // btnBack_pnlOfflineActivation
            // 
            this.btnBack_pnlOfflineActivation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBack_pnlOfflineActivation.Location = new System.Drawing.Point(289, 341);
            this.btnBack_pnlOfflineActivation.Name = "btnBack_pnlOfflineActivation";
            this.btnBack_pnlOfflineActivation.Size = new System.Drawing.Size(148, 27);
            this.btnBack_pnlOfflineActivation.TabIndex = 6;
            this.btnBack_pnlOfflineActivation.Text = "<< Back";
            this.btnBack_pnlOfflineActivation.Click += new System.EventHandler(this.btnBack_pnlOfflineActivation_Click);
            // 
            // btnCopyComputerID
            // 
            this.btnCopyComputerID.Location = new System.Drawing.Point(8, 97);
            this.btnCopyComputerID.Name = "btnCopyComputerID";
            this.btnCopyComputerID.Size = new System.Drawing.Size(160, 28);
            this.btnCopyComputerID.TabIndex = 2;
            this.btnCopyComputerID.Text = "Copy To Clipboard";
            this.btnCopyComputerID.Click += new System.EventHandler(this.btnCopyComputerID_Click);
            // 
            // txtComputerID
            // 
            this.txtComputerID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtComputerID.Location = new System.Drawing.Point(7, 73);
            this.txtComputerID.Name = "txtComputerID";
            this.txtComputerID.ReadOnly = true;
            this.txtComputerID.Size = new System.Drawing.Size(428, 21);
            this.txtComputerID.TabIndex = 1;
            // 
            // lblOfflineActivation
            // 
            this.lblOfflineActivation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblOfflineActivation.Location = new System.Drawing.Point(6, 15);
            this.lblOfflineActivation.Name = "lblOfflineActivation";
            this.lblOfflineActivation.Size = new System.Drawing.Size(431, 51);
            this.lblOfflineActivation.TabIndex = 0;
            this.lblOfflineActivation.Text = "To activate your license offline, send the computer ID shown below to us so that " +
    "we can send you a pre-activated license.";
            // 
            // pnlProgressBars
            // 
            this.pnlProgressBars.BackgroundImage = global::VeriSignature.Properties.Resources.LicenseEvalDialog;
            this.pnlProgressBars.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlProgressBars.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlProgressBars.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlProgressBars.Location = new System.Drawing.Point(0, 0);
            this.pnlProgressBars.Name = "pnlProgressBars";
            this.pnlProgressBars.Size = new System.Drawing.Size(196, 466);
            this.pnlProgressBars.TabIndex = 3;
            // 
            // EvaluationInfoDialog
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(834, 466);
            this.Controls.Add(this.pnlOfflineActivation);
            this.Controls.Add(this.pnlShowEvaluationInfo);
            this.Controls.Add(this.pnlEnterLicense);
            this.Controls.Add(this.pnlLicenseInfo);
            this.Controls.Add(this.pnlProgressBars);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EvaluationInfoDialog";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AppName";
            this.TopMost = true;
            this.Closing += new System.ComponentModel.CancelEventHandler(this.EvaluationInfoDialog_Closing);
            this.Load += new System.EventHandler(this.ShowEvaluationForm_Load);
            this.pnlShowEvaluationInfo.ResumeLayout(false);
            this.pnlEnterLicense.ResumeLayout(false);
            this.pnlEnterLicense.PerformLayout();
            this.pnlLicenseInfo.ResumeLayout(false);
            this.pnlOfflineActivation.ResumeLayout(false);
            this.pnlOfflineActivation.PerformLayout();
            this.ResumeLayout(false);

		}

        #endregion

        private System.Windows.Forms.Panel pnlProgressBars;
        private System.Windows.Forms.Button btnEnterLicense;
        private System.Windows.Forms.Button btnPurchase;
        internal System.Windows.Forms.Button btnContinueEval;
        private System.Windows.Forms.Label lblThankYou;
        private System.Windows.Forms.Label lblEnterLicense;
        private System.Windows.Forms.Label lbPurchase;
        private System.Windows.Forms.Label lblContinueEval;
        private System.Windows.Forms.Panel pnlShowEvaluationInfo;
        private System.Windows.Forms.Panel pnlEnterLicense;
        private System.Windows.Forms.Button btnUnlock;
        private System.Windows.Forms.Button btnPasteFromClipboard;
        private System.Windows.Forms.Button btnLoadFromFile;
        private System.Windows.Forms.TextBox txtLicenseCode;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Button btnEnterLicenseGoBack;
        private System.Windows.Forms.Label lblInvalidLicenseEntered;
        private System.Windows.Forms.Panel pnlLicenseInfo;
        private System.Windows.Forms.Label lblLienseInfo;
        private System.Windows.Forms.Label lblThankYou_pnlLicenseInfo;
        internal System.Windows.Forms.Button btnStartProgram;
        private System.Windows.Forms.Label lblExit;
        private System.Windows.Forms.Button btnExitProgram;
        private Button btnOfflineActivation;
        private Panel pnlOfflineActivation;
        private Label lblOfflineActivation;
        private Button btnBack_pnlOfflineActivation;
        private Button btnCopyComputerID;
        private TextBox txtComputerID;

        internal EvaluationInfoDialog()
        {
            InitializeComponent();
        }

        // Evaluation Info settings...

        private string productName;
        public new string ProductName
        {
            get { return productName; }
            set { productName = value; }
        }

        private EvaluationDialogPane startPane = EvaluationDialogPane.EvaluationInfo;
        public EvaluationDialogPane StartPane
        {
            get { return startPane; }
            set { startPane = value; }
        }

        bool showOfflineActivationButton = false;
        public bool ShowOfflineActivationButton
        {
            get { return showOfflineActivationButton; }
            set { showOfflineActivationButton = value; }
        }

        string purchaseURL = "http://www.ssware.com";
        public string PurchaseURL
        {
            get { return purchaseURL; }
            set { purchaseURL = value; }
        }

        internal bool useDateExpires = false;
        public bool UseDateExpires
        {
            get { return useDateExpires; }
            set { useDateExpires = value; }
        }




        internal CryptoLicense license; // The license for which the eval info is shown

        public EvaluationInfoDialog(CryptoLicense license)
            :this()
        {
            this.license = license;
        }

        private void btnEnterLicense_Click(object sender, EventArgs e)
        {
            // Move to the 'Enter License' pane
            SetActivePanel(pnlEnterLicense);
        }

        // The different panels : Enter License', 'Eval Info', 'Thank You', etc
        Panel[] panels;

        void SetActivePanel(Panel p)
        {
            Init_pnlProgressBars();

            // Hide all panels
            foreach (Panel pnl in panels)
            {
                pnl.Visible = false;
                pnl.Dock = DockStyle.None;
            }

            // Call respective initialization routine
            if (p == pnlShowEvaluationInfo)
                Init_pnlShowEvaluationInfo();
            else if (p == pnlEnterLicense)
                Init_pnlEnterLicense();
            else if (p == pnlLicenseInfo)
                Init_pnlLicenseInfo();
            else if (p == pnlOfflineActivation)
                Init_pnlOfflineActivation();

            // Show and position active panel
            p.Visible = true;
            p.Dock = DockStyle.Fill;

            this.PerformLayout();
        }

        void Init_pnlOfflineActivation()
        {
            txtComputerID.Text = license.GetLocalMachineCodeAsString();
        }

        void Init_pnlShowEvaluationInfo()
        {
            // Start "ProductLicense" ver: 1.0.7 date: 04-29-15
            //lblThankYou.Text = "Thank you for evaluating " + productName + ".";
            lblThankYou.Text = "Thank you for using the " + productName + ".";
            //lblEnterLicense.Text = "If you have a license for " + productName + ", click the 'Enter License...' button below to enter it and unlock the software.";
            lblEnterLicense.Text = "If you want to enter a new " + productName + " License, click the 'Enter License' button below.";
            //lbPurchase.Text = "To purchase " + productName + ", click the 'Purchase..' button below.";
            lbPurchase.Text = "To purchase a new " + productName + " License, click the 'Purchase Information' button below.";
            //lblExit.Text = "To exit " + productName + ", click the 'Exit Program' button below.";
            lblExit.Text = "To exit the " + productName + " application, click the 'Exit' button below.";
            // End "ProductLicense" ver: 1.0.7 date: 04-29-15
            if (!license.ValidateSignature() || license.IsEvaluationExpired() || DateTime.Now > license.DateExpires)
            {
                LicenseStatus ls = LicenseStatus.ExecutionsExceeded | LicenseStatus.UniqueUsageDaysExceeded | LicenseStatus.UsageDaysExceeded;
                if (useDateExpires)
                    ls |= LicenseStatus.Expired;

                if ((license.Status & ~(ls)) != 0)
                {
                    // Start "ProductLicense" ver: 1.0.7 date: 05-20-15
                    //lblContinueEval.Text = string.Format("Your license is invalid, Status = {0}. To continue using the software, please purchase using the 'Purchase..' button below. If you have a license, use the 'Enter License...' button below", license.Status);
                    lblContinueEval.Text = string.Format("Your license is invalid, Status = {0}. A new license is required to continue using ConfigOS Foundry. If you need to purchase a new license, please select the ‘Purchase Information’ button below. If you have a new license, please select the 'Enter License' button below.", license.Status);
                    // End "ProductLicense" ver: 1.0.7 date: 05-20-15
                }
                else
                {
                    // Start "ProductLicense" ver: 1.0.7 date: 04-30-15
                    //lblContinueEval.Text = "Your evaluation has expired. To continue using the software, please purchase using the 'Purchase..' button below. If you have a license, use the 'Enter License...' button below";
                    lblContinueEval.Text = "Your license has expired. A new license is required to continue using ConfigOS Foundry. If you need to purchase a new license, please select the ‘Purchase Information’ button below. If you have a new license, please select the 'Enter License' button below.";
                    // End "ProductLicense" ver: 1.0.7 date: 04-30-15
                }
                lblContinueEval.ForeColor = Color.Red;
                btnContinueEval.Enabled = false;
            }
            else
            {
                // Start "ProductLicense" ver: 1.0.7 date: 04-29-15
                //lblContinueEval.Text = "To continue your evaluation of " + productName + ", click the 'Continue Evaluation...' button below.";
                lblContinueEval.Text = "To continue with your license for the " + productName + ", click the 'Continue' button below.";
                // End "ProductLicense" ver: 1.0.7 date: 04-29-15
            }
        }

        void Init_pnlEnterLicense()
        {
            lblHeader.Text = "Enter license text for " + productName + " below:";
            lblInvalidLicenseEntered.Visible = false;
            btnOfflineActivation.Visible = showOfflineActivationButton;
        }

        void Init_pnlLicenseInfo()
        {
            lblThankYou_pnlLicenseInfo.Text = "Thank you for purchasing " + productName + ".";

            // Start "ProductLicense" ver: 1.0.7 date: 04-29-15
            //if (!IsNullOrEmpty(license.UserData))
            //{
            //    lblLienseInfo.Text = "Your license info is as follows : " + license.UserData;
            //    lblLienseInfo.Visible = true;
            //}
            //else
            //{
            //    lblLienseInfo.Visible = false;
            //}
            lblLienseInfo.Visible = false;
            // End "ProductLicense" ver: 1.0.7 date: 04-29-15
        }

        void Init_pnlProgressBars()
        {
            if ((license.LicenseCode==null || license.LicenseCode==string.Empty) || (!license.IsEvaluationLicense() && !(useDateExpires && license.HasDateExpires)))
            {
                pnlProgressBars.Visible = false;
                return;
            }
            else
            {
                pnlProgressBars.Visible = true;
                pnlProgressBars.Controls.Clear();

                if (useDateExpires && license.HasDateExpires)
                {
                    TimeSpan ts = (license.DateExpires.Date - DateTime.Now.Date);
                    int remaining = ts.Days;
                    if (remaining < 0)
                        remaining = 0;
                    string temp = string.Format("License expires on {0}. {1} days remaining.", license.DateExpires.ToString("dd-MMM-yyyy"), remaining);
                    int max = (license.DateExpires.Date - license.DateGenerated.Date).Days;
                    if (max < 0)
                        max = 0;
                    ProgressControl prg = new ProgressControl(temp, remaining, max);
                    pnlProgressBars.Controls.Add(prg);
                    prg.Dock = DockStyle.Top;
                }
                if (license.HasMaxUsageDays)
                {
                    int remaining = license.MaxUsageDays - license.CurrentUsageDays;
                    if (remaining < 0)
                        remaining = 0;
                    else
                        remaining++;

                    string temp = string.Format("{0} of {1} days remaining.", remaining, license.MaxUsageDays);
                    ProgressControl prg = new ProgressControl(temp, remaining, license.MaxUsageDays);
                    pnlProgressBars.Controls.Add(prg);
                    prg.Dock = DockStyle.Top;
                }
                if (license.HasMaxUniqueUsageDays)
                {
                    int remaining = license.MaxUniqueUsageDays - license.CurrentUniqueUsageDays;
                    if (remaining < 0)
                        remaining = 0;
                    else if (remaining < license.MaxUniqueUsageDays)
                        remaining++;

                    string temp = string.Format("{0} of {1} unique usage days remaining.", remaining, license.MaxUniqueUsageDays);
                    ProgressControl prg = new ProgressControl(temp, remaining, license.MaxUniqueUsageDays);
                    pnlProgressBars.Controls.Add(prg);
                    prg.Dock = DockStyle.Top;
                }
                if (license.HasMaxExecutions)
                {
                    int remaining = license.MaxExecutions - license.CurrentExecutions;
                    if (remaining < 0)
                        remaining = 0;
                    else if (remaining < license.MaxExecutions)
                        remaining++;

                    string temp = string.Format("{0} of {1} runs remaining.", remaining, license.MaxExecutions);
                    ProgressControl prg = new ProgressControl(temp, remaining, license.MaxExecutions);
                    pnlProgressBars.Controls.Add(prg);
                    prg.Dock = DockStyle.Top;
                }
            }
        }



        private void ShowEvaluationForm_Load(object sender, EventArgs e)
        {
            this.ClientSize = new Size(pnlProgressBars.Width + pnlShowEvaluationInfo.Width, pnlShowEvaluationInfo.Height);
            this.MinimumSize = this.Size;
            panels = new Panel[] { pnlEnterLicense, pnlShowEvaluationInfo, pnlLicenseInfo, pnlOfflineActivation };
            this.Text = productName;

            // Set active panel
            if (startPane == EvaluationDialogPane.EvaluationInfo)
                SetActivePanel(pnlShowEvaluationInfo);
            else
                SetActivePanel(pnlEnterLicense);

        }

        private void btnLoadFromFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "All Files (*.*)|*.*";
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                using (StreamReader sr = File.OpenText(dlg.FileName))
                {
                    txtLicenseCode.Text = sr.ReadToEnd();
                }
            }
        }

        private void btnPasteFromClipboard_Click(object sender, EventArgs e)
        {
			IDataObject dataObj = Clipboard.GetDataObject();
            txtLicenseCode.Text = dataObj.GetData(DataFormats.Text,true) as string;
        }

        private void btnUnlock_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsNullOrEmpty(txtLicenseCode.Text))
                {
                    lblInvalidLicenseEntered.Text = "You have not entered a license. Please enter a valid license and try again.";
                    lblInvalidLicenseEntered.Visible = true;
                    return;
                }

                if (!IsNullOrEmpty(txtLicenseCode.Text))
                {
                    lblInvalidLicenseEntered.Text = "Checking. Please wait...";
                    Application.DoEvents();

                    // Create temp license object
                    CryptoLicense temp = Activator.CreateInstance(license.GetType()) as CryptoLicense;
                    temp.StorageMode = license.StorageMode;
                    temp.ValidationKey = license.ValidationKey;
                    temp.LicenseServiceURL = license.LicenseServiceURL;
                    temp.HostAssembly = license.HostAssembly;
                    temp.LicenseServiceSettingsFilePath = license.LicenseServiceSettingsFilePath;
                    temp.RegistryStoragePath = license.RegistryStoragePath;
                    temp.FileStoragePath = license.FileStoragePath;

                    // See if its a serial
                    SerialValidationResult result = temp.GetLicenseFromSerial(txtLicenseCode.Text, string.Empty);
                    if (result == SerialValidationResult.Failed)
                    {
                        // Its a serial, but is invalid
                        string str = "Serial Validation Failed: ";
                        Exception ex = temp.GetStatusException(LicenseStatus.SerialCodeInvalid);
                        if (ex != null && !IsNullOrEmpty(ex.Message)) // Additional info available for the status
                        {
                            str += ex.Message;
                        }
                        else
                        {
                            str += "<no additional information>";
                        }
                        lblInvalidLicenseEntered.Text = str;
                        lblInvalidLicenseEntered.Visible = true;
                        return;
                    }

                    // Not a serial, set .LicenseCode property
                    if (result == SerialValidationResult.NotASerial)
                        temp.LicenseCode = txtLicenseCode.Text;

                    // Validate license
                    if (temp.Status == LicenseStatus.Valid)
                    {
                        // Valid , dispose old license, replace with 'temp' license
                        license.Dispose();
                        license = temp;
                    }

                    if (temp.Status != LicenseStatus.Valid)
                    {
                        string additional = "Error code: " + temp.Status.ToString();
                        Exception ex = temp.GetStatusException(temp.Status);
                        if (ex != null)
                            additional += ". Error message: " + ex.Message;
                        lblInvalidLicenseEntered.Text = "You have entered an invalid license. Please enter a valid license and try again.\n" + additional;
                        lblInvalidLicenseEntered.Visible = true;
                    }
                    else
                    {
                        SetActivePanel(pnlLicenseInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                lblInvalidLicenseEntered.Text = ex.GetType().ToString() + " - " + ex.Message;
                lblInvalidLicenseEntered.Visible = true;
            }
        }

        private void btnEnterLicenseGoBack_Click(object sender, EventArgs e)
        {
            SetActivePanel(pnlShowEvaluationInfo);
        }


        private void btnStartProgram_Click(object sender, EventArgs e)
        {
                this.Close();
        }

        private void btnExitProgram_Click(object sender, EventArgs e)
        {
            exitButtonClicked = true;
            this.Close();
        }

        private void btnPurchase_Click(object sender, EventArgs e)
        {
            try
            {
                // Start "ProductLicense" ver: 1.0.7 date: 04-29-15
                //ShellExecute(purchaseURL);
                SCLDInfoForm scldinfo = new SCLDInfoForm();
                scldinfo.ShowDialog();
                // End "ProductLicense" ver: 1.0.7 date: 04-29-15
            }
            catch { }
        }

		bool exitButtonClicked = false;
		private void EvaluationInfoDialog_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
            // Set dialog result to 'OK" if 'Exit Program' button was clicked
            // Start "ProductLicense" ver: 1.0.7 date: 04-29-15
            //if (exitButtonClicked == false)
            //    this.DialogResult = DialogResult.OK;
            // End "ProductLicense" ver: 1.0.7 date: 04-29-15
		}

        private void btnOfflineActivation_Click(object sender, EventArgs e)
        {
            SetActivePanel(pnlOfflineActivation);
        }

        private void btnCopyComputerID_Click(object sender, EventArgs e)
        {
            DataObject data = new DataObject(DataFormats.Text, txtComputerID.Text);
            Clipboard.SetDataObject(data);
        }

        private void btnBack_pnlOfflineActivation_Click(object sender, EventArgs e)
        {
            SetActivePanel(pnlEnterLicense);
        }

        // Helper methods...

        internal static bool IsNullOrEmpty(Array a)
        {
            return a == null || a.Length == 0;
        }

        internal static bool IsNullOrEmpty(string str)
        {
            return str == null || str.Length == 0;
        }

        internal static void ShellExecute(string cmd)
        {
            ProcessStartInfo pi = new ProcessStartInfo(cmd, string.Empty);
            pi.UseShellExecute = true;
            pi.Verb = "open";
            pi.WindowStyle = ProcessWindowStyle.Maximized;
            System.Diagnostics.Process.Start(pi);
        }

        // Code to show the dialog along with specific license checks 
        internal bool ShowDialogInt()
        {
            CryptoLicense originalLicense = this.license;
            // Optional: if start pane is 'eval info' and license is NOT eval license, return
            if (this.StartPane == EvaluationDialogPane.EvaluationInfo && (originalLicense.IsEvaluationLicense() == false && (this.useDateExpires && originalLicense.HasDateExpires) == false))
                return true;

            LicenseStatus dummy = originalLicense.Status; // force license validation

            // Show the dialog
            this.ShowDialog();

            // User entered a new valid license OR
            // current license is still valid (eval has not expired)
            if (this.DialogResult != DialogResult.Cancel && this.license.ValidateSignature() && !this.license.IsEvaluationExpired() && !(this.useDateExpires && this.license.HasDateExpires && DateTime.Now > this.license.DateExpires))
            {
                // Check that a new license was entered
                if (originalLicense.LicenseCode != this.license.LicenseCode)
                {
                    // Copy settings from the new license to current license
                    originalLicense.CopySettingsFrom_WithState(this.license);

                    // Save newly entered license so that it will be loaded next time your software starts
                    originalLicense.Save();
                }

                return true;
            }

            if (!originalLicense.IsEvaluationExpired())
                originalLicense.RevertNotifyLicenseValidated();

            // If user selected 'Exit Program' or if the license has expired, return false
            return false;
        }


    }


    public enum EvaluationDialogPane
    {
        EvaluationInfo,
        EnterLicense,
    }

    // User control made up of a label and a progress bar - used for display various 
    // evaluation properties such as days remaining

    internal class ProgressControl : UserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblCaption = new System.Windows.Forms.Label();
            this.prg = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // lblCaption
            // 
            this.lblCaption.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCaption.TextAlign = ContentAlignment.MiddleLeft;
            this.lblCaption.Location = new System.Drawing.Point(9, 10);
            this.lblCaption.Name = "lblCaption";
            this.lblCaption.Size = new System.Drawing.Size(179, 34);
            this.lblCaption.TabIndex = 0;
            this.lblCaption.Text = "Label";
            this.lblCaption.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // prg
            // 
            this.prg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.prg.Location = new System.Drawing.Point(9, 45);
            this.prg.Name = "prg";
            this.prg.Size = new System.Drawing.Size(179, 20);
            this.prg.TabIndex = 1;
            // 
            // ProgressControl
            // 
            this.Controls.Add(this.prg);
            this.Controls.Add(this.lblCaption);
            this.Name = "ProgressControl";
            // End "ProductLicense" ver: 1.0.7 date: 04-30-15
            //this.Size = new System.Drawing.Size(201, 78);
            this.Size = new System.Drawing.Size(201, 68);
            // End "ProductLicense" ver: 1.0.7 date: 04-30-15
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblCaption;
        private System.Windows.Forms.ProgressBar prg;

        internal ProgressControl()
        {
            InitializeComponent();
        }

        internal ProgressControl(string caption, int value, int max)
            : this()
        {
            if (value > max)
                value = max;
            if (value < 0)
                value = 0;
            lblCaption.Text = caption;
            prg.Minimum = 0;
            prg.Maximum = max;
            prg.Value = value;
        }
    }


}