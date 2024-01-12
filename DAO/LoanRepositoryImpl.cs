using LoanManagement.Entity;
using LoanManagement.Util;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoanManagement.Myexception;

namespace LoanManagement.DAO
{
    public class LoanRepositoryImpl: ILoanRepository
    {
        public SqlConnection connection;

        public void ApplyLoan(Loan loan)
        {
            try
            {
                connection = DBConnUtil.getDBConn();
                
                connection.Open();

                string propertyAddress = "", carModel = "";
                int propertyValue = 0, carValue = 0;

                if (loan.LoanType.Equals("HomeLoan", StringComparison.OrdinalIgnoreCase))
                {
                    Console.Write("Enter Property Address: ");
                    propertyAddress = Console.ReadLine();
                    Console.Write("Enter Property Value: ");
                    propertyValue = int.Parse(Console.ReadLine());
                }
                else
                {
                    Console.Write("Enter Car Model: ");
                    carModel = Console.ReadLine();
                    Console.Write("Enter Car Value: ");
                    carValue = int.Parse(Console.ReadLine());
                }

                loan.LoanStatus = "Pending";

                Console.WriteLine("Do you want to apply for this loan? (Yes/No)");
                string userInput = Console.ReadLine();

                if (userInput.Equals("Yes", StringComparison.OrdinalIgnoreCase))
                {
                    string insertQuery = "INSERT INTO Loan ( customer_id, principal_amount, interest_rate, loan_term_months, loan_type, loan_status)" +
                        "values ( @Customer_id, @Principal_amount, @Interest_rate, @Loan_term_months, @Loan_type, @Loan_status)";
                    int status = 0;
                    
                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                           
                        command.Parameters.AddWithValue("@Customer_id", loan.CustomerId);
                        command.Parameters.AddWithValue("@Principal_amount", loan.PrincipalAmount);
                        command.Parameters.AddWithValue("@Interest_rate", loan.InterestRate);
                        command.Parameters.AddWithValue("@Loan_term_months", loan.LoanTerm);
                        command.Parameters.AddWithValue("@Loan_type", loan.LoanType);
                        command.Parameters.AddWithValue("@Loan_status", loan.LoanStatus);
                            

                        status = command.ExecuteNonQuery();
                        command.Parameters.Clear();
                    }

                    if (status > 0)
                    {
                        int loanId;
                        insertQuery = "SELECT loan_id FROM Loan where Customer_id=@CustomerId AND Principal_amount=@PrincipalAmount";
                    
                        using (SqlCommand command = new SqlCommand(insertQuery, connection))
                        {
                            command.Parameters.AddWithValue("@CustomerId", loan.CustomerId);
                            command.Parameters.AddWithValue("@PrincipalAmount", loan.PrincipalAmount);


                            loanId = Convert.ToInt32(command.ExecuteScalar());
                        }

                        if (loan.LoanType.Equals("HomeLoan", StringComparison.OrdinalIgnoreCase))
                            {
                                insertQuery = "INSERT INTO HomeLoan VALUES (@LoanId, @propertyAddress, @propertyValue)";
                        
                                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                                {
                                    command.Parameters.AddWithValue("@LoanId", loanId);
                                    command.Parameters.AddWithValue("@propertyAddress", propertyAddress);
                                    command.Parameters.AddWithValue("@propertyvalue", propertyValue);
                                    int n = command.ExecuteNonQuery();
                                    if (n > 0)
                                    {
                                        Console.WriteLine("Loan application submitted successfully.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Loan application failed.");
                                    }
                                }
                            }
                            else
                            {
                                insertQuery = "INSERT INTO CarLoan VALUES (@LoanId, @carModel, @carValue)";
                                
                                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                                {
                                    command.Parameters.AddWithValue("@LoanId", loanId);
                                    command.Parameters.AddWithValue("@carModel", carModel);
                                    command.Parameters.AddWithValue("@carValue", carValue);
                                    int n = command.ExecuteNonQuery();
                                    if (n > 0)
                                    {
                                        Console.WriteLine("Loan application submitted successfully.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Loan application failed.");
                                    }
                                }
                            }
                     
                    }
                    else
                    {
                        Console.WriteLine("Loan application canceled by the user.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error applying for loan: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
           
        }
        
        public decimal CalculateInterest(decimal principalAmount, decimal interestRate, int loanTerm)
        {
            // Calculate interest based on provided parameters
            return (principalAmount * interestRate * loanTerm) / 12;
        }
        public decimal CalculateInterest(int loanId)
        {
            try
            {
                connection = DBConnUtil.getDBConn();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select * from  Loan where loan_id= {loanId}";
                connection.Open();

                SqlDataReader data = cmd.ExecuteReader();

                if (data.Read())
                {
                    decimal PrincipalAmount = Convert.ToDecimal(data["principal_amount"]);
                    decimal InterestRate = Convert.ToDecimal(data["interest_rate"]);
                    int LoanTerm = Convert.ToInt32(data["loan_term_months"]);
                    // Calculate interest based on loan details

                    data.Close();
                    connection.Close();
                    return CalculateInterest(PrincipalAmount, InterestRate, LoanTerm);

                    

                }
                else
                {
                    data.Close();
                    

                    throw new InvalidLoanException("Loan not found");
                }
            }
            catch (InvalidLoanException ex)
            {
                Console.WriteLine($"Error calculating interest: {ex.Message}");
                return -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calculating interest: {ex.Message}");
                return -1;
            }
            finally
            {
                connection.Close();
            }
                
        }

        public void LoanStatus(int loanId)
        {
            try
            {
                connection = DBConnUtil.getDBConn();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select * from  Loan where loan_id= {loanId}";
                connection.Open();

                SqlDataReader data = cmd.ExecuteReader();

                if (data.Read())
                {
                    string loan_status =data["loan_status"].ToString();
                 
                    int customerid = Convert.ToInt32(data["customer_id"]);
                    
                    data.Close();
                    connection.Close();

                    connection = DBConnUtil.getDBConn();
                    SqlCommand cmd1 = new SqlCommand();
                    cmd1.Connection = connection;
                    cmd1.CommandType = System.Data.CommandType.Text;
                    cmd1.CommandText = $"Select * from  Customer where customer_id= {customerid}";
                    connection.Open();

                    SqlDataReader data1 = cmd1.ExecuteReader();
                    if (data1.Read())
                    {
                        int CreditScore = Convert.ToInt32(data1["credit_score"]);
                        string LoanStatus = null;
                        // Check credit score and update loan status
                        if (CreditScore > 650)
                        {
                            LoanStatus = "Approved";
                            Console.WriteLine("Loan approved!");
                        }
                        else
                        {
                            LoanStatus = "Rejected";
                            Console.WriteLine("Loan rejected due to low credit score.");
                        }
                        data1.Close();
                        connection.Close();

                        connection = DBConnUtil.getDBConn();
                        SqlCommand cmd2 = new SqlCommand();
                        cmd2.Connection = connection;
                        cmd2.CommandType = System.Data.CommandType.Text;
                        cmd2.CommandText = "UPDATE Loan SET loan_status = @Status WHERE loan_id= @loanId";
                        cmd2.Parameters.AddWithValue("@Status",LoanStatus);
                        cmd2.Parameters.AddWithValue("@loanId", loanId);

                        connection.Open();

                        int updateStatus = cmd2.ExecuteNonQuery();
                       
                    }
                }


            
                else
                {
                    data.Close();
                    

                    throw new InvalidLoanException("Loan not found"); ;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
            finally
            {
                connection.Close();
            }


          
        }

        public decimal CalculateEMI(int loanId)
        {
            try
            {
                connection = DBConnUtil.getDBConn();
                
                connection.Open();

                string selectQuery = "SELECT principal_amount, interest_rate, loan_term_months FROM Loan WHERE loan_id = @LoanId";
                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@LoanId", loanId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            decimal principalAmount = Convert.ToDecimal(reader["principal_amount"]);
                            decimal interestRate = Convert.ToDecimal(reader["interest_rate"]);
                            int loanTerm = Convert.ToInt32(reader["loan_term_months"]);

                            return CalculateEMI(loanId, principalAmount, interestRate, loanTerm);
                        }
                        else
                        {
                            throw new InvalidLoanException("Loan not found.");
                        }
                    }
                }
            
            }
            catch (InvalidLoanException ex)
            {
                Console.WriteLine($"Error calculating interest: {ex.Message}");
                return -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calculating interest: {ex.Message}");
                return -1;
            }
            finally
            {
                connection.Close();
            }
            
        }

        public decimal CalculateEMI(int loanId, decimal principalAmount, decimal interestRate, int loanTerm)
        {
            decimal r = interestRate / 12 / 100;
            decimal n = loanTerm;
            return (principalAmount * r * (decimal)Math.Pow((double)(1 + r), (double)n)) / (decimal)(Math.Pow((double)(1 + r), (double)n) - 1);
        }

        public void LoanRepayment(int loanId, decimal amount)
        {
            try
            {
                connection = DBConnUtil.getDBConn();
                
                connection.Open();

                string selectQuery = "SELECT principal_amount, interest_rate, loan_term_months FROM Loan WHERE loan_id = @LoanId";
                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@LoanId", loanId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            decimal principalAmount = Convert.ToDecimal(reader["principal_amount"]);
                            decimal interestRate = Convert.ToDecimal(reader["interest_rate"]);
                            int loanTerm = Convert.ToInt32(reader["loan_term_months"]);

                            decimal emi = CalculateEMI(loanId, principalAmount, interestRate, loanTerm);

                            int noOfEmiToPay = (int)(amount / emi);

                            if (noOfEmiToPay == 0 || amount < emi)
                            {
                                Console.WriteLine("Payment rejected. Insufficient amount for at least one EMI.");
                            }
                            else
                            {
                                Console.WriteLine($"Paid {noOfEmiToPay} EMIs. Remaining amount: {amount % emi}");
                                connection.Close();
                                //updating DB
                                UpdateLoanRepayment(loanId, noOfEmiToPay, emi);
                                   
                                Console.WriteLine("Loan repayment variable updated in the database.");
                            }
                        }
                        else
                        {
                            throw new InvalidLoanException("Loan not found.");
                        }
                    }
                }
                
            }
            catch (InvalidLoanException ex)
            {
                Console.WriteLine($"Error processing loan repayment: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing loan repayment: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
           
        }

        private void UpdateLoanRepayment(int loanId, int noOfEmiToPay, decimal emiAmount)
        {
            try
            {
                connection = DBConnUtil.getDBConn();
                
                connection.Open();

                string selectQuery = "SELECT principal_amount FROM Loan WHERE loan_id = @LoanId";
                using (SqlCommand selectCommand = new SqlCommand(selectQuery, connection))
                {
                    selectCommand.Parameters.AddWithValue("@LoanId", loanId);

                    decimal currentPrincipalAmount = Convert.ToDecimal(selectCommand.ExecuteScalar());

                    decimal newPrincipalAmount = currentPrincipalAmount - Math.Floor((noOfEmiToPay * emiAmount));

                    string updateQuery = "UPDATE Loan SET principal_amount = @NewPrincipalAmount WHERE loan_id = @LoanId";
                    using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                    {
                        updateCommand.Parameters.AddWithValue("@LoanId", loanId);
                        updateCommand.Parameters.AddWithValue("@NewPrincipalAmount", newPrincipalAmount);

                        updateCommand.ExecuteNonQuery();
                    }

                    Console.WriteLine($"Loan repayment variable for Loan ID {loanId} updated. New Principal Amount: {newPrincipalAmount}");
                }
            
            }
            catch (Exception ex)
            {
                throw new InvalidLoanException($"Error updating loan repayment variable: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
            
        }

        public List<Loan> GetAllLoans()
        {
            List<Loan> allLoans = new List<Loan>();

            try
            {
                connection = DBConnUtil.getDBConn();
                
                connection.Open();

                string selectQuery = "SELECT * FROM Loan";
                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Loan loan = new Loan
                                {
                                    LoanId = Convert.ToInt32(reader["loan_id"]),
                                    PrincipalAmount = Convert.ToDecimal(reader["principal_amount"]),
                                    InterestRate = Convert.ToDecimal(reader["interest_rate"]),
                                    LoanTerm = Convert.ToInt32(reader["loan_term_months"]),
                                    LoanType = reader["loan_type"].ToString(),
                                    LoanStatus = reader["loan_status"].ToString(),
                                    CustomerId = Convert.ToInt32(reader["customer_id"])
                                };

                                allLoans.Add(loan);
                            }
                            return allLoans;
                        }
                        else
                        {
                            throw new InvalidLoanException("No Loans Found");
                        }
                    }
                }
                
            }
            catch (InvalidLoanException ex)
            {
                Console.WriteLine($"Error retrieving all loans: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving all loans: {ex.Message}");
                return null;
            }
            finally
            {
                connection.Close();
            }
           
        }

        public Loan GetLoanById(int loanId)
        {
            try
            {
                connection = DBConnUtil.getDBConn();
                
                connection.Open();

                string selectQuery = "SELECT * FROM Loan WHERE loan_id = @LoanId";
                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@LoanId", loanId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Loan loan = new Loan
                            {
                                LoanId = Convert.ToInt32(reader["loan_id"]),
                                PrincipalAmount = Convert.ToDecimal(reader["principal_amount"]),
                                InterestRate = Convert.ToDecimal(reader["interest_rate"]),
                                LoanTerm = Convert.ToInt32(reader["loan_term_months"]),
                                LoanType = reader["loan_type"].ToString(),
                                LoanStatus = reader["loan_status"].ToString(),
                                CustomerId = Convert.ToInt32(reader["customer_id"])
                            };

                            return loan;
                        }
                        else
                        {
                            throw new InvalidLoanException("Loan not found.");
                        }
                    }
                }
            
            }
            catch (InvalidLoanException ex)
            {
                Console.WriteLine($"Error retrieving loan by ID: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving loan by ID: {ex.Message}");
                return null;
            }
            finally
            {
                connection.Close();
            }
            
        }

    }
}
