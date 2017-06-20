using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PS1_UDP_Client

{
    class Program
    {
        IPAddress IP;
        String stringIP;

        static void Main(string[] args)
        {
            Program program = new Program(); // objekt klasy program
            string choice;
            UdpClient udpclient = new UdpClient();

            Console.WriteLine("Co chcesz zrobic?");
            Console.WriteLine("1. Rozsylanie multicast.");
            Console.WriteLine("2. Rozsylasnie broadcast");
            Console.Write("Dowolny inny wybór spowoduje wyjście z programu.\n\nTwoj wybor: ");
            choice = Console.ReadLine();

            if (choice.Equals("1"))
            {
                program.isValidIP();
                udpclient.JoinMulticastGroup(program.IP);

            }
            else if (choice.Equals("2"))
            {
                program.IP = IPAddress.Parse("255.255.255.255");

            }

            else
            {
                System.Environment.Exit(-1);
            }
            
            
            IPEndPoint remoteep = new IPEndPoint(program.IP, 2222);
            Byte[] buffer = null;
            string message;

            while (true)
            {
                Console.Write("Wiadomosc do wyslania: ");
                buffer = Encoding.Unicode.GetBytes(message=Console.ReadLine());
                if (String.IsNullOrEmpty(message))
                {
                    break;
                }
                udpclient.Send(buffer, buffer.Length, remoteep);
            }


            
        }
        public void isValidIP()
        {
            uint intIP = 0;
            uint intMinIP = 0;
            uint intMaxIP = 0;
            Console.Write("Podaj adres multicastowy: ");
            stringIP = Console.ReadLine();                  // ip w stringu


            try
            {
                IP = IPAddress.Parse(stringIP);                 // ip 

                byte[] bytes = IP.GetAddressBytes();
                Array.Reverse(bytes); // flip big-endian(network order) to little-endian
                intIP = BitConverter.ToUInt32(bytes, 0);

                IPAddress minIP = IPAddress.Parse("224.0.0.0");
                bytes = minIP.GetAddressBytes();
                Array.Reverse(bytes);
                intMinIP = BitConverter.ToUInt32(bytes, 0);

                IPAddress maxIP = IPAddress.Parse("239.255.255.255");
                bytes = maxIP.GetAddressBytes();
                Array.Reverse(bytes);
                intMaxIP = BitConverter.ToUInt32(bytes, 0);

                if (intIP >= intMinIP && intIP <= intMaxIP)
                {

                }

                else
                {
                    Console.WriteLine("IP jest spoza zakresu adresow multicastowych.\n");
                    isValidIP();
                }

            }
            catch (System.FormatException e)
            {
                Console.WriteLine("Podana wartosc nie jest poprawnym adresem IP\n");
                isValidIP();
            }



        }
    }
}
