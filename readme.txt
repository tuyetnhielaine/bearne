https://www.virustotal.com/gui/file/9b0451345c1db4a0f2c1f3b3d22d0374c1eb9cd3181a98e9f1e98e5e47ceeae3?nocache=1
Hướng dẫn Client:
Bước 1: Tải phần mềm VS STUDIO https://visualstudio.microsoft.com/downloads/
Bước 2: Mở file VlAutoUpdateClient\Common.cs
Bước 3: Thay đổi IP máy chủ của bạn.
Bước 4: Mở file VlAuto.sln và chọn Build.
Bước 5: Khi build xong sản phẩm sẽ ở trong thư mục bin/Debug hoặc bin/Release
Hướng dẫn Tool:
Bước 1: Copy VlAutoUpdateTool.exe vào thư mục updatefiles (có thể không cần). 
Bước 2: Copy các file mà bạn muốn cập nhật vào thư mục updatefiles, và chạy VlAutoUpdateTool.exe tạo checksum.txt danh sách file.
Bước 3: Copy thư mục updatefiles, lên webserver của bạn (var\www\html\updatefiles), ví dụ: 192.168.1.110/updatefiles

*Lưu ý: Mở file VlAutoApp.txt để xem kết quả tải về
Received HTTP response headers after 1.1858ms - 200
End processing HTTP request after 1.5148ms - 200

Hiển thị kết quả 200 là thành công, khác là 404 do config sai hoặc code bị lỗi, hãy kiểm tra xem có tải trực tiếp từ webserver được hay không.
