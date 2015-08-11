SET IDENTITY_INSERT [Category] ON;
GO;

INSERT INTO [Category] ([Id], [Name], [Description], [Image], [Version]) VALUES (1000000, 'Test category 1', 'Test description 1', 'Test category photo 1', 1);
GO;

INSERT INTO [Category] ([Id], [Name], [Description], [Image], [Version]) VALUES (1000001, 'Test category 2', 'Test description 2', 'Test category photo 2', 1);
GO;
INSERT INTO [Category] ([Id], [Name], [Description], [Image], [Version]) VALUES (1000002, 'Test category 3', 'Test description 3', 'Test category photo 3', 1);
GO;

SET IDENTITY_INSERT [Category] OFF;
GO;

-- Remove all test data
DELETE FROM [Category];
GO;