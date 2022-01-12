- Những chức năng chính:
+ Quản lý: Bài viết, danh mục, sản phẩm, đơn hàng, phân quyền.
+ người dùng: đăng kí, đăng nhập, giỏ hàng, đặt hàng.


- Hướng dẫn chạy Project
+ Sau khi tải về hệ thống bao gồm thư mục chứa source code, file báo cáo và hướng
dẫn chạy. Để chạy file source code của hệ thống người dùng sử dụng Visual Studio 2019
đảm bảo hỗ trợ ASP .Net Core 5.0 trước khi mở project.
Trước khi build project người dùng cần vào đường dẫn để khởi chạy cơ sở dữ
liệu. Người dùng vào SQL Manager tiến hành restore database (đảm bảo chưa có
database nào với tên là ShopOnline) với file backup theo đường dẫn bên dưới:
/Database/ShopOnline.bak
+ Sau đó người dùng chạy project lên theo đường dẫn:
/ShopOnline.sln
+Tại source code của hệ thống, người dùng cần chỉnh sửa 2 file theo đúng tên
SQLServer của máy người chạy:
- Appsettting.json
- Models/ShopOnlineContext.cs
Sau khi chạy project, người dùng có thể sử dụng tài khoản để test chương trình:
Email: admin@gmail.com
Password: 123123