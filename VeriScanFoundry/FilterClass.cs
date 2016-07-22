using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VeriSignature
{
    public class FilterClass
    {
        private string filterElement;
        public string FilterElement
        {
            get { return filterElement; }
            set { filterElement = value; }
        }
        private string filterKeyword;
        public string FilterKeyword
        {
            get { return filterKeyword; }
            set { filterKeyword = value; }
        }
        private bool mustMatch;
        public bool MustMatch
        {
            get { return mustMatch; }
            set { mustMatch = value; }
        }
        private bool notInclude;
        public bool NotInclude
        {
            get { return notInclude; }
            set { notInclude = value; }
        }

        public FilterClass(string FilterElement, string FilterKeyword, bool MustMatch, bool NotInclude)
        {
            filterElement = FilterElement;
            filterKeyword = FilterKeyword;
            mustMatch = MustMatch;
            notInclude = NotInclude;
        }
    }
}
