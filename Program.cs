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

            // List <Account> Accounts;
            printList (transactions);
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
                        transactionelements[1].PadRight(20),
                        transactionelements[2].PadRight(20),
                        transactionelements[3].PadRight(20),
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
                    .WriteLine($"Date: {item.Date.ToString("d").PadRight(20)} From: {item.From.PadRight(20)} To:{item.To.PadRight(20)} Narrative: {item.Narrative.PadRight(20)} Amount: £{item.Amount}");
            }
        }
    }
}
