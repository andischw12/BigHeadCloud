SET NOCOUNT ON;
SET XACT_ABORT ON;

/* Safe to run again. GameKey 2 is Rosh Gadol 2. */
IF OBJECT_ID(N'dbo.BigHeadGameData', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.BigHeadGameData
    (
        OID            VARCHAR(200) NOT NULL,
        GameKey        TINYINT NOT NULL,
        PlayerSlot     TINYINT NOT NULL,
        DataJson       NVARCHAR(MAX) NOT NULL,
        Points         INT NOT NULL CONSTRAINT DF_BigHeadGameData_Points DEFAULT (0),
        ShabbatPoints  INT NOT NULL CONSTRAINT DF_BigHeadGameData_ShabbatPoints DEFAULT (0),
        HanukkaPoints  INT NOT NULL CONSTRAINT DF_BigHeadGameData_HanukkaPoints DEFAULT (0),
        PurimPoints    INT NOT NULL CONSTRAINT DF_BigHeadGameData_PurimPoints DEFAULT (0),
        CreatedAt      DATETIME NOT NULL CONSTRAINT DF_BigHeadGameData_CreatedAt DEFAULT (GETDATE()),
        UpdatedAt      DATETIME NOT NULL CONSTRAINT DF_BigHeadGameData_UpdatedAt DEFAULT (GETDATE()),
        CONSTRAINT PK_BigHeadGameData PRIMARY KEY CLUSTERED (OID, GameKey, PlayerSlot),
        CONSTRAINT CK_BigHeadGameData_GameKey CHECK (GameKey > 0),
        CONSTRAINT CK_BigHeadGameData_PlayerSlot CHECK (PlayerSlot BETWEEN 0 AND 3),
        CONSTRAINT CK_BigHeadGameData_DataJson CHECK (LEN(DataJson) BETWEEN 2 AND 250000)
    );
    CREATE INDEX IX_BigHeadGameData_GameKey_UpdatedAt
        ON dbo.BigHeadGameData (GameKey, UpdatedAt DESC)
        INCLUDE (OID, PlayerSlot, Points);
END;
GO

IF OBJECT_ID(N'dbo.BigHeadGameResults', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.BigHeadGameResults
    (
        ResultID    BIGINT IDENTITY(1, 1) NOT NULL CONSTRAINT PK_BigHeadGameResults PRIMARY KEY CLUSTERED,
        OID         VARCHAR(200) NOT NULL,
        GameKey     TINYINT NOT NULL,
        WinnerName  NVARCHAR(200) NOT NULL,
        LoserName   NVARCHAR(200) NOT NULL,
        CreatedAt   DATETIME NOT NULL CONSTRAINT DF_BigHeadGameResults_CreatedAt DEFAULT (GETDATE())
    );
    CREATE INDEX IX_BigHeadGameResults_OID_CreatedAt
        ON dbo.BigHeadGameResults (OID, CreatedAt DESC)
        INCLUDE (GameKey, WinnerName, LoserName);
END;
GO
