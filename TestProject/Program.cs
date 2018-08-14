using System;
using System.Data.Odbc;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            var testApp = new TestApp(Console.WriteLine, Console.ReadLine);
            testApp.Start();
        }
    }
}
