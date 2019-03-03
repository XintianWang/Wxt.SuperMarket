using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wxt.OnlineSuperMarket.Data.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }
        public string TelephoneNumber { get; set; }

        public override string ToString()
        {
            return $"Id = {Id} UserName = {UserName} EmailAddress = {EmailAddress} TelephoneNumber = {TelephoneNumber}";
        }
    }
}
