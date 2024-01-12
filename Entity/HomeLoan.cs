using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanManagement.Entity
{
    public class HomeLoan : Loan
    {
        // Additional attributes
    public string PropertyAddress { get; set; }
        public int PropertyValue { get; set; }

        // Default constructor
        public HomeLoan()
        {
        }

        // Overloaded constructor with parameters
        public HomeLoan(int loanId, int customerid, decimal principalAmount, decimal interestRate, int loanTerm, string loanType, string loanStatus,
                        string propertyAddress, int propertyValue)
            : base(customerid, principalAmount, interestRate, loanTerm, loanType, loanStatus,loanId)
        {
            PropertyAddress = propertyAddress;
            PropertyValue = propertyValue;
        }

        // Method to print information
        public new void PrintLoanInfo()
        {
            base.PrintLoanInfo();
            Console.WriteLine($"Property Address: {PropertyAddress}");
            Console.WriteLine($"Property Value: {PropertyValue}");
        }
    }
}
