USE [AWSNet]
GO
SET IDENTITY_INSERT [dbo].[Products] ON 
GO
INSERT [dbo].[Products] ([ID], [Name], [CategoryID], [UnitPrice], [UnitsStock]) VALUES (4, N'TV 32"', 1, CAST(5000.20 AS Decimal(10, 2)), 2)
GO
INSERT [dbo].[Products] ([ID], [Name], [CategoryID], [UnitPrice], [UnitsStock]) VALUES (6, N'TV 40" Sanyo', 1, CAST(8000.60 AS Decimal(10, 2)), 8)
GO
INSERT [dbo].[Products] ([ID], [Name], [CategoryID], [UnitPrice], [UnitsStock]) VALUES (8, N'Laptop Acer I3 /14" / 8 RAM/SDD 320', 2, CAST(12000.00 AS Decimal(10, 2)), 5)
GO
SET IDENTITY_INSERT [dbo].[Products] OFF
GO
