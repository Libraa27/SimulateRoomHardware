# HỆ THỐNG MÔ PHỎNG GIÁM SÁT ĐIỆN NĂNG PHÒNG TRỌ THÔNG MINH (IoT-MQTT)

Dự án bài tập nhóm xây dựng hệ thống mô phỏng IoT giám sát công suất tiêu thụ điện và trạng thái thiết bị (Điều hòa) theo thời gian thực của các phòng trọ dựa trên giao thức MQTT (v5) và cấu trúc dữ liệu JSON.

## 👥 THÀNH VIÊN VÀ CHI TIẾT PHÂN CÔNG CÔNG VIỆC

Nhóm chúng em chủ động phân chia dự án thành 2 mảng rõ rệt: **Xử lý Hạ tầng Core bên dưới (Backend)** và **Xây dựng Ứng dụng quản lý đầu cuối (Application & Documentation)** để đảm bảo khối lượng công việc đồng đều.

### 1. Bạn 1 (GitHub: @Libraa27) - Đảm nhận: Backend & Core Logic
* **Thiết lập hạ tầng kết nối:** Nghiên cứu và cài đặt thư viện phần mềm `MQTTnet v5` qua NuGet.
* **Xây dựng Trạm phát dữ liệu phần cứng (`SimulateRoomHardware`):**
  * Thiết kế luồng truyền tải dữ liệu đa phòng (Phòng 101, 102, 103) lên các kênh (Topics) tương ứng.
  * Viết thuật toán sinh số thông minh: Tự động nhận diện khung giờ hệ thống để giả lập công suất thực tế (Giờ cao điểm ban đêm tự động tăng tải tiêu thụ từ 800W-2000W và bật điều hòa; Giờ thấp điểm ban ngày tự động giảm tải).
* **Xây dựng Trạm thu Console (`MQTT_Receiver`):** Cấu hình cơ chế lắng nghe, hứng trọn gói tin JSON từ Cloud Broker trực tuyến (`broker.emqx.io`) và bóc tách dữ liệu real-time.

### 2. Anh (GitHub: @Tên_Tài_Khoản_Của_Anh) - Đảm nhận: Mobile App, Frontend & Documentation
* **Xây dựng Ứng dụng Di động (`MQTT_Mobile_App`):**
  * Thiết kế cấu trúc giao diện người dùng (UI) trên nền tảng chéo `.NET MAUI` để hiển thị trực quan dữ liệu phòng trọ dưới dạng các thẻ (Frames) theo dõi.
  * Viết logic xử lý cập nhật giao diện không đồng bộ (`MainThread`) khi có gói tin MQTT đổ về để đảm bảo ứng dụng chạy mượt mà, không giật lag.
* **Tối ưu hóa & Nghiệm thu:** Phối hợp kiểm thử kết nối chéo giữa hệ thống Console phát và Mobile thu để sửa các lỗi đồng bộ cú pháp payload dữ liệu.
* **Tài liệu & Báo cáo:** Quản lý kho lưu trữ mã nguồn trên GitHub, viết tài liệu hướng dẫn (`README.md`) và hoàn thiện báo cáo/slide thuyết trình hệ thống trước giảng viên.

---

## 🚀 KIẾN TRÚC HỆ THỐNG
* **Data Source:** Bộ giả lập Console C# (`SimulateRoomHardware`).
* **Message Broker:** EMQX Cloud Broker (`broker.emqx.io:1883`).
* **Format:** JSON String (RoomNumber, TotalPower, IsAcOn, Timestamp).
* **End-user Monitoring:** .NET MAUI Mobile App (`MQTT_Mobile_App`).
