using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LogicNP.CryptoLicensing;

namespace VeriSignature
{
    class ProductLicense
    {
        public enum COSFoundryFeatures
        {
            COSFoundryFeature1 = 1, // Signature builder
            COSFoundryFeature2, // Create signature container
            COSFoundryFeature3, // feature 3
            COSFoundryFeature4, // feature 4
            COSFoundryFeature5, // feature 5
            COSFoundryFeature6, // feature 6
            COSFoundryFeature7, // feature 7
            COSFoundryFeature8, // feature 8
            COSFoundryFeature9, // feature 9
            COSFoundryFeature10, // feature 10
        }

        public class Constants
        {
            public const string validationKey = "AMAAMADH42sVJ9Bz4k83U+yvpfpAUK6AScjVHHXY0BIKuc4fREbaF2nVG9YjtBVEPGublikDAAEAAQ==";
        }

        public class Methods
        {
            public static CryptoLicense CreateLicense()
            {
                CryptoLicense ret = new CryptoLicense();

                ret.ValidationKey = ProductLicense.Constants.validationKey;

                return ret;
            }

        }

        public class Runtime
        {
            static Boolean cosFoundryFeature1 = false;
            public static Boolean CosFoundryFeature1
            {
                get
                {
                    return cosFoundryFeature1;
                }
                set
                {
                    cosFoundryFeature1 = value;
                }
            }
            static Boolean cosFoundryFeature2 = false;
            public static Boolean CosFoundryFeature2
            {
                get
                {
                    return cosFoundryFeature2;
                }
                set
                {
                    cosFoundryFeature2 = value;
                }
            }
            static Boolean cosFoundryFeature3 = false;
            public static Boolean CosFoundryFeature3
            {
                get
                {
                    return cosFoundryFeature3;
                }
                set
                {
                    cosFoundryFeature3 = value;
                }
            }
            static Boolean cosFoundryFeature4 = false;
            public static Boolean CosFoundryFeature4
            {
                get
                {
                    return cosFoundryFeature4;
                }
                set
                {
                    cosFoundryFeature4 = value;
                }
            }
            static Boolean cosFoundryFeature5 = false;
            public static Boolean CosFoundryFeature5
            {
                get
                {
                    return cosFoundryFeature5;
                }
                set
                {
                    cosFoundryFeature5 = value;
                }
            }
            static Boolean cosFoundryFeature6 = false;
            public static Boolean CosFoundryFeature6
            {
                get
                {
                    return cosFoundryFeature6;
                }
                set
                {
                    cosFoundryFeature6 = value;
                }
            }
            static Boolean cosFoundryFeature7 = false;
            public static Boolean CosFoundryFeature7
            {
                get
                {
                    return cosFoundryFeature7;
                }
                set
                {
                    cosFoundryFeature7 = value;
                }
            }
            static Boolean cosFoundryFeature8 = false;
            public static Boolean CosFoundryFeature8
            {
                get
                {
                    return cosFoundryFeature8;
                }
                set
                {
                    cosFoundryFeature8 = value;
                }
            }
            static Boolean cosFoundryFeature9 = false;
            public static Boolean CosFoundryFeature9
            {
                get
                {
                    return cosFoundryFeature9;
                }
                set
                {
                    cosFoundryFeature9 = value;
                }
            }
            static Boolean cosFoundryFeature10 = false;
            public static Boolean CosFoundryFeature10
            {
                get
                {
                    return cosFoundryFeature10;
                }
                set
                {
                    cosFoundryFeature10 = value;
                }
            }

            // License expiration date
            static string licenceExpitationDate = "";
            public static string LicenceExpitationDate
            {
                get
                {
                    return licenceExpitationDate;
                }
                set
                {
                    licenceExpitationDate = value;
                }
            }
        }

    }
}
