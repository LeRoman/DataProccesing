using System;
using System.Collections.Generic;
using System.Text;


namespace DataProcessing
{
    class Transaction
    { 
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string address { get; set; }
        public decimal payment { get; set; }
        public DateTime date { get; set; }
        public long account_number { get; set; }
        public string service { get; set; }
    }
}
