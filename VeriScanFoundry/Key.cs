using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace VeriSignature
{
    class Key
    {
        #region Fields & Properties

        private string errorMessage = "";
        public string ErrorMessage
        {
            get { return errorMessage; }
            set { errorMessage = value; }
        }

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

        #endregion

        #region Generate Keys

        public bool GenerateKey(string keysFileName, string keyLenght, string keyIVLenght)
        {
            errorMessage = "";
            bool retGenerateKeyPair = false;
            try
            {
                string key = CreateKey(System.Convert.ToInt32(keyLenght));
                string keyIV = CreateKey(System.Convert.ToInt32(keyIVLenght));
                //
                using (StreamWriter writer = new StreamWriter(keysFileName))
                {
                    writer.WriteLine(key);
                    writer.WriteLine(keyIV);
                    retGenerateKeyPair = true;
                }
            }
            catch (Exception err)
            {
                errorMessage = "The key was not successfully generated. " + err.Message;
            }
            return retGenerateKeyPair;
        }

        private String CreateKey(int numBytes)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[numBytes];

            rng.GetBytes(buff);
            return BytesToHexString(buff);
        }

        private String BytesToHexString(byte[] bytes)
        {
            StringBuilder hexString = new StringBuilder(64);

            for (int counter = 0; counter < bytes.Length; counter++)
            {
                hexString.Append(String.Format("{0:X2}", bytes[counter]));
            }
            return hexString.ToString();
        }

        #endregion

        #region SC unique keys

        public void EncryptionKeysFoundry()
        {
            aesKey = "00F7CFDABAC02B1A4958724D1B342794";
            aesKeyIV = "C8C30AE8F0DC36AE";
        }
        //
        public void EncryptionKeysClient()
        {
            aesKey = "4837644FE8F0D95D11A610F2690C5F85";
            aesKeyIV = "C3962215BE383AAE";
        }

        #endregion

    }
}
