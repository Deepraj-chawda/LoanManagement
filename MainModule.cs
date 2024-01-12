using LoanManagement.DAO;
using LoanManagement.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanManagement
{
    internal class MainModule
    {
        static void Main(string[] args)
        {
            LoanRepositoryImpl loanRepositoryImpl = new LoanRepositoryImpl();

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("======================================================================================================================");
                Console.WriteLine("                                     LOAN MANAGEMENT SYSTEM DASHBOARD      ");
                Console.WriteLine("======================================================================================================================");
                Console.ResetColor();

                Console.WriteLine("\n1. Apply For Loan");
                Console.WriteLine("2. Calculate Interest");
                Console.WriteLine("3. Get Loan Status");
                Console.WriteLine("4. Calculate EMI");
                Console.WriteLine("5. Pay EMI");
                Console.WriteLine("6. Get All Loan Details");
                Console.WriteLine("7. Get Laon Details By ID");
                Console.WriteLine("8. Exit");
                Console.WriteLine("Choose between 1 - 8");
                int choice = int.Parse(Console.ReadLine());
                
                switch (choice)
                {
                    case 1:
                        try
                        {
                            Loan loan = new Loan();
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine("Enter loan details:");

                            Console.Write("Customer ID: ");
                            int customerId = int.Parse(Console.ReadLine());

                            Console.Write("Principal Amount: ");
                            decimal principalAmount = decimal.Parse(Console.ReadLine());

                            Console.Write("Interest Rate: ");
                            decimal interestRate = decimal.Parse(Console.ReadLine());

                            Console.Write("Loan Term (months): ");
                            int loanTerm = int.Parse(Console.ReadLine());

                            

                            Console.Write("Loan Type (CarLoan/HomeLoan): ");
                            string loanType = Console.ReadLine();

                            loan.CustomerId = customerId;
                            loan.PrincipalAmount = principalAmount;
                            loan.InterestRate = interestRate;
                            loan.LoanTerm = loanTerm;
                            
                            loan.LoanType = loanType;

                            loanRepositoryImpl.ApplyLoan(loan);
                            Console.ResetColor();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                    case 2:
                        try
                        {
                            Console.Write("Enter Loan Id to calculate interest : ");
                            int loanId = Convert.ToInt32(Console.ReadLine());
                            decimal interest = loanRepositoryImpl.CalculateInterest(loanId);
                            Console.WriteLine("Interest amount is {0}", interest);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                    case 3:
                        try
                        {
                            Console.Write("Enter LoanId to check loan status : ");
                            int loanId = Convert.ToInt32(Console.ReadLine());
                            loanRepositoryImpl.LoanStatus(loanId);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                    case 4:
                        try
                        {
                            Console.Write("Enter LoanId to calculate EMI : ");
                            int loanId = Convert.ToInt32(Console.ReadLine());
                            decimal emi = loanRepositoryImpl.CalculateEMI(loanId);
                            Console.WriteLine("EMI per month is {0}", emi);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                    case 5:
                        try
                        {
                            Console.Write("Enter LoanId to pay EMI : ");
                            int loanId = Convert.ToInt32(Console.ReadLine());
                            Console.Write("Enter amount you want to pay : ");
                            decimal amount = Convert.ToDecimal(Console.ReadLine());
                            loanRepositoryImpl.LoanRepayment(loanId, amount);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                    case 6:
                        try
                        {
                            List<Loan> loans = new List<Loan>();
                            loans = loanRepositoryImpl.GetAllLoans();
                            Console.ForegroundColor = ConsoleColor.Green;
                            if (loans != null && loans.Count > 0)
                            {
                                foreach (Loan loan in loans)
                                {
                                    loan.PrintLoanInfo();
                                    Console.WriteLine();
                                }
                            }
                            Console.ResetColor();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                    case 7:
                        try
                        {
                            Console.Write("Enter LoanId to get loan details : ");

                            
                            int loanId = Convert.ToInt32(Console.ReadLine());
                            
                            Loan loan = loanRepositoryImpl.GetLoanById(loanId);
                            Console.ForegroundColor = ConsoleColor.Green;
                            if (loan != null)
                            {
                                loan.PrintLoanInfo();
                             }
                            Console.ResetColor();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;

                    case 8:
                        Console.WriteLine("Exiting ...");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invaild Option! please select between 1-9");
                        break;
                }
            } 
        }
    }

}
    
