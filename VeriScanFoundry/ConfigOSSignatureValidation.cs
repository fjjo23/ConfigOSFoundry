using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.IO;

namespace ValidateXMLSchema
{
    class ConfigOSSignatureValidation
    {
        private string errorMessage;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set { errorMessage = value; }
        }

        private List<string> hives = new List<string>();
        public List<string> Hives
        {
            get { return hives; }
            set { hives = value; }
        }

        private List<string> valueRegExps = new List<string>();

        private List<string> categories = new List<string>();
        public List<string> Categories
        {
            get { return categories; }
            set { categories = value; }
        }

        private List<string> regTypes = new List<string>();
        public List<string> RegTypes
        {
            get { return regTypes; }
            set { regTypes = value; }
        }

        private string errorFile;
        public string ErrorFile
        {
            get { return errorFile; }
            set { errorFile = value; }
        }

        public ConfigOSSignatureValidation()
        {
            valueRegExps.Add(@"\[Min\.\.\.\d+\],\d+");
            valueRegExps.Add(@"\[Max\.\.\.\d+\],\d+");
            valueRegExps.Add(@"\[\S+\|\S+\],\S+");
            //valueRegExps.Add(@"\[\S+\s\|\s\S+\],\S+");
            valueRegExps.Add(@"\[[\S\s]+\|[\S\s]+\],[\S\s]+");
            valueRegExps.Add(@"\[\d+\.\.\.\d+\],\d+");
            valueRegExps.Add(@"\[!\S+\],\S+");
        }

        public string ConfigOSSchemaValidation(string schemaFile, string xmlFile)
        {
            errorMessage = "";
            XmlReader groups = null;
            try
            {
                XmlReaderSettings groupsSettings = new XmlReaderSettings();
                groupsSettings.Schemas.Add(null, schemaFile);
                groupsSettings.ValidationType = ValidationType.Schema;
                groupsSettings.ValidationEventHandler += new ValidationEventHandler(groupsSettingsSettingsValidationEventHandler);

                groups = XmlReader.Create(xmlFile, groupsSettings);

                while (groups.Read()) { }
                groups.Close();
            }
            catch(Exception err)
            {
                WriteErrorFile("Schema validation. ", err.Message);
                try
                {
                    groups.Close();
                }
                catch
                {
                }
            }
            //            
            if (File.Exists(errorFile))
            {
                errorMessage = "The file '" + xmlFile + "' has schema errors. You can find detailed error information on the file '" + errorFile + "'.";
            }
            return errorMessage;
        }

        private void groupsSettingsSettingsValidationEventHandler(object sender, ValidationEventArgs e)
        {
            IXmlLineInfo lineinfo = ((IXmlLineInfo)sender);
            //
            if (e.Severity == XmlSeverityType.Warning)
            {
                WriteErrorFile("Schema validation. ", "Warning line " + lineinfo.LineNumber.ToString() + ", " + e.Message);
            }
            else if (e.Severity == XmlSeverityType.Error)
            {
                WriteErrorFile("Schema validation. ", "Error line " + lineinfo.LineNumber.ToString() + ", " + e.Message);
            }
        }

        public string ConfigOSValuesValidation(string xmlFile)
        {
            bool areThereErrors = false;
            string myErrorMessage = "";
            try
            {
                XDocument xd = XDocument.Load(xmlFile);
                IEnumerable<XElement> groups = from bgp in xd.Elements("Groups").Elements("Items").Elements("Group")
                                               select bgp;
                foreach (XElement group in groups)
                {
                    string groupId = "";
                    string where = "";
                    string type = "";
                    string value = "";
                    string gCategory = "";
                    string ignore = "";
                    string ignoreReason = "";
                    foreach (XElement groupChild in group.Elements())
                    {
                        string gName = groupChild.Name.ToString().Trim().ToUpper();
                        string gValue = groupChild.Value.ToString().Trim();
                        switch (gName)
                        {
                            case "GROUPID":
                                groupId = gValue;
                                break;
                            case "WHERE":
                                if (gValue.Split('\\').Length - 1 > 2)
                                {
                                    gCategory = "Registry";
                                }
                                else
                                {
                                    if (categories.Contains(gValue))
                                    {
                                        gCategory = gValue;
                                    }
                                    else
                                    {
                                        gCategory = "No Validation";
                                    }
                                }
                                where = gValue;
                                break;
                            case "TYPE":
                                type = gValue;
                                break;
                            case "VALUE":
                                value = gValue;
                                break;
                            case "IGNORE":
                                ignore = gValue;
                                break;
                            case "IGNOREREASON":
                                ignoreReason = gValue;
                                break;
                        }
                    }
                    // Validate
                    if (gCategory != "No Validation")
                    {
                        if (gCategory == "Registry")
                        {
                            if (ValidateRegistry(where, type).Length > 0)
                            {
                                WriteErrorFile("Values validation. ", groupId + ": " + errorMessage);
                                areThereErrors = true;
                            }
                        }
                        if (ValidateValue(value).Length > 0)
                        {
                            WriteErrorFile("Values validation. ", groupId + ": " + errorMessage);
                            areThereErrors = true;
                        }
                        if (ValidateIgnore(ignore).Length > 0)
                        {
                            WriteErrorFile("Values validation. ", groupId + ": " + errorMessage);
                            areThereErrors = true;
                        }
                        if (ValidateIgnoreReason(ignore, ignoreReason).Length > 0)
                        {
                            WriteErrorFile("Values validation. ", groupId + ": " + errorMessage);
                            areThereErrors = true;
                        }
                    }
                }
                if (areThereErrors)
                {
                    myErrorMessage = "The file '" + xmlFile + "' has values errors. You can find detailed error information on the file '" + errorFile + "'.";
                }
            }
            catch (Exception err)
            {
                WriteErrorFile("Values validation. ", err.Message);
                myErrorMessage = "The file '" + xmlFile + "' has reading xml errors. You can find detailed error information on the file '" + errorFile + "'.";
            }
            return myErrorMessage;
        }

        public string ValidateRegistry(string where, string type)
        {
            errorMessage = "";
            try
            {
                if (errorMessage.Length < 1)
                {
                    // Validates where for registry path
                    if (where.Length > 0)
                    {
                        int countBackSlash = where.Split('\\').Length - 1;
                        bool countDoubleBackSlash = where.Contains(@"\\");
                        if (countBackSlash > 2 && where.EndsWith(@"\") && !where.Contains(@"\\"))
                        {
                            string hive = where.Substring(0, where.IndexOf(@"\"));
                            if (hive.Length > 0 && (hives.Contains(hive)))
                            {
                                // OK
                            }
                            else
                            {
                                errorMessage = "The registry path must start with one of the following: " + string.Join(",", hives) + ".";
                            }
                        }
                        else
                        {
                            errorMessage = "The format of the registry path on 'Where' is invalid.";
                        }
                    }
                    else
                    {
                        errorMessage = "Registry path must be entered on 'Where'.";
                    }
                }
                if (errorMessage.Length < 1)
                {
                    // Validates registry value type
                    if (type.Length < 1)
                    {
                        errorMessage = "Select a registry type.";
                    }
                    else
                    {
                        if (!regTypes.Contains(type))
                        {
                            errorMessage = "The registry type is wrong.";
                        }
                    }
                }
            }
            catch
            {
            }
            return errorMessage;
        }

        public string ValidateValue(string value)
        {
            errorMessage = "";
            try
            {
                if (value.Contains("["))
                {
                    bool isValid = false;
                    foreach (string valueRegExp in valueRegExps)
                    {
                        Regex rx = new Regex(valueRegExp);
                        // Find matches.
                        MatchCollection matches = rx.Matches(value);
                        if (matches.Count > 0)
                        {
                            isValid = true;
                            break;
                        }
                    }
                    if (!isValid)
                    {
                        errorMessage = "The format of the value is not valid.";
                    }
                }
            }
            catch
            {
            }
            return errorMessage;
        }

        public string ValidateIgnoreReason(string ignore, string ignoreReason)
        {
            errorMessage = "";
            try
            {
                if (ignore != "" && ignore != "0")
                {
                    if (ignoreReason.Length < 1)
                    {
                        errorMessage = "The ignore reason is missing.";
                    }
                }
            }
            catch
            {
            }
            return errorMessage;
        }

        public string ValidateIgnore(string ignore)
        {
            errorMessage = "";
            try
            {
                if (ignore.Length > 0 && ignore != "" && ignore != "0")
                {
                    string[] ignoreValues = ignore.Split(',');
                    foreach (string ignoreValue in ignoreValues)
                    {
                        int ignoreValueInt = Convert.ToInt32(ignoreValue);
                        if (ignoreValueInt < 1 || ignoreValueInt > 3)
                        {
                            errorMessage = "The format of ignore value is wrong or out of range.";
                        }
                    }
                }
            }
            catch
            {
            }
            return errorMessage;
        }

        private void WriteErrorFile(string strCategory, string strMessage)
        {
            using (StreamWriter writer = new StreamWriter(new FileStream(errorFile, FileMode.Append)))
            {
                writer.WriteLine("{0}{1}", strCategory, strMessage);
            }
        }
    }
}
