USE EnglishCenterDB;
GO

-- Xóa dữ liệu cũ theo đúng thứ tự ràng buộc
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

-- Reset Identity
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
DBCC CHECKIDENT ('OrderItems',       RESEED, 0);
GO

-- Seed Users
INSERT INTO Users (FullName, Email, Username, Phone, PasswordHash, Role, AvatarUrl, IsActive, CreatedAt, WalletBalance)
VALUES
(N'Quản trị hệ thống', 'admin@englishcenter.local', 'admin',        '0901000000', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Admin',   NULL, 1, '2024-01-01 08:00:00', 1000000),
(N'Nguyễn Thị Hương',  'huong.nguyen@dol.com',      'huong.nguyen', '0902000001', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Teacher', NULL, 1, '2024-01-05 08:00:00', 0),
(N'Trần Văn Minh',     'minh.tran@dol.com',         'minh.tran',    '0902000002', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Teacher', NULL, 1, '2024-01-05 08:00:00', 0),
(N'Phạm Thanh Lan',    'lan.pham@dol.com',          'lan.pham',     '0902000003', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Teacher', NULL, 1, '2024-01-06 08:00:00', 0),
(N'Lê Quốc Bảo',       'bao.le@dol.com',            'bao.le',       '0902000004', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Teacher', NULL, 1, '2024-01-06 08:00:00', 0),
(N'Hoàng Thị Thu',     'thu.hoang@dol.com',         'thu.hoang',    '0902000005', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Teacher', NULL, 1, '2024-01-07 08:00:00', 0),
(N'Võ Đình Khoa',      'khoa.vo@dol.com',           'khoa.vo',      '0902000006', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Teacher', NULL, 0, '2024-01-07 08:00:00', 0),
(N'Nguyễn Văn An',     'an.nguyen@gmail.com',       'an.nguyen',    '0903000001', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Student', NULL, 1, '2024-02-01 09:00:00', 500000),
(N'Trần Thị Bích',     'bich.tran@gmail.com',       'bich.tran',    '0903000002', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Student', NULL, 1, '2024-02-01 09:00:00', 0);
GO

-- Seed Courses
INSERT INTO Courses (Name, Level, Description, TuitionFee, ThumbnailUrl, IsActive)
VALUES
(N'Tiếng Anh Cơ Bản',         'A1', N'Khóa học dành cho người mới bắt đầu.', 2500000, NULL, 1),
(N'Tiếng Anh Sơ Cấp',         'A2', N'Xây dựng nền tảng từ vựng 1000 từ.',  3000000, NULL, 1),
(N'Tiếng Anh Trung Cấp',      'B1', N'Nâng cao kỹ năng nghe nói đọc viết.', 3500000, NULL, 1);
GO

-- Seed PurchasableItems
INSERT INTO PurchasableItems (Name, Description, Price, ImageUrl, IsActive, CreatedAt)
VALUES
(N'Giáo trình IELTS Foundation', N'Sách học IELTS cho người mới bắt đầu.', 150000, NULL, 1, GETDATE()),
(N'Bộ đề thi thử Cambridge B1', N'10 đề thi thử chuẩn cấu trúc quốc tế.', 120000, NULL, 1, GETDATE()),
(N'Từ điển Oxford English', N'Từ điển song ngữ phiên bản mới nhất.', 450000, NULL, 1, GETDATE()),
(N'Tai nghe học tiếng Anh', N'Tai nghe chuyên dụng cho luyện nghe.', 350000, NULL, 1, GETDATE());
GO

-- Seed Classes
INSERT INTO Classes (ClassName, CourseId, TeacherId, StartDate, EndDate, Status, CreatedAt)
VALUES
(N'A1-Sáng-K1',      1, 2, '2024-03-01', '2024-06-30', 'Finished', '2024-02-15 08:00:00'),
(N'A1-Tối-K1',       1, 3, '2024-03-01', '2024-06-30', 'Finished', '2024-02-15 08:00:00');
GO

-- SELECT Summary
SELECT 'Users'       AS [Table], COUNT(*) AS [Count] FROM Users
UNION ALL SELECT 'Courses',     COUNT(*) FROM Courses
UNION ALL SELECT 'Classes',     COUNT(*) FROM Classes
UNION ALL SELECT 'PurchasableItems', COUNT(*) FROM PurchasableItems;
GO
