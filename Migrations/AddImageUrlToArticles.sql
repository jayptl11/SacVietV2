-- Add ImageUrl column to Articles table (nullable)
IF NOT EXISTS (
    SELECT 1
    FROM sys.columns c
    JOIN sys.tables t ON c.object_id = t.object_id
    WHERE t.name = 'Articles' AND c.name = 'ImageUrl'
)
BEGIN
    ALTER TABLE dbo.Articles ADD ImageUrl NVARCHAR(500) NULL;
END
GO

-- Optional: create index if you plan to filter by ImageUrl frequently (usually not needed)
-- CREATE INDEX IX_Articles_ImageUrl ON dbo.Articles(ImageUrl);
