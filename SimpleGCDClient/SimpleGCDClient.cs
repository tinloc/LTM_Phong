using System;
using System.IO;
using System.Net.Sockets;

public class SimpleGCDClient
{
    private const int PORT = 9999;

    public static void Main()
    {
        try
        {
            TcpClient client = new TcpClient();
            client.Connect("127.0.0.1", PORT);

            Console.WriteLine("🔵 Đã kết nối tới Server!");
            Console.WriteLine("🔹 Client LocalEndPoint: " + client.Client.LocalEndPoint);
            Console.WriteLine("🔹 Server RemoteEndPoint: " + client.Client.RemoteEndPoint);
            Console.WriteLine("Nhập hai số nguyên cách nhau bởi khoảng trắng (ví dụ: 18 24)");
            Console.WriteLine("Gõ 'exit' để thoát.\n");

            using (var stream = client.GetStream())
            using (var reader = new StreamReader(stream))
            using (var writer = new StreamWriter(stream))
            {
                writer.AutoFlush = true;

                while (true)
                {
                    Console.Write("👉 Nhập: ");
                    string input = Console.ReadLine();

                    writer.WriteLine(input);

                    string response = reader.ReadLine();
                    Console.WriteLine("📩 Server trả về: " + response);

                    if (input.ToLower() == "exit")
                        break;
                }
            }

            client.Close();
            Console.WriteLine("🔴 Ngắt kết nối khỏi Server.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ Lỗi: " + ex.Message);
        }
    }
}
