using System;

namespace BankApp
{
    abstract class Account
    {
        // Private fields - cannot be accessed outside this class
        private string accNumber;
        private double balance;

        // Protected field - accessible in derived classes
        protected string accType;

        // Public property - read-only from outside
        public string AccountNumber => accNumber;

        // Constructor
        public Account(string num, double bal, string type)
        {
            accNumber = num;
            balance = bal;
            accType = type;
        }

        // Public methods - accessible to all
        public void Deposit(double amount)
        {
            balance += amount;
            Console.WriteLine($"Deposited {amount}. New balance: {balance}");
        }

        public void Withdraw(double amount)
        {
            if (amount <= balance)
            {
                balance -= amount;
                Console.WriteLine($"Withdrawn {amount}. New balance: {balance}");
            }
            else
            {
                Console.WriteLine("Insufficient funds!");
            }
        }

        // Protected method - only for derived classes
        protected double GetBalance()
        {
            return balance;
        }

        // Abstract method - implemented differently in subclasses
        public abstract void CalculateInterest();
    }

    // SavingsAccount inherits from Account
    class SavingsAccount : Account
    {
        private double interestRate = 0.05;

        public SavingsAccount(string num, double bal)
            : base(num, bal, "Savings") { }

        public override void CalculateInterest()
        {
            double interest = GetBalance() * interestRate;
            Console.WriteLine($"Interest for Savings Account: {interest}");
        }
    }

    // CurrentAccount inherits from Account
    class CurrentAccount : Account
    {
        private double serviceCharge = 100;

        public CurrentAccount(string num, double bal)
            : base(num, bal, "Current") { }

        public override void CalculateInterest()
        {
            Console.WriteLine("No interest for Current Accounts.");
            Console.WriteLine($"Monthly Service Charge: {serviceCharge}");
        }
    }

    // Customer class
    class Customer
    {
        public string Name { get; set; }
        public Account MyAccount { get; set; }

        public Customer(string name, Account acc)
        {
            Name = name;
            MyAccount = acc;
        }

        public void DisplayDetails()
        {
            Console.WriteLine($"\nCustomer Name: {Name}");
            Console.WriteLine($"Account Number: {MyAccount.AccountNumber}");
        }
    }

    // Internal class - accessible only within this project
    internal class BankEmployee
    {
        public void ViewCustomer(Customer c)
        {
            Console.WriteLine($"\n[Employee View]");
            Console.WriteLine($"Customer: {c.Name}, Account: {c.MyAccount.AccountNumber}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            SavingsAccount sa = new SavingsAccount("S001", 5000);
            Customer cust1 = new Customer("Vathsan", sa);
            cust1.DisplayDetails();

            sa.Deposit(1500);
            sa.Withdraw(2000);
            sa.CalculateInterest();

            CurrentAccount ca = new CurrentAccount("C001", 10000);
            Customer cust2 = new Customer("Loosu Koomutta", ca);
            cust2.DisplayDetails();

            ca.Withdraw(3000);
            ca.CalculateInterest();

            BankEmployee emp = new BankEmployee();
            emp.ViewCustomer(cust1);
            emp.ViewCustomer(cust2);
        }
    }
}
