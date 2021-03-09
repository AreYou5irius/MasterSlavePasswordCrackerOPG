using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using Slave.model;

namespace Slave
{
    class Program
    {
        static void Main(string[] args)
        {
            //her laves en liste der skal bruges til at indeholde chunks
            List<string> chunkList = new List<string>();
            //her laves en liste der skal bruges til at indeholde passwords
            List<UserInfo> passwordList = new List<UserInfo>();

            //clienten/slaven prøver at connecte til master via ip og port nr
            TcpClient clientSocket = new TcpClient("localhost", 6789);


            Stream ns = clientSocket.GetStream();
            StreamReader sr = new StreamReader(ns);
            StreamWriter sw = new StreamWriter(ns) { AutoFlush = true };


            sw.WriteLine("password");
            while (!sr.EndOfStream)
            {
                List<string> stringPasswords = new List<string>();
                stringPasswords.Add(sr.ReadLine());

                //passwordList.Add(sr.ReadLine());

                //Console.WriteLine(passwordList);
                Console.WriteLine(stringPasswords);

                //passwordList = stringPasswords.ConvertAll("string", UserInfo); //skal konvetere fra en string list til en userInfo list

            }


            sw.WriteLine("chunk");
            while (!sr.EndOfStream)
            {
                chunkList.Add(sr.ReadLine());

            }
            foreach (var word in chunkList)
            {
                IEnumerable<UserInfoClearText> crackedPassword = Cracking.CheckWordWithVariations(word, passwordList);

                sw.WriteLine(crackedPassword);

                Console.WriteLine(word);
            }

            sw.WriteLine("stop");


            Console.ReadKey();

            ns.Close();
            clientSocket.Close();
        }
    }
}
