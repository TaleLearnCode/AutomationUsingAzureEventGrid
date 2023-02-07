MERGE dbo.StorageAccount AS TARGET
USING (VALUES (1, 'stthumbnailgenerator', 'TenantSpecific',     'images',                    1),
              (2, 'stthumbnailgenerator', 'TenantSpecific',     'thumbnails',                NULL),
              (3, 'stthumbnailgenerator', 'SpeakingEngagements', 'presentations',            NULL),
              (4, 'stthumbnailgenerator', 'SpeakingEngagements', 'thumbnails/presentations', NULL))
AS SOURCE (StorageAccountId, ResourceName, ContainerName, FolderPath, AllowSubfolders)
ON TARGET.StorageAccountId = SOURCE.StorageAccountId
WHEN MATCHED THEN UPDATE SET TARGET.ResourceName    = SOURCE.ResourceName,
                             TARGET.ContainerName   = SOURCE.ContainerName,
                             TARGET.FolderPath      = SOURCE.FolderPath,
                             TARGET.AllowSubfolders = SOURCE.AllowSubfolders
WHEN NOT MATCHED THEN INSERT (StorageAccountId,
                              ResourceName,
                              ContainerName,
                              FolderPath,
                              AllowSubfolders)
                      VALUES (SOURCE.StorageAccountId,
                              SOURCE.ResourceName,
                              SOURCE.ContainerName,
                              SOURCE.FolderPath,
                              SOURCE.AllowSubfolders)
WHEN NOT MATCHED BY SOURCE THEN DELETE;