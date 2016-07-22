using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace VeriSignature
{
    class Foundry
    {
        private string errorMessage = "";
        public string ErrorMessage
        {
            get { return errorMessage; }
            set { errorMessage = value; }
        }

        #region Get vendor short name

        public string GetVendor(string signatureFile)
        {
            errorMessage = "";
            string vendor = "";
            if (File.Exists(signatureFile))
            {
                try
                {
                    XmlDocument xmlSignatures = new XmlDocument();
                    xmlSignatures.Load(signatureFile);
                    XmlNodeList descriptions = xmlSignatures.SelectNodes("Signature/Description");
                    foreach (XmlNode description in descriptions)
                    {
                        XmlNodeList descriptionChildren = description.ChildNodes;
                        foreach (XmlNode descriptionChild in descriptionChildren)
                        {
                            if (descriptionChild.Name.Equals("ProductShortName"))
                            {
                                vendor = descriptionChild.InnerText;
                                break;
                            }
                        }
                    }
                }
                catch(Exception err)
                {
                    errorMessage = err.Message;
                }
            }
            return vendor;
        }

        #endregion

        #region Check if the file is an XML file and check if the file contains some specific tags

        public bool IsXMLFile(string signatureFile, string node, string fileType)
        {
            errorMessage = "";
            bool isXMLFile = false;
            if (File.Exists(signatureFile))
            {
                try
                {
                    XmlDocument xmlSignatures = new XmlDocument();
                    xmlSignatures.Load(signatureFile);
                    XmlNodeList descriptions = xmlSignatures.SelectNodes(node);
                    if (fileType == "VeriScan")
                    {
                        if (descriptions.Count > 0)
                        {
                            isXMLFile = true;
                        }
                    }
                    else
                    {
                        if (fileType == "ConfigOS")
                        {
                            foreach (XmlNode description in descriptions)
                            {
                                foreach (XmlAttribute att in description.Attributes)
                                {
                                    if (att.Name == "SysArea")
                                    {
                                        if (att.Value.ToUpper() == "SecurityPolicy".ToUpper() || att.Value.ToUpper() == "FilePermission".ToUpper())
                                        {
                                            isXMLFile = true;
                                            break;
                                        }
                                    }
                                }
                                if (isXMLFile)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
                catch (Exception err)
                {
                    errorMessage = err.Message;
                }
            }
            return isXMLFile;
        }

        #endregion

        #region Get encryption keys

        private string aesKey = "";
        public string AesKey
        {
            get { return aesKey; }
            set { aesKey = value; }
        }

        private string aesKeyIV = "";
        public string AesKeyIV
        {
            get { return aesKeyIV; }
            set { aesKeyIV = value; }
        }
        public void GetEncryptionKeys()
        {
            aesKey = "00F7CFDABAC02B1A4958724D1B342794";
            aesKeyIV = "C8C30AE8F0DC36AE";
        }

        #endregion

        #region Remove a file

        public void RemoveFile(string fileToRemove)
        {
            try
            {
                if (File.Exists(fileToRemove))
                {
                    File.Delete(fileToRemove);
                }
            }
            catch
            {
            }
        }
        #endregion

        #region Convert Byte[] array to string

        public string ByteToString(byte[] array)
        {
            errorMessage = "";
            string sha256 = "";
            try
            {
                for (int i = 0; i < array.Length; i++)
                {
                    sha256 = sha256 + String.Format("{0:X2}", array[i]);
                }
            }
            catch(Exception err)
            {
                errorMessage = err.Message;
            }
            return sha256;
        }

        #endregion

        #region Remove work signatures folder

        public void RemoveSignatureFolder(string myWorksignaturesFolder)
        {
            if (Directory.Exists(myWorksignaturesFolder))
            {
                try
                {
                    Directory.Delete(myWorksignaturesFolder, true);
                }
                catch (Exception err)
                {
                    //Console.Write(err.Message);
                }
            }
        }

        #endregion

        #region Create signatures folder

        public void CreateSignatureFolder(string myWorksignaturesFolder)
        {
            if (!Directory.Exists(myWorksignaturesFolder))
            {
                try
                {
                    Directory.CreateDirectory(myWorksignaturesFolder);
                }
                catch
                {
                }
            }
        }

        #endregion

    }
}
