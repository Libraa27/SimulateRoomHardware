using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MQTTnet;

namespace SimulateRoomHardware
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.Title = "MÁY PHÁT DỮ LIỆU TỰ ĐỘNG - HỆ THỐNG PHÒNG TRỌ";

            var factory = new MqttClientFactory();
            using (var mqttClient = factory.CreateMqttClient())
            {
                // 1. Cấu hình kết nối đến trạm trung chuyển trực tuyến EMQX
                var options = new MqttClientOptionsBuilder()
                    .WithTcpServer("broker.emqx.io", 1883)
                    .Build();

                Console.WriteLine("=== ĐANG KẾT NỐI ĐẾN TRẠM BROKER MQTT... ===");
                await mqttClient.ConnectAsync(options);
                Console.WriteLine("=== KẾT NỐI THÀNH CÔNG! BẮT ĐẦU ĐẨY DỮ LIỆU... ===\n");

                var random = new Random();

                // Danh sách các phòng trọ cần giả lập dữ liệu
                string[] rooms = { "101", "102", "103" };

                while (true)
                {
                    // Lấy giờ hiện tại của hệ thống để làm căn cứ sinh số logic
                    int currentHour = DateTime.Now.Hour;
                    bool isNight = (currentHour >= 18 || currentHour < 6);

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"--- Khung giờ hiện tại: {DateTime.Now:HH:mm:ss} ({(isNight ? "Cao điểm - Ban đêm" : "Thấp điểm - Ban ngày")}) ---");
                    Console.ResetColor();

                    // Vòng lặp chạy qua từng phòng để phát dữ liệu
                    foreach (var room in rooms)
                    {
                        double power;
                        bool isAcOn;

                        if (isNight)
                        {
                            // Ban đêm: Tiêu thụ điện nhiều (800W - 2000W)
                            power = Math.Round(random.NextDouble() * (2000 - 800) + 800, 1);
                            isAcOn = random.Next(100) < 80; // 80% là bật điều hòa
                        }
                        else
                        {
                            // Ban ngày: Tiêu thụ điện ít (50W - 250W)
                            power = Math.Round(random.NextDouble() * (250 - 50) + 50, 1);
                            isAcOn = random.Next(100) < 15; // Chỉ có 15% bật điều hòa
                        }

                        // Đóng gói dữ liệu thành JSON
                        var roomData = new
                        {
                            RoomNumber = room,
                            TotalPower = power,
                            IsAcOn = isAcOn,
                            Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                        };

                        string jsonPayload = JsonSerializer.Serialize(roomData);

                        // Phát dữ liệu lên tần số (Topic) riêng biệt của từng phòng: pms/rooms/101, pms/rooms/102...
                        string topic = $"pms/rooms/{room}";

                        var message = new MqttApplicationMessageBuilder()
                            .WithTopic(topic)
                            .WithPayload(Encoding.UTF8.GetBytes(jsonPayload))
                            .Build();

                        await mqttClient.PublishAsync(message);
                        Console.WriteLine($"[Phòng {room}] -> Gửi tới `{topic}`: {jsonPayload}");
                    }

                    Console.WriteLine("----------------------------------------------------------------------\n");

                    // Cứ sau 5 giây lại cập nhật dữ liệu của tất cả các phòng một lần
                    await Task.Delay(1000);
                }
            }
        }
    }
}