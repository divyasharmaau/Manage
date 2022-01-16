delete from AspNetUserClaims
delete from AspNetUserRoles
delete from AspNetUsers
delete from AspNetRoles


INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'44b9d347-e5a7-417c-a9ce-138037fa70d2', N'Registered User', N'REGISTERED USER', N'85153d18-fe47-4c50-b2ca-7c250bf93e52')
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'ffff79e0-ce86-4e94-9754-c27c3f6608b7', N'Administrator', N'ADMINISTRATOR', N'fefff4b7-e936-4944-b29e-99e5261bbe72')
GO

INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [Title], [FirstName], [MiddleName], [LastName], [JobTitle], [Status], [JoiningDate], [DaysWorkedInWeek], [Manager], [DepartmentId], [NumberOfHoursWorkedPerDay]) VALUES (N'691418e1-394a-439c-870f-6466f74e0d53', N'demo.user', N'DEMO.USER', N'useratdemo@gmail.com', N'USERATDEMO@GMAIL.COM', 1, N'AQAAAAEAACcQAAAAEOU+T9l1wSZTIkILdHvexPwCSx4/YBsVy6FvhTTCpCfi9Ib0vuB/sW2qs3PX00UYxw==', N'QSQEKMNER6J5M5HEI56U4ULPRPMUCAV7', N'b2a016e4-b185-4188-bd9a-91f0cc93084c', NULL, 0, 0, NULL, 0, 0, N'Mr', N'Demo', NULL, N'User', N'Software Engineer', N'Full-Time', CAST(N'2021-11-01T00:00:00.0000000' AS DateTime2), 5, N'Otis Chapman', 1, 7.6)
GO

INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [Title], [FirstName], [MiddleName], [LastName], [JobTitle], [Status], [JoiningDate], [DaysWorkedInWeek], [Manager], [DepartmentId], [NumberOfHoursWorkedPerDay]) VALUES (N'a9067a75-b6ee-4c23-a3e2-f19648957347', N'breta.collins', N'BRETA.COLLINS', N'demoasadmn@gmail.com', N'demoasadmn@gmail.com', 1, N'AQAAAAEAACcQAAAAEB7+k042Pib+vryyjWarxdd9E9MfOWVElyMUMlyW+GU2RcBV3UuDIrwV2qh+5Re9kw==', N'5OG45RA2IRXZTUOYBWZ4ZMXA7DETEDLZ', N'8f707803-e148-46f4-a1c6-51a5c0dc92ad', NULL, 0, 0, NULL, 0, 0, N'Ms', N'Breta', NULL, N'Collins', N' Head of Administration', N'Full-Time', CAST(N'2021-03-02T00:00:00.0000000' AS DateTime2), 5, NULL, 2, 7.6)
GO

INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'a9067a75-b6ee-4c23-a3e2-f19648957347', N'44b9d347-e5a7-417c-a9ce-138037fa70d2')
GO
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'a9067a75-b6ee-4c23-a3e2-f19648957347', N'ffff79e0-ce86-4e94-9754-c27c3f6608b7')
GO


SET IDENTITY_INSERT [dbo].[AspNetUserClaims] ON 
GO
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (17, N'a9067a75-b6ee-4c23-a3e2-f19648957347', N'Create Role', N'Create Role')
GO
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (18, N'a9067a75-b6ee-4c23-a3e2-f19648957347', N'Edit Role', N'Edit Role')
GO
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (19, N'a9067a75-b6ee-4c23-a3e2-f19648957347', N'Delete Role', N'Delete Role')
GO

SET IDENTITY_INSERT [dbo].[AspNetUserClaims] OFF
GO
