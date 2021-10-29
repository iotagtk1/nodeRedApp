using System;

namespace nodeRedApp
{
    class Program
    {
        static void Main(string[] args)
        {

            foreach (string str in args)
            {
                Console.WriteLine("引数　" + str);
            }
            
        }
    }
}