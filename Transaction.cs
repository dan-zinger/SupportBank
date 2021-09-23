using System;
using System.Collections.Generic;

namespace SupportBank
{
    class Transaction
    {
        public DateTime Date { get; set; }

        public String From { get; set; }

        public String To { get; set; }

        public String Narrative { get; set; }

        public Decimal Amount { get; set; }

        public Transaction(
            DateTime transactionDate,
            string from,
            string to,
            string narrative,
            decimal amount
        )
        {
            Date = transactionDate;
            From = from;
            To = to;
            Narrative = narrative;
            Amount = amount;
        }
    }
}
