using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine("Digite uma opcao");

            Console.WriteLine("1 - SOMA");
            Console.WriteLine("2 - SUBTRAÇÃO");
            Console.WriteLine("3 - DIVISÃO");
            Console.WriteLine("4 - MULTIPLICAÇÃO");
            Console.WriteLine("0 - SAIR\n");

            String op = Console.ReadLine();

            if (!IsValidOption(op))
            {
                Console.WriteLine("Opção inválida.");
                return;
            }

            CConsole.Write("Digite o primeiro número: ");
            string num1 = Console.ReadLine();
            if (!IsValidNumber(num1))
            {
                Console.WriteLine("Número inválido.");
                return;
            }

            Console.Write("Digite o segundo número: ");
            string num2 = Console.ReadLine();
            if (!IsValidNumber(num2))
            {
                Console.WriteLine("Número inválido.");
                return;
            }

            string request = op + ";" + num1 + ";" + num2;
            byte[] requestData = Encoding.ASCII.GetBytes(request);

            string serverIp = "192.168.1.6"; // endereço IP do servidor.
            int serverPort = 55555;

            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(serverIp), serverPort);

            using (Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            {
                try
                {
                    clientSocket.SendTo(requestData, serverEndPoint);

                    byte[] responseData = new byte[1024];
                    int receivedDataLength = clientSocket.Receive(responseData);

                    string response = Encoding.ASCII.GetString(responseData, 0, receivedDataLength);

                    Console.WriteLine(response);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro: " + ex.Message);
                }
            }
        }

        static bool IsValidOption(string option)
        {
            int parsedOption;
            return int.TryParse(option, out parsedOption) && parsedOption >= 0 && parsedOption <= 4;
        }

        static bool IsValidNumber(string number)
        {
            double parsedNumber;
            return double.TryParse(number, out parsedNumber);
        }
    }
}