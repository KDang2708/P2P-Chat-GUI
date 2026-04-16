# ?? P2P Chat GUI (.NET 8 WinForms)

D? án ?ng d?ng chat Peer-to-Peer (P2P) tr?c ti?p gi?a 2 máy tính thông qua m?ng LAN ho?c Internet, du?c xây d?ng b?ng **C# và .NET 8 WinForms**.

## ? Tính nang chính

- K?t n?i P2P **thu?n túy** thông qua TCP Sockets.
- Có kh? nang ho?t d?ng du?i vai trò **Host** (ch? k?t n?i) ho?c **Client** (ch? d?ng k?t n?i).
- Giao di?n ngu?i dùng WinForms don gi?n, d? s? d?ng.
- Nh?n tin theo th?i gian th?c (Realtime), ch?ng d?t gãy gói tin b?ng k? thu?t **Length-Prefixed Framing**.
- Ho?t d?ng da lu?ng b?ng sync/await giúp giao di?n luôn mu?t mà.
- T? d?ng ch?n s? c? t? k?t n?i (Loopback) và có trang b? gi?i h?n kích c? tin nh?n d? b?o m?t (t?i da 10MB).

## ?? N?n t?ng & Công ngh? s? d?ng

| Thành ph?n       | Công c? / Thu vi?n                  |
|------------------|-------------------------------------|
| Ngôn ng?         | C# 12.0                             |
| N?n t?ng         | .NET 8                              |
| Giao di?n        | Windows Forms (WinForms)            |
| M?ng             | System.Net.Sockets (TCP)            |
| X? lý d?ng th?i  | async / await, Task, NetworkStream  |

## ?? Cài d?t & Ch?y ?ng d?ng

### Yêu c?u h? th?ng

- Windows 10 / 11.
- .NET 8.0 Desktop Runtime (N?u ch? ch?y file build).
- Visual Studio 2022 (N?u mu?n m? và ch?y source code).

### Các bu?c ch?y code

1. **Clone d? án v? máy:**
   \\\ash
   git clone https://github.com/KDang2708/P2P-Chat-GUI.git
   cd P2P-Chat-GUI
   \\\

2. M? Solution b?ng **Visual Studio** (ho?c dùng du?ng d?n Code/P2PChatGUI).
3. Nh?n **F5** ho?c ch?n **Start** d? build và ch?y ?ng d?ng.

## ? Hu?ng d?n s? d?ng

?ng d?ng cho phép chat 1-1 gi?a hai thi?t b? (ho?c ch?y 2 tab trên cùng 1 máy b?ng IP vòng l?p 127.0.0.1).

1. **Máy 1 (Làm Host):** 
   - Nh?p IP máy b?n (ví d?: 192.168.1.10 ho?c 127.0.0.1 n?u test c?c b?) và Port mu?n m? (VD: 8888).
   - Nh?n **Host**.
2. **Máy 2 (Làm Client):**
   - Nh?p IP và Port tuong ?ng c?a Máy 1.
   - Nh?n **Connect**.
3. Sau khi k?t n?i thành công, c? 2 s? nh?n du?c thông báo "*? Ðã k?t n?i*". Nút gõ tin nh?n s? du?c b?t lên.

*Luu ý: Nút Host/Connect/Disconnect s? du?c ?ng d?ng t? d?ng khóa ho?c m? l?p l?i tùy theo tr?ng thái m?ng d? tránh l?i.*

## ?? C?u trúc d? án

\\\
P2P-Chat-GUI/
+-- Code/                           # Toàn b? mã ngu?n C#
¦   +-- P2PChatGUI/
¦       +-- ChatNetworkCore.cs      # Core x? lý logic m?ng TCP P2P (Socket, Async, Framing)
¦       +-- Form1.cs                # Giao di?n chính (Các nút, TextBox chat)
¦       +-- P2PChatGUI.csproj       # File project C# (.NET 8)
+-- DOCX/                           # Báo cáo d? án
+-- Extra/                          # Tài li?u c?u trúc, so d?
+-- PPTX/                           # Slide thuy?t trình
+-- README.md                       # File hi?n th? này
\\\

## ?? H?n ch? hi?n t?i

- Do s? d?ng k?t n?i TCP thu?n túy, d? nh?n tin qua Internet ngu?i dùng (Host) c?n ph?i m? c?ng m?ng (Port Forwarding) trên Router.
- Hi?n t?i chua h? tr? mã hóa E2E, tin nh?n du?c truy?n di theo d?ng b?n rõ.
- Ch? h? tr? k?t n?i 1-1, chua có các tính nang luu l?i nhóm ho?c l?ch s? h?i tho?i dài h?n.

## ?? License

MIT License  
B?n hoàn toàn t? do s? d?ng, ch?nh s?a và phân ph?i (ghi rõ ngu?n n?u có th?).
