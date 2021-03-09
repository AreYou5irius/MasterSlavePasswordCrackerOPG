using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualBasic.CompilerServices;
using Slave.model;
using Slave.utilities;


namespace Master
{

    class MasterClass
    {
        private BlockingCollection<List<string>> chunks;
        private List<UserInfo> passwords = new List<UserInfo>();
        private TcpClient connectionSocket;
        private TcpListener server;
        private Stream ns;
        private StreamReader sr;
        private StreamWriter sw;

        public MasterClass()
        {
            //passwords = new List<string>();
            //ReadPassword("passwords.txt");
            passwords = PasswordFileHandler.ReadPasswordFile("passwords.txt");
            CreateChunks();
            chunks.Take();
        }

        public void Listen(int port)
        {

            server = new TcpListener(IPAddress.Loopback, port);  //her laver vi en server der lytter 

            server.Start(); //her starter vi den


            Console.WriteLine("Server is listning on port: " + port); //her fortæller den at serveren lytter og på hvilken port den lytter

            AcceptClient(); //her acceptere den en client/slave(laver en forbindelse)

            var request = sr.ReadLine();  //den modtager en besked fra slave i consolen


            ServeClient(request); //giver beskeden videre til en metode der udfra beskeden vudere hvad den skal sende retur til slaven


            //CheckSingleWord(); maja har tilføjet -----------det er denne der samler de crackede passwords dele

            CloseConnection(); //når den er færdig med at udfører arbejde lukker den forbindelsen til slaven

        }



        //private void ReadPassword(string filename)
        //{   //det er en god ide at lave et break point for at sikre den henter filen korrekt
        //    var fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read); //her laves en forbindelse til en fil hvor der åbnes og laves adgang.
        //    using var fileStreamReader = new StreamReader(fileStream);
        //    while (!fileStreamReader.EndOfStream) //så længe streamReaderen ikke har læst filen færdig så sendes linjer til passwords listen og når den er færdig lukkes forbindelsen 
        //    {
        //        passwords.Add(fileStreamReader.ReadLine());
        //    }
        //    fileStream.Close();
        //    fileStreamReader.Close();
        //}

        private void CreateChunks()
        {
            chunks = new BlockingCollection<List<string>>();

            using FileStream fs = new FileStream("webster-dictionary.txt", FileMode.Open, FileAccess.Read); //her laves en forbindelse til filen "webster-dictionary.txt" hvor den åbner filen og får adgang

            using var dictionary = new StreamReader(fs);  //her laves en dictionary hvor filens indehold bliver gemt i

            var tempList = new List<string>(); //her laves en midligertidig liste


            while (!dictionary.EndOfStream)  //så længe dictionary'en bliver fyldt med info fra filen så..
            {

                if (tempList.Count % 100 == 0)
                {
                    chunks.Add(tempList);
                    tempList = new List<string>();
                }
                tempList.Add(dictionary.ReadLine());

            }
        }

        private void AcceptClient()
        {
            connectionSocket = server.AcceptTcpClient(); //her laver den forbindelsen til clienten/slaven

            Console.WriteLine("Client connected"); //her printer den til consolen at der er en forbindelse

            ns = connectionSocket.GetStream(); //her åbnes en stream så man kan sende og modetage til/fra client/slaven
            sr = new StreamReader(ns);
            sw = new StreamWriter(ns) { AutoFlush = true };
        }

        private void ServeClient(string request)
        {
            while (request != "stop")
            {
                switch (request)
                {
                    case "password":
                        SendPasswords();
                        break;

                    case "chunk":
                        SendChunks();
                        break;
                }

                request = sr.ReadLine();
            }

        }

        //her sender den alle passwords i listen til client/slaven
        private void SendPasswords()
        {
            Console.WriteLine("sending passwords.......");
            foreach (var pass in passwords)
            {
                sw.WriteLine(pass.EntryptedPasswordBase64);
            }

            Console.WriteLine("finished sending passwords");
        }

        //her sender den en chunk (bestemt af create chunk metoden) af listen/blokingcollection chunks men pga "take() husker den hvad den sidst har sendt og starter fra sidste sendt"
        private void SendChunks()
        {
            Console.WriteLine("sending chunks.......");

            foreach (var chunk in chunks.Take())
            {
                sw.WriteLine(chunk);
            }

            Console.WriteLine("finished sending chunks");
        }

        //her lukkes forbindelsen
        private void CloseConnection()
        {
            ns.Close();
            connectionSocket.Close();
        }

    }
}
