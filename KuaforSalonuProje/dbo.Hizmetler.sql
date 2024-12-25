CREATE TABLE [dbo].[Hizmetler] (
    [HizmetId]  INT             IDENTITY (1, 1) NOT NULL,
    [HizmetAdi] NVARCHAR (MAX)  NOT NULL,
    [Ucret]     DECIMAL (18, 2) NOT NULL,
    [Sure]      NVARCHAR (MAX)  NOT NULL,
    CONSTRAINT [PK_Hizmetler] PRIMARY KEY CLUSTERED ([HizmetId] ASC)
);

