USE EnglishCenterDB;
GO

DELETE FROM Attendances;
DELETE FROM Enrollments;
DELETE FROM ClassSchedules;
DELETE FROM Classes;
DELETE FROM OtpCodes;
DELETE FROM Users;
DELETE FROM Courses;

DBCC CHECKIDENT ('Users',          RESEED, 0);
DBCC CHECKIDENT ('Courses',        RESEED, 0);
DBCC CHECKIDENT ('Classes',        RESEED, 0);
DBCC CHECKIDENT ('ClassSchedules', RESEED, 0);
DBCC CHECKIDENT ('Enrollments',    RESEED, 0);
DBCC CHECKIDENT ('Attendances',    RESEED, 0);
DBCC CHECKIDENT ('OtpCodes',       RESEED, 0);
GO

INSERT INTO Users (FullName, Email, Phone, PasswordHash, Role, AvatarUrl, IsActive, CreatedAt)
VALUES
(N'Nguyễn Thị Hương',  'huong.nguyen@dol.com',  '0902000001', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Teacher', NULL, 1, '2024-01-05 08:00:00'),
(N'Trần Văn Minh',     'minh.tran@dol.com',     '0902000002', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Teacher', NULL, 1, '2024-01-05 08:00:00'),
(N'Phạm Thanh Lan',    'lan.pham@dol.com',      '0902000003', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Teacher', NULL, 1, '2024-01-06 08:00:00'),
(N'Lê Quốc Bảo',      'bao.le@dol.com',        '0902000004', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Teacher', NULL, 1, '2024-01-06 08:00:00'),
(N'Hoàng Thị Thu',     'thu.hoang@dol.com',     '0902000005', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Teacher', NULL, 1, '2024-01-07 08:00:00'),
(N'Võ Đình Khoa',      'khoa.vo@dol.com',       '0902000006', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Teacher', NULL, 0, '2024-01-07 08:00:00'),
(N'Nguyễn Văn An',     'an.nguyen@gmail.com',   '0903000001', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Student', NULL, 1, '2024-02-01 09:00:00'),
(N'Trần Thị Bích',     'bich.tran@gmail.com',   '0903000002', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Student', NULL, 1, '2024-02-01 09:00:00'),
(N'Phạm Văn Cường',    'cuong.pham@gmail.com',  '0903000003', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Student', NULL, 1, '2024-02-02 09:00:00'),
(N'Lê Thị Dung',       'dung.le@gmail.com',     '0903000004', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Student', NULL, 1, '2024-02-02 09:00:00'),
(N'Hoàng Văn Em',      'em.hoang@gmail.com',    '0903000005', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Student', NULL, 1, '2024-02-03 09:00:00'),
(N'Võ Thị Phương',     'phuong.vo@gmail.com',   '0903000006', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Student', NULL, 1, '2024-02-03 09:00:00'),
(N'Đặng Văn Giang',    'giang.dang@gmail.com',  '0903000007', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Student', NULL, 1, '2024-02-04 09:00:00'),
(N'Bùi Thị Hà',        'ha.bui@gmail.com',      '0903000008', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Student', NULL, 1, '2024-02-04 09:00:00'),
(N'Ngô Văn Hùng',      'hung.ngo@gmail.com',    '0903000009', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Student', NULL, 1, '2024-02-05 09:00:00'),
(N'Đinh Thị Khanh',    'khanh.dinh@gmail.com',  '0903000010', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Student', NULL, 1, '2024-02-05 09:00:00'),
(N'Lý Văn Long',       'long.ly@gmail.com',     '0903000011', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Student', NULL, 1, '2024-02-06 09:00:00'),
(N'Mai Thị My',        'my.mai@gmail.com',      '0903000012', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Student', NULL, 1, '2024-02-06 09:00:00'),
(N'Phan Văn Nam',      'nam.phan@gmail.com',    '0903000013', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Student', NULL, 1, '2024-02-07 09:00:00'),
(N'Quách Thị Oanh',    'oanh.quach@gmail.com',  '0903000014', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Student', NULL, 1, '2024-02-07 09:00:00'),
(N'Tạ Văn Phong',      'phong.ta@gmail.com',    '0903000015', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Student', NULL, 1, '2024-02-08 09:00:00'),
(N'Trịnh Thị Quỳnh',   'quynh.trinh@gmail.com', '0903000016', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Student', NULL, 1, '2024-02-08 09:00:00'),
(N'Vũ Văn Rồng',       'rong.vu@gmail.com',     '0903000017', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Student', NULL, 1, '2024-02-09 09:00:00'),
(N'Lưu Thị Sen',       'sen.luu@gmail.com',     '0903000018', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Student', NULL, 1, '2024-02-09 09:00:00'),
(N'Cao Văn Tùng',      'tung.cao@gmail.com',    '0903000019', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Student', NULL, 1, '2024-02-10 09:00:00'),
(N'Hồ Thị Uyên',       'uyen.ho@gmail.com',     '0903000020', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Student', NULL, 1, '2024-02-10 09:00:00'),
(N'Dương Văn Vinh',    'vinh.duong@gmail.com',  '0903000021', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Student', NULL, 1, '2024-03-01 09:00:00'),
(N'Kiều Thị Xuân',     'xuan.kieu@gmail.com',   '0903000022', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Student', NULL, 1, '2024-03-01 09:00:00'),
(N'Mạc Văn Yên',       'yen.mac@gmail.com',     '0903000023', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Student', NULL, 1, '2024-03-02 09:00:00'),
(N'Tống Thị Zoan',     'zoan.tong@gmail.com',   '0903000024', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Student', NULL, 1, '2024-03-02 09:00:00'),
(N'Chu Văn Anh',       'anh.chu@gmail.com',     '0903000025', '$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', 'Student', NULL, 1, '2024-03-03 09:00:00');
GO

INSERT INTO Courses (Name, Level, Description, TuitionFee, ThumbnailUrl, IsActive)
VALUES
(N'Tiếng Anh Cơ Bản',         'A1', N'Khóa học dành cho người mới bắt đầu, học bảng chữ cái, phát âm và giao tiếp căn bản.', 2500000, NULL, 1),
(N'Tiếng Anh Sơ Cấp',         'A2', N'Xây dựng nền tảng từ vựng 1000 từ, ngữ pháp căn bản và kỹ năng hội thoại đơn giản.',  3000000, NULL, 1),
(N'Tiếng Anh Trung Cấp',      'B1', N'Nâng cao kỹ năng nghe nói đọc viết, luyện thi B1 CEFR, từ vựng 2500 từ.',              3500000, NULL, 1),
(N'Tiếng Anh Trung Cấp Trên', 'B2', N'Chuẩn bị cho kỳ thi IELTS 5.0-6.0, TOEFL iBT 60-80, giao tiếp lưu loát.',            4500000, NULL, 1),
(N'Tiếng Anh Cao Cấp',        'C1', N'Luyện thi IELTS 6.5-7.5, TOEFL iBT 90-110, tiếng Anh học thuật và thương mại.',       5500000, NULL, 1),
(N'Tiếng Anh Thành Thạo',     'C2', N'Cấp độ cao nhất, luyện thi IELTS 8.0+, tiếng Anh bản ngữ, phiên dịch.',               7000000, NULL, 1);
GO

INSERT INTO Classes (ClassName, CourseId, TeacherId, StartDate, EndDate, Status, CreatedAt)
VALUES
(N'A1-Sáng-K1',      1, 2, '2024-03-01', '2024-06-30', 'Finished', '2024-02-15 08:00:00'),
(N'A1-Tối-K1',       1, 3, '2024-03-01', '2024-06-30', 'Finished', '2024-02-15 08:00:00'),
(N'A2-Sáng-K1',      2, 4, '2024-04-01', '2024-07-31', 'Finished', '2024-03-15 08:00:00'),
(N'B1-Cuối tuần-K1', 3, 5, '2024-05-01', '2024-08-31', 'Finished', '2024-04-15 08:00:00'),
(N'A1-Sáng-K2',      1, 2, '2025-01-06', '2025-04-30', 'Ongoing',  '2024-12-20 08:00:00'),
(N'A1-Tối-K2',       1, 3, '2025-01-06', '2025-04-30', 'Ongoing',  '2024-12-20 08:00:00'),
(N'A2-Sáng-K2',      2, 4, '2025-02-03', '2025-05-31', 'Ongoing',  '2025-01-15 08:00:00'),
(N'A2-Tối-K2',       2, 5, '2025-02-03', '2025-05-31', 'Ongoing',  '2025-01-15 08:00:00'),
(N'B1-Sáng-K2',      3, 6, '2025-03-03', '2025-06-30', 'Ongoing',  '2025-02-15 08:00:00'),
(N'B2-Cuối tuần-K1', 4, 2, '2025-01-11', '2025-07-31', 'Ongoing',  '2024-12-20 08:00:00'),
(N'A1-Sáng-K3',      1, 3, '2026-05-05', '2026-08-29', 'Upcoming', '2026-04-01 08:00:00'),
(N'A1-Tối-K3',       1, 4, '2026-05-05', '2026-08-29', 'Upcoming', '2026-04-01 08:00:00'),
(N'A2-Sáng-K3',      2, 5, '2026-05-04', '2026-08-28', 'Upcoming', '2026-04-01 08:00:00'),
(N'A2-Tối-K3',       2, 2, '2026-05-06', '2026-09-04', 'Upcoming', '2026-04-01 08:00:00'),
(N'B1-Sáng-K3',      3, 3, '2026-05-04', '2026-08-28', 'Upcoming', '2026-04-05 08:00:00'),
(N'B1-Tối-K3',       3, 6, '2026-05-06', '2026-09-04', 'Upcoming', '2026-04-05 08:00:00'),
(N'B2-Sáng-K2',      4, 4, '2026-05-04', '2026-10-30', 'Upcoming', '2026-04-05 08:00:00'),
(N'C1-Cuối tuần-K1', 5, 5, '2026-05-09', '2026-11-28', 'Upcoming', '2026-04-10 08:00:00'),
(N'C2-Sáng-K1',      6, 2, '2026-06-01', '2026-12-31', 'Upcoming', '2026-04-10 08:00:00');
GO

INSERT INTO ClassSchedules (ClassId, DayOfWeek, StartTime, EndTime)
VALUES
(1,2,'08:00','10:00'),(1,4,'08:00','10:00'),(1,6,'08:00','10:00'),
(2,3,'18:00','20:30'),(2,5,'18:00','20:30'),
(3,2,'08:00','10:30'),(3,4,'08:00','10:30'),
(4,7,'08:00','11:00'),(4,8,'13:00','16:00'),
(5,2,'08:00','10:00'),(5,4,'08:00','10:00'),(5,6,'08:00','10:00'),
(6,3,'18:30','20:30'),(6,5,'18:30','20:30'),
(7,2,'10:30','12:30'),(7,4,'10:30','12:30'),
(8,3,'19:00','21:00'),(8,5,'19:00','21:00'),
(9,2,'08:00','10:30'),(9,3,'08:00','10:30'),(9,4,'08:00','10:30'),
(10,7,'13:00','16:00'),(10,8,'08:00','11:00'),
(11,2,'08:00','10:00'),(11,4,'08:00','10:00'),(11,6,'08:00','10:00'),
(12,3,'18:00','20:00'),(12,5,'18:00','20:00'),
(13,2,'08:30','10:30'),(13,4,'08:30','10:30'),
(14,4,'19:00','21:00'),(14,6,'19:00','21:00'),
(15,2,'10:30','13:00'),(15,4,'10:30','13:00'),
(16,3,'18:00','20:30'),(16,5,'18:00','20:30'),
(17,3,'08:00','10:30'),(17,5,'08:00','10:30'),
(18,7,'08:00','11:30'),(18,8,'08:00','11:30'),
(19,2,'07:30','09:30'),(19,3,'07:30','09:30'),(19,4,'07:30','09:30'),(19,5,'07:30','09:30');
GO

INSERT INTO Enrollments (StudentId, ClassId, EnrolledAt, Status)
VALUES
(8, 5,'2024-12-22 10:00:00','Confirmed'),(9, 5,'2024-12-22 10:30:00','Confirmed'),
(10,5,'2024-12-23 09:00:00','Confirmed'),(11,5,'2024-12-23 09:30:00','Confirmed'),
(12,5,'2024-12-24 08:00:00','Confirmed'),(13,5,'2024-12-24 08:30:00','Confirmed'),
(14,6,'2024-12-22 14:00:00','Confirmed'),(15,6,'2024-12-22 14:30:00','Confirmed'),
(16,6,'2024-12-23 11:00:00','Confirmed'),(17,6,'2024-12-23 11:30:00','Confirmed'),
(18,6,'2024-12-24 10:00:00','Confirmed'),
(19,7,'2025-01-16 09:00:00','Confirmed'),(20,7,'2025-01-16 09:30:00','Confirmed'),
(21,7,'2025-01-17 08:00:00','Confirmed'),(22,7,'2025-01-17 08:30:00','Confirmed'),
(23,8,'2025-01-16 15:00:00','Confirmed'),(24,8,'2025-01-16 15:30:00','Confirmed'),
(25,8,'2025-01-17 14:00:00','Confirmed'),(26,8,'2025-01-17 14:30:00','Confirmed'),
(27,8,'2025-01-18 09:00:00','Confirmed'),
(28,9,'2025-02-16 09:00:00','Confirmed'),(29,9,'2025-02-16 09:30:00','Confirmed'),
(30,9,'2025-02-17 08:00:00','Confirmed'),(31,9,'2025-02-17 08:30:00','Confirmed'),
(32,10,'2024-12-22 10:00:00','Confirmed'),(8,10,'2024-12-22 10:30:00','Confirmed'),
(9, 11,'2026-04-15 08:00:00','Registered'),(10,11,'2026-04-15 08:30:00','Registered'),
(11,11,'2026-04-16 09:00:00','Confirmed'), (12,11,'2026-04-16 09:30:00','Registered'),
(27,11,'2026-04-17 10:00:00','Registered'),
(13,12,'2026-04-15 14:00:00','Confirmed'), (14,12,'2026-04-15 14:30:00','Registered'),
(28,12,'2026-04-16 11:00:00','Registered'),
(19,13,'2026-04-16 08:00:00','Confirmed'), (20,13,'2026-04-16 08:30:00','Registered'),
(21,13,'2026-04-17 09:00:00','Registered'),
(22,14,'2026-04-16 15:00:00','Registered'),(23,14,'2026-04-17 09:30:00','Registered'),
(29,14,'2026-04-18 10:00:00','Confirmed'),
(24,15,'2026-04-17 08:00:00','Registered'),(25,15,'2026-04-17 08:30:00','Registered'),
(30,15,'2026-04-18 09:00:00','Confirmed'),
(26,16,'2026-04-18 14:00:00','Registered'),(31,16,'2026-04-18 14:30:00','Registered'),
(32,17,'2026-04-19 08:00:00','Registered'),(8, 17,'2026-04-19 08:30:00','Registered'),
(9, 18,'2026-04-19 09:00:00','Registered'),
(10,19,'2026-04-19 10:00:00','Registered');
GO

INSERT INTO Attendances (ClassId, StudentId, Date, IsPresent, Note) VALUES
(5,8,'2025-01-06',1,NULL),(5,9,'2025-01-06',1,NULL),(5,10,'2025-01-06',1,NULL),
(5,11,'2025-01-06',0,N'Nghỉ có phép'),(5,12,'2025-01-06',1,NULL),(5,13,'2025-01-06',1,NULL),
(5,8,'2025-01-08',1,NULL),(5,9,'2025-01-08',1,NULL),(5,10,'2025-01-08',0,N'Đến muộn'),
(5,11,'2025-01-08',1,NULL),(5,12,'2025-01-08',1,NULL),(5,13,'2025-01-08',0,NULL),
(5,8,'2025-01-10',1,NULL),(5,9,'2025-01-10',0,NULL),(5,10,'2025-01-10',1,NULL),
(5,11,'2025-01-10',1,NULL),(5,12,'2025-01-10',1,NULL),(5,13,'2025-01-10',1,NULL),
(5,8,'2025-01-13',1,NULL),(5,9,'2025-01-13',1,NULL),(5,10,'2025-01-13',1,NULL),
(5,11,'2025-01-13',1,NULL),(5,12,'2025-01-13',0,NULL),(5,13,'2025-01-13',1,NULL),
(5,8,'2025-01-15',1,NULL),(5,9,'2025-01-15',1,NULL),(5,10,'2025-01-15',1,NULL),
(5,11,'2025-01-15',1,NULL),(5,12,'2025-01-15',1,NULL),(5,13,'2025-01-15',1,NULL),
(5,8,'2025-01-17',0,N'Nghỉ ốm'),(5,9,'2025-01-17',1,NULL),(5,10,'2025-01-17',1,NULL),
(5,11,'2025-01-17',1,NULL),(5,12,'2025-01-17',1,NULL),(5,13,'2025-01-17',0,NULL),
(5,8,'2025-01-20',1,NULL),(5,9,'2025-01-20',1,NULL),(5,10,'2025-01-20',0,NULL),
(5,11,'2025-01-20',1,NULL),(5,12,'2025-01-20',1,NULL),(5,13,'2025-01-20',1,NULL),
(6,14,'2025-01-07',1,NULL),(6,15,'2025-01-07',1,NULL),(6,16,'2025-01-07',0,N'Nghỉ phép'),
(6,17,'2025-01-07',1,NULL),(6,18,'2025-01-07',1,NULL),
(6,14,'2025-01-09',1,NULL),(6,15,'2025-01-09',0,NULL),(6,16,'2025-01-09',1,NULL),
(6,17,'2025-01-09',1,NULL),(6,18,'2025-01-09',1,NULL),
(6,14,'2025-01-14',1,NULL),(6,15,'2025-01-14',1,NULL),(6,16,'2025-01-14',1,NULL),
(6,17,'2025-01-14',1,NULL),(6,18,'2025-01-14',0,N'Nghỉ không phép'),
(6,14,'2025-01-16',1,NULL),(6,15,'2025-01-16',1,NULL),(6,16,'2025-01-16',1,NULL),
(6,17,'2025-01-16',0,NULL),(6,18,'2025-01-16',1,NULL),
(6,14,'2025-01-21',1,NULL),(6,15,'2025-01-21',1,NULL),(6,16,'2025-01-21',1,NULL),
(6,17,'2025-01-21',1,NULL),(6,18,'2025-01-21',1,NULL),
(6,14,'2025-01-23',0,NULL),(6,15,'2025-01-23',1,NULL),(6,16,'2025-01-23',1,NULL),
(6,17,'2025-01-23',1,NULL),(6,18,'2025-01-23',1,NULL),
(7,19,'2025-02-03',1,NULL),(7,20,'2025-02-03',1,NULL),(7,21,'2025-02-03',0,NULL),(7,22,'2025-02-03',1,NULL),
(7,19,'2025-02-05',1,NULL),(7,20,'2025-02-05',0,NULL),(7,21,'2025-02-05',1,NULL),(7,22,'2025-02-05',1,NULL),
(7,19,'2025-02-10',1,NULL),(7,20,'2025-02-10',1,NULL),(7,21,'2025-02-10',1,NULL),(7,22,'2025-02-10',0,N'Nghỉ ốm'),
(7,19,'2025-02-12',1,NULL),(7,20,'2025-02-12',1,NULL),(7,21,'2025-02-12',1,NULL),(7,22,'2025-02-12',1,NULL),
(7,19,'2025-02-17',0,NULL),(7,20,'2025-02-17',1,NULL),(7,21,'2025-02-17',1,NULL),(7,22,'2025-02-17',1,NULL),
(7,19,'2025-02-19',1,NULL),(7,20,'2025-02-19',1,NULL),(7,21,'2025-02-19',0,NULL),(7,22,'2025-02-19',1,NULL),
(8,23,'2025-02-04',1,NULL),(8,24,'2025-02-04',1,NULL),(8,25,'2025-02-04',1,NULL),
(8,26,'2025-02-04',0,NULL),(8,27,'2025-02-04',1,NULL),
(8,23,'2025-02-06',1,NULL),(8,24,'2025-02-06',0,N'Nghỉ phép'),(8,25,'2025-02-06',1,NULL),
(8,26,'2025-02-06',1,NULL),(8,27,'2025-02-06',1,NULL),
(8,23,'2025-02-11',1,NULL),(8,24,'2025-02-11',1,NULL),(8,25,'2025-02-11',0,NULL),
(8,26,'2025-02-11',1,NULL),(8,27,'2025-02-11',1,NULL),
(8,23,'2025-02-13',1,NULL),(8,24,'2025-02-13',1,NULL),(8,25,'2025-02-13',1,NULL),
(8,26,'2025-02-13',1,NULL),(8,27,'2025-02-13',0,N'Nghỉ ốm'),
(9,28,'2025-03-03',1,NULL),(9,29,'2025-03-03',1,NULL),(9,30,'2025-03-03',0,NULL),(9,31,'2025-03-03',1,NULL),
(9,28,'2025-03-04',1,NULL),(9,29,'2025-03-04',0,NULL),(9,30,'2025-03-04',1,NULL),(9,31,'2025-03-04',1,NULL),
(9,28,'2025-03-05',1,NULL),(9,29,'2025-03-05',1,NULL),(9,30,'2025-03-05',1,NULL),(9,31,'2025-03-05',0,NULL),
(9,28,'2025-03-10',1,NULL),(9,29,'2025-03-10',1,NULL),(9,30,'2025-03-10',1,NULL),(9,31,'2025-03-10',1,NULL),
(9,28,'2025-03-11',0,N'Nghỉ phép'),(9,29,'2025-03-11',1,NULL),(9,30,'2025-03-11',1,NULL),(9,31,'2025-03-11',1,NULL),
(9,28,'2025-03-12',1,NULL),(9,29,'2025-03-12',1,NULL),(9,30,'2025-03-12',0,NULL),(9,31,'2025-03-12',1,NULL),
(10,32,'2025-01-11',1,NULL),(10,8,'2025-01-11',1,NULL),
(10,32,'2025-01-12',1,NULL),(10,8,'2025-01-12',0,N'Nghỉ phép'),
(10,32,'2025-01-18',1,NULL),(10,8,'2025-01-18',1,NULL),
(10,32,'2025-01-19',0,NULL),(10,8,'2025-01-19',1,NULL),
(10,32,'2025-01-25',1,NULL),(10,8,'2025-01-25',1,NULL),
(10,32,'2025-01-26',1,NULL),(10,8,'2025-01-26',1,NULL);
GO

SELECT 'Users'       AS [Table], COUNT(*) AS [Count] FROM Users
UNION ALL SELECT 'Courses',     COUNT(*) FROM Courses
UNION ALL SELECT 'Classes',     COUNT(*) FROM Classes
UNION ALL SELECT 'Schedules',   COUNT(*) FROM ClassSchedules
UNION ALL SELECT 'Enrollments', COUNT(*) FROM Enrollments
UNION ALL SELECT 'Attendances', COUNT(*) FROM Attendances;
GO
