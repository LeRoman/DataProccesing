using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace DataProcessing
{
    class Transaction
    {
        [Required]
        [StringLength(10, MinimumLength = 3)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 3)]
        public string LastName { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Address { get; set; }

        [Required]
        public decimal Payment { get; set; }

        [Required]
        [RegularExpression(@"\d{4}-\d{2}-\d{2}")]
        public DateTime Date { get; set; }

        [Required]
        [RegularExpression(@"\d+\.\d")]
        public long AccountNumber { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Service { get; set; }

        public Transaction(string[] array)
        {
            FirstName = array[0];
            LastName = array[1];
            Address = array[2];
            Payment = Convert.ToDecimal(array[3]);
            Date = ConvertStringToDate(array[4]);
            AccountNumber = Convert.ToInt64(array[5]);
            Service = array[6];
        }

        private DateTime ConvertStringToDate(string v)
        {
            int day = Convert.ToInt32(v.Substring(5, 2));
            int month = Convert.ToInt32(v.Substring(8, 2));
            int year = Convert.ToInt32(v.Substring(0, 4));

            return new DateTime(year, month, day);
        }
    }

}
