﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PresentationWPF.ZATestApp.Regex
{
    public class TestData
    {
        private string emailAddress;
        private string dateString;
        private string productCode;

        public string EmailAddress
        {
            get { return emailAddress; }
            set { emailAddress = value; }
        }

        public string DateString
        {
            get { return dateString; }
            set { dateString = value; }
        }

        // Pattern: @@@.### 
        // @ = letter
        // # = number
        public string ProductCode
        {
            get { return productCode; }
            set { productCode = value; }
        }

        public string MiscellaneousInput { get; set; }
    }
}
