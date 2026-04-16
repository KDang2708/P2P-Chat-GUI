# 💬 P2P Chat GUI (.NET 8 WinForms)

Dự án ứng dụng chat Peer-to-Peer (P2P) trực tiếp giữa 2 máy tính thông qua mạng LAN hoặc Internet(yêu cầu Port Forwarding), được xây dựng bằng **C# và .NET 8 WinForms**.

## 🚀 Tính năng chính

- Kết nối P2P **thuần túy** thông qua TCP Sockets.
- Có khả năng hoạt động dưới vai trò **Host** (chờ kết nối) hoặc **Client** (chủ động kết nối).
- Giao diện người dùng WinForms đơn giản, dễ sử dụng.
- Nhắn tin theo thời gian thực (Realtime).
- Hoạt động đa luồng bằng `async/await` giúp giao diện luôn mượt mà.

## 🧰 Nền tảng & Công nghệ sử dụng

| Thành phần      | Công cụ / Thư viện                 |
| --------------- | ---------------------------------- |
| Ngôn ngữ        | C# 12.0                            |
| Nền tảng        | .NET 8                             |
| Giao diện       | Windows Forms (WinForms)           |
| Mạng            | System.Net.Sockets (TCP)           |
| Xử lý đồng thời | async / await, Task, NetworkStream |

## 📦 Cài đặt & Chạy ứng dụng

### Yêu cầu hệ thống

- Windows 10 / 11.
- .NET 8.0 Desktop Runtime (nếu chỉ chạy file build).
- Visual Studio 2022 (nếu muốn mở và chạy source code).

### Các bước chạy code

1. **Clone dự án về máy:**

   ```bash
   git clone https://github.com/KDang2708/P2P-Chat-GUI.git
   cd P2P-Chat-GUI
   ```

2. Mở Solution bằng **Visual Studio** (hoặc dùng đường dẫn `Code/P2PChatGUI`).
3. Nhấn **F5** hoặc chọn **Start** để build và chạy ứng dụng.

## 🧪 Hướng dẫn sử dụng

Ứng dụng cho phép chat 1-1 giữa hai thiết bị (hoặc chạy 2 tab trên cùng 1 máy bằng IP vòng lặp `127.0.0.1`).

1. **Máy 1 (Làm Host):**
   - Nhập IP máy bạn (ví dụ: `192.168.1.10` hoặc `127.0.0.1` nếu test cục bộ) và Port muốn mở (VD: `8888`).
   - Nhấn **Host**.
2. **Máy 2 (Làm Client):**
   - Nhập IP và Port tương ứng của Máy 1.
   - Nhấn **Connect**.
3. Sau khi kết nối thành công, cả 2 sẽ nhận được thông báo "**✅ Đã kết nối**". Nút gửi tin nhắn sẽ được bật lên.

## 📁 Cấu trúc dự án

```text
P2P-Chat-GUI/
├── Code/                           # Toàn bộ mã nguồn C#
│   └── P2PChatGUI/
│       ├── ChatNetworkCore.cs      # Core xử lý logic mạng TCP P2P (Socket, Async, Framing)
│       ├── Form1.cs                # Giao diện chính (các nút, TextBox chat)
│       └── P2PChatGUI.csproj       # File project C# (.NET 8)
├── DOCX/                           # Báo cáo dự án
├── Extra/                          # Tài liệu cấu trúc, sơ đồ
├── PPTX/                           # Slide thuyết trình
└── README.md                       # File hiển thị này
```

## ⚠️ Hạn chế hiện tại

- Do sử dụng kết nối TCP thuần túy, để nhắn tin qua Internet người dùng (Host) cần phải mở cổng mạng (Port Forwarding) trên Router.
- Hiện tại chưa hỗ trợ mã hóa E2E, tin nhắn được truyền đi theo dạng bản rõ.
- Chỉ hỗ trợ kết nối 1-1, chưa có các tính năng lưu lại nhóm hoặc lịch sử hội thoại dài hạn.
