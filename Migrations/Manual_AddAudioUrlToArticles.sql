-- Migration: Add AudioUrl column to Articles table
-- Purpose: Store Google Drive audio URL for Podcast category articles
-- Date: 2024-01-XX
-- Version: 3.3

USE [SacViet];
GO

-- Check if column exists before adding
IF NOT EXISTS (
    SELECT * FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'[dbo].[Articles]') 
    AND name = 'AudioUrl'
)
BEGIN
    PRINT 'Adding AudioUrl column to Articles table...';
    
    ALTER TABLE [dbo].[Articles]
    ADD [AudioUrl] NVARCHAR(500) NULL;
    
    PRINT '? AudioUrl column added successfully!';
END
ELSE
BEGIN
    PRINT '?? AudioUrl column already exists. No action needed.';
END
GO

-- Verify the column was added
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    CHARACTER_MAXIMUM_LENGTH,
    IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Articles' AND COLUMN_NAME = 'AudioUrl';
GO
