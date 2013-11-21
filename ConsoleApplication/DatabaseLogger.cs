using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication
{
    class DatabaseLogger : ILogger
    {
        public DatabaseLogger()
        {

        }
        public void Write(string msg)
        {
            Console.WriteLine(string.Format("'{0}' has been written to database",msg));
        }
    }
}
