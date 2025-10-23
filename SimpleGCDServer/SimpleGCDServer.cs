using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

public class SimpleGCDServer
{
    private const int PORT = 9999;

    public static void Main()
    {
        try
        {
            TcpListener listener = new TcpListener(IPAddress.Parse("127.0.0.1"), PORT);
            listener.Start();

            Console.WriteLine("🟢 Server đang chạy tại: " + listener.LocalEndpoint);
            Console.WriteLine("Đang chờ Client kết nối...");

            Socket socket = listener.AcceptSocket();
            Console.WriteLine("✅ Client đã kết nối từ " + socket.RemoteEndPoint);

            using (var stream = new NetworkStream(socket))
            using (var reader = new StreamReader(stream))
            using (var writer = new StreamWriter(stream))
            {
                writer.AutoFlush = true;

                while (true)
                {
                    string message = reader.ReadLine();
                    if (message == null) continue;

                    if (message.ToLower() == "exit")
                    {
                        writer.WriteLine("BYE");
                        Console.WriteLine("👋 Client ngắt kết nối.");
                        break;
                    }

                    // Dữ liệu client gửi lên có dạng: "a b"
                    string[] parts = message.Split(' ');
                    if (parts.Length != 2 ||
                        !int.TryParse(parts[0], out int a) ||
                        !int.TryParse(parts[1], out int b))
                    {
                        writer.WriteLine("❌ Dữ liệu không hợp lệ. Hãy nhập 2 số nguyên.");
                        continue;
                    }

                    int gcd = GCD(a, b);
                    Console.WriteLine($"📥 Nhận từ client: {a}, {b} -> USCLN = {gcd}");
                    writer.WriteLine($"USCLN({a}, {b}) = {gcd}");
                }
            }

            socket.Close();
            listener.Stop();
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ Lỗi: " + ex.Message);
        }
    }

    // Hàm tính USCLN (Euclid)
    static int GCD(int a, int b)
    {
        while (b != 0)
        {
            int temp = b;
            b = a % b;
            a = temp;
        }
        return Math.Abs(a);
    }
}
