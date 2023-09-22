using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class MainClass
{
    public static void Main()
    {   
        int receivedDataLength;
        byte[] data = new byte[1024];

        IPEndPoint ip = new IPEndPoint(IPAddress.Any, 55555);

        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        socket.Bind(ip);

        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 55555);
        EndPoint Remote = (EndPoint)(sender);

        while (true)
        {
            int i = 0;

            data = new byte[1024];
            receivedDataLength = socket.ReceiveFrom(data, ref Remote);

            string recv = Encoding.ASCII.GetString(data, 0, receivedDataLength);
           
            if (recv.StartsWith("1")) {
                string[] parts = recv.Split(';');

                if (parts.Length == 3) {

                    string op = parts[0];
                    double num1 = double.Parse(parts[1]);
                    double num2 = double.Parse(parts[2]);
                    double result = 0;    

                    if (double.TryParse(parts[1], out num1) && double.TryParse(parts[2], out num2))
                    {

                        switch (op)
                        {
                            case "1": // SOMA
                                result = num1 + num2;
                                break;
                            case "2": // SUBTRAÇÃO
                                result = num1 - num2;
                                break;
                            case "3": // DIVISÃO
                                if (num2 != 0)
                                    result = num1 / num2;
                                else
                                    result = 0; // Tratamento de divisão por zero
                                break;
                            case "4": // MULTIPLICAÇÃO
                                result = num1 * num2;
                                break;
                            default:
                                break;
                        }
                        
                        string response = "Resultado: " + result;
                        byte[] responseData = Encoding.ASCII.GetBytes(response);
                        socket.SendTo(responseData, responseData.Length, SocketFlags.None, Remote);

                    }
                    else
                    {
                        string response = "Erro: Valores inválidos.";
                        byte[] responseData = Encoding.ASCII.GetBytes(response);
                        socket.SendTo(responseData, responseData.Length, SocketFlags.None, Remote);
                    }
                }
                else
                {
                    string response = "Erro: Operação não suportada.";
                    byte[] responseData = Encoding.ASCII.GetBytes(response);
                    socket.SendTo(responseData, responseData.Length, SocketFlags.None, Remote);
                }
            }
        }     
    }
}