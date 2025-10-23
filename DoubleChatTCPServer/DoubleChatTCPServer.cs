using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class DoubleChatServer
{
    private const int PORT = 9999;
    private static TcpClient client1;
    private static TcpClient client2;

    public static void Main()
    {
        TcpListener listener = new TcpListener(IPAddress.Any, PORT);
        listener.Start();
        Console.WriteLine("🟢 Server đang lắng nghe trên cổng " + PORT);

        Console.WriteLine("⏳ Đang chờ Client 1 kết nối...");
        client1 = listener.AcceptTcpClient();
        Console.WriteLine("✅ Client 1 đã kết nối từ " + client1.Client.RemoteEndPoint);

        Console.WriteLine("⏳ Đang chờ Client 2 kết nối...");
        client2 = listener.AcceptTcpClient();
        Console.WriteLine("✅ Client 2 đã kết nối từ " + client2.Client.RemoteEndPoint);

        Console.WriteLine("🚀 Hai client đã kết nối. Bắt đầu chat qua server...");

        Thread t1 = new Thread(() => ForwardMessage(client1, client2, "Client 1"));
        Thread t2 = new Thread(() => ForwardMessage(client2, client1, "Client 2"));

        t1.Start();
        t2.Start();

        t1.Join();
        t2.Join();

        client1.Close();
        client2.Close();
        listener.Stop();
        Console.WriteLine("🔴 Server đã dừng.");
    }

    static void ForwardMessage(TcpClient fromClient, TcpClient toClient, string senderName)
    {
        try
        {
            using (var reader = new StreamReader(fromClient.GetStream()))
            using (var writerToOther = new StreamWriter(toClient.GetStream()))
            {
                writerToOther.AutoFlush = true;
                string msg;
                while ((msg = reader.ReadLine()) != null)
                {
                    if (msg.ToLower() == "exit")
                    {
                        writerToOther.WriteLine($"🔴 {senderName} đã thoát.");
                        break;
                    }
                    Console.WriteLine($"💬 {senderName}: {msg}");
                    writerToOther.WriteLine($"{senderName}: {msg}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠️ Lỗi từ {senderName}: {ex.Message}");
        }
    }
}
