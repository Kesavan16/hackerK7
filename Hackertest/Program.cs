using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendedCode
{
    public class AccountBalance
    {
        private readonly object balanceLock = new object();
        private decimal balance;

        public AccountBalance(decimal initialBalance) => balance = initialBalance;

        public decimal Debit(decimal a)
        {
           
                //This function is written to debit the passed amount from the account balance.
                if (a < 0)
                {
                   
                    throw new ArgumentOutOfRangeException(nameof(a), "The debit amount cannot be negative.");
                }

            decimal appliedAmount = 0;
            lock (balanceLock)

            {
                
                    if (balance >= a)
                    {
                        balance -= a;
                        appliedAmount = a;
                    }
            }
            return appliedAmount;
        }

        public void Credit(decimal amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "The credit amount cannot be negative.");
            }

            lock (balanceLock)
            {
                balance += amount;
            }
        }

        public decimal GetBalance()
        {
            lock (balanceLock)
            {
                return balance;
            }
        }
    }


    class AccountTest
    {
        static async Task Main()
        {
           var account = new AccountBalance(1000);
            var tasks = new Task[100];
            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = Task.Run(() => Update(account));
            }
            await Task.WhenAll(tasks);
            Console.WriteLine($"Account's balance is {account.GetBalance()}");

            Console.ReadLine();
            // Output:
            // Account's balance is 2000
        }

        static void Update(AccountBalance account)
        {
            //decimal[] amounts = { 0, 2, -3, 6, -2, -1, 8, -5, 11, -6 };
            decimal[] amounts = { 2, 6, 11 };
            foreach (var amount in amounts)
            {
                if (amount >= 0)
                {
                    account.Credit(amount);
                }
                else
                {
                    //account.Debit(Math.Abs(amount));
                    account.Debit((amount));
                }
            }
        }
    }

}