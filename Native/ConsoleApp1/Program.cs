using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("~sadf".Remove(0,1).Split('~').FirstOrDefault().Trim());
            Console.ReadLine();
        }
    }
}
