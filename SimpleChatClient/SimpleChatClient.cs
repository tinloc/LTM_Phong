using System;
using System.IO;
using System.Net.Sockets;

public class SimpleChatClient
{
    private const int PORT_NUMBER = 9999;

    public static void Main()
    {
        try
        {
            TcpClient client = new TcpClient();
            client.Connect("127.0.0.1", PORT_NUMBER);

            Console.WriteLine("🔵 Connected to SimpleChatServer.");

            // Hiển thị LocalEndPoint và RemoteEndPoint của client
            Console.WriteLine("🔹 Client LocalEndPoint: " + client.Client.LocalEndPoint);
            Console.WriteLine("🔹 Server RemoteEndPoint: " + client.Client.RemoteEndPoint);

            using (var stream = client.GetStream())
            using (var reader = new StreamReader(stream))
            using (var writer = new StreamWriter(stream))
            {
                writer.AutoFlush = true;

                while (true)
                {
                    Console.Write("Enter your name: ");
                    string str = Console.ReadLine();

                    writer.WriteLine(str);
                    string response = reader.ReadLine();
                    Console.WriteLine($"📩 Server: {response}");

                    if (response.Equals("BYE", StringComparison.OrdinalIgnoreCase))
                        break;
                }
            }

            client.Close();
            Console.WriteLine("🔴 Disconnected from server.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ Error: " + ex.Message);
        }
    }
}
