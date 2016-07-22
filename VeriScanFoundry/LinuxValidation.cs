using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;

namespace VeriSignature
{
    class LinuxValidation
    {
        private string groupId;
        public string GroupId
        {
            get { return groupId; }
            set { groupId = value; }
        }

        private string where;
        public string Where
        {
            get { return where; }
            set { where = value; }
        }

        private string applied;
        public string Applied
        {
            get { return applied; }
            set { applied = value; }
        }

        private string type;
        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        private string valuel;
        public string Valuel
        {
            get { return valuel; }
            set { valuel = value; }
        }

        private string ignore;
        public string Ignore
        {
            get { return ignore; }
            set { ignore = value; }
        }

        private string ignoreReason;
        public string IgnoreReason
        {
            get { return ignoreReason; }
            set { ignoreReason = value; }
        }
        //
        private string errorMessage;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set { errorMessage = value; }
        }
        //
        private List<string> retValues = new List<string>();
        public List<string> RetValues
        {
            get { return retValues; }
            set { retValues = value; }
        }

        private string errorFile;
        public string ErrorFile
        {
            get { return errorFile; }
            set { errorFile = value; }
        }

        //public LinuxValidation(string GroupId, string Where, string Applied, string Type, string Valuel, string Ignore, string IgnoreReason)
        //{
        //    groupId = GroupId;
        //    where = Where;
        //    applied = Applied;
        //    type = Type;
        //    valuel = Valuel;
        //    ignore = Ignore;
        //    ignoreReason = IgnoreReason;
        //}
        private bool skipCondition = false;

        public string Validate(string validationFile, bool isInteractive)
        {
            skipCondition = false;
            errorMessage = "";
            try
            {
                XDocument doc = XDocument.Load(validationFile);
                IEnumerable<XElement> validations = doc.Root.Descendants("Validation");
                foreach (XElement validation in validations)
                {
                    // Get condition
                    XElement condition = validation.Element("Condition");
                    string conditionTag = condition.Attribute("Tag").Value.ToString().ToUpper();
                    string conditionValue = condition.Attribute("Value").Value.ToString();
                    string conditionMetha = condition.Attribute("Metacharacter").Value.ToString();
                    // Get target
                    XElement target = validation.Element("Target");
                    string targetTag = target.Attribute("Tag").Value.ToString().ToUpper();
                    string targetMetha = target.Attribute("Metacharacter").Value.ToString();
                    string dispTargetMetha = "";
                    switch (targetMetha)
                    {
                        case "^":
                            dispTargetMetha = "start with";
                            break;
                        case "$":
                            dispTargetMetha = "end with";
                            break;
                        case "!=":
                            dispTargetMetha = "not equal";
                            break;
                        default:
                            dispTargetMetha = "are";
                            break;
                    }
                    // Get valid values
                    XElement validValues = validation.Element("ValidValues");
                    IEnumerable<XElement> addValues = validValues.Descendants("Add");
                    List<string> laddValues = new List<string>();
                    foreach (XElement addValue in addValues)
                    {
                        laddValues.Add(addValue.Attribute("Value").Value.ToString());
                    }
                    //
                    string strRightValues = string.Join(", ", laddValues.ToArray());
                    bool retIsCondition = false;
                    string condValue = "";
                    switch (conditionTag)
                    {
                        case "WHERE":
                            retIsCondition = IsCondition(where, conditionValue, conditionMetha);
                            condValue = where;
                            break;
                        case "APPLIED":
                            retIsCondition = IsCondition(applied, conditionValue, conditionMetha);
                            condValue = applied;
                            break;
                        case "TYPE":
                            retIsCondition = IsCondition(type, conditionValue, conditionMetha);
                            condValue = type;
                            break;
                        case "VALUE":
                            retIsCondition = IsCondition(valuel, conditionValue, conditionMetha);
                            condValue = valuel;
                            break;
                        default:
                            break;
                    }
                    //
                    if (retIsCondition)
                    {
                        bool retIsTarget = false;
                        string tValue = "";
                        switch (targetTag)
                        {
                            case "WHERE":
                                retIsTarget = IsTarget(where, laddValues, targetMetha);
                                tValue = where;
                                break;
                            case "APPLIED":
                                retIsTarget = IsTarget(applied, laddValues, targetMetha);
                                tValue = applied;
                                break;
                            case "TYPE":
                                retIsTarget = IsTarget(type, laddValues, targetMetha);
                                tValue = type;
                                break;
                            case "VALUE":
                                retIsTarget = IsTarget(valuel, laddValues, targetMetha);
                                tValue = valuel;
                                break;
                        }
                        if (skipCondition)
                        {
                            errorMessage = "";
                            break;
                        }
                        else
                        {
                            if (!retIsTarget)
                            {
                                if (isInteractive)
                                {
                                    errorMessage = errorMessage + "The '" + targetTag + "' value for '" + condValue + "' is not typical." + Environment.NewLine + "Typical values " + dispTargetMetha + ": " + strRightValues + Environment.NewLine + Environment.NewLine;
                                }
                                else
                                {
                                    errorMessage = errorMessage + "Values validation. " + groupId + " " + targetTag + ": " + tValue + " for '" + condValue + "' is not typical." + Environment.NewLine;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception err)
            {
                errorMessage = "Error during validation. " + err.Message;
            }
            return errorMessage;
        }

        private bool IsCondition(string leftValue, string rightValue, string methaCharacter)
        {
            bool evalCondition = false;
            switch (methaCharacter)
            {
                case "^":
                    if (leftValue.StartsWith(rightValue))
                    {
                        evalCondition = true;
                    }
                    break;
                case "$":
                    if (leftValue.EndsWith(rightValue))
                    {
                        evalCondition = true;
                    }
                    break;
                case "=":
                    if (leftValue == rightValue)
                    {
                        evalCondition = true;
                    }
                    break;
                case "!=":
                    if (leftValue != rightValue)
                    {
                        evalCondition = true;
                    }
                    break;
                case "c":
                case "C":
                    if (leftValue.Contains(rightValue))
                    {
                        evalCondition = true;
                    }
                    break;
            }
            return evalCondition;
        }

        private bool IsTarget(string targetValue, List<string> goodValues, string methaCharacter)
        {
            bool evalTarget = false;
            if (goodValues.Count > 0)
            {

                switch (methaCharacter)
                {
                    case "^":
                        foreach (string goodValue in goodValues)
                        {
                            if (targetValue.StartsWith(goodValue))
                            {
                                evalTarget = true;
                                break;
                            }
                        }
                        break;
                    case "$":
                        foreach (string goodValue in goodValues)
                        {
                            if (targetValue.EndsWith(goodValue))
                            {
                                evalTarget = true;
                                break;
                            }
                        }
                        break;
                    case "=":
                        foreach (string goodValue in goodValues)
                        {
                            if (targetValue == goodValue)
                            {
                                evalTarget = true;
                                break;
                            }
                        }
                        break;
                    case "!=":
                        foreach (string goodValue in goodValues)
                        {
                            if (targetValue != goodValue)
                            {
                                evalTarget = true;
                                break;
                            }
                        }
                        break;
                }
            }
            else
            {
                skipCondition = true;
            }
            return evalTarget;
        }

        public void GetValidValues(string validationFile, string _valGroupName)
        {
            retValues.Clear();
            try
            {
                XDocument doc = XDocument.Load(validationFile);
                IEnumerable<XElement> validations = doc.Root.Descendants("Validation");
                foreach (XElement validation in validations)
                {
                    // Get Validation group name
                    string valGroupName = validation.Attribute("Name").Value.ToString().ToUpper();
                    // Get values
                    if (valGroupName == _valGroupName)
                    {
                        // Get valid values
                        XElement validValues = validation.Element("ValidValues");
                        IEnumerable<XElement> addValues = validValues.Descendants("Add");
                        //
                        foreach (XElement addValue in addValues)
                        {
                            retValues.Add(addValue.Attribute("Value").Value.ToString());
                        }
                        break;
                    }
                }
            }
            catch { };
        }

        public string ValidateLinuxSignature(string signatureFile, string validationFile)
        {
            string myErrorMessage = "";
            bool areThereErrors = false;
            try
            {
                XDocument doc = XDocument.Load(signatureFile);
                IEnumerable<XElement> groups = doc.Root.Descendants("Group");

                foreach (XElement group in groups)
                {
                    string vGroupId = group.Element("GroupId").Value.ToString();
                    string vWhere = group.Element("Where").Value.ToString();
                    string vApplied = group.Element("Applied").Value.ToString();
                    string vType = group.Element("Type").Value.ToString();
                    string vValue = group.Element("Value").Value.ToString();
                    string vIgnore = group.Element("Ignore").Value.ToString();
                    string vIgnoreReason = group.Element("IgnoreReason").Value.ToString();
                    // 
                    groupId = vGroupId;
                    where = vWhere;
                    applied = vApplied;
                    type = vType;
                    valuel = vValue;
                    ignore = vIgnore;
                    ignoreReason = vIgnoreReason;
                    // Validate with the linux validation file
                    string myErrMessage = Validate(validationFile, false);
                    if (myErrMessage.Length > 0)
                    {
                        WriteErrorFile(myErrMessage);
                        areThereErrors = true;
                    }
                }
                if (areThereErrors)
                {
                    myErrorMessage = "The file '" + signatureFile + "' has values errors. You can find detailed error information on the file '" + errorFile + "'.";
                }
            }
            catch (Exception err)
            {
                WriteErrorFile(groupId + ". " + err.Message);
                myErrorMessage = "The file '" + signatureFile + "' has reading xml errors. You can find detailed error information on the file '" + errorFile + "'.";
            }
            return myErrorMessage;
        }

        private void WriteErrorFile(string strMessage)
        {
            using (StreamWriter writer = new StreamWriter(new FileStream(errorFile, FileMode.Append)))
            {
                writer.WriteLine(strMessage);
            }
        }
    }
}
