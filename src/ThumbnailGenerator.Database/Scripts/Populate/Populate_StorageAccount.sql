MERGE dbo.StorageAccount AS TARGET
USING (VALUES (1, 1, 'stmythumbnailgenerator', 'TenantSpecific',      'images',                   1),
              (2, 0, 'stmythumbnailgenerator', 'TenantSpecific',      'thumbnails',               NULL),
              (3, 1, 'stmythumbnailgenerator', 'speakingengagements', 'presentations',            NULL),
              (4, 0, 'stmythumbnailgenerator', 'speakingengagements', 'thumbnails/presentations', NULL))
AS SOURCE (StorageAccountId,
           CascadeDeletes,
           ResourceName,
           ContainerName,
           FolderPath,
           AllowSubfolders)
ON TARGET.StorageAccountId = SOURCE.StorageAccountId
WHEN MATCHED THEN UPDATE SET TARGET.ResourceName    = SOURCE.ResourceName,
                             TARGET.ContainerName   = SOURCE.ContainerName,
                             TARGET.FolderPath      = SOURCE.FolderPath,
                             TARGET.AllowSubfolders = SOURCE.AllowSubfolders,
                             TARGET.CascadeDeletes  = SOURCE.CascadeDeletes
WHEN NOT MATCHED THEN INSERT (StorageAccountId,
                              ResourceName,
                              ContainerName,
                              FolderPath,
                              AllowSubfolders,
                              CascadeDeletes)
                      VALUES (SOURCE.StorageAccountId,
                              SOURCE.ResourceName,
                              SOURCE.ContainerName,
                              SOURCE.FolderPath,
                              SOURCE.AllowSubfolders,
                              SOURCE.CascadeDeletes)
WHEN NOT MATCHED BY SOURCE THEN DELETE;