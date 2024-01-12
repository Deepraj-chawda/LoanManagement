using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LoanManagement.Entity
{
    public class Loan
    {
        // Attributes
        public int LoanId { get; set; }
        public int CustomerId { get; set; }
        public decimal PrincipalAmount { get; set; }
        public decimal InterestRate { get; set; }
        public int LoanTerm { get; set; }
        public string LoanType { get; set; }
        public string LoanStatus { get; set; }

        // Default constructor
        public Loan()
        {
        }

        // Overloaded constructor with parameters
        public Loan( int customerid, decimal principalAmount, decimal interestRate, int loanTerm, string loanType, string loanStatus, [Optional] int loanId)
        {
            LoanId = loanId;
            CustomerId = customerid;
            PrincipalAmount = principalAmount;
            InterestRate = interestRate;
            LoanTerm = loanTerm;
            LoanType = loanType;
            LoanStatus = loanStatus;
        }

        // Method to print information
        public void PrintLoanInfo()
        {
            Console.WriteLine($"Loan ID: {LoanId}");
            Console.WriteLine($"Customer ID: {CustomerId}");
            
            Console.WriteLine($"Principal Amount: {PrincipalAmount}");
            Console.WriteLine($"Interest Rate: {InterestRate}");
            Console.WriteLine($"Loan Term: {LoanTerm} months");
            Console.WriteLine($"Loan Type: {LoanType}");
            Console.WriteLine($"Loan Status: {LoanStatus}");
        }
    }
}
