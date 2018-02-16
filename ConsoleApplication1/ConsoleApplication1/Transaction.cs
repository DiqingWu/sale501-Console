using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Transaction
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string[] Items { get; set; }
        public double[] Costs { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public DateTime Date { get; set; }
        /*
        // Program Doesn't need over load yet

        public Tansaction(string first,string last,string[] items,int[] cost,string email,int id,DateTime date)
        {
            FirstName = first;
            LastName = last;
            Items = items;
            Costs = cost;
            Email = email;
            ID = id;
            Date = date;

        }
        */
        public Transaction(string first, string last, string[] items, string address, double[] cost, string email, DateTime date)
        {
            FirstName = first;
            LastName = last;
            Items = items;
            Costs = cost;
            Email = email;
            Date = date;
            Address = address;
        }
    }
}
