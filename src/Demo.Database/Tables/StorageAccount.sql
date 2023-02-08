CREATE TABLE dbo.StorageAccount
(
  StorageAccountId INT           NOT NULL,
  ResourceName     VARCHAR(24)   NOT NULL,
  ContainerName    VARCHAR(63)   NOT NULL,
  FolderPath       VARCHAR(1024)     NULL,
  AllowSubfolders  BIT               NULL,
  CascadeDeletes   BIT           NOT NULL,
  CONSTRAINT pkcStorageAccount PRIMARY KEY CLUSTERED (StorageAccountId)
)
GO

EXEC sp_addextendedproperty @level0name=N'dbo', @level1name=N'StorageAccount',                                   @value=N'Represents a storage account used by the Thumbnail Generator.',                                                @name=N'MS_Description', @level0type=N'SCHEMA', @level1type=N'TABLE';
GO
EXEC sp_addextendedproperty @level0name=N'dbo', @level1name=N'StorageAccount', @level2name=N'StorageAccountId',  @value=N'Identifier for the referenced storage account.',                                                               @name=N'MS_Description', @level0type=N'SCHEMA', @level1type=N'TABLE', @level2type=N'COLUMN';
GO
EXEC sp_addextendedproperty @level0name=N'dbo', @level1name=N'StorageAccount', @level2name=N'ResourceName',      @value=N'The resource name of the configured storage account.',                                                         @name=N'MS_Description', @level0type=N'SCHEMA', @level1type=N'TABLE', @level2type=N'COLUMN';
GO
EXEC sp_addextendedproperty @level0name=N'dbo', @level1name=N'StorageAccount', @level2name=N'ContainerName',     @value=N'The name of the configured storage container.',                                                                @name=N'MS_Description', @level0type=N'SCHEMA', @level1type=N'TABLE', @level2type=N'COLUMN';
GO
EXEC sp_addextendedproperty @level0name=N'dbo', @level1name=N'StorageAccount', @level2name=N'FolderPath',        @value=N'The blob folder path prefrix.  Leave NULL for no folder suffix or to include all folders within a container.', @name=N'MS_Description', @level0type=N'SCHEMA', @level1type=N'TABLE', @level2type=N'COLUMN';
GO
EXEC sp_addextendedproperty @level0name=N'dbo', @level1name=N'StorageAccount', @level2name=N'AllowSubfolders',   @value=N'Indicates whether subfolders are allowed from FolderPath.  Ignored if FolderPath is NULL.',                    @name=N'MS_Description', @level0type=N'SCHEMA', @level1type=N'TABLE', @level2type=N'COLUMN';
GO
EXEC sp_addextendedproperty @level0name=N'dbo', @level1name=N'StorageAccount', @level2name=N'CascadeDeletes',    @value=N'Indicates whether generated thumbnails will be deleted when the origin is deleted.',                           @name=N'MS_Description', @level0type=N'SCHEMA', @level1type=N'TABLE', @level2type=N'COLUMN';
GO
EXEC sp_addextendedproperty @level0name=N'dbo', @level1name=N'StorageAccount', @level2name=N'pkcStorageAccount', @value=N'Defines the primary key for the StorageAccount table using the StorageAccountId column.',                      @name=N'MS_Description', @level0type=N'SCHEMA', @level1type=N'TABLE', @level2type=N'CONSTRAINT';
GO