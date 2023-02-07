CREATE TABLE dbo.ThumbnailGeneratorConfig
(
  ThumbnailGeneratorConfigId   INT          NOT NULL,
  ThumbnailGeneratorConfigName VARCHAR(100) NOT NULL,
  OriginStorageAccountId       INT          NOT NULL,
  ThumbnailStorageAccountId    INT          NOT NULL,
  ThumbnailHeight              INT          NOT NULL,
  ThumbnailWidth               INT          NOT NULL,
  ThumbnailPrefix              VARCHAR(20)      NULL,
  ThumbnailSuffix              VARCHAR(20)      NULL,
  CONSTRAINT pkcThumbnailGenerator PRIMARY KEY CLUSTERED (ThumbnailGeneratorConfigId),
  CONSTRAINT fkThumbnailGenerator_StorageAccount_OriginalStorageAccountId  FOREIGN KEY (OriginStorageAccountId)   REFERENCES dbo.StorageAccount (StorageAccountId),
  CONSTRAINT fkThumbnailGenerator_StorageAccount_ThumbnailStorageAccountId FOREIGN KEY (ThumbnailStorageAccountId) REFERENCES dbo.StorageAccount (StorageAccountId)
)
GO

EXEC sp_addextendedproperty @level0name=N'dbo', @level1name=N'ThumbnailGeneratorConfig',                                                                               @value=N'Represents the configuration settings for a particular thumbnail generator activity.',                                                       @name=N'MS_Description', @level0type=N'SCHEMA', @level1type=N'TABLE';
GO
EXEC sp_addextendedproperty @level0name=N'dbo', @level1name=N'ThumbnailGeneratorConfig', @level2name=N'ThumbnailGeneratorConfigId',                                    @value=N'Identifier for the thumbnail genrator configuration.',                                                                                       @name=N'MS_Description', @level0type=N'SCHEMA', @level1type=N'TABLE', @level2type=N'COLUMN';
GO
EXEC sp_addextendedproperty @level0name=N'dbo', @level1name=N'ThumbnailGeneratorConfig', @level2name=N'ThumbnailGeneratorConfigName',                                  @value=N'The name of the thumbnail generator configuration; used to make the configuration human identifiable.',                                      @name=N'MS_Description', @level0type=N'SCHEMA', @level1type=N'TABLE', @level2type=N'COLUMN';
GO
EXEC sp_addextendedproperty @level0name=N'dbo', @level1name=N'ThumbnailGeneratorConfig', @level2name=N'OriginStorageAccountId',                                        @value=N'Identifier of the storage account whether the origin container resides.',                                                                    @name=N'MS_Description', @level0type=N'SCHEMA', @level1type=N'TABLE', @level2type=N'COLUMN';
GO
EXEC sp_addextendedproperty @level0name=N'dbo', @level1name=N'ThumbnailGeneratorConfig', @level2name=N'ThumbnailStorageAccountId',                                     @value=N'Identifier of the storage account whether the thumbnails are to be stored.',                                                                 @name=N'MS_Description', @level0type=N'SCHEMA', @level1type=N'TABLE', @level2type=N'COLUMN';
GO
EXEC sp_addextendedproperty @level0name=N'dbo', @level1name=N'ThumbnailGeneratorConfig', @level2name=N'ThumbnailHeight',                                               @value=N'The height of the outputted thumbnail images.  Setting to 0 indicates that th ethumbnail generator shall be dtermine the outputted height.', @name=N'MS_Description', @level0type=N'SCHEMA', @level1type=N'TABLE', @level2type=N'COLUMN';
GO
EXEC sp_addextendedproperty @level0name=N'dbo', @level1name=N'ThumbnailGeneratorConfig', @level2name=N'ThumbnailWidth',                                                @value=N'The width of the outputed thumbnail images.',                                                                                                @name=N'MS_Description', @level0type=N'SCHEMA', @level1type=N'TABLE', @level2type=N'COLUMN';
GO
EXEC sp_addextendedproperty @level0name=N'dbo', @level1name=N'ThumbnailGeneratorConfig', @level2name=N'ThumbnailPrefix',                                               @value=N'The prefix, if any, to add to the outputted thumbnail image file name.',                                                                     @name=N'MS_Description', @level0type=N'SCHEMA', @level1type=N'TABLE', @level2type=N'COLUMN';
GO
EXEC sp_addextendedproperty @level0name=N'dbo', @level1name=N'ThumbnailGeneratorConfig', @level2name=N'ThumbnailSuffix',                                               @value=N'The suffix, if any, to add to the outputted thumbnail image file name.',                                                                     @name=N'MS_Description', @level0type=N'SCHEMA', @level1type=N'TABLE', @level2type=N'COLUMN';
GO
EXEC sp_addextendedproperty @level0name=N'dbo', @level1name=N'ThumbnailGeneratorConfig', @level2name=N'pkcThumbnailGenerator',                                         @value=N'Defines the primary key for the ThumbnailGeneratorConfig table using the ThumbnailGeneratorConfigId column.',                                @name=N'MS_Description', @level0type=N'SCHEMA', @level1type=N'TABLE', @level2type=N'CONSTRAINT';
GO
EXEC sp_addextendedproperty @level0name=N'dbo', @level1name=N'ThumbnailGeneratorConfig', @level2name=N'fkThumbnailGenerator_StorageAccount_OriginalStorageAccountId',  @value=N'Defines the relationship between the ThumbnailGeneratorConfig and StorageAccount tables using the OriginStorageId column.',                  @name=N'MS_Description', @level0type=N'SCHEMA', @level1type=N'TABLE', @level2type=N'CONSTRAINT';
GO
EXEC sp_addextendedproperty @level0name=N'dbo', @level1name=N'ThumbnailGeneratorConfig', @level2name=N'fkThumbnailGenerator_StorageAccount_ThumbnailStorageAccountId', @value=N'Defines the relationship between the ThumbnailGeneratorConfig and StorageAccount tables using the ThumbnailStorageId column.',               @name=N'MS_Description', @level0type=N'SCHEMA', @level1type=N'TABLE', @level2type=N'CONSTRAINT';
GO