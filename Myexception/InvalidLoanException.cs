﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanManagement.Myexception
{
    public class InvalidLoanException : Exception
    {
        
        public InvalidLoanException(string message) : base(message) { }
    }
}