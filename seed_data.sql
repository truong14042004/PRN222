USE EnglishCenterDB;
GO

-- 1. Xóa dữ liệu cũ theo đúng thứ tự ràng buộc khóa ngoại
DELETE FROM CartItems;
DELETE FROM OrderItems;
DELETE FROM Orders;
DELETE FROM PurchasableItems;
DELETE FROM StudentAnswers;
DELETE FROM QuizResults;
DELETE FROM QuizOptions;
DELETE FROM QuizQuestions;
DELETE FROM Quizzes;
DELETE FROM CourseProgresses;
DELETE FROM Attendances;
DELETE FROM Enrollments;
DELETE FROM ClassSchedules;
DELETE FROM Classes;
DELETE FROM EmailLogs;
DELETE FROM EmailOtps;
DELETE FROM OtpCodes;
DELETE FROM Users;
DELETE FROM Courses;

-- 2. Reset Identity cho các bảng
DBCC CHECKIDENT ('Quizzes',          RESEED, 0);
DBCC CHECKIDENT ('QuizQuestions',    RESEED, 0);
DBCC CHECKIDENT ('QuizOptions',      RESEED, 0);
DBCC CHECKIDENT ('QuizResults',      RESEED, 0);
DBCC CHECKIDENT ('StudentAnswers',   RESEED, 0);
DBCC CHECKIDENT ('CourseProgresses', RESEED, 0);
DBCC CHECKIDENT ('EmailOtps',        RESEED, 0);
DBCC CHECKIDENT ('EmailLogs',        RESEED, 0);
DBCC CHECKIDENT ('Users',            RESEED, 0);
DBCC CHECKIDENT ('Courses',          RESEED, 0);
DBCC CHECKIDENT ('Classes',          RESEED, 0);
DBCC CHECKIDENT ('ClassSchedules',   RESEED, 0);
DBCC CHECKIDENT ('Enrollments',      RESEED, 0);
DBCC CHECKIDENT ('Attendances',      RESEED, 0);
DBCC CHECKIDENT ('OtpCodes',         RESEED, 0);
DBCC CHECKIDENT ('PurchasableItems', RESEED, 0);
DBCC CHECKIDENT ('Orders',           RESEED, 0);
DBCC CHECKIDENT ('CartItems',        RESEED, 0);
GO

-- 3. Seed Users (Bổ sung WalletBalance và IsActive)
-- Mật khẩu mặc định: Admin@123 (hash tương tự bản cũ)
INSERT INTO Users (FullName, Email, Username, Phone, PasswordHash, Role, IsActive, CreatedAt, WalletBalance, RegistrationFeePaid)
VALUES
(N'Quản trị hệ thống', 'admin@englishcenter.local', 'admin',        '0901000000', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Admin',   1, '2024-01-01 08:00:00', 1000000, 1),
(N'Nguyễn Thị Hương',  'huong.nguyen@dol.com',      'huong.nguyen', '0902000001', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Teacher', 1, '2024-01-05 08:00:00', 0, 1),
(N'Trần Văn Minh',     'minh.tran@dol.com',         'minh.tran',    '0902000002', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Teacher', 1, '2024-01-05 08:00:00', 0, 1),
(N'Phạm Thanh Lan',    'lan.pham@dol.com',          'lan.pham',     '0902000003', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Teacher', 1, '2024-01-06 08:00:00', 0, 1),
(N'Lê Quốc Bảo',       'bao.le@dol.com',            'bao.le',       '0902000004', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Teacher', 1, '2024-01-06 08:00:00', 0, 1),
(N'Hoàng Thị Thu',     'thu.hoang@dol.com',         'thu.hoang',    '0902000005', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Teacher', 1, '2024-01-07 08:00:00', 0, 1),
(N'Nguyễn Văn An',     'an.nguyen@gmail.com',       'an.nguyen',    '0903000001', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Student', 1, '2024-02-01 09:00:00', 500000, 1),
(N'Trần Thị Bích',     'bich.tran@gmail.com',       'bich.tran',    '0903000002', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Student', 1, '2024-02-01 09:00:00', 0, 1),
(N'Phạm Văn Cường',    'cuong.pham@gmail.com',      'cuong.pham',   '0903000003', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Student', 1, '2024-02-02 09:00:00', 0, 1),
(N'Lê Thị Dung',       'dung.le@gmail.com',         'dung.le',      '0903000004', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Student', 1, '2024-02-02 09:00:00', 0, 1),
(N'Hoàng Văn Em',      'em.hoang@gmail.com',        'em.hoang',     '0903000005', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Student', 1, '2024-02-03 09:00:00', 0, 1),
(N'Võ Thị Phương',     'phuong.vo@gmail.com',       'phuong.vo',    '0903000006', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Student', 1, '2024-02-03 09:00:00', 0, 1),
(N'Đặng Văn Giang',    'giang.dang@gmail.com',      'giang.dang',   '0903000007', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Student', 1, '2024-02-04 09:00:00', 0, 1),
(N'Bùi Thị Hà',        'ha.bui@gmail.com',          'ha.bui',       '0903000008', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Student', 1, '2024-02-04 09:00:00', 0, 1),
(N'Ngô Văn Hùng',      'hung.ngo@gmail.com',        'hung.ngo',     '0903000009', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Student', 1, '2024-02-05 09:00:00', 0, 1),
(N'Đinh Thị Khanh',    'khanh.dinh@gmail.com',      'khanh.dinh',   '0903000010', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Student', 1, '2024-02-05 09:00:00', 0, 1),
(N'Chu Văn Anh',       'anh.chu@gmail.com',         'anh.chu',      '0903000025', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Student', 1, '2024-03-03 09:00:00', 0, 1);
GO

-- 4. Seed Courses
INSERT INTO Courses (Name, Level, Description, TuitionFee, IsActive)
VALUES
(N'Tiếng Anh Cơ Bản',         'A1', N'Khóa học dành cho người mới bắt đầu, học bảng chữ cái, phát âm và giao tiếp căn bản.', 2500, 1),
(N'Tiếng Anh Sơ Cấp',         'A2', N'Xây dựng nền tảng từ vựng 1000 từ, ngữ pháp căn bản và kỹ năng hội thoại đơn giản.',  3000, 1),
(N'Tiếng Anh Trung Cấp',      'B1', N'Nâng cao kỹ năng nghe nói đọc viết, luyện thi B1 CEFR, từ vựng 2500 từ.',              3500, 1),
(N'Tiếng Anh Trung Cấp Trên', 'B2', N'Chuẩn bị cho kỳ thi IELTS 5.0-6.0, TOEFL iBT 60-80, giao tiếp lưu loát.',            4500, 1),
(N'Tiếng Anh Cao Cấp',        'C1', N'Luyện thi IELTS 6.5-7.5, TOEFL iBT 90-110, tiếng Anh học thuật và thương mại.',       5500, 1),
(N'Tiếng Anh Thành Thạo',     'C2', N'Cấp độ cao nhất, luyện thi IELTS 8.0+, tiếng Anh bản ngữ, phiên dịch.',               7000, 1);
GO

-- 5. Seed Classes
INSERT INTO Classes (ClassName, CourseId, TeacherId, StartDate, EndDate, Status, CreatedAt)
VALUES
(N'A1-Sáng-K1',      1, 2, '2024-03-01', '2024-06-30', 'Finished', '2024-02-15 08:00:00'),
(N'A1-Tối-K1',       1, 3, '2024-03-01', '2024-06-30', 'Finished', '2024-02-15 08:00:00'),
(N'A2-Sáng-K1',      2, 4, '2024-04-01', '2024-07-31', 'Finished', '2024-03-15 08:00:00'),
(N'A1-Sáng-K3',      1, 3, '2026-05-05', '2026-08-29', 'Upcoming', '2026-04-01 08:00:00'),
(N'B1-Sáng-K3',      3, 3, '2026-05-04', '2026-08-28', 'Upcoming', '2026-04-05 08:00:00');
GO

-- 6. Seed ClassSchedules
INSERT INTO ClassSchedules (ClassId, DayOfWeek, StartTime, EndTime)
VALUES
(1,2,'08:00','10:00'),(1,4,'08:00','10:00'),(1,6,'08:00','10:00'),
(2,3,'18:00','20:30'),(2,5,'18:00','20:30'),
(4,2,'08:00','10:00'),(4,4,'08:00','10:00'),(4,6,'08:00','10:00');
GO

-- 7. Seed Enrollments
INSERT INTO Enrollments (StudentId, ClassId, EnrolledAt, Status)
VALUES
(7, 1,'2024-02-15 10:00:00','Confirmed'),
(8, 1,'2024-02-15 10:30:00','Confirmed'),
(7, 4,'2026-04-15 08:00:00','Registered');
GO

-- 8. Seed Attendances
INSERT INTO Attendances (ClassId, StudentId, Date, IsPresent, Note) VALUES
(1,7,'2024-03-01',1,NULL),(1,8,'2024-03-01',1,NULL),
(1,7,'2024-03-03',1,NULL),(1,8,'2024-03-03',0,N'Nghỉ phép');
GO

-- 9. Seed PurchasableItems
INSERT INTO PurchasableItems (Name, Description, Price, IsActive, CreatedAt)
VALUES
(N'Giáo trình IELTS Foundation', N'Sách học IELTS cho người mới.', 1500, 1, GETDATE()),
(N'Bộ đề thi thử Cambridge B1', N'10 đề thi chuẩn cấu trúc.', 1200, 1, GETDATE()),
(N'Tai nghe học tiếng Anh', N'Tai nghe chuyên dụng luyện nghe.', 3500, 1, GETDATE()),
(N'Từ điển Oxford English', N'Phiên bản bìa cứng mới nhất.', 4500, 1, GETDATE());
GO

-- Kiểm tra kết quả
SELECT 'Users'       AS [Table], COUNT(*) AS [Count] FROM Users
UNION ALL SELECT 'Courses',     COUNT(*) FROM Courses
UNION ALL SELECT 'Classes',     COUNT(*) FROM Classes
UNION ALL SELECT 'PurchasableItems', COUNT(*) FROM PurchasableItems
UNION ALL SELECT 'Enrollments', COUNT(*) FROM Enrollments;
GO
