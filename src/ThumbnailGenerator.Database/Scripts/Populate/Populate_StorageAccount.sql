MERGE dbo.StorageAccount AS TARGET
USING (VALUES (1, 1, 'stthumbnailgenerator2', 'TenantSpecific',      'images',                   1),
              (2, 0, 'stthumbnailgenerator2', 'TenantSpecific',      'thumbnails',               NULL),
              (3, 1, 'stthumbnailgenerator2', 'SpeakingEngagements', 'presentations',            NULL),
              (4, 0, 'stthumbnailgenerator2', 'SpeakingEngagements', 'thumbnails/presentations', NULL))
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