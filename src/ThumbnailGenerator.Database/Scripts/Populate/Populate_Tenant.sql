SET IDENTITY_INSERT dbo.Tenant ON
GO

MERGE dbo.Tenant AS TARGET
USING (VALUES (1, '9EAEC392-C918-4D59-843F-B56A9D36E34B', 'Northstar Senior Living', 'sqldb-thumbnailgenerator-dev-use', 'sql-NorthstarSeniorLiving-dev-use'))
AS SOURCE (TenantId,
           TenantKey,
           TenantName,
           DatabaseServerName,
           DatabaseName)
ON TARGET.TenantId = SOURCE.TenantId
WHEN MATCHED THEN UPDATE SET TARGET.TenantKey              = SOURCE.TenantKey,
                             TARGET.TenantName             = SOURCE.TenantName,
                             TARGET.DatabaseServerName     = SOURCE.DatabaseServerName,
                             TARGET.DatabaseName           = SOURCE.DatabaseName
WHEN NOT MATCHED THEN INSERT (TenantId,
                              TenantKey,
                              TenantName,
                              DatabaseServerName,
                              DatabaseName)
                      VALUES (SOURCE.TenantId,
                              SOURCE.TenantKey,
                              SOURCE.TenantName,
                              SOURCE.DatabaseServerName,
                              SOURCE.DatabaseName)
WHEN NOT MATCHED BY SOURCE THEN DELETE;

SET IDENTITY_INSERT dbo.Tenant OFF
GO