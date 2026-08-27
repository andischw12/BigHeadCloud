SET NOCOUNT ON;

SELECT
    CASE WHEN OBJECT_ID(N'dbo.BigHeadGameData', N'U') IS NOT NULL THEN 1 ELSE 0 END AS BigHeadGameDataExists,
    CASE WHEN OBJECT_ID(N'dbo.BigHeadGameResults', N'U') IS NOT NULL THEN 1 ELSE 0 END AS BigHeadGameResultsExists;

SELECT
    C.name AS ColumnName,
    TYPE_NAME(C.user_type_id) AS SqlType,
    C.max_length AS MaxLength,
    C.is_nullable AS IsNullable
FROM sys.columns AS C
WHERE C.object_id = OBJECT_ID(N'dbo.BigHeadGameData')
ORDER BY C.column_id;

SELECT
    CASE WHEN
        OBJECT_ID(N'dbo.BigHeadGameData', N'U') IS NOT NULL
        AND COL_LENGTH(N'dbo.BigHeadGameData', N'OID') IS NOT NULL
        AND COL_LENGTH(N'dbo.BigHeadGameData', N'GameKey') IS NOT NULL
        AND COL_LENGTH(N'dbo.BigHeadGameData', N'PlayerSlot') IS NOT NULL
        AND COL_LENGTH(N'dbo.BigHeadGameData', N'DataJson') IS NOT NULL
        AND COL_LENGTH(N'dbo.BigHeadGameData', N'Points') IS NOT NULL
        AND COL_LENGTH(N'dbo.BigHeadGameData', N'ShabbatPoints') IS NOT NULL
        AND COL_LENGTH(N'dbo.BigHeadGameData', N'HanukkaPoints') IS NOT NULL
        AND COL_LENGTH(N'dbo.BigHeadGameData', N'PurimPoints') IS NOT NULL
        AND COL_LENGTH(N'dbo.BigHeadGameData', N'UpdatedAt') IS NOT NULL
    THEN N'OK - existing table is compatible'
    ELSE N'ERROR - existing table is missing one or more required columns'
    END AS CompatibilityResult;
