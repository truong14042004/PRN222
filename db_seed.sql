-- EnglishCenterDB Full Data Dump
-- Generated at: 04/23/2026 03:05:53
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
INSERT INTO [Attendances] ([Id], [ClassId], [StudentId], [Date], [IsPresent], [Note]) VALUES (5, 6, 18, '2026-04-23 00:00:00.000', 1, N'HELLO');
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Attendances') AND is_identity = 1) SET IDENTITY_INSERT [Attendances] OFF;
GO

-- Dumping data for table Classes
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Classes') AND is_identity = 1) SET IDENTITY_INSERT [Classes] ON;
INSERT INTO [Classes] ([Id], [ClassName], [CourseId], [TeacherId], [StartDate], [EndDate], [Status], [CreatedAt]) VALUES (1, N'A1-Sáng-K1', 1, 2, '2024-03-01 00:00:00.000', '2024-06-30 00:00:00.000', N'Finished', '2024-02-15 08:00:00.000');
INSERT INTO [Classes] ([Id], [ClassName], [CourseId], [TeacherId], [StartDate], [EndDate], [Status], [CreatedAt]) VALUES (2, N'A1-Tối-K1', 1, 3, '2024-03-01 00:00:00.000', '2024-06-30 00:00:00.000', N'Finished', '2024-02-15 08:00:00.000');
INSERT INTO [Classes] ([Id], [ClassName], [CourseId], [TeacherId], [StartDate], [EndDate], [Status], [CreatedAt]) VALUES (3, N'A2-Sáng-K1', 2, 4, '2024-04-01 00:00:00.000', '2024-07-31 00:00:00.000', N'Finished', '2024-03-15 08:00:00.000');
INSERT INTO [Classes] ([Id], [ClassName], [CourseId], [TeacherId], [StartDate], [EndDate], [Status], [CreatedAt]) VALUES (4, N'A1-Sáng-K3', 1, 3, '2026-05-05 00:00:00.000', '2026-08-29 00:00:00.000', N'Upcoming', '2026-04-01 08:00:00.000');
INSERT INTO [Classes] ([Id], [ClassName], [CourseId], [TeacherId], [StartDate], [EndDate], [Status], [CreatedAt]) VALUES (5, N'B1-Sáng-K3', 3, 3, '2026-05-04 00:00:00.000', '2026-08-28 00:00:00.000', N'Upcoming', '2026-04-05 08:00:00.000');
INSERT INTO [Classes] ([Id], [ClassName], [CourseId], [TeacherId], [StartDate], [EndDate], [Status], [CreatedAt]) VALUES (6, N'Tiếng Anh nâng cao dành cho người Việt', 4, 19, '2026-04-24 00:00:00.000', '2026-08-07 00:00:00.000', N'Ongoing', '2026-04-22 23:47:16.748');
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Classes') AND is_identity = 1) SET IDENTITY_INSERT [Classes] OFF;
GO

-- Dumping data for table ClassSchedules
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('ClassSchedules') AND is_identity = 1) SET IDENTITY_INSERT [ClassSchedules] ON;
INSERT INTO [ClassSchedules] ([Id], [ClassId], [DayOfWeek], [StartTime], [EndTime]) VALUES (19, 6, 5, 04:38:00, 07:41:00);
INSERT INTO [ClassSchedules] ([Id], [ClassId], [DayOfWeek], [StartTime], [EndTime]) VALUES (20, 6, 6, 02:38:00, 03:38:00);
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('ClassSchedules') AND is_identity = 1) SET IDENTITY_INSERT [ClassSchedules] OFF;
GO

-- Dumping data for table Courses
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Courses') AND is_identity = 1) SET IDENTITY_INSERT [Courses] ON;
INSERT INTO [Courses] ([Id], [Name], [Level], [Description], [TuitionFee], [ThumbnailUrl], [IsActive]) VALUES (1, N'Tiếng Anh Cơ Bản', N'A1', N'Khóa học dành cho người mới bắt đầu, học bảng chữ cái, phát âm và giao tiếp căn bản.', 2500.00, N'https://bizweb.dktcdn.net/thumb/large/100/469/746/products/cc4943fc-aadf-4f80-b5ec-5a71bc9a36b1.jpg?v=1725865373883', 1);
INSERT INTO [Courses] ([Id], [Name], [Level], [Description], [TuitionFee], [ThumbnailUrl], [IsActive]) VALUES (2, N'Tiếng Anh Sơ Cấp', N'A2', N'Xây dựng nền tảng từ vựng 1000 từ, ngữ pháp căn bản và kỹ năng hội thoại đơn giản.', 3000.00, N'https://ntvcdn.b-cdn.net/ntv/wp-content/uploads/2018/11/GrammarLearningCurve_SoCap_1.jpg', 1);
INSERT INTO [Courses] ([Id], [Name], [Level], [Description], [TuitionFee], [ThumbnailUrl], [IsActive]) VALUES (3, N'Tiếng Anh Trung Cấp', N'B1', N'Nâng cao kỹ năng nghe nói đọc viết, luyện thi B1 CEFR, từ vựng 2500 từ.', 3500.00, N'https://truongkinhtecongnghe.edu.vn/wp-content/uploads/2024/06/tuyen-snh-trung-cap-tieng-anh.jpg', 1);
INSERT INTO [Courses] ([Id], [Name], [Level], [Description], [TuitionFee], [ThumbnailUrl], [IsActive]) VALUES (4, N'Tiếng Anh Trung Cấp Trên', N'B2', N'Chuẩn bị cho kỳ thi IELTS 5.0-6.0, TOEFL iBT 60-80, giao tiếp lưu loát.', 4500.00, N'https://cdn1.fahasa.com/media/catalog/product/i/m/image_192628.jpg', 1);
INSERT INTO [Courses] ([Id], [Name], [Level], [Description], [TuitionFee], [ThumbnailUrl], [IsActive]) VALUES (5, N'Tiếng Anh Cao Cấp', N'C1', N'Luyện thi IELTS 6.5-7.5, TOEFL iBT 90-110, tiếng Anh học thuật và thương mại.', 5500.00, N'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS2ncrph9_fJ9T3zkRj8sVh2zQbvRhSsRaeBQ&s', 1);
INSERT INTO [Courses] ([Id], [Name], [Level], [Description], [TuitionFee], [ThumbnailUrl], [IsActive]) VALUES (6, N'Tiếng Anh Thành Thạo', N'C2', N'Cấp độ cao nhất, luyện thi IELTS 8.0+, tiếng Anh bản ngữ, phiên dịch.', 7000.00, N'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSqMyMkgBcIM0smXzGD3gnHQ_WZ3WUNTCXCCw&s', 1);
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
INSERT INTO [EmailLogs] ([Id], [UserId], [Recipient], [Subject], [Body], [Type], [IsSent], [ErrorMessage], [CreatedAt]) VALUES (1, 18, N'mndkhanh@gmail.com', N'Xác nhận đăng ký lớp B1-Sáng-K3', N'
<br> Xin chào Mai Nguyễn Duy Khánh,
<br>
<br> Đăng ký của bạn đã được xác nhận!
<br>
<br> Thông tin lớp học:
<br> - Lớp: B1-Sáng-K3
<br> - Khóa học: Tiếng Anh Trung Cấp
<br> - Giáo viên: Trần Văn Minh
<br> - Ngày bắt đầu: 04/05/2026
<br>
<br> Lịch học:
<br> Chưa có lịch học
<br>
<br> Mã xác nhận: 712793
<br>
<br> Vui lòng đến trung tâm đúng giờ và mang theo mã xác nhận này.
<br>
<br> Trân trọng,
<br> English Center
<br>        ', 0, 1, NULL, '2026-04-22 16:43:38.189');
INSERT INTO [EmailLogs] ([Id], [UserId], [Recipient], [Subject], [Body], [Type], [IsSent], [ErrorMessage], [CreatedAt]) VALUES (2, 18, N'mndkhanh@gmail.com', N'Xác nhận đăng ký lớp A1-Sáng-K3', N'
<br> Xin chào Mai Nguyễn Duy Khánh,
<br>
<br> Đăng ký của bạn đã được xác nhận!
<br>
<br> Thông tin lớp học:
<br> - Lớp: A1-Sáng-K3
<br> - Khóa học: Tiếng Anh Cơ Bản
<br> - Giáo viên: Trần Văn Minh
<br> - Ngày bắt đầu: 05/05/2026
<br>
<br> Lịch học:
<br> - Thứ Ba: 08:00 - 10:00<br>- Thứ Năm: 08:00 - 10:00<br>- Thứ Bảy: 08:00 - 10:00
<br>
<br> Mã xác nhận: 639573
<br>
<br> Vui lòng đến trung tâm đúng giờ và mang theo mã xác nhận này.
<br>
<br> Trân trọng,
<br> English Center
<br>        ', 0, 1, NULL, '2026-04-22 16:43:38.799');
INSERT INTO [EmailLogs] ([Id], [UserId], [Recipient], [Subject], [Body], [Type], [IsSent], [ErrorMessage], [CreatedAt]) VALUES (3, 7, N'an.nguyen@gmail.com', N'Xác nhận đăng ký lớp A1-Sáng-K3', N'
<br> Xin chào Nguyễn Văn An,
<br>
<br> Đăng ký của bạn đã được xác nhận!
<br>
<br> Thông tin lớp học:
<br> - Lớp: A1-Sáng-K3
<br> - Khóa học: Tiếng Anh Cơ Bản
<br> - Giáo viên: Trần Văn Minh
<br> - Ngày bắt đầu: 05/05/2026
<br>
<br> Lịch học:
<br> - Thứ Ba: 08:00 - 10:00<br>- Thứ Năm: 08:00 - 10:00<br>- Thứ Bảy: 08:00 - 10:00
<br>
<br> Mã xác nhận: 930614
<br>
<br> Vui lòng đến trung tâm đúng giờ và mang theo mã xác nhận này.
<br>
<br> Trân trọng,
<br> English Center
<br>        ', 0, 1, NULL, '2026-04-22 16:43:39.310');
INSERT INTO [EmailLogs] ([Id], [UserId], [Recipient], [Subject], [Body], [Type], [IsSent], [ErrorMessage], [CreatedAt]) VALUES (4, 18, N'mndkhanh@gmail.com', N'Xác nhận đăng ký lớp Tiếng Anh nâng cao dành cho người Việt', N'
<br> Xin chào Mai Nguyễn Duy Khánh,
<br>
<br> Đăng ký của bạn đã được xác nhận!
<br>
<br> Thông tin lớp học:
<br> - Lớp: Tiếng Anh nâng cao dành cho người Việt
<br> - Khóa học: Tiếng Anh Trung Cấp Trên
<br> - Giáo viên: Hoàng Thị Thu
<br> - Ngày bắt đầu: 23/04/2026
<br>
<br> Lịch học:
<br> - Thứ Ba: 06:00 - 07:30<br>- Thứ Năm: 12:00 - 01:40
<br>
<br> Mã xác nhận: 266487
<br>
<br> Vui lòng đến trung tâm đúng giờ và mang theo mã xác nhận này.
<br>
<br> Trân trọng,
<br> English Center
<br>        ', 0, 1, NULL, '2026-04-22 16:52:17.041');
INSERT INTO [EmailLogs] ([Id], [UserId], [Recipient], [Subject], [Body], [Type], [IsSent], [ErrorMessage], [CreatedAt]) VALUES (5, 18, N'mndkhanh@gmail.com', N'Xác nhận đăng ký lớp Tiếng Anh nâng cao dành cho người Việt', N'
<br> Xin chào Mai Nguyễn Duy Khánh,
<br>
<br> Đăng ký của bạn đã được xác nhận!
<br>
<br> Thông tin lớp học:
<br> - Lớp: Tiếng Anh nâng cao dành cho người Việt
<br> - Khóa học: Tiếng Anh Trung Cấp Trên
<br> - Giáo viên: Hoàng Thị Thu
<br> - Ngày bắt đầu: 23/04/2026
<br>
<br> Lịch học:
<br> - Thứ Ba: 06:00 - 07:30<br>- Thứ Năm: 12:00 - 01:40
<br>
<br> Mã xác nhận: 443752
<br>
<br> Vui lòng đến trung tâm đúng giờ và mang theo mã xác nhận này.
<br>
<br> Trân trọng,
<br> English Center
<br>        ', 0, 1, NULL, '2026-04-22 16:55:18.468');
INSERT INTO [EmailLogs] ([Id], [UserId], [Recipient], [Subject], [Body], [Type], [IsSent], [ErrorMessage], [CreatedAt]) VALUES (6, 18, N'mndkhanh@gmail.com', N'Xác nhận đăng ký lớp B1-Sáng-K3', N'
<br> Xin chào Mai Nguyễn Duy Khánh,
<br>
<br> Đăng ký của bạn đã được xác nhận!
<br>
<br> Thông tin lớp học:
<br> - Lớp: B1-Sáng-K3
<br> - Khóa học: Tiếng Anh Trung Cấp
<br> - Giáo viên: Trần Văn Minh
<br> - Ngày bắt đầu: 04/05/2026
<br>
<br> Lịch học:
<br> Chưa có lịch học
<br>
<br> Mã xác nhận: 723600
<br>
<br> Vui lòng đến trung tâm đúng giờ và mang theo mã xác nhận này.
<br>
<br> Trân trọng,
<br> English Center
<br>        ', 0, 1, NULL, '2026-04-22 16:59:10.086');
INSERT INTO [EmailLogs] ([Id], [UserId], [Recipient], [Subject], [Body], [Type], [IsSent], [ErrorMessage], [CreatedAt]) VALUES (11, 19, N'mndkhanh3@gmail.com', N'Đăng ký tài khoản thành công - English Center', N'
                <h2>Xin chào Nguyễn Căn Bình,</h2>
                <p>Tài khoản của bạn đã được xác thực và tạo thành công.</p>
                <p><strong>Tên đăng nhập:</strong> canbinh</p>
                <p>Bạn có thể đăng nhập ngay để bắt đầu sử dụng hệ thống.</p>
                <p>Trân trọng,<br />English Center</p>
            ', 0, 1, NULL, '2026-04-22 19:04:01.171');
INSERT INTO [EmailLogs] ([Id], [UserId], [Recipient], [Subject], [Body], [Type], [IsSent], [ErrorMessage], [CreatedAt]) VALUES (12, 18, N'mndkhanh@gmail.com', N'Xác nhận đăng ký lớp A1-Sáng-K3', N'<br> Xin chào Mai Nguyễn Duy Khánh,<br><br> Đăng ký của bạn đã được xác nhận!<br><br> Thông tin lớp học:<br> - Lớp: A1-Sáng-K3<br> - Khóa học: Tiếng Anh Cơ Bản<br> - Giáo viên: Trần Văn Minh<br> - Ngày bắt đầu: 05/05/2026<br><br> Lịch học:<br> Chưa có lịch học<br><br> Mã xác nhận: 962200<br><br> Vui lòng đến trung tâm đúng giờ và mang theo mã xác nhận này.<br><br> Trân trọng,<br> English Center<br>        ', 0, 1, NULL, '2026-04-22 19:25:45.170');
INSERT INTO [EmailLogs] ([Id], [UserId], [Recipient], [Subject], [Body], [Type], [IsSent], [ErrorMessage], [CreatedAt]) VALUES (13, 20, N'khanhbimn@gmail.com', N'Đăng ký tài khoản thành công - English Center', N'
                <h2>Xin chào Nguyễn Nè,</h2>
                <p>Tài khoản của bạn đã được xác thực và tạo thành công.</p>
                <p><strong>Tên đăng nhập:</strong> ne</p>
                <p>Bạn có thể đăng nhập ngay để bắt đầu sử dụng hệ thống.</p>
                <p>Trân trọng,<br />English Center</p>
            ', 0, 1, NULL, '2026-04-22 19:59:21.539');
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('EmailLogs') AND is_identity = 1) SET IDENTITY_INSERT [EmailLogs] OFF;
GO

-- Dumping data for table EmailOtps
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('EmailOtps') AND is_identity = 1) SET IDENTITY_INSERT [EmailOtps] ON;
INSERT INTO [EmailOtps] ([Id], [Email], [OtpCode], [Type], [ExpiresAt], [IsUsed], [CreatedAt]) VALUES (0, N'mndkhanh@gmail.com', N'470599', 1, '2026-04-22 16:06:30.405', 1, '2026-04-22 16:01:30.405');
INSERT INTO [EmailOtps] ([Id], [Email], [OtpCode], [Type], [ExpiresAt], [IsUsed], [CreatedAt]) VALUES (4, N'mndkhanh3@gmail.com', N'134891', 1, '2026-04-22 19:08:27.155', 1, '2026-04-22 19:03:27.155');
INSERT INTO [EmailOtps] ([Id], [Email], [OtpCode], [Type], [ExpiresAt], [IsUsed], [CreatedAt]) VALUES (5, N'khanhbimn@gmail.com', N'884823', 1, '2026-04-22 20:03:30.469', 1, '2026-04-22 19:58:30.469');
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('EmailOtps') AND is_identity = 1) SET IDENTITY_INSERT [EmailOtps] OFF;
GO

-- Dumping data for table Enrollments
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Enrollments') AND is_identity = 1) SET IDENTITY_INSERT [Enrollments] ON;
INSERT INTO [Enrollments] ([Id], [StudentId], [ClassId], [EnrolledAt], [Status], [ConfirmationCode], [ConfirmedAt]) VALUES (1, 18, 6, '2026-04-23 02:43:22.019', N'Confirmed', N'908025', '2026-04-23 02:45:18.717');
INSERT INTO [Enrollments] ([Id], [StudentId], [ClassId], [EnrolledAt], [Status], [ConfirmationCode], [ConfirmedAt]) VALUES (2, 18, 5, '2026-04-23 02:43:22.020', N'Confirmed', N'450400', '2026-04-23 02:43:28.487');
INSERT INTO [Enrollments] ([Id], [StudentId], [ClassId], [EnrolledAt], [Status], [ConfirmationCode], [ConfirmedAt]) VALUES (3, 18, 4, '2026-04-23 02:55:07.402', N'Confirmed', N'358864', '2026-04-23 02:55:15.270');
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
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (30, 6, 1, 4, 4500.00, NULL, 0);
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (31, 6, 1, 3, 3500.00, NULL, 0);
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (32, 6, 1, 2, 1200.00, NULL, 0);
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (33, 7, 1, 3, 3500.00, NULL, 0);
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (34, 7, 1, 2, 1200.00, NULL, 0);
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (35, 8, 1, 2, 1200.00, NULL, 0);
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (36, 9, 1, 4, 4500.00, NULL, 0);
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (37, 9, 1, 3, 3500.00, NULL, 0);
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (38, 9, 1, 2, 1200.00, NULL, 0);
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (39, 10, 1, 2, 1200.00, NULL, 0);
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (40, 11, 0, 5, 3500.00, NULL, 0);
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (41, 11, 1, 3, 3500.00, NULL, 0);
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (42, 11, 1, 2, 1200.00, NULL, 0);
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (43, 12, 0, 6, 4500.00, NULL, 0);
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (44, 13, 1, 1, 1800.00, NULL, 0);
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (45, 13, 1, 3, 3500.00, NULL, 0);
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (46, 14, 1, 4, 4500.00, NULL, 0);
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (47, 14, 1, 3, 3500.00, NULL, 0);
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (48, 14, 1, 2, 1200.00, NULL, 0);
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (49, 15, 1, 4, 4500.00, NULL, 2);
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (50, 15, 1, 2, 1200.00, NULL, 1);
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (51, 16, 0, 4, 2500.00, NULL, 1);
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (52, 17, 0, 4, 2500.00, NULL, 1);
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (53, 18, 0, 4, 2500.00, NULL, 1);
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (54, 19, 0, 4, 2500.00, NULL, 1);
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (55, 20, 0, 6, 4500.00, NULL, 1);
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (56, 20, 0, 5, 3500.00, NULL, 1);
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (57, 21, 0, 6, 4500.00, NULL, 1);
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (58, 21, 0, 5, 3500.00, NULL, 1);
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (59, 22, 0, 4, 2500.00, NULL, 1);
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (60, 23, 1, 2, 1200.00, NULL, 1);
INSERT INTO [OrderItems] ([Id], [OrderId], [ItemType], [ItemId], [Price], [PurchasableItemId], [Quantity]) VALUES (61, 24, 1, 1, 1800.00, NULL, 1);
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
INSERT INTO [Orders] ([Id], [UserId], [TotalAmount], [BalanceUsed], [PayOSAmount], [OrderCode], [Status], [CreatedAt]) VALUES (13, 18, 5300.00, 5300.00, 0.00, 20260423013229, 1, '2026-04-23 01:32:29.971');
INSERT INTO [Orders] ([Id], [UserId], [TotalAmount], [BalanceUsed], [PayOSAmount], [OrderCode], [Status], [CreatedAt]) VALUES (14, 18, 9200.00, 4700.00, 4500.00, 20260423013303, 1, '2026-04-23 01:33:03.501');
INSERT INTO [Orders] ([Id], [UserId], [TotalAmount], [BalanceUsed], [PayOSAmount], [OrderCode], [Status], [CreatedAt]) VALUES (15, 18, 10200.00, 0.00, 10200.00, 20260423014448, 2, '2026-04-23 01:44:48.666');
INSERT INTO [Orders] ([Id], [UserId], [TotalAmount], [BalanceUsed], [PayOSAmount], [OrderCode], [Status], [CreatedAt]) VALUES (16, 18, 2500.00, 0.00, 2500.00, 20260423015408, 2, '2026-04-23 01:54:08.182');
INSERT INTO [Orders] ([Id], [UserId], [TotalAmount], [BalanceUsed], [PayOSAmount], [OrderCode], [Status], [CreatedAt]) VALUES (17, 19, 2500.00, 0.00, 2500.00, 20260423020547, 2, '2026-04-23 02:05:47.216');
INSERT INTO [Orders] ([Id], [UserId], [TotalAmount], [BalanceUsed], [PayOSAmount], [OrderCode], [Status], [CreatedAt]) VALUES (18, 19, 2500.00, 0.00, 2500.00, 20260423020648, 1, '2026-04-23 02:06:48.030');
INSERT INTO [Orders] ([Id], [UserId], [TotalAmount], [BalanceUsed], [PayOSAmount], [OrderCode], [Status], [CreatedAt]) VALUES (19, 18, 2500.00, 0.00, 2500.00, 20260423022451, 1, '2026-04-23 02:24:51.808');
INSERT INTO [Orders] ([Id], [UserId], [TotalAmount], [BalanceUsed], [PayOSAmount], [OrderCode], [Status], [CreatedAt]) VALUES (20, 18, 8000.00, 8000.00, 0.00, 20260423024123, 1, '2026-04-23 02:41:23.037');
INSERT INTO [Orders] ([Id], [UserId], [TotalAmount], [BalanceUsed], [PayOSAmount], [OrderCode], [Status], [CreatedAt]) VALUES (21, 18, 8000.00, 8000.00, 0.00, 20260423024321, 1, '2026-04-23 02:43:21.980');
INSERT INTO [Orders] ([Id], [UserId], [TotalAmount], [BalanceUsed], [PayOSAmount], [OrderCode], [Status], [CreatedAt]) VALUES (22, 18, 2500.00, 2500.00, 0.00, 20260423025507, 1, '2026-04-23 02:55:07.296');
INSERT INTO [Orders] ([Id], [UserId], [TotalAmount], [BalanceUsed], [PayOSAmount], [OrderCode], [Status], [CreatedAt]) VALUES (23, 18, 1200.00, 1200.00, 0.00, 20260423025713, 1, '2026-04-23 02:57:13.203');
INSERT INTO [Orders] ([Id], [UserId], [TotalAmount], [BalanceUsed], [PayOSAmount], [OrderCode], [Status], [CreatedAt]) VALUES (24, 20, 1800.00, 0.00, 1800.00, 20260423025933, 2, '2026-04-23 02:59:33.709');
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Orders') AND is_identity = 1) SET IDENTITY_INSERT [Orders] OFF;
GO

-- Dumping data for table PurchasableItems
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('PurchasableItems') AND is_identity = 1) SET IDENTITY_INSERT [PurchasableItems] ON;
INSERT INTO [PurchasableItems] ([Id], [Name], [Description], [Price], [ImageUrl], [IsActive], [CreatedAt], [Quantity]) VALUES (1, N'Giáo trình IELTS Foundation', N'Sách học IELTS cho người mới.', 1800.00, N'https://cdn1.fahasa.com/media/catalog/product/z/_/z.jpg', 1, '2026-04-22 22:36:26.850', 9);
INSERT INTO [PurchasableItems] ([Id], [Name], [Description], [Price], [ImageUrl], [IsActive], [CreatedAt], [Quantity]) VALUES (2, N'Bộ đề thi thử Cambridge B1', N'10 đề thi chuẩn cấu trúc.', 1200.00, N'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRNeQW7ZwTfqytOTSlgt7GQeY4AVkvmj4f0cA&s', 1, '2026-04-22 22:36:26.850', 8);
INSERT INTO [PurchasableItems] ([Id], [Name], [Description], [Price], [ImageUrl], [IsActive], [CreatedAt], [Quantity]) VALUES (3, N'Tai nghe học tiếng Anh', N'Tai nghe chuyên dụng luyện nghe.', 3500.00, N'https://cdn.tgdd.vn/Products/Images/54/251139/chup-tai-rapoo-h120-den-15-600x600.jpg', 1, '2026-04-22 22:36:26.850', 8);
INSERT INTO [PurchasableItems] ([Id], [Name], [Description], [Price], [ImageUrl], [IsActive], [CreatedAt], [Quantity]) VALUES (4, N'Từ điển Oxford English', N'Phiên bản bìa cứng mới nhất.', 4500.00, N'https://www.netabooks.vn/Data/Sites/1/Product/34986/tu-dien-oxford-anh-viet-350000-tu-hop-cung-xanh.jpg', 1, '2026-04-22 22:36:26.850', 9);
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('PurchasableItems') AND is_identity = 1) SET IDENTITY_INSERT [PurchasableItems] OFF;
GO

-- Dumping data for table QuizOptions
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('QuizOptions') AND is_identity = 1) SET IDENTITY_INSERT [QuizOptions] ON;
INSERT INTO [QuizOptions] ([Id], [QuestionId], [OptionText], [IsCorrect], [SortOrder]) VALUES (0, 1, N'mouses', 0, 1);
INSERT INTO [QuizOptions] ([Id], [QuestionId], [OptionText], [IsCorrect], [SortOrder]) VALUES (1, 1, N'mice', 1, 2);
INSERT INTO [QuizOptions] ([Id], [QuestionId], [OptionText], [IsCorrect], [SortOrder]) VALUES (2, 1, N'mouse', 0, 3);
INSERT INTO [QuizOptions] ([Id], [QuestionId], [OptionText], [IsCorrect], [SortOrder]) VALUES (3, 1, N'meese', 0, 4);
INSERT INTO [QuizOptions] ([Id], [QuestionId], [OptionText], [IsCorrect], [SortOrder]) VALUES (4, 2, N'play', 0, 1);
INSERT INTO [QuizOptions] ([Id], [QuestionId], [OptionText], [IsCorrect], [SortOrder]) VALUES (5, 2, N'plays', 1, 2);
INSERT INTO [QuizOptions] ([Id], [QuestionId], [OptionText], [IsCorrect], [SortOrder]) VALUES (6, 2, N'playing', 0, 3);
INSERT INTO [QuizOptions] ([Id], [QuestionId], [OptionText], [IsCorrect], [SortOrder]) VALUES (7, 2, N'played', 0, 4);
INSERT INTO [QuizOptions] ([Id], [QuestionId], [OptionText], [IsCorrect], [SortOrder]) VALUES (8, 4, N'go', 1, 1);
INSERT INTO [QuizOptions] ([Id], [QuestionId], [OptionText], [IsCorrect], [SortOrder]) VALUES (9, 4, N'goes', 0, 2);
INSERT INTO [QuizOptions] ([Id], [QuestionId], [OptionText], [IsCorrect], [SortOrder]) VALUES (10, 4, N'going', 0, 3);
INSERT INTO [QuizOptions] ([Id], [QuestionId], [OptionText], [IsCorrect], [SortOrder]) VALUES (11, 4, N'gone', 0, 4);
INSERT INTO [QuizOptions] ([Id], [QuestionId], [OptionText], [IsCorrect], [SortOrder]) VALUES (12, 6, N'small', 0, 1);
INSERT INTO [QuizOptions] ([Id], [QuestionId], [OptionText], [IsCorrect], [SortOrder]) VALUES (13, 6, N'large', 1, 2);
INSERT INTO [QuizOptions] ([Id], [QuestionId], [OptionText], [IsCorrect], [SortOrder]) VALUES (14, 6, N'tiny', 0, 3);
INSERT INTO [QuizOptions] ([Id], [QuestionId], [OptionText], [IsCorrect], [SortOrder]) VALUES (15, 6, N'short', 0, 4);
INSERT INTO [QuizOptions] ([Id], [QuestionId], [OptionText], [IsCorrect], [SortOrder]) VALUES (16, 7, N'sad', 1, 1);
INSERT INTO [QuizOptions] ([Id], [QuestionId], [OptionText], [IsCorrect], [SortOrder]) VALUES (17, 17, N'sad', 1, 1);
INSERT INTO [QuizOptions] ([Id], [QuestionId], [OptionText], [IsCorrect], [SortOrder]) VALUES (18, 7, N'joyful', 0, 2);
INSERT INTO [QuizOptions] ([Id], [QuestionId], [OptionText], [IsCorrect], [SortOrder]) VALUES (19, 17, N'joyful', 0, 2);
INSERT INTO [QuizOptions] ([Id], [QuestionId], [OptionText], [IsCorrect], [SortOrder]) VALUES (20, 7, N'glad', 0, 3);
INSERT INTO [QuizOptions] ([Id], [QuestionId], [OptionText], [IsCorrect], [SortOrder]) VALUES (21, 17, N'glad', 0, 3);
INSERT INTO [QuizOptions] ([Id], [QuestionId], [OptionText], [IsCorrect], [SortOrder]) VALUES (22, 7, N'excited', 0, 4);
INSERT INTO [QuizOptions] ([Id], [QuestionId], [OptionText], [IsCorrect], [SortOrder]) VALUES (23, 17, N'excited', 0, 4);
INSERT INTO [QuizOptions] ([Id], [QuestionId], [OptionText], [IsCorrect], [SortOrder]) VALUES (24, 9, N'watch', 0, 1);
INSERT INTO [QuizOptions] ([Id], [QuestionId], [OptionText], [IsCorrect], [SortOrder]) VALUES (25, 9, N'watched', 1, 2);
INSERT INTO [QuizOptions] ([Id], [QuestionId], [OptionText], [IsCorrect], [SortOrder]) VALUES (26, 9, N'watching', 0, 3);
INSERT INTO [QuizOptions] ([Id], [QuestionId], [OptionText], [IsCorrect], [SortOrder]) VALUES (27, 9, N'watches', 0, 4);
INSERT INTO [QuizOptions] ([Id], [QuestionId], [OptionText], [IsCorrect], [SortOrder]) VALUES (28, 11, N'drink', 0, 1);
INSERT INTO [QuizOptions] ([Id], [QuestionId], [OptionText], [IsCorrect], [SortOrder]) VALUES (29, 11, N'drinks', 1, 2);
INSERT INTO [QuizOptions] ([Id], [QuestionId], [OptionText], [IsCorrect], [SortOrder]) VALUES (30, 11, N'drinking', 0, 3);
INSERT INTO [QuizOptions] ([Id], [QuestionId], [OptionText], [IsCorrect], [SortOrder]) VALUES (31, 11, N'drank', 0, 4);
INSERT INTO [QuizOptions] ([Id], [QuestionId], [OptionText], [IsCorrect], [SortOrder]) VALUES (32, 13, N'a', 0, 1);
INSERT INTO [QuizOptions] ([Id], [QuestionId], [OptionText], [IsCorrect], [SortOrder]) VALUES (33, 13, N'an', 1, 2);
INSERT INTO [QuizOptions] ([Id], [QuestionId], [OptionText], [IsCorrect], [SortOrder]) VALUES (34, 13, N'the', 0, 3);
INSERT INTO [QuizOptions] ([Id], [QuestionId], [OptionText], [IsCorrect], [SortOrder]) VALUES (35, 13, N'no article', 0, 4);
INSERT INTO [QuizOptions] ([Id], [QuestionId], [OptionText], [IsCorrect], [SortOrder]) VALUES (36, 15, N'am', 0, 1);
INSERT INTO [QuizOptions] ([Id], [QuestionId], [OptionText], [IsCorrect], [SortOrder]) VALUES (37, 15, N'are', 1, 2);
INSERT INTO [QuizOptions] ([Id], [QuestionId], [OptionText], [IsCorrect], [SortOrder]) VALUES (38, 15, N'is', 0, 3);
INSERT INTO [QuizOptions] ([Id], [QuestionId], [OptionText], [IsCorrect], [SortOrder]) VALUES (39, 15, N'be', 0, 4);
INSERT INTO [QuizOptions] ([Id], [QuestionId], [OptionText], [IsCorrect], [SortOrder]) VALUES (40, 17, N'is', 0, 1);
INSERT INTO [QuizOptions] ([Id], [QuestionId], [OptionText], [IsCorrect], [SortOrder]) VALUES (41, 17, N'are', 1, 2);
INSERT INTO [QuizOptions] ([Id], [QuestionId], [OptionText], [IsCorrect], [SortOrder]) VALUES (42, 17, N'be', 0, 3);
INSERT INTO [QuizOptions] ([Id], [QuestionId], [OptionText], [IsCorrect], [SortOrder]) VALUES (43, 17, N'been', 0, 4);
INSERT INTO [QuizOptions] ([Id], [QuestionId], [OptionText], [IsCorrect], [SortOrder]) VALUES (44, 19, N'read', 0, 1);
INSERT INTO [QuizOptions] ([Id], [QuestionId], [OptionText], [IsCorrect], [SortOrder]) VALUES (45, 19, N'reads', 1, 2);
INSERT INTO [QuizOptions] ([Id], [QuestionId], [OptionText], [IsCorrect], [SortOrder]) VALUES (46, 19, N'reading', 0, 3);
INSERT INTO [QuizOptions] ([Id], [QuestionId], [OptionText], [IsCorrect], [SortOrder]) VALUES (47, 19, N'readed', 0, 4);
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('QuizOptions') AND is_identity = 1) SET IDENTITY_INSERT [QuizOptions] OFF;
GO

-- Dumping data for table QuizQuestions
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('QuizQuestions') AND is_identity = 1) SET IDENTITY_INSERT [QuizQuestions] ON;
INSERT INTO [QuizQuestions] ([Id], [QuizId], [QuestionText], [QuestionType], [Point], [SortOrder]) VALUES (1, 1, N'What is the plural of "mouse"?', 1, 10, 1);
INSERT INTO [QuizQuestions] ([Id], [QuizId], [QuestionText], [QuestionType], [Point], [SortOrder]) VALUES (2, 1, N'He ___ football.', 1, 10, 2);
INSERT INTO [QuizQuestions] ([Id], [QuizId], [QuestionText], [QuestionType], [Point], [SortOrder]) VALUES (3, 1, N'I ___ a teacher.', 2, 10, 3);
INSERT INTO [QuizQuestions] ([Id], [QuizId], [QuestionText], [QuestionType], [Point], [SortOrder]) VALUES (4, 1, N'They ___ to school.', 1, 10, 4);
INSERT INTO [QuizQuestions] ([Id], [QuizId], [QuestionText], [QuestionType], [Point], [SortOrder]) VALUES (5, 1, N'She ___ cooking now.', 2, 10, 5);
INSERT INTO [QuizQuestions] ([Id], [QuizId], [QuestionText], [QuestionType], [Point], [SortOrder]) VALUES (6, 1, N'Synonym of "big"?', 1, 10, 6);
INSERT INTO [QuizQuestions] ([Id], [QuizId], [QuestionText], [QuestionType], [Point], [SortOrder]) VALUES (7, 1, N'Antonym of "happy"?', 1, 10, 7);
INSERT INTO [QuizQuestions] ([Id], [QuizId], [QuestionText], [QuestionType], [Point], [SortOrder]) VALUES (8, 1, N'We ___ English every day.', 2, 10, 8);
INSERT INTO [QuizQuestions] ([Id], [QuizId], [QuestionText], [QuestionType], [Point], [SortOrder]) VALUES (9, 1, N'I ___ TV yesterday.', 1, 10, 9);
INSERT INTO [QuizQuestions] ([Id], [QuizId], [QuestionText], [QuestionType], [Point], [SortOrder]) VALUES (10, 1, N'They have ___ homework.', 2, 10, 10);
INSERT INTO [QuizQuestions] ([Id], [QuizId], [QuestionText], [QuestionType], [Point], [SortOrder]) VALUES (11, 1, N'She ___ coffee.', 1, 10, 11);
INSERT INTO [QuizQuestions] ([Id], [QuizId], [QuestionText], [QuestionType], [Point], [SortOrder]) VALUES (12, 1, N'He ___ football every day.', 2, 10, 12);
INSERT INTO [QuizQuestions] ([Id], [QuizId], [QuestionText], [QuestionType], [Point], [SortOrder]) VALUES (13, 1, N'___ apple', 1, 10, 13);
INSERT INTO [QuizQuestions] ([Id], [QuizId], [QuestionText], [QuestionType], [Point], [SortOrder]) VALUES (14, 1, N'I ___ been to London.', 2, 10, 14);
INSERT INTO [QuizQuestions] ([Id], [QuizId], [QuestionText], [QuestionType], [Point], [SortOrder]) VALUES (15, 1, N'We ___ going now.', 1, 10, 15);
INSERT INTO [QuizQuestions] ([Id], [QuizId], [QuestionText], [QuestionType], [Point], [SortOrder]) VALUES (16, 1, N'She ___ to school yesterday.', 2, 10, 16);
INSERT INTO [QuizQuestions] ([Id], [QuizId], [QuestionText], [QuestionType], [Point], [SortOrder]) VALUES (17, 1, N'They ___ happy.', 1, 10, 17);
INSERT INTO [QuizQuestions] ([Id], [QuizId], [QuestionText], [QuestionType], [Point], [SortOrder]) VALUES (18, 1, N'It ___ raining.', 2, 10, 18);
INSERT INTO [QuizQuestions] ([Id], [QuizId], [QuestionText], [QuestionType], [Point], [SortOrder]) VALUES (19, 1, N'I ___ a book.', 1, 10, 19);
INSERT INTO [QuizQuestions] ([Id], [QuizId], [QuestionText], [QuestionType], [Point], [SortOrder]) VALUES (20, 1, N'We ___ finished.', 2, 10, 20);
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('QuizQuestions') AND is_identity = 1) SET IDENTITY_INSERT [QuizQuestions] OFF;
GO

-- Dumping data for table QuizResults
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('QuizResults') AND is_identity = 1) SET IDENTITY_INSERT [QuizResults] ON;
INSERT INTO [QuizResults] ([Id], [StudentId], [QuizId], [Score], [TotalPoints], [CorrectCount], [TotalQuestions], [TimeTaken], [CompletedAt]) VALUES (0, 18, 1, 0.00, 200, 0, 20, 00:00:52.2928819, '2026-04-22 20:05:10.314');
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('QuizResults') AND is_identity = 1) SET IDENTITY_INSERT [QuizResults] OFF;
GO

-- Dumping data for table Quizzes
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Quizzes') AND is_identity = 1) SET IDENTITY_INSERT [Quizzes] ON;
INSERT INTO [Quizzes] ([Id], [CourseId], [Title], [Description], [TimeLimitMinutes], [StartAt], [EndAt], [IsActive], [CreatedAt]) VALUES (1, 1, N'English Mixed Test', N'Bài kiểm tra tổng hợp 20 câu', 30, '2026-04-22 19:54:03.266', '2026-04-29 19:54:03.266', 1, '2026-04-22 19:54:03.266');
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Quizzes') AND is_identity = 1) SET IDENTITY_INSERT [Quizzes] OFF;
GO

-- Dumping data for table StudentAnswers
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('StudentAnswers') AND is_identity = 1) SET IDENTITY_INSERT [StudentAnswers] ON;
INSERT INTO [StudentAnswers] ([Id], [QuizResultId], [QuestionId], [SelectedOptionId], [FillInAnswer], [IsCorrect], [PointsEarned]) VALUES (0, 0, 1, NULL, NULL, 0, 0);
INSERT INTO [StudentAnswers] ([Id], [QuizResultId], [QuestionId], [SelectedOptionId], [FillInAnswer], [IsCorrect], [PointsEarned]) VALUES (1, 0, 2, NULL, NULL, 0, 0);
INSERT INTO [StudentAnswers] ([Id], [QuizResultId], [QuestionId], [SelectedOptionId], [FillInAnswer], [IsCorrect], [PointsEarned]) VALUES (2, 0, 3, NULL, NULL, 0, 0);
INSERT INTO [StudentAnswers] ([Id], [QuizResultId], [QuestionId], [SelectedOptionId], [FillInAnswer], [IsCorrect], [PointsEarned]) VALUES (3, 0, 4, NULL, NULL, 0, 0);
INSERT INTO [StudentAnswers] ([Id], [QuizResultId], [QuestionId], [SelectedOptionId], [FillInAnswer], [IsCorrect], [PointsEarned]) VALUES (4, 0, 5, NULL, NULL, 0, 0);
INSERT INTO [StudentAnswers] ([Id], [QuizResultId], [QuestionId], [SelectedOptionId], [FillInAnswer], [IsCorrect], [PointsEarned]) VALUES (5, 0, 6, NULL, NULL, 0, 0);
INSERT INTO [StudentAnswers] ([Id], [QuizResultId], [QuestionId], [SelectedOptionId], [FillInAnswer], [IsCorrect], [PointsEarned]) VALUES (6, 0, 7, NULL, NULL, 0, 0);
INSERT INTO [StudentAnswers] ([Id], [QuizResultId], [QuestionId], [SelectedOptionId], [FillInAnswer], [IsCorrect], [PointsEarned]) VALUES (7, 0, 8, NULL, NULL, 0, 0);
INSERT INTO [StudentAnswers] ([Id], [QuizResultId], [QuestionId], [SelectedOptionId], [FillInAnswer], [IsCorrect], [PointsEarned]) VALUES (8, 0, 9, NULL, NULL, 0, 0);
INSERT INTO [StudentAnswers] ([Id], [QuizResultId], [QuestionId], [SelectedOptionId], [FillInAnswer], [IsCorrect], [PointsEarned]) VALUES (9, 0, 10, NULL, NULL, 0, 0);
INSERT INTO [StudentAnswers] ([Id], [QuizResultId], [QuestionId], [SelectedOptionId], [FillInAnswer], [IsCorrect], [PointsEarned]) VALUES (10, 0, 11, NULL, NULL, 0, 0);
INSERT INTO [StudentAnswers] ([Id], [QuizResultId], [QuestionId], [SelectedOptionId], [FillInAnswer], [IsCorrect], [PointsEarned]) VALUES (11, 0, 12, NULL, NULL, 0, 0);
INSERT INTO [StudentAnswers] ([Id], [QuizResultId], [QuestionId], [SelectedOptionId], [FillInAnswer], [IsCorrect], [PointsEarned]) VALUES (12, 0, 13, NULL, NULL, 0, 0);
INSERT INTO [StudentAnswers] ([Id], [QuizResultId], [QuestionId], [SelectedOptionId], [FillInAnswer], [IsCorrect], [PointsEarned]) VALUES (13, 0, 14, NULL, NULL, 0, 0);
INSERT INTO [StudentAnswers] ([Id], [QuizResultId], [QuestionId], [SelectedOptionId], [FillInAnswer], [IsCorrect], [PointsEarned]) VALUES (14, 0, 15, NULL, NULL, 0, 0);
INSERT INTO [StudentAnswers] ([Id], [QuizResultId], [QuestionId], [SelectedOptionId], [FillInAnswer], [IsCorrect], [PointsEarned]) VALUES (15, 0, 16, NULL, NULL, 0, 0);
INSERT INTO [StudentAnswers] ([Id], [QuizResultId], [QuestionId], [SelectedOptionId], [FillInAnswer], [IsCorrect], [PointsEarned]) VALUES (16, 0, 17, NULL, NULL, 0, 0);
INSERT INTO [StudentAnswers] ([Id], [QuizResultId], [QuestionId], [SelectedOptionId], [FillInAnswer], [IsCorrect], [PointsEarned]) VALUES (17, 0, 18, NULL, NULL, 0, 0);
INSERT INTO [StudentAnswers] ([Id], [QuizResultId], [QuestionId], [SelectedOptionId], [FillInAnswer], [IsCorrect], [PointsEarned]) VALUES (18, 0, 19, NULL, NULL, 0, 0);
INSERT INTO [StudentAnswers] ([Id], [QuizResultId], [QuestionId], [SelectedOptionId], [FillInAnswer], [IsCorrect], [PointsEarned]) VALUES (19, 0, 20, NULL, NULL, 0, 0);
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('StudentAnswers') AND is_identity = 1) SET IDENTITY_INSERT [StudentAnswers] OFF;
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
INSERT INTO [Users] ([Id], [FullName], [Email], [Username], [Phone], [PasswordHash], [Role], [AvatarUrl], [IsActive], [CreatedAt], [WalletBalance], [RegistrationFeePaid]) VALUES (18, N'Mai Nguyễn Duy Khánh', N'mndkhanh@gmail.com', N'mndkhanh', N'0362718422', N'$2a$11$Si3YM5MhfzgbwNf0HbRYbuO62D9NTLhLiSpHXlyeqhlXVXT.lQlY.', N'Student', NULL, 1, '2026-04-22 23:01:53.348', 80300.00, 0);
INSERT INTO [Users] ([Id], [FullName], [Email], [Username], [Phone], [PasswordHash], [Role], [AvatarUrl], [IsActive], [CreatedAt], [WalletBalance], [RegistrationFeePaid]) VALUES (19, N'Nguyễn Căn Bình', N'mndkhanh3@gmail.com', N'canbinh', N'0362718422', N'$2a$11$EM.ClshrICOl/vugSMk51.1/4Fky1nUzQ4sH/nQeeCRX/wb3JaZqi', N'Teacher', N'https://htmediagroup.vn/wp-content/uploads/2022/11/Anh-58-copy-min.jpg.webp', 1, '2026-04-23 02:03:56.081', 100000.00, 0);
INSERT INTO [Users] ([Id], [FullName], [Email], [Username], [Phone], [PasswordHash], [Role], [AvatarUrl], [IsActive], [CreatedAt], [WalletBalance], [RegistrationFeePaid]) VALUES (20, N'Nguyễn Nè', N'khanhbimn@gmail.com', N'ne', N'0362718422', N'$2a$11$QYapsTqClf0wdAFZz5IPa.LE7tBGPW.XIfwjGRmZsVKQ1fCcVFgIK', N'Student', NULL, 1, '2026-04-23 02:59:16.426', 0.00, 0);
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Users') AND is_identity = 1) SET IDENTITY_INSERT [Users] OFF;
GO

-- Re-enable all constraints
EXEC sp_MSforeachtable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL';
GO

