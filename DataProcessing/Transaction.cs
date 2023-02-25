using Newtonsoft.Json;
using System;
using System.Globalization;

namespace DataProcessing
{
    [JsonObject(MemberSerialization.OptIn)]
    class Transaction
    {
        [JsonIgnore]
        public string FirstName { get; set; }
        [JsonIgnore]
        public string LastName { get; set; }

        [JsonProperty("name")]
        public string FullName { get; set; }

        [JsonIgnore]
        public string City { get; set; }

        [JsonIgnore]
        public string Address { get; set; }

        [JsonProperty("payment")]
        public decimal Payment { get; set; }



        public DateTime Date { get; set; }

        [JsonProperty("date")]
        public string TimeForJson { get; set; }

        [JsonProperty("account_number")]
        public long AccountNumber { get; set; }

        [JsonIgnore]
        public string Service { get; set; }

        public Transaction(string[] array)
        {
            IFormatProvider provider = CultureInfo.InvariantCulture;
            FirstName = array[0];
            LastName = array[1];
            City = GetCity(array[2]);
            Address = GetAdress(array[2]);
            Payment = Convert.ToDecimal(array[3], provider);
            Date = ConvertStringToDate(array[4]);
            AccountNumber = Convert.ToInt64(array[5]);
            Service = array[6];
            FullName = LastName + ' ' + FirstName;
            TimeForJson = Date.ToString("d");
        }

        private string GetAdress(string v)
        {
            return v.Substring(v.IndexOf(',') + 1).TrimEnd('”');
        }

        private string GetCity(string v)
        {
            return v.Substring(0, v.IndexOf(',')).TrimStart('“');
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
