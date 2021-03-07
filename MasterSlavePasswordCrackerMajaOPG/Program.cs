using System;

namespace Master
{
    class Program
    {
        static void Main(string[] args)
        {
            MasterClass master = new MasterClass();
            master.Listen(6789);
            Console.ReadKey();
        }
    }
}
