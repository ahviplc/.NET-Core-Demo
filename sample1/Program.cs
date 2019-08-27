using System;
using sample1.pojo;

namespace sample1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.Beep(1000,1000);
            Console.WriteLine(1+1);
             MyClass c1 = new MyClass();
            Console.WriteLine($"Hello World! {c1.ReturnMessage()}");
        }
    }
}
