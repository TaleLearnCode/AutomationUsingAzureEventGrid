MERGE dbo.ThumbnailGeneratorConfig AS TARGET
USING (VALUES (1, 'Tenant Specific', 1, 2, 0, 100, NULL, NULL),
              (2, 'Speaking Engagements', 3, 4, 0, 150, NULL, NULL))
AS SOURCE (ThumbnailGeneratorConfigId,
           ThumbnailGeneratorConfigName,
           OriginStorageAccountId,
           ThumbnailStorageAccountId,
           ThumbnailHeight,
           ThumbnailWidth,
           ThumbnailPrefix,
           ThumbnailSuffix)
ON TARGET.ThumbnailGeneratorConfigId = SOURCE.ThumbnailGeneratorConfigId
WHEN MATCHED THEN UPDATE SET TARGET.ThumbnailGeneratorConfigName = SOURCE.ThumbnailGeneratorConfigName,
                             TARGET.OriginStorageAccountId       = SOURCE.OriginStorageAccountId,
                             TARGET.ThumbnailStorageAccountId    = SOURCE.ThumbnailStorageAccountId,
                             TARGET.ThumbnailHeight              = SOURCE.ThumbnailHeight,
                             TARGET.ThumbnailWidth               = SOURCE.ThumbnailWidth,
                             TARGET.ThumbnailPrefix              = SOURCE.ThumbnailPrefix,
                             TARGET.ThumbnailSuffix              = SOURCE.ThumbnailSuffix
WHEN NOT MATCHED THEN INSERT (ThumbnailGeneratorConfigId,
                              ThumbnailGeneratorConfigName,
                              OriginStorageAccountId,
                              ThumbnailStorageAccountId,
                              ThumbnailHeight,
                              ThumbnailWidth,
                              ThumbnailPrefix,
                              ThumbnailSuffix)
                      VALUES (SOURCE.ThumbnailGeneratorConfigId,
                              SOURCE.ThumbnailGeneratorConfigName,
                              SOURCE.OriginStorageAccountId,
                              SOURCE.ThumbnailStorageAccountId,
                              SOURCE.ThumbnailHeight,
                              SOURCE.ThumbnailWidth,
                              SOURCE.ThumbnailPrefix,
                              SOURCE.ThumbnailSuffix)
WHEN NOT MATCHED BY SOURCE THEN DELETE;