# SacViet - Website Tin T?c Phong Cách VietnamPlus.vn

## T?ng quan d? án

Website tin t?c ???c xây d?ng theo mô hình ASP.NET Core MVC v?i ki?n trúc Repository và Service Layer, thi?t k? giao di?n gi?ng VietnamPlus.vn.

## C?u trúc d? án

### 1. **Architecture Layers**

```
??? Models/                    # Entity Framework Models
??? Repositories/              # Data Access Layer
?   ??? IArticleRepository.cs
?   ??? ArticleRepository.cs
?   ??? ICategoryRepository.cs
?   ??? CategoryRepository.cs
??? Services/                  # Business Logic Layer
?   ??? INewsService.cs
?   ??? NewsService.cs
??? ViewModels/               # View Models
?   ??? HomePageViewModel.cs
?   ??? SearchViewModel.cs
??? Controllers/              # MVC Controllers
?   ??? HomeController.cs
?   ??? ArticleController.cs
?   ??? CategoryController.cs
??? Views/                    # Razor Views
```

### 2. **Các tính n?ng chính**

#### **Trang ch? (Homepage)**
- ? B? c?c 3 c?t gi?ng VietnamPlus.vn
  - C?t trái: Tin nh? (3 bài vi?t)
  - C?t gi?a: Tin n?i b?t chính + tin ph?
  - C?t ph?i: Tin m?i nh?t (8 bài)
- ? Header ?? v?i logo trung tâm
- ? Menu ?i?u h??ng 8 chuyên m?c
- ? Thanh ngày tháng
- ? Section tin n?i b?t khác

#### **8 Chuyên m?c chính**
1. Nh?p v?n hóa (có 2 chuyên m?c con: Trong n??c, Th? gi?i)
2. Du l?ch
3. Con ng??i
4. ?m th?c
5. Trang ph?c và Ki?n trúc
6. Thanh - S?c
7. Multimedia (Podcast, Video, Infographic, Longform, eMagazine)
8. Ch?ng tham

#### **Ch?c n?ng tìm ki?m**
- ? Tìm ki?m theo t? khóa
- ? L?c theo chuyên m?c
- ? L?c theo tác gi?
- ? L?c theo th?i gian ??ng (t? ngày - ??n ngày)
- ? Phân trang k?t qu?

#### **Chi ti?t bài vi?t**
- ? Hi?n th? ??y ?? n?i dung bài vi?t
- ? Thông tin tác gi?, th?i gian ??ng, l??t xem
- ? Tags/t? khóa
- ? Bài vi?t liên quan
- ? Nút chia s? m?ng xã h?i

## H??ng d?n cài ??t

### 1. **Yêu c?u h? th?ng**
- .NET 8 SDK
- SQL Server
- Visual Studio 2022 ho?c VS Code

### 2. **C?u hình Database**

C?p nh?t connection string trong `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=localhost;Initial Catalog=SacViet;User ID=sa;Password=YourPassword;TrustServerCertificate=True;"
}
```

### 3. **Ch?y Migration** (n?u c?n)

```bash
dotnet ef database update
```

### 4. **Seed d? li?u m?u**

?? test giao di?n, b?n c?n thêm d? li?u m?u vào database:

**Categories** (8 chuyên m?c chính):
```sql
INSERT INTO Categories (CategoryName, Slug, DisplayOrder, IsActive, CreatedAt) VALUES
('Nh?p v?n hóa', 'nhip-van-hoa', 1, 1, GETDATE()),
('Du l?ch', 'du-lich', 2, 1, GETDATE()),
('Con ng??i', 'con-nguoi', 3, 1, GETDATE()),
('?m th?c', 'am-thuc', 4, 1, GETDATE()),
('Trang ph?c và Ki?n trúc', 'trang-phuc-kien-truc', 5, 1, GETDATE()),
('Thanh - S?c', 'thanh-sac', 6, 1, GETDATE()),
('Multimedia', 'multimedia', 7, 1, GETDATE()),
('Ch?ng tham', 'chong-tham', 8, 1, GETDATE());
```

**Users** (Tác gi?):
```sql
INSERT INTO Users (Email, FullName, RoleId, IsActive, CreatedAt) VALUES
('admin@sacviet.vn', 'Nguy?n V?n A', 1, 1, GETDATE()),
('editor@sacviet.vn', 'Tr?n Th? B', 2, 1, GETDATE());
```

**Articles** (Bài vi?t m?u):
```sql
INSERT INTO Articles (Title, Slug, Summary, Content, CategoryId, AuthorId, Status, PublishedAt, ViewCount, CreatedAt) VALUES
('Khám phá v? ??p v?n hóa Vi?t Nam', 'kham-pha-ve-dep-van-hoa-viet-nam', 
 'V?n hóa Vi?t Nam v?i nh?ng nét ??p ??c ?áo...', 
 '<p>N?i dung chi ti?t bài vi?t...</p>', 
 1, 1, 'Published', GETDATE(), 100, GETDATE());
```

### 5. **Ch?y ?ng d?ng**

```bash
dotnet run
```

Ho?c nh?n F5 trong Visual Studio.

Truy c?p: `https://localhost:5001`

## C?u trúc URL

- **Trang ch?**: `/`
- **Chi ti?t bài vi?t**: `/bai-viet/{slug}`
- **Chuyên m?c**: `/category/{categoryId}`
- **Tìm ki?m**: `/Home/Search?keyword=...&categoryId=...&fromDate=...&toDate=...`

## Thi?t k? giao di?n

### Màu s?c chính
- **Màu ?? ch? ??o**: `#dc3545` (gi?ng VietnamPlus)
- **Màu ?? ??m**: `#c82333`
- **N?n tr?ng**: `#ffffff`
- **N?n xám nh?t**: `#f8f9fa`
- **Text chính**: `#212529`

### Typography
- Font chính: `Segoe UI, Tahoma, Geneva, Verdana, sans-serif`
- Tiêu ?? chính: 2rem - 3.5rem
- Tiêu ?? ph?: 1rem - 1.5rem
- N?i dung: 1rem - 1.1rem

### Layout
- Container: Bootstrap container/container-fluid
- Grid: Bootstrap 12-column grid
- Responsive breakpoints: 768px, 992px, 1200px

## Tính n?ng ?ã tri?n khai

? **Repository Pattern** - Tách bi?t data access logic
? **Service Layer** - Business logic layer
? **Dependency Injection** - ASP.NET Core DI
? **Async/Await** - X? lý b?t ??ng b?
? **Entity Framework Core** - ORM
? **Responsive Design** - Mobile-friendly
? **SEO-friendly URLs** - Slug-based routing
? **View Components** - Reusable UI components

## Các c?i ti?n có th? thêm

?? Authentication & Authorization (??ng nh?p/??ng ký)
?? Comment system (H? th?ng bình lu?n)
?? Admin Panel (Qu?n lý n?i dung)
?? Rich Text Editor (CKEditor/TinyMCE)
?? Image Upload & Management
?? Newsletter subscription
?? Social media integration
?? Analytics & Tracking
?? Caching (Redis/Memory Cache)
?? CDN integration
?? API endpoints (Web API)
?? Progressive Web App (PWA)

## Liên h? & H? tr?

N?u có b?t k? câu h?i ho?c v?n ?? nào, vui lòng liên h?:
- Email: info@sacviet.vn
- Website: https://sacviet.vn

---

**Copyright © 2025 SacViet. All rights reserved.**