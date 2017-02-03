using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hosts.Startup;
using System.Reflection;

namespace Hosts.Startup
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
