using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SupportBank
{
    class Program
    {
        static void Main(string[] args)
        {
            var transactions = ProcessTransactions("Transactions2014.csv");
            var AccountsDictionary = CreateAccounts(transactions);
            AccountsDictionary = PopulateAccounts(AccountsDictionary, transactions);
            getBalances(AccountsDictionary);
            welcomescreen(AccountsDictionary, transactions);
        }

        private static List<Transaction> ProcessTransactions(string path)
        {
            var transactions = new List<Transaction>();
            var lines = File.ReadAllLines(path).Skip(1);
            foreach (string line in lines)
            {
                string[] transactionelements = line.Split(",");
                Transaction newTransaction =
                    new Transaction(DateTime.Parse(transactionelements[0]),
                        transactionelements[1],
                        transactionelements[2],
                        transactionelements[3],
                        Decimal.Parse(transactionelements[4]));
                transactions.Add (newTransaction);
            }
            return transactions;
        }

        private static void printList(List<Transaction> listToPrint)
        {
            foreach (var item in listToPrint)
            {
                Console
                    .WriteLine($"Date: {item.Date.ToString("d").PadRight(12)} From: {item.From.PadRight(10)} To:{item.To.PadRight(10)} Narrative: {item.Narrative.PadRight(35)} Amount: £{item.Amount}");
            }
        }

        //we want to return the created dictionary so this shouldn't be void!
        private static Dictionary<string, Account> CreateAccounts(List<Transaction> transactions)
        {
            var AccountsDictionary = new Dictionary <string, Account>(); 
            foreach (var transaction in transactions)
            {
                if(!AccountsDictionary.ContainsKey(transaction.From)) 
                {
                    var account = new Account(transaction.From);
                    AccountsDictionary.Add(transaction.From, account);
                }
                
                if(!AccountsDictionary.ContainsKey(transaction.To))  
                {
                    var account = new Account(transaction.To);
                    AccountsDictionary.Add(transaction.To, account);
                }
            }
            return AccountsDictionary;
        }
        
        private static void addTransaction(string name, Transaction transaction, Dictionary<string, Account> AccountsDictionary)
        {
            AccountsDictionary[name].AccountTransactions.Add(transaction);
        }

        private static Dictionary<string, Account> PopulateAccounts(Dictionary<string, Account> AccountsDictionary, List<Transaction> transactions)
        {
            foreach(var name in AccountsDictionary.Keys)
            {
                foreach(var transaction in transactions)
                {
                    if (name == transaction.From)
                    {   
                        addTransaction(transaction.To, transaction, AccountsDictionary);
                        addTransaction(transaction.From, transaction, AccountsDictionary);
                    }   
                }
            }
            return AccountsDictionary;
        }

        private static Decimal getBalance(Account account)
        {
            decimal balance = 0;
            foreach (var transaction in account.AccountTransactions)
            {

                balance = (account.Name == transaction.From) ? balance - transaction.Amount : balance + transaction.Amount;

            }
            return balance;
        }

        private static void getBalances(Dictionary<string, Account> AccountsDictionary)
        {
            foreach (var account in AccountsDictionary.Values)
            {
                account.AccountBalance = getBalance(account); 
            }
        }

        private static void printBalances(Dictionary<string, Account> AccountsDictionary)
        {
            foreach (var name in AccountsDictionary)
            {
                Console.WriteLine("{0} {1} {2}",
                 name.Value.Name, 
                 name.Value.AccountBalance > 0 ? "owes":"is owed",
                 Math.Abs(name.Value.AccountBalance));
            }
            Console.WriteLine("------------------------------------------------------");
        }

        private static void printAccountTransactions(Dictionary<string, Account> AccountsDictionary, List <Transaction> transactions)
        {
            Console.WriteLine("Please select a user account from the following list");
            Console.WriteLine("------------------------------------------------------");
            foreach (var name in AccountsDictionary.Keys)
            {
                Console.WriteLine(name);
            }
            Console.WriteLine("------------------------------------------------------");
            Console.WriteLine("Please select a user account from the above list");
            Console.WriteLine("------------------------------------------------------");

            var userInput = Console.ReadLine();

            Console.WriteLine("{0} {1} {2} {3} £{4}",
                "Date".PadRight(15),
                "From".PadRight(20),
                "To".PadRight(20),
                "Narrative".PadRight(35),
                "Amount"
                );

            List <Transaction> sortedList = AccountsDictionary[userInput].AccountTransactions.OrderBy(o => o.Date).ToList();
            foreach (var item in sortedList)
            {
                Console.WriteLine("{0} {1} {2} {3} {4}",
                item.Date.ToString("d").PadRight(15),
                item.From.PadRight(20),
                item.To.PadRight(20),
                item.Narrative.PadRight(35),
                string.Format("{00:C}", item.Amount)
                );
            }
            Console.WriteLine("------------------------------------------------------");
        }

        static void welcomescreen(Dictionary<string, Account> AccountsDictionary, List <Transaction> transactions)
        {
            Console.WriteLine("------------------------------------------------------");
            var go = true;
            while (go)
            {
                Console.WriteLine("Please enter 1, 2 or 3 to select from the following options:");
                Console.WriteLine("1: Print all names and balances");
                Console.WriteLine("2: Print all transactions for a specific account");
                Console.WriteLine("3: Exit program");
                var userInput = Console.ReadLine();

                switch(userInput)
                {
                    case "1":
                        Console.WriteLine("------------------------------------------------------");
                        Console.WriteLine("You have selected 'Print all names and balances'");
                        Console.WriteLine("------------------------------------------------------");
                        printBalances(AccountsDictionary);
                        break;
                    case "2":
                        Console.WriteLine("------------------------------------------------------");
                        Console.WriteLine("You have selected 'Print all transactions for a specific account'");
                        Console.WriteLine("------------------------------------------------------");
                        printAccountTransactions(AccountsDictionary, transactions);
                        break;
                    case "3":
                        go = false;
                        Console.WriteLine("------------------------------------------------------");
                        Console.WriteLine("Thanks for using SupportBank");
                        Console.WriteLine("------------------------------------------------------");
                        break;
                    default:
                        Console.WriteLine("------------------------------------------------------");
                        Console.WriteLine("You have not selected a valid option. Please try again doofus.");
                        Console.WriteLine("------------------------------------------------------");
                        break;
                }
            }
        }


        // private static Dictionary<string, Account> PopulateAccounts(Dictionary<string, Account> AccountsDictionary, List<Transaction> transactions)
        // {
        //     foreach(var name in AccountsDictionary.Keys)
        //     {
        //         foreach(var transaction in transactions)
        //         {
        //             if (name == transaction.From)
        //             {   
        //                 addTransaction(transaction.To, transaction, AccountsDictionary);
        //                 DateTime date = new DateTime();
        //                 Transaction copy = new Transaction(date, "From", "to", "Narrative", 0);
        //                 copy = transaction;
        //                 copy.Amount = -copy.Amount;

        //                 addTransaction(transaction.From, copy, AccountsDictionary);
        //                 // AccountsDictionary[transaction.To].AccountTransactions.Add(transaction);
        //                 // AccountsDictionary[transaction.To].AccountBalance += transaction.Amount;

        //                 // transaction.Amount = -transaction.Amount;
        //                 // AccountsDictionary[name].AccountTransactions.Add(transaction);
        //                 // AccountsDictionary[name].AccountBalance += transaction.Amount;
                        
                        

        //             }
                    
                    
        //         }
        //     }

        //     return AccountsDictionary;
        // }
    }
}
