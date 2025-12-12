-- Create table to store inline media (images/videos) with optional title
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'ArticleInlineMedia')
BEGIN
    CREATE TABLE dbo.ArticleInlineMedia (
        InlineMediaID INT IDENTITY(1,1) PRIMARY KEY,
        ArticleID INT NOT NULL,
        MediaType NVARCHAR(20) NOT NULL, -- 'image' | 'video'
        Url NVARCHAR(1000) NOT NULL,
        Title NVARCHAR(500) NULL,
        DisplayOrder INT NOT NULL DEFAULT(0),
        CreatedAt DATETIME NULL DEFAULT(GETDATE()),
        CONSTRAINT FK_ArticleInlineMedia_Articles FOREIGN KEY (ArticleID)
            REFERENCES dbo.Articles(ArticleID) ON DELETE CASCADE
    );
    
    CREATE INDEX IX_ArticleInlineMedia_Article_DisplayOrder
        ON dbo.ArticleInlineMedia(ArticleID, DisplayOrder);
END
GO
