```markdown
<p align="center">
  <img src="https://img.shields.io/badge/P2P%20Chat-GUI-blue?style=for-the-badge&logo=python&logoColor=white&color=6f42c1" alt="P2P Chat GUI">
  <img src="https://img.shields.io/badge/Status-Completed-success?style=for-the-badge&logo=checkmarx" alt="Status">
  <img src="https://img.shields.io/badge/License-MIT-green?style=for-the-badge" alt="License MIT">
  <img src="https://img.shields.io/badge/Python-3.10+-yellow?style=for-the-badge&logo=python" alt="Python">
</p>

<h1 align="center">P2P Chat GUI</h1>

<p align="center">
  <strong>Ứng dụng nhắn tin Peer-to-Peer trực tiếp</strong><br>
  Không cần server trung tâm • Giao diện đồ họa đẹp mắt • Kết nối bằng IP + Port
</p>

<p align="center">
  <a href="#-demo"><strong>Xem Demo</strong></a> •
  <a href="#-tính-năng">Tính năng</a> •
  <a href="#-cài-đặt">Cài đặt</a> •
  <a href="#-cấu-trúc-thư-mục">Cấu trúc</a> •
  <a href="#-công-nghệ">Công nghệ</a>
</p>

<br>

## ✨ Demo

<p align="center">
  <img src="Extra/screenshots/chat-interface.png" width="48%" alt="Giao diện chat chính">
  <img src="Extra/screenshots/connect-screen.png" width="48%" alt="Màn hình kết nối">
</p>

> **Lưu ý**: Thay thế đường dẫn ảnh trên bằng ảnh thật của bạn trong thư mục `Extra/screenshots/`.  
> Nếu bạn đã quay video demo, hãy upload lên GitHub Releases hoặc YouTube và thay link vào đây.

## 🚀 Tính năng chính

- Kết nối P2P **thuần túy** (không qua server trung gian)
- Nhập IP + Port thủ công để kết nối
- Gửi/nhận tin nhắn realtime, hỗ trợ Unicode & emoji
- Hiển thị lịch sử chat rõ ràng
- Trạng thái kết nối trực quan (đang chờ • đã kết nối • mất kết nối)
- Giao diện hiện đại, responsive (sử dụng customtkinter)
- Multi-threading: không bị treo khi gửi/nhận
- Dễ mở rộng: hỗ trợ gửi file, voice note (có thể thêm sau)

## 🛠️ Công nghệ sử dụng

| Thành phần       | Công cụ / Thư viện              |
|------------------|----------------------------------|
| Ngôn ngữ         | Python 3.10+                     |
| Giao diện        | Tkinter + customtkinter          |
| Mạng             | socket (TCP)                     |
| Đa luồng         | threading                        |
| Theme UI         | customtkinter (hoặc ttkbootstrap)|
| Quản lý dự án    | Git                              |

## 📥 Cài đặt & Chạy nhanh

### Yêu cầu hệ thống

- Python 3.10 trở lên
- pip (đã cài sẵn trong hầu hết các bản Python)

### Các bước

```bash
# 1. Clone hoặc tải dự án về
git clone https://github.com/[username-cua-ban]/P2P-Chat-GUI.git
# hoặc tải ZIP và giải nén

cd P2P-Chat-GUI

# 2. Cài đặt các thư viện cần thiết
pip install -r Code/requirements.txt

# 3. Chạy ứng dụng
python Code/src/main.py
```

**Nội dung file `Code/requirements.txt`** (nên có):

```
customtkinter>=5.2.0
pillow>=9.0.0    # nếu dùng hình ảnh/icon
```

## ⚡ Hướng dẫn sử dụng (Demo 2 người)

1. **Cả hai máy** đều chạy chương trình
2. **Người A**: copy địa chỉ IP của mình (thường là `192.168.x.x` nếu cùng LAN)
3. **Người B**: nhập IP của A + port (mặc định 5555) → nhấn "Kết nối"
4. Người A thấy thông báo "Đã kết nối" → bắt đầu chat!

**Test nhanh trên 1 máy**:
- Chạy 2 instance chương trình
- Dùng IP `127.0.0.1` (localhost)
- Port khác nhau nếu cần (ví dụ: 5555 và 5556)

## 📂 Cấu trúc thư mục dự án

```
P2P-Chat-GUI/
├── Code/                    # Toàn bộ mã nguồn
│   ├── src/
│   │   ├── main.py
│   │   ├── chat_app.py
│   │   └── utils.py         # (nếu có hàm hỗ trợ)
│   └── requirements.txt
├── DOCX/                    # Báo cáo đồ án
│   └── BaoCao_P2PChat.docx
├── Extra/                   # Tài liệu bổ sung
│   ├── diagrams/            # UML, sequence diagram, class diagram
│   ├── screenshots/         # Ảnh chụp màn hình demo
│   └── references/          # Tài liệu tham khảo, bài báo
├── PPTX/                    # Slide thuyết trình
│   └── TrinhBay_P2PChat.pptx
└── README.md                # File này
```

## ⚠️ Hạn chế hiện tại

- Phải biết trước IP (không có cơ chế discovery tự động)
- Không vượt NAT/router tự động (cần cùng LAN hoặc port forwarding)
- Tin nhắn chưa được mã hóa end-to-end
- Không có danh sách bạn bè / lưu trữ lịch sử lâu dài

## 📜 License

MIT License  
Bạn hoàn toàn tự do sử dụng, chỉnh sửa, phân phối (ghi nguồn nếu có thể).

## ❤️ Tác giả

**Dang**  
Ho Chi Minh City, Việt Nam • 2026

<p align="center">
  <sub>Made with ❤️ and endless debug sessions</sub>
</p>
```