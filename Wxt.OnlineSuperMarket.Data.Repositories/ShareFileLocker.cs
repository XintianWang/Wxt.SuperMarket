using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Wxt.OnlineSuperMarket.Data.Repositories
{
    public class ShareFileLocker
    {
        public FileStream LockObj(string locker)
        {
            while (true)
            {
                try
                {
                    var result = File.Open(locker, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    return result;
                }
                catch (Exception e)
                {
#if DEBUG
                    Console.WriteLine(e.Message);
#endif
                    Thread.Sleep(100);
                }
            }
        }

        public void UnlockObj(FileStream stream)
        {
            while (true)
            {
                try
                {
                    stream.Close();
                    return;
                }
                catch (Exception e)
                {
#if DEBUG
                    Console.WriteLine(e.Message);
#endif
                    Thread.Sleep(100);
                }
            }
        }
    }
}
