using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Wxt.OnlineSuperMarket.Test.DebugConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string md5string = Encoding.UTF8.GetString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes("lnw")));
        }
    }
}
