using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

public class DoubleChatClient
{
    private const int PORT = 9999;

    public static void Main()
    {
        try
        {
            TcpClient client = new TcpClient();
            client.Connect("127.0.0.1", PORT);

            Console.WriteLine("🔵 Kết nối thành công đến Server!");
            Console.WriteLine("🔹 LocalEndPoint: " + client.Client.LocalEndPoint);
            Console.WriteLine("🔹 RemoteEndPoint: " + client.Client.RemoteEndPoint);
            Console.WriteLine("Bắt đầu chat (gõ 'exit' để thoát).");

            NetworkStream stream = client.GetStream();
            StreamReader reader = new StreamReader(stream);
            StreamWriter writer = new StreamWriter(stream);
            writer.AutoFlush = true;

            // Thread đọc tin nhắn từ server
            Thread receiveThread = new Thread(() =>
            {
                try
                {
                    string response;
                    while ((response = reader.ReadLine()) != null)
                    {
                        Console.WriteLine("\n📩 " + response);
                        Console.Write("Bạn: ");
                    }
                }
                catch
                {
                    Console.WriteLine("\n🔴 Mất kết nối với Server.");
                }
            });
            receiveThread.Start();

            // Gửi tin nhắn
            string message;
            while (true)
            {
                Console.Write("Bạn: ");
                message = Console.ReadLine();
                writer.WriteLine(message);
                if (message.ToLower() == "exit")
                    break;
            }

            client.Close();
            Console.WriteLine("👋 Ngắt kết nối.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ Lỗi: " + ex.Message);
        }
    }
}
