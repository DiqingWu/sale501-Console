using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    // track of the re-enter rebate data changes.
    class DataBase
    {
        /// <summary>
        /// database values
        /// </summary>
        static private Dictionary<int, Transaction> _transactionDataBase = new Dictionary<int, Transaction>();
        static private int _idCount;
        static private Dictionary<int, Rebate> _rebateTransaction = new Dictionary<int, Rebate>();
        static public DateTime _currentDate = new DateTime(2018, 6, 1);

        /// <summary>
        /// constuctor for database
        /// </summary>
        /// <param name="first"></param>
        /// <param name="last"></param>
        /// <param name="items"></param>
        /// <param name="address"></param>
        /// <param name="cost"></param>
        /// <param name="email"></param>
        /// <param name="id"></param>
        /// <param name="date"></param>
        static public void AddTransaction(string first, string last, string[] items, string address, double[] cost, string email, int id, DateTime date)
        {

            if (!CheckTransactionExist(id))
            {
                _transactionDataBase.Add(id, new Transaction(first, last, items, address, cost, email, date));
            }
            else
            {
                // return something
                //error beacuse id is already exist
            }

        }
        /// <summary>
        /// check keyvalue is exist.
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        static public bool CheckTransactionExist(int b)
        {
            foreach (int s in _transactionDataBase.Keys)
            {
                if (s == b)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// check keyvalue is exist.
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        static public bool CheckRebateExist(int b)
        {
            foreach (int s in _rebateTransaction.Keys)
            {
                if (s == b)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// check and find vaildrebates.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        static public Transaction GetTransaction(int id)
        {
            if (!CheckTransactionExist(id))
                return null;
            return _transactionDataBase[id];
        }
        /// <summary>
        /// CheckTransactionDate
        /// </summary>
        /// <param name="time"></param> compare two value 
        /// <param name="rebate"></param>
        /// <returns></returns> return if this transaction if out of date.
        static public int CheckTransactionDate(DateTime time)//, Transaction transaction)
        {
            //DateTime latestDate = transaction.Date;
            //latestDate.AddDays(15);
            //latestDate.AddMonths(1);
            // add 1month and 15 to see the expaire date.
            //return DateTime.Compare(time, latestDate);
            return DateTime.Compare(time, _currentDate);
        }

        /// <summary>
        /// generate next new id
        /// </summary>
        /// <returns></returns> id with int
        static public int idGenerator()
        {
            _idCount++;
            return _idCount; 
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>return format of id ex: #0042
        static public string getIdFormat(int id)
        {
            if (id < 10)
                return "#000" + id;
            else if (id < 100)
                return "#00" + id;
            else if (id < 1000)
                return "#0" + id;
            else
                return "#" + id;
        }

        static public string GetItemCostList(int id)
        {
            if (!CheckTransactionExist(id))
                return "Id is not exist.";
            string output = "";
            Transaction data = GetTransaction(id);
            string[] items = data.Items;
            double[] costs = data.Costs;
            if (items.Length != costs.Length)
                return "Data Implement incorrcet.";
            for (int i=0;i<costs.Length;i++)
            {
                if (costs[i] == 0)
                {
                    output += "Item(" + (i + 1) + "): " + items[i] + "\tCost: Returned\n";
                }
                else
                output += "Item(" + (i + 1) + "): " + items[i] + "\tCost: $" + costs[i]+"\n";
            }
            return output;
        }
        
        static public string ReturnItem(int id, string item,DateTime currentDate)
        {
            if (!CheckTransactionExist(id))
                return "Id does not exist.";
            if (CheckTransactionDate(currentDate)<0)//, GetTransaction(id)) < 0)
            {
                return "Date expired, it is too late to return item, need to be in 1 month and 15 days.";
            }

            Transaction data = GetTransaction(id);
            int count=0;
            foreach(string s in data.Items)
            {
                if (s == item)
                {
                    data.Costs[count] = 0;
                    return "Item returned successfully.";
                }
                count++;
            }

            return "Item does not exist.";
        }

         static public string EnterRebate(int id,double rebate,DateTime date)
        {
            if (!CheckTransactionExist(id))
                return "Id is not exist.";
            if (CheckRebateExist(id))
            {
                _rebateTransaction[id].Date = date;
                _rebateTransaction[id].Off = rebate;
                return "Rebate is already exist, value is replaced by new rebate";

            }   
            _rebateTransaction.Add(id, new Rebate(date,rebate));
            return "Rebate Added successfully";
        }
        static private int GetTotalCost(int id)
        {
            Transaction data = GetTransaction(id);
            int total = 0;
            foreach (int i in data.Costs)
                total += i;
            return total;
        }

        static public string GenerateRebateCheck()
        {
            
            double off;
            Transaction data;
            string output = "";
            foreach (int id in _rebateTransaction.Keys)
            {
                data = GetTransaction(id);
                if (CheckTransactionDate(_rebateTransaction[id].Date)<0)//,data) < 0)// check out of date or not
                {
                    output += "Id: " + getIdFormat(id) + " Rebate is out of dates.\n\n";

                }
                else
                {
                    
                    off = 1 - _rebateTransaction[id].Off / 100;
                    output += "Transaction ID: " + getIdFormat(id) + "\nName: " + data.FirstName + " " + data.LastName + "\nAddress: " + data.Address + "\nList: \n" + GetItemCostList(id) + "Total Cost: $" + GetTotalCost(id) + "\nAfter Apply Rebate: $" + (GetTotalCost(id) * off) + "\nCheck is being mail to the address also Email to " + data.Email+"\n";
                }
                

            }
            return output;
        }
    }
}
