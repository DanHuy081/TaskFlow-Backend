  # 🚀 TaskFlow: Hệ thống Quản lý Dự án tích hợp AI

  **Nền tảng quản lý công việc thông minh, kết hợp giữa quy trình theo dõi truyền thống và Trợ lý ảo AI nhận thức ngữ cảnh.**

  [![React](https://img.shields.io/badge/React-18.x-blue.svg?style=flat&logo=react)](https://reactjs.org/)
  [![.NET](https://img.shields.io/badge/.NET-8.0-purple.svg?style=flat&logo=dotnet)](https://dotnet.microsoft.com/)
  [![SQL Server](https://img.shields.io/badge/SQL_Server-2022-red.svg?style=flat&logo=microsoft-sql-server)](https://www.microsoft.com/sql-server)
  [![Gemini AI](https://img.shields.io/badge/Google_Gemini-Flash-orange.svg?style=flat&logo=google)](https://deepmind.google/technologies/gemini/)

</div>

---

## 💡 Giới thiệu dự án

**TaskFlow** là một nền tảng quản lý công việc và dự án toàn diện, được thiết kế để giải quyết bài toán phân mảnh dữ liệu và thiếu đồng bộ trong làm việc nhóm. 

Được xây dựng trên nền tảng kiến trúc **Clean Architecture** mạnh mẽ, TaskFlow không chỉ dừng lại ở các bảng Kanban thông thường mà còn tích hợp sâu một **Chatbot AI sử dụng kỹ thuật RAG (Retrieval-Augmented Generation)**. Thay vì chỉ ghi nhận tiến độ, TaskFlow chủ động hỗ trợ người dùng phân tích dữ liệu dự án, trả lời câu hỏi theo ngữ cảnh và tự động hóa việc tạo công việc thông qua ngôn ngữ tự nhiên.

### ✨ Tính năng nổi bật

- **🧠 Trợ lý AI nhận thức ngữ cảnh (Context-Aware AI)**: Tích hợp Google Gemini. AI có khả năng nhận biết bạn đang xem dự án hay nhóm nào, từ đó cung cấp dữ liệu phân tích và tóm tắt chính xác.
- **⚡ AI Action Agent**: Chuyển đổi các câu lệnh ngôn ngữ tự nhiên thành hành động thực tế trên cơ sở dữ liệu (Ví dụ: *"Tạo một task fix bug đăng nhập có độ ưu tiên cao vào ngày mai"*).
- **📊 Bảng điều khiển (Dashboard) Real-time**: Trực quan hóa sức khỏe dự án, tỷ lệ hoàn thành công việc và phân bổ khối lượng công việc.
- **💬 Không gian làm việc nhóm thống nhất**: Tích hợp chat nhóm trực tiếp trong ứng dụng và tính năng bình luận văn bản đa dạng (Rich-text) với thông báo tức thì khi được `@mention`.
- **📋 Quản lý phân cấp linh hoạt**: Cấu trúc đa tầng (`Space` > `Team` > `List` > `Task`) giúp tổ chức mọi thứ từ công việc cá nhân đến các dự án quy mô lớn.

---

## 📸 Ảnh chụp màn hình


| Tổng quan Dashboard |Bảng Kanban |
| :---: | :---: |
| <img width="612" height="374" alt="image" src="https://github.com/user-attachments/assets/96ce2541-4e3e-45e0-aa43-c8eaafa84308" />|<img width="689" height="340" alt="image" src="https://github.com/user-attachments/assets/bce87e27-1689-45eb-b786-a808f6df7b43" />
| **Trợ lý AI Chatbot** |**Lịch** |
| <img width="676" height="445" alt="image" src="https://github.com/user-attachments/assets/5cbed46f-9c2d-44f2-827c-2e8333b5f9e2" />|<img width="690" height="339" alt="image" src="https://github.com/user-attachments/assets/90c0138b-116e-4289-8dc6-d44f61091a82" />|

---

## 🛠️ Công nghệ sử dụng

### Frontend
- **Framework**: ReactJS 18 + TypeScript + Vite
- **UI & Giao diện**: TailwindCSS, Ant Design, Framer Motion
- **Quản lý State**: Redux Toolkit / Context API
- **Gọi API**: Axios

### Backend
- **Framework**: ASP.NET Core Web API 8.0 (C# 12)
- **Kiến trúc**: Clean Architecture / N-Tier
- **Cơ sở dữ liệu & ORM**: Microsoft SQL Server, Entity Framework Core 8
- **Tích hợp AI**: Google.GenerativeAI SDK (Gemini Flash)
- **Bảo mật**: JWT Bearer Authentication, BCrypt Password Hashing

---

## ⚙️ Hướng dẫn cài đặt
### Yêu cầu hệ thống
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/) (v18 trở lên)
- SQL Server (Cài đặt máy chủ ảo hoặc Docker)
- Có sẵn Google Gemini API Key

### 1. Cài đặt Backend
```bash
# Clone repository về máy
git clone [https://github.com/yourusername/TaskFlow.git](https://github.com/yourusername/TaskFlow.git)

# Di chuyển vào thư mục Backend
cd TaskFlow/Backend

# Cấu hình Connection String & API Keys trong file appsettings.json:
# "ConnectionStrings": { "DefaultConnection": "..." }
# "Gemini": { "ApiKey": "YOUR_API_KEY" }

# Chạy database migrations để tạo bảng
dotnet ef database update

# Khởi chạy server
dotnet run

# Di chuyển vào thư mục Frontend
cd ../Frontend

# Cài đặt các thư viện (dependencies)
npm install

# Cấu hình biến môi trường
# Tạo một file .env và thêm dòng sau:
# VITE_API_URL=https://localhost:7041/api

# Khởi chạy server development
npm run dev

🧠 Cơ chế hoạt động của AI (RAG Pipeline)
Hệ thống AI trong TaskFlow không chỉ gửi chuỗi văn bản thô cho LLM. Quá trình này trải qua các bước RAG:

Bắt ngữ cảnh (Context Capture): Frontend gửi thông tin vị trí hiện tại của user (ID của Team hoặc List).

Truy xuất dữ liệu (Data Retrieval): Backend truy vấn an toàn các dữ liệu liên quan (Danh sách Task, Thành viên, Deadline) từ SQL Server.

Đóng gói Prompt (Prompt Engineering): Dữ liệu được nhúng vào một Mega-Prompt làm tài liệu tham khảo cho AI.

Định tuyến hành động (Action Routing): Nếu AI phát hiện người dùng muốn tạo công việc, nó sẽ trả về chuỗi JSON cấu trúc. Backend sẽ bắt lấy JSON này để thực thi hàm TaskService.CreateAsync().
