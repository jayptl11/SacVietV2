-- Thêm c?t VideoUrl vào b?ng dbo.Articles
IF NOT EXISTS (
    SELECT 1 
    FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_SCHEMA = 'dbo' 
      AND TABLE_NAME = 'Articles' 
      AND COLUMN_NAME = 'VideoUrl'
)
BEGIN
    ALTER TABLE dbo.Articles
    ADD VideoUrl NVARCHAR(500) NULL;
    
    PRINT '?ã thêm c?t VideoUrl vào b?ng dbo.Articles';
END
ELSE
BEGIN
    PRINT 'C?t VideoUrl ?ã t?n t?i trong b?ng dbo.Articles';
END
GO
