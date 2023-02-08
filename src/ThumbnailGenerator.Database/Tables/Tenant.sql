CREATE TABLE dbo.Tenant
(
  TenantId           INT           NOT NULL IDENTITY(1,1),
  TenantKey          CHAR(36)      NOT NULL,
  TenantName         NVARCHAR(100) NOT NULL,
  DatabaseServerName VARCHAR(100)  NOT NULL,
  DatabaseName       VARCHAR(100)  NOT NULL,
  CONSTRAINT pkcTenant PRIMARY KEY CLUSTERED (TenantId)
)
GO

EXEC sp_addextendedproperty @level0name=N'dbo', @level1name=N'Tenant',                                    @value=N'Represents a tenant organization.',                                        @name=N'MS_Description', @level0type=N'SCHEMA', @level1type=N'TABLE';
GO
EXEC sp_addextendedproperty @level0name=N'dbo', @level1name=N'Tenant', @level2name=N'TenantId',           @value=N'Identifier of the tenant record.',                                         @name=N'MS_Description', @level0type=N'SCHEMA', @level1type=N'TABLE', @level2type=N'COLUMN';
GO
EXEC sp_addextendedproperty @level0name=N'dbo', @level1name=N'Tenant', @level2name=N'TenantKey',          @value=N'Key to be used for applications to identify a tenant organization.',       @name=N'MS_Description', @level0type=N'SCHEMA', @level1type=N'TABLE', @level2type=N'COLUMN';
GO
EXEC sp_addextendedproperty @level0name=N'dbo', @level1name=N'Tenant', @level2name=N'TenantName',         @value=N'The name of the tenant organization.',                                     @name=N'MS_Description', @level0type=N'SCHEMA', @level1type=N'TABLE', @level2type=N'COLUMN';
GO
EXEC sp_addextendedproperty @level0name=N'dbo', @level1name=N'Tenant', @level2name=N'DatabaseServerName', @value=N'The name of the primary database server used by the tenant organization.', @name=N'MS_Description', @level0type=N'SCHEMA', @level1type=N'TABLE', @level2type=N'COLUMN';
GO
EXEC sp_addextendedproperty @level0name=N'dbo', @level1name=N'Tenant', @level2name=N'DatabaseName',       @value=N'The name of the database used by the tenant organization.',                @name=N'MS_Description', @level0type=N'SCHEMA', @level1type=N'TABLE', @level2type=N'COLUMN';
GO
EXEC sp_addextendedproperty @level0name=N'dbo', @level1name=N'Tenant', @level2name=N'pkcTenant',          @value=N'Defines the primary key for the Tenant table using the TenantId column.',  @name=N'MS_Description', @level0type=N'SCHEMA', @level1type=N'TABLE', @level2type=N'CONSTRAINT';
GO