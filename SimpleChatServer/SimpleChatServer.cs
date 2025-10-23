using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

public class SimpleChatServer
{
    private const int PORT_NUMBER = 9999;

    public static void Main()
    {
        try
        {
            IPAddress address = IPAddress.Parse("127.0.0.1");
            TcpListener listener = new TcpListener(address, PORT_NUMBER);
            listener.Start();

            Console.WriteLine("🟢 Server started on " + listener.LocalEndpoint);
            Console.WriteLine("Waiting for a connection...");

            Socket socket = listener.AcceptSocket();
            Console.WriteLine("✅ Connection received!");
            Console.WriteLine("🔹 Server LocalEndPoint: " + socket.LocalEndPoint);
            Console.WriteLine("🔹 Client RemoteEndPoint: " + socket.RemoteEndPoint);

            using (var stream = new NetworkStream(socket))
            using (var reader = new StreamReader(stream))
            using (var writer = new StreamWriter(stream))
            {
                writer.AutoFlush = true;

                while (true)
                {
                    string str = reader.ReadLine();
                    if (string.IsNullOrEmpty(str)) continue;

                    Console.WriteLine($"💬 Client gửi: {str}");

                    if (str.Equals("EXIT", StringComparison.OrdinalIgnoreCase))
                    {
                        writer.WriteLine("BYE");
                        Console.WriteLine("👋 Client yêu cầu thoát.");
                        break;
                    }

                    writer.WriteLine("Hello " + str);
                }
            }

            socket.Close();
            listener.Stop();
            Console.WriteLine("🔴 Server stopped.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ Error: " + ex.Message);
        }
    }
}
