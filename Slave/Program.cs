using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

namespace Slave
{
    class Program
    {
        static void Main(string[] args)
        {
            //her laves en liste der skal bruges til at indeholde chunks
            List<string> chunkList = new List<string>();
            //her laves en liste der skal bruges til at indeholde passwords
            List<string> passwordList = new List<string>();

            //clienten/slaven prøver at connecte til master via ip og port nr
            TcpClient clientSocket = new TcpClient("localhost", 6789);


            Stream ns = clientSocket.GetStream();
            StreamReader sr = new StreamReader(ns);
            StreamWriter sw = new StreamWriter(ns) { AutoFlush = true };


            sw.WriteLine("password");
            sw.WriteLine("chunk");
            //clint need logic to Se video--------------------------------------!!!!
            sw.WriteLine("stop");

            List<string> data = new List<string>();

            while (!sr.EndOfStream)
            {
                data.Add(sr.ReadLine());

            }

            foreach (var word in data)
            {
                Console.WriteLine(word);
            }

            //string responce = sr.ReadLine();

            //while (!sr.EndOfStream)
            //{
            //    //if (chunk)
            //    //{
            //    chunkList.Add(sr.ReadLine());
            //    //}
            //    //else if (password)
            //    //{
            //    passwordList.Add(sr.ReadLine());
            //    //}
            //    //else
            //    //{
            //    //Console.WriteLine("nothing was addet to any list");
            //    //}
            //}

            //chunkList.Add(sr.ReadLine());
            //passwordList.Add(sr.ReadLine());



            //Console.WriteLine("responce from slave: " + responce);
            Console.ReadKey();

            ns.Close();
            clientSocket.Close();
        }
    }
}
