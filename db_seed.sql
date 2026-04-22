-- EnglishCenterDB Full Data Dump
-- Generated at: 04/23/2026 00:42:12
USE [EnglishCenterDB];
GO

-- Disable all constraints
EXEC sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL';
GO

-- Clean up existing data
DELETE FROM [Attendances];
DELETE FROM [CartItems];
DELETE FROM [Classes];
DELETE FROM [ClassSchedules];
DELETE FROM [CourseProgresses];
DELETE FROM [Courses];
DELETE FROM [EmailLogs];
DELETE FROM [EmailOtps];
DELETE FROM [Enrollments];
DELETE FROM [OrderItems];
DELETE FROM [Orders];
DELETE FROM [OtpCodes];
DELETE FROM [PurchasableItems];
DELETE FROM [QuizOptions];
DELETE FROM [QuizQuestions];
DELETE FROM [QuizResults];
DELETE FROM [Quizzes];
DELETE FROM [StudentAnswers];
DELETE FROM [Users];
GO

-- Dumping data for table Attendances
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Attendances') AND is_identity = 1) SET IDENTITY_INSERT [Attendances] ON;
INSERT INTO [Attendances] ([Id], [ClassId], [StudentId], [Date], [IsPresent], [Note]) VALUES (1, 1, 7, '2024-03-01 00:00:00.000', 1, NULL);
INSERT INTO [Attendances] ([Id], [ClassId], [StudentId], [Date], [IsPresent], [Note]) VALUES (2, 1, 8, '2024-03-01 00:00:00.000', 1, NULL);
INSERT INTO [Attendances] ([Id], [ClassId], [StudentId], [Date], [IsPresent], [Note]) VALUES (3, 1, 7, '2024-03-03 00:00:00.000', 1, NULL);
INSERT INTO [Attendances] ([Id], [ClassId], [StudentId], [Date], [IsPresent], [Note]) VALUES (4, 1, 8, '2024-03-03 00:00:00.000', 0, N'Nghỉ phép');
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Attendances') AND is_identity = 1) SET IDENTITY_INSERT [Attendances] OFF;
GO

-- Dumping data for table CartItems
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('CartItems') AND is_identity = 1) SET IDENTITY_INSERT [CartItems] ON;
INSERT INTO [CartItems] ([Id], [UserId], [ItemType], [ItemId], [AddedAt], [Quantity]) VALUES (27, 18, 1, 3, '2026-04-23 00:20:25.889', 0);
INSERT INTO [CartItems] ([Id], [UserId], [ItemType], [ItemId], [AddedAt], [Quantity]) VALUES (28, 18, 1, 1, '2026-04-23 00:40:02.924', 0);
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('CartItems') AND is_identity = 1) SET IDENTITY_INSERT [CartItems] OFF;
GO

-- Dumping data for table Classes
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Classes') AND is_identity = 1) SET IDENTITY_INSERT [Classes] ON;
INSERT INTO [Classes] ([Id], [ClassName], [CourseId], [TeacherId], [StartDate], [EndDate], [Status], [CreatedAt]) VALUES (1, N'A1-Sáng-K1', 1, 2, '2024-03-01 00:00:00.000', '2024-06-30 00:00:00.000', N'Finished', '2024-02-15 08:00:00.000');
INSERT INTO [Classes] ([Id], [ClassName], [CourseId], [TeacherId], [StartDate], [EndDate], [Status], [CreatedAt]) VALUES (2, N'A1-Tối-K1', 1, 3, '2024-03-01 00:00:00.000', '2024-06-30 00:00:00.000', N'Finished', '2024-02-15 08:00:00.000');
INSERT INTO [Classes] ([Id], [ClassName], [CourseId], [TeacherId], [StartDate], [EndDate], [Status], [CreatedAt]) VALUES (3, N'A2-Sáng-K1', 2, 4, '2024-04-01 00:00:00.000', '2024-07-31 00:00:00.000', N'Finished', '2024-03-15 08:00:00.000');
INSERT INTO [Classes] ([Id], [ClassName], [CourseId], [TeacherId], [StartDate], [EndDate], [Status], [CreatedAt]) VALUES (4, N'A1-Sáng-K3', 1, 3, '2026-05-05 00:00:00.000', '2026-08-29 00:00:00.000', N'Upcoming', '2026-04-01 08:00:00.000');
INSERT INTO [Classes] ([Id], [ClassName], [CourseId], [TeacherId], [StartDate], [EndDate], [Status], [CreatedAt]) VALUES (5, N'B1-Sáng-K3', 3, 3, '2026-05-04 00:00:00.000', '2026-08-28 00:00:00.000', N'Upcoming', '2026-04-05 08:00:00.000');
INSERT INTO [Classes] ([Id], [ClassName], [CourseId], [TeacherId], [StartDate], [EndDate], [Status], [CreatedAt]) VALUES (6, N'Tiếng Anh nâng cao dành cho người Việt', 4, 6, '2026-04-23 00:00:00.000', '2026-07-02 00:00:00.000', N'Upcoming', '2026-04-22 23:47:16.748');
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Classes') AND is_identity = 1) SET IDENTITY_INSERT [Classes] OFF;
GO

-- Dumping data for table ClassSchedules
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('ClassSchedules') AND is_identity = 1) SET IDENTITY_INSERT [ClassSchedules] ON;
INSERT INTO [ClassSchedules] ([Id], [ClassId], [DayOfWeek], [StartTime], [EndTime]) VALUES (1, 1, 2, 08:00:00, 10:00:00);
INSERT INTO [ClassSchedules] ([Id], [ClassId], [DayOfWeek], [StartTime], [EndTime]) VALUES (2, 1, 4, 08:00:00, 10:00:00);
INSERT INTO [ClassSchedules] ([Id], [ClassId], [DayOfWeek], [StartTime], [EndTime]) VALUES (3, 1, 6, 08:00:00, 10:00:00);
INSERT INTO [ClassSchedules] ([Id], [ClassId], [DayOfWeek], [StartTime], [EndTime]) VALUES (4, 2, 3, 18:00:00, 20:30:00);
INSERT INTO [ClassSchedules] ([Id], [ClassId], [DayOfWeek], [StartTime], [EndTime]) VALUES (5, 2, 5, 18:00:00, 20:30:00);
INSERT INTO [ClassSchedules] ([Id], [ClassId], [DayOfWeek], [StartTime], [EndTime]) VALUES (6, 4, 2, 08:00:00, 10:00:00);
INSERT INTO [ClassSchedules] ([Id], [ClassId], [DayOfWeek], [StartTime], [EndTime]) VALUES (7, 4, 4, 08:00:00, 10:00:00);
INSERT INTO [ClassSchedules] ([Id], [ClassId], [DayOfWeek], [StartTime], [EndTime]) VALUES (8, 4, 6, 08:00:00, 10:00:00);
INSERT INTO [ClassSchedules] ([Id], [ClassId], [DayOfWeek], [StartTime], [EndTime]) VALUES (15, 6, 2, 18:00:00, 19:30:00);
INSERT INTO [ClassSchedules] ([Id], [ClassId], [DayOfWeek], [StartTime], [EndTime]) VALUES (16, 6, 4, 12:00:00, 13:40:00);
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('ClassSchedules') AND is_identity = 1) SET IDENTITY_INSERT [ClassSchedules] OFF;
GO

-- Dumping data for table Courses
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Courses') AND is_identity = 1) SET IDENTITY_INSERT [Courses] ON;
INSERT INTO [Courses] ([Id], [Name], [Level], [Description], [TuitionFee], [ThumbnailUrl], [IsActive]) VALUES (1, N'Tiếng Anh Cơ Bản', N'A1', N'Khóa học dành cho người mới bắt đầu, học bảng chữ cái, phát âm và giao tiếp căn bản.', 2500.00, NULL, 1);
INSERT INTO [Courses] ([Id], [Name], [Level], [Description], [TuitionFee], [ThumbnailUrl], [IsActive]) VALUES (2, N'Tiếng Anh Sơ Cấp', N'A2', N'Xây dựng nền tảng từ vựng 1000 từ, ngữ pháp căn bản và kỹ năng hội thoại đơn giản.', 3000.00, NULL, 1);
INSERT INTO [Courses] ([Id], [Name], [Level], [Description], [TuitionFee], [ThumbnailUrl], [IsActive]) VALUES (3, N'Tiếng Anh Trung Cấp', N'B1', N'Nâng cao kỹ năng nghe nói đọc viết, luyện thi B1 CEFR, từ vựng 2500 từ.', 3500.00, NULL, 1);
INSERT INTO [Courses] ([Id], [Name], [Level], [Description], [TuitionFee], [ThumbnailUrl], [IsActive]) VALUES (4, N'Tiếng Anh Trung Cấp Trên', N'B2', N'Chuẩn bị cho kỳ thi IELTS 5.0-6.0, TOEFL iBT 60-80, giao tiếp lưu loát.', 4500.00, NULL, 1);
INSERT INTO [Courses] ([Id], [Name], [Level], [Description], [TuitionFee], [ThumbnailUrl], [IsActive]) VALUES (5, N'Tiếng Anh Cao Cấp', N'C1', N'Luyện thi IELTS 6.5-7.5, TOEFL iBT 90-110, tiếng Anh học thuật và thương mại.', 5500.00, NULL, 1);
INSERT INTO [Courses] ([Id], [Name], [Level], [Description], [TuitionFee], [ThumbnailUrl], [IsActive]) VALUES (6, N'Tiếng Anh Thành Thạo', N'C2', N'Cấp độ cao nhất, luyện thi IELTS 8.0+, tiếng Anh bản ngữ, phiên dịch.', 7000.00, NULL, 1);
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Courses') AND is_identity = 1) SET IDENTITY_INSERT [Courses] OFF;
GO

-- Dumping data for table EmailLogs
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('EmailLogs') AND is_identity = 1) SET IDENTITY_INSERT [EmailLogs] ON;
INSERT INTO [EmailLogs] ([Id], [UserId], [Recipient], [Subject], [Body], [Type], [IsSent], [ErrorMessage], [CreatedAt]) VALUES (0, 18, N'mndkhanh@gmail.com', N'Đăng ký tài khoản thành công - English Center', N'
                <h2>Xin chào Mai Nguyễn Duy Khánh,</h2>
                <p>Tài khoản của bạn đã được xác thực và tạo thành công.</p>
                <p><strong>Tên đăng nhập:</strong> mndkhanh</p>
                <p>Bạn có thể đăng nhập ngay để bắt đầu sử dụng hệ thống.</p>
                <p>Trân trọng,<br />English Center</p>
            ', 0, 1, NULL, '2026-04-22 16:02:09.158');
INSERT INTO [EmailLogs] ([Id], [UserId], [Recipient], [Subject], [Body], [Type], [IsSent], [ErrorMessage], [CreatedAt]) VALUES (1, 18, N'mndkhanh@gmail.com', N'Xác nhận đăng ký lớp B1-Sáng-K3', N'<br> Xin chào Mai Nguyễn Duy Khánh,<br><br> Đăng ký của bạn đã được xác nhận!<br><br> Thông tin lớp học:<br> - Lớp: B1-Sáng-K3<br> - Khóa học: Tiếng Anh Trung Cấp<br> - Giáo viên: Trần Văn Minh<br> - Ngày bắt đầu: 04/05/2026<br><br> Lịch học:<br> Chưa có lịch học<br><br> Mã xác nhận: 712793<br><br> Vui lòng đến trung tâm đúng giờ và mang theo mã xác nhận này.<br><br> Trân trọng,<br> English Center<br>        ', 0, 1, NULL, '2026-04-22 16:43:38.189');
INSERT INTO [EmailLogs] ([Id], [UserId], [Recipient], [Subject], [Body], [Type], [IsSent], [ErrorMessage], [CreatedAt]) VALUES (2, 18, N'mndkhanh@gmail.com', N'Xác nhận đăng ký lớp A1-Sáng-K3', N'<br> Xin chào Mai Nguyễn Duy Khánh,<br><br> Đăng ký của bạn đã được xác nhận!<br><br> Thông tin lớp học:<br> - Lớp: A1-Sáng-K3<br> - Khóa học: Tiếng Anh Cơ Bản<br> - Giáo viên: Trần Văn Minh<br> - Ngày bắt đầu: 05/05/2026<br><br> Lịch học:<br> - Thứ Ba: 08:00 - 10:00<br>- Thứ Năm: 08:00 - 10:00<br>- Thứ Bảy: 08:00 - 10:00<br><br> Mã xác nhận: 639573<br><br> Vui lòng đến trung tâm đúng giờ và mang theo mã xác nhận này.<br><br> Trân trọng,<br> English Center<br>        ', 0, 1, NULL, '2026-04-22 16:43:38.799');
INSERT INTO [EmailLogs] ([Id], [UserId], [Recipient], [Subject], [Body], [Type], [IsSent], [ErrorMessage], [CreatedAt]) VALUES (3, 7, N'an.nguyen@gmail.com', N'Xác nhận đăng ký lớp A1-Sáng-K3', N'<br> Xin chào Nguyễn Văn An,<br><br> Đăng ký của bạn đã được xác nhận!<br><br> Thông tin lớp học:<br> - Lớp: A1-Sáng-K3<br> - Khóa học: Tiếng Anh Cơ Bản<br> - Giáo viên: Trần Văn Minh<br> - Ngày bắt đầu: 05/05/2026<br><br> Lịch học:<br> - Thứ Ba: 08:00 - 10:00<br>- Thứ Năm: 08:00 - 10:00<br>- Thứ Bảy: 08:00 - 10:00<br><br> Mã xác nhận: 930614<br><br> Vui lòng đến trung tâm đúng giờ và mang theo mã xác nhận này.<br><br> Trân trọng,<br> English Center<br>        ', 0, 1, NULL, '2026-04-22 16:43:39.310');
INSERT INTO [EmailLogs] ([Id], [UserId], [Recipient], [Subject], [Body], [Type], [IsSent], [ErrorMessage], [CreatedAt]) VALUES (4, 18, N'mndkhanh@gmail.com', N'Xác nhận đăng ký lớp Tiếng Anh nâng cao dành cho người Việt', N'<br> Xin chào Mai Nguyễn Duy Khánh,<br><br> Đăng ký của bạn đã được xác nhận!<br><br> Thông tin lớp học:<br> - Lớp: Tiếng Anh nâng cao dành cho người Việt<br> - Khóa học: Tiếng Anh Trung Cấp Trên<br> - Giáo viên: Hoàng Thị Thu<br> - Ngày bắt đầu: 23/04/2026<br><br> Lịch học:<br> - Thứ Ba: 06:00 - 07:30<br>- Thứ Năm: 12:00 - 01:40<br><br> Mã xác nhận: 266487<br><br> Vui lòng đến trung tâm đúng giờ và mang theo mã xác nhận này.<br><br> Trân trọng,<br> English Center<br>        ', 0, 1, NULL, '2026-04-22 16:52:17.041');
INSERT INTO [EmailLogs] ([Id], [UserId], [Recipient], [Subject], [Body], [Type], [IsSent], [ErrorMessage], [CreatedAt]) VALUES (5, 18, N'mndkhanh@gmail.com', N'Xác nhận đăng ký lớp Tiếng Anh nâng cao dành cho người Việt', N'<br> Xin chào Mai Nguyễn Duy Khánh,<br><br> Đăng ký của bạn đã được xác nhận!<br><br> Thông tin lớp học:<br> - Lớp: Tiếng Anh nâng cao dành cho người Việt<br> - Khóa học: Tiếng Anh Trung Cấp Trên<br> - Giáo viên: Hoàng Thị Thu<br> - Ngày bắt đầu: 23/04/2026<br><br> Lịch học:<br> - Thứ Ba: 06:00 - 07:30<br>- Thứ Năm: 12:00 - 01:40<br><br> Mã xác nhận: 443752<br><br> Vui lòng đến trung tâm đúng giờ và mang theo mã xác nhận này.<br><br> Trân trọng,<br> English Center<br>        ', 0, 1, NULL, '2026-04-22 16:55:18.468');
INSERT INTO [EmailLogs] ([Id], [UserId], [Recipient], [Subject], [Body], [Type], [IsSent], [ErrorMessage], [CreatedAt]) VALUES (6, 18, N'mndkhanh@gmail.com', N'Xác nhận đăng ký lớp B1-Sáng-K3', N'<br> Xin chào Mai Nguyễn Duy Khánh,<br><br> Đăng ký của bạn đã được xác nhận!<br><br> Thông tin lớp học:<br> - Lớp: B1-Sáng-K3<br> - Khóa học: Tiếng Anh Trung Cấp<br> - Giáo viên: Trần Văn Minh<br> - Ngày bắt đầu: 04/05/2026<br><br> Lịch học:<br> Chưa có lịch học<br><br> Mã xác nhận: 723600<br><br> Vui lòng đến trung tâm đúng giờ và mang theo mã xác nhận này.<br><br> Trân trọng,<br> English Center<br>        ', 0, 1, NULL, '2026-04-22 16:59:10.086');
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('EmailLogs') AND is_identity = 1) SET IDENTITY_INSERT [EmailLogs] OFF;
GO

-- Dumping data for table EmailOtps
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('EmailOtps') AND is_identity = 1) SET IDENTITY_INSERT [EmailOtps] ON;
INSERT INTO [EmailOtps] ([Id], [Email], [OtpCode], [Type], [ExpiresAt], [IsUsed], [CreatedAt]) VALUES (0, N'mndkhanh@gmail.com', N'470599', 1, '2026-04-22 16:06:30.405', 1, '2026-04-22 16:01:30.405');
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('EmailOtps') AND is_identity = 1) SET IDENTITY_INSERT [EmailOtps] OFF;
GO

-- Dumping data for table Enrollments
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Enrollments') AND is_identity = 1) SET IDENTITY_INSERT [Enrollments] ON;
INSERT INTO [Enrollments] ([Id], [StudentId], [ClassId], [EnrolledAt], [Status], [ConfirmationCode], [ConfirmedAt]) VALUES (1, 7, 1, '2024-02-15 10:00:00.000', N'Confirmed', NULL, NULL);
INSERT INTO [Enrollments] ([Id], [StudentId], [ClassId], [EnrolledAt], [Status], [ConfirmationCode], [ConfirmedAt]) VALUES (2, 8, 1, '2024-02-15 10:30:00.000', N'Confirmed', NULL, NULL);
INSERT INTO [Enrollments] ([Id], [StudentId], [ClassId], [EnrolledAt], [Status], [ConfirmationCode], [ConfirmedAt]) VALUES (3, 7, 4, '2026-04-15 08:00:00.000', N'Confirmed', N'930614', '2026-04-22 23:43:33.839');
INSERT INTO [Enrollments] ([Id], [StudentId], [ClassId], [EnrolledAt], [Status], [ConfirmationCode], [ConfirmedAt]) VALUES (4, 18, 4, '2026-04-22 23:15:40.884', N'Registered', N'639573', '2026-04-22 23:43:33.115');
INSERT INTO [Enrollments] ([Id], [StudentId], [ClassId], [EnrolledAt], [Status], [ConfirmationCode], [ConfirmedAt]) VALUES (5, 18, 5, '2026-04-22 23:32:09.364', N'Confirmed', N'723600', '2026-04-22 23:59:04.596');
INSERT INTO [Enrollments] ([Id], [StudentId], [ClassId], [EnrolledAt], [Status], [ConfirmationCode], [ConfirmedAt]) VALUES (6, 18, 6, '2026-04-22 23:51:58.490', N'Confirmed', N'443752', '2026-04-22 23:55:12.861');
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Enrollments') AND is_identity = 1) SET IDENTITY_INSERT [Enrollments] OFF;
GO

-- Dumping data for table OrderItems
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('OrderItems') AND is_identity = 1) SET IDENTITY_INSERT [OrderItems] ON;
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (20, 1, 1, 1, 1500.00, NULL, 0);
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (21, 1, 1, 2, 1200.00, NULL, 0);
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (22, 2, 1, 3, 3500.00, NULL, 0);
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (23, 2, 1, 2, 1200.00, NULL, 0);
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (24, 3, 1, 1, 1500.00, NULL, 0);
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (25, 3, 0, 4, 2500.00, NULL, 0);
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (26, 4, 1, 3, 3500.00, NULL, 0);
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (27, 4, 1, 2, 1200.00, NULL, 0);
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (28, 4, 0, 4, 2500.00, NULL, 0);
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (29, 5, 1, 3, 3500.00, NULL, 0);
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (30, 6, 1, 4, 4500.00, NULL, 1);
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (31, 6, 1, 3, 3500.00, NULL, 1);
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (32, 6, 1, 2, 1200.00, NULL, 1);
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (33, 7, 1, 3, 3500.00, NULL, 1);
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (34, 7, 1, 2, 1200.00, NULL, 1);
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (35, 8, 1, 2, 1200.00, NULL, 1);
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (36, 9, 1, 4, 4500.00, NULL, 1);
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (37, 9, 1, 3, 3500.00, NULL, 1);
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (38, 9, 1, 2, 1200.00, NULL, 1);
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (39, 10, 1, 2, 1200.00, NULL, 1);
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (40, 11, 0, 5, 3500.00, NULL, 1);
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (41, 11, 1, 3, 3500.00, NULL, 1);
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (42, 11, 1, 2, 1200.00, NULL, 1);
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (43, 12, 0, 6, 4500.00, NULL, 1);
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('OrderItems') AND is_identity = 1) SET IDENTITY_INSERT [OrderItems] OFF;
GO

-- Dumping data for table Orders
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Orders') AND is_identity = 1) SET IDENTITY_INSERT [Orders] ON;
INSERT INTO [Orders] ([Id], [UserId], [TotalAmount], [BalanceUsed], [PayOSAmount], [OrderCode], [Status], [CreatedAt]) VALUES (1, 18, 2700.00, 0.00, 2700.00, 20260422230246, 0, '2026-04-22 23:02:46.341');
INSERT INTO [Orders] ([Id], [UserId], [TotalAmount], [BalanceUsed], [PayOSAmount], [OrderCode], [Status], [CreatedAt]) VALUES (2, 18, 4700.00, 0.00, 4700.00, 20260422230611, 0, '2026-04-22 23:06:11.306');
INSERT INTO [Orders] ([Id], [UserId], [TotalAmount], [BalanceUsed], [PayOSAmount], [OrderCode], [Status], [CreatedAt]) VALUES (3, 18, 4000.00, 0.00, 4000.00, 20260422231306, 2, '2026-04-22 23:13:06.552');
INSERT INTO [Orders] ([Id], [UserId], [TotalAmount], [BalanceUsed], [PayOSAmount], [OrderCode], [Status], [CreatedAt]) VALUES (4, 18, 7200.00, 0.00, 7200.00, 20260422231514, 1, '2026-04-22 23:15:14.072');
INSERT INTO [Orders] ([Id], [UserId], [TotalAmount], [BalanceUsed], [PayOSAmount], [OrderCode], [Status], [CreatedAt]) VALUES (5, 18, 3500.00, 3500.00, 0.00, 20260422232139, 1, '2026-04-22 23:21:39.255');
INSERT INTO [Orders] ([Id], [UserId], [TotalAmount], [BalanceUsed], [PayOSAmount], [OrderCode], [Status], [CreatedAt]) VALUES (6, 18, 9200.00, 6500.00, 2700.00, 20260422232249, 0, '2026-04-22 23:22:49.711');
INSERT INTO [Orders] ([Id], [UserId], [TotalAmount], [BalanceUsed], [PayOSAmount], [OrderCode], [Status], [CreatedAt]) VALUES (7, 18, 4700.00, 0.00, 4700.00, 20260422232537, 1, '2026-04-22 23:25:37.087');
INSERT INTO [Orders] ([Id], [UserId], [TotalAmount], [BalanceUsed], [PayOSAmount], [OrderCode], [Status], [CreatedAt]) VALUES (8, 18, 1200.00, 1200.00, 0.00, 20260422232703, 1, '2026-04-22 23:27:03.530');
INSERT INTO [Orders] ([Id], [UserId], [TotalAmount], [BalanceUsed], [PayOSAmount], [OrderCode], [Status], [CreatedAt]) VALUES (9, 18, 9200.00, 5300.00, 3900.00, 20260422232713, 1, '2026-04-22 23:27:13.184');
INSERT INTO [Orders] ([Id], [UserId], [TotalAmount], [BalanceUsed], [PayOSAmount], [OrderCode], [Status], [CreatedAt]) VALUES (10, 18, 1200.00, 0.00, 1200.00, 20260422233008, 2, '2026-04-22 23:30:08.705');
INSERT INTO [Orders] ([Id], [UserId], [TotalAmount], [BalanceUsed], [PayOSAmount], [OrderCode], [Status], [CreatedAt]) VALUES (11, 18, 8200.00, 0.00, 8200.00, 20260422233142, 1, '2026-04-22 23:31:42.496');
INSERT INTO [Orders] ([Id], [UserId], [TotalAmount], [BalanceUsed], [PayOSAmount], [OrderCode], [Status], [CreatedAt]) VALUES (12, 18, 4500.00, 0.00, 4500.00, 20260422235133, 1, '2026-04-22 23:51:33.191');
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Orders') AND is_identity = 1) SET IDENTITY_INSERT [Orders] OFF;
GO

-- Dumping data for table PurchasableItems
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('PurchasableItems') AND is_identity = 1) SET IDENTITY_INSERT [PurchasableItems] ON;
INSERT INTO [PurchasableItems] ([Id], [Name], [Description], [Price], [ImageUrl], [IsActive], [CreatedAt]) VALUES (1, N'Giáo trình IELTS Foundation', N'Sách học IELTS cho người mới.', 1800.00, N'https://cdn1.fahasa.com/media/catalog/product/z/_/z.jpg', 1, '2026-04-22 22:36:26.850');
INSERT INTO [PurchasableItems] ([Id], [Name], [Description], [Price], [ImageUrl], [IsActive], [CreatedAt]) VALUES (2, N'Bộ đề thi thử Cambridge B1', N'10 đề thi chuẩn cấu trúc.', 1200.00, N'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRNeQW7ZwTfqytOTSlgt7GQeY4AVkvmj4f0cA&s', 1, '2026-04-22 22:36:26.850');
INSERT INTO [PurchasableItems] ([Id], [Name], [Description], [Price], [ImageUrl], [IsActive], [CreatedAt]) VALUES (3, N'Tai nghe học tiếng Anh', N'Tai nghe chuyên dụng luyện nghe.', 3500.00, N'https://cdn.tgdd.vn/Products/Images/54/251139/chup-tai-rapoo-h120-den-15-600x600.jpg', 1, '2026-04-22 22:36:26.850');
INSERT INTO [PurchasableItems] ([Id], [Name], [Description], [Price], [ImageUrl], [IsActive], [CreatedAt]) VALUES (4, N'Từ điển Oxford English', N'Phiên bản bìa cứng mới nhất.', 4500.00, N'https://www.netabooks.vn/Data/Sites/1/Product/34986/tu-dien-oxford-anh-viet-350000-tu-hop-cung-xanh.jpg', 1, '2026-04-22 22:36:26.850');
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('PurchasableItems') AND is_identity = 1) SET IDENTITY_INSERT [PurchasableItems] OFF;
GO

-- Dumping data for table Users
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Users') AND is_identity = 1) SET IDENTITY_INSERT [Users] ON;
INSERT INTO [Users] ([Id], [FullName], [Email], [Username], [Phone], [PasswordHash], [Role], [AvatarUrl], [IsActive], [CreatedAt], [WalletBalance], [RegistrationFeePaid]) VALUES (1, N'Admin DOL', N'admin@dol.com', N'admin', N'0901000001', N'$2a$11$p/XqWwgolcMuN5tWWPgTGOHpVGYg7I62dqfI5Lyl5pOOdSyXrDGsG', N'Admin', NULL, 1, '2024-01-01 08:00:00.000', 1000000.00, 1);
INSERT INTO [Users] ([Id], [FullName], [Email], [Username], [Phone], [PasswordHash], [Role], [AvatarUrl], [IsActive], [CreatedAt], [WalletBalance], [RegistrationFeePaid]) VALUES (2, N'Nguyễn Thị Hương', N'huong.nguyen@dol.com', N'huong.nguyen', N'0902000001', N'$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', N'Teacher', NULL, 1, '2024-01-05 08:00:00.000', 0.00, 1);
INSERT INTO [Users] ([Id], [FullName], [Email], [Username], [Phone], [PasswordHash], [Role], [AvatarUrl], [IsActive], [CreatedAt], [WalletBalance], [RegistrationFeePaid]) VALUES (3, N'Trần Văn Minh', N'minh.tran@dol.com', N'minh.tran', N'0902000002', N'$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', N'Teacher', NULL, 1, '2024-01-05 08:00:00.000', 0.00, 1);
INSERT INTO [Users] ([Id], [FullName], [Email], [Username], [Phone], [PasswordHash], [Role], [AvatarUrl], [IsActive], [CreatedAt], [WalletBalance], [RegistrationFeePaid]) VALUES (4, N'Phạm Thanh Lan', N'lan.pham@dol.com', N'lan.pham', N'0902000003', N'$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', N'Teacher', NULL, 1, '2024-01-06 08:00:00.000', 0.00, 1);
INSERT INTO [Users] ([Id], [FullName], [Email], [Username], [Phone], [PasswordHash], [Role], [AvatarUrl], [IsActive], [CreatedAt], [WalletBalance], [RegistrationFeePaid]) VALUES (5, N'Lê Quốc Bảo', N'bao.le@dol.com', N'bao.le', N'0902000004', N'$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', N'Teacher', NULL, 1, '2024-01-06 08:00:00.000', 0.00, 1);
INSERT INTO [Users] ([Id], [FullName], [Email], [Username], [Phone], [PasswordHash], [Role], [AvatarUrl], [IsActive], [CreatedAt], [WalletBalance], [RegistrationFeePaid]) VALUES (6, N'Hoàng Thị Thu', N'thu.hoang@dol.com', N'thu.hoang', N'0902000005', N'$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', N'Teacher', NULL, 1, '2024-01-07 08:00:00.000', 0.00, 1);
INSERT INTO [Users] ([Id], [FullName], [Email], [Username], [Phone], [PasswordHash], [Role], [AvatarUrl], [IsActive], [CreatedAt], [WalletBalance], [RegistrationFeePaid]) VALUES (7, N'Nguyễn Văn An', N'an.nguyen@gmail.com', N'an.nguyen', N'0903000001', N'$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', N'Student', NULL, 1, '2024-02-01 09:00:00.000', 500000.00, 1);
INSERT INTO [Users] ([Id], [FullName], [Email], [Username], [Phone], [PasswordHash], [Role], [AvatarUrl], [IsActive], [CreatedAt], [WalletBalance], [RegistrationFeePaid]) VALUES (8, N'Trần Thị Bích', N'bich.tran@gmail.com', N'bich.tran', N'0903000002', N'$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', N'Student', NULL, 1, '2024-02-01 09:00:00.000', 0.00, 1);
INSERT INTO [Users] ([Id], [FullName], [Email], [Username], [Phone], [PasswordHash], [Role], [AvatarUrl], [IsActive], [CreatedAt], [WalletBalance], [RegistrationFeePaid]) VALUES (9, N'Phạm Văn Cường', N'cuong.pham@gmail.com', N'cuong.pham', N'0903000003', N'$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', N'Student', NULL, 1, '2024-02-02 09:00:00.000', 0.00, 1);
INSERT INTO [Users] ([Id], [FullName], [Email], [Username], [Phone], [PasswordHash], [Role], [AvatarUrl], [IsActive], [CreatedAt], [WalletBalance], [RegistrationFeePaid]) VALUES (10, N'Lê Thị Dung', N'dung.le@gmail.com', N'dung.le', N'0903000004', N'$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', N'Student', NULL, 1, '2024-02-02 09:00:00.000', 0.00, 1);
INSERT INTO [Users] ([Id], [FullName], [Email], [Username], [Phone], [PasswordHash], [Role], [AvatarUrl], [IsActive], [CreatedAt], [WalletBalance], [RegistrationFeePaid]) VALUES (11, N'Hoàng Văn Em', N'em.hoang@gmail.com', N'em.hoang', N'0903000005', N'$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', N'Student', NULL, 1, '2024-02-03 09:00:00.000', 0.00, 1);
INSERT INTO [Users] ([Id], [FullName], [Email], [Username], [Phone], [PasswordHash], [Role], [AvatarUrl], [IsActive], [CreatedAt], [WalletBalance], [RegistrationFeePaid]) VALUES (12, N'Võ Thị Phương', N'phuong.vo@gmail.com', N'phuong.vo', N'0903000006', N'$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', N'Student', NULL, 1, '2024-02-03 09:00:00.000', 0.00, 1);
INSERT INTO [Users] ([Id], [FullName], [Email], [Username], [Phone], [PasswordHash], [Role], [AvatarUrl], [IsActive], [CreatedAt], [WalletBalance], [RegistrationFeePaid]) VALUES (13, N'Đặng Văn Giang', N'giang.dang@gmail.com', N'giang.dang', N'0903000007', N'$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', N'Student', NULL, 1, '2024-02-04 09:00:00.000', 0.00, 1);
INSERT INTO [Users] ([Id], [FullName], [Email], [Username], [Phone], [PasswordHash], [Role], [AvatarUrl], [IsActive], [CreatedAt], [WalletBalance], [RegistrationFeePaid]) VALUES (14, N'Bùi Thị Hà', N'ha.bui@gmail.com', N'ha.bui', N'0903000008', N'$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', N'Student', NULL, 1, '2024-02-04 09:00:00.000', 0.00, 1);
INSERT INTO [Users] ([Id], [FullName], [Email], [Username], [Phone], [PasswordHash], [Role], [AvatarUrl], [IsActive], [CreatedAt], [WalletBalance], [RegistrationFeePaid]) VALUES (15, N'Ngô Văn Hùng', N'hung.ngo@gmail.com', N'hung.ngo', N'0903000009', N'$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', N'Student', NULL, 1, '2024-02-05 09:00:00.000', 0.00, 1);
INSERT INTO [Users] ([Id], [FullName], [Email], [Username], [Phone], [PasswordHash], [Role], [AvatarUrl], [IsActive], [CreatedAt], [WalletBalance], [RegistrationFeePaid]) VALUES (16, N'Đinh Thị Khanh', N'khanh.dinh@gmail.com', N'khanh.dinh', N'0903000010', N'$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', N'Student', NULL, 1, '2024-02-05 09:00:00.000', 0.00, 1);
INSERT INTO [Users] ([Id], [FullName], [Email], [Username], [Phone], [PasswordHash], [Role], [AvatarUrl], [IsActive], [CreatedAt], [WalletBalance], [RegistrationFeePaid]) VALUES (17, N'Chu Văn Anh', N'anh.chu@gmail.com', N'anh.chu', N'0903000025', N'$2a$11$BvWcRd9paw1M5suEaFfOweUKwe9UpXOOJQbAEIRithXAv/.mcMfJy', N'Student', NULL, 1, '2024-03-03 09:00:00.000', 0.00, 1);
INSERT INTO [Users] ([Id], [FullName], [Email], [Username], [Phone], [PasswordHash], [Role], [AvatarUrl], [IsActive], [CreatedAt], [WalletBalance], [RegistrationFeePaid]) VALUES (18, N'Mai Nguyễn Duy Khánh', N'mndkhanh@gmail.com', N'mndkhanh', N'0362718422', N'$2a$11$Si3YM5MhfzgbwNf0HbRYbuO62D9NTLhLiSpHXlyeqhlXVXT.lQlY.', N'Student', NULL, 1, '2026-04-22 23:01:53.348', 10000.00, 0);
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Users') AND is_identity = 1) SET IDENTITY_INSERT [Users] OFF;
GO

-- Re-enable all constraints
EXEC sp_MSforeachtable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL';
GO

