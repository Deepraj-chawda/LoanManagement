using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanManagement.Entity
{
    public class CarLoan : Loan
    {
        // Additional attributes
        public string CarModel { get; set; }
        public int CarValue { get; set; }

        // Default constructor
        public CarLoan()
        {
        }

        // Overloaded constructor with parameters
        public CarLoan(int loanId, int customerid, decimal principalAmount, decimal interestRate, int loanTerm, string loanType, string loanStatus,
                       string carModel, int carValue)
            : base( customerid , principalAmount, interestRate, loanTerm, loanType, loanStatus, loanId)
        {
            CarModel = carModel;
            CarValue = carValue;
        }

        // Method to print information
        public new void PrintLoanInfo()
        {
            base.PrintLoanInfo();
            Console.WriteLine($"Car Model: {CarModel}");
            Console.WriteLine($"Car Value: {CarValue}");
        }
    }
}
