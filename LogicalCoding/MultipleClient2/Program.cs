﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatServer
{
    class Client
    {

        static log4net.ILog log = log4net.LogManager.GetLogger(typeof(Client));

        static int ServerPort = 1234;
        static Socket sck;
        static string name = "";
        static string msg = "";
        public static void Send()
        {
            while (true)
            {

                msg = Console.ReadLine();
                try
                {

                    byte[] sData = Encoding.Default.GetBytes(name + "#" + msg);
                    sck.Send(sData, 0, sData.Length, 0);
                }
                catch (Exception e)
                {
                    log.Error(e);
                }
            }
        }

        public static void Read()
        {

            while (true)
            {
                if (msg.Equals("logout"))
                    return;
                try
                {
                    // read the message sent to this client
                    byte[] Buffer = new byte[255];
                    int rec = sck.Receive(Buffer, 0, Buffer.Length, 0);
                    Array.Resize(ref Buffer, rec);
                    string dis = Encoding.Default.GetString(Buffer);
                    Console.WriteLine(dis);
                }
                catch (Exception e)
                {

                    log.Error(e);
                }
            }
        }

        static void Connection()
        {
            sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Console.Write("Enter Name : ");
            name = Console.ReadLine();
            Console.WriteLine("Connecting.....");

            IPEndPoint ed = new IPEndPoint(IPAddress.Parse("127.0.0.1"), ServerPort);

            try
            {
                sck.Connect(ed);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            byte[] send = Encoding.Default.GetBytes(name);
            sck.Send(send, 0, send.Length, 0);
            byte[] Buf = new byte[255];
            int rec = sck.Receive(Buf, 0, Buf.Length, 0);
            Array.Resize(ref Buf, rec);
            string dis = Encoding.Default.GetString(Buf);
            Console.WriteLine(dis + " Connected.....");
            Console.Write("Enter Message U want To Connect :");
            string conn = Console.ReadLine();
            send = Encoding.Default.GetBytes(conn);
            sck.Send(send, 0, send.Length, 0);
        }


        static void Main(string[] args)
        {
            log4net.Config.BasicConfigurator.Configure();
            try
            {
                Connection();
                Thread sendMessage = new Thread(new ThreadStart(Send));
                Thread readMessage = new Thread(new ThreadStart(Read));

                sendMessage.Start();
                readMessage.Start();
            }
            catch (Exception ex)
            { log.Error(ex.Message); }
        }
    }
}