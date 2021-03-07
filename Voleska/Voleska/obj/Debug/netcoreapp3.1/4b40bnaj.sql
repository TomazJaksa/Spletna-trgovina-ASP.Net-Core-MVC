IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [AspNetRoles] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Blog] (
    [ID] int NOT NULL IDENTITY,
    [Naslov] nvarchar(max) NULL,
    [Slika] nvarchar(max) NULL,
    [Povzetek] nvarchar(max) NULL,
    [Clanek] nvarchar(max) NULL,
    [Bloger] nvarchar(max) NULL,
    [Dodan] datetime2 NOT NULL,
    [Posodobljen] datetime2 NOT NULL,
    [SteviloLike] int NOT NULL,
    [SteviloDislike] int NOT NULL,
    CONSTRAINT [PK_Blog] PRIMARY KEY ([ID])
);

GO

CREATE TABLE [Material] (
    [ID] int NOT NULL IDENTITY,
    [Ime] nvarchar(max) NULL,
    [Opis] nvarchar(max) NULL,
    CONSTRAINT [PK_Material] PRIMARY KEY ([ID])
);

GO

CREATE TABLE [Novice] (
    [ID] int NOT NULL IDENTITY,
    [Email] nvarchar(max) NULL,
    [Dodan] datetime2 NOT NULL,
    [Posodobljen] datetime2 NOT NULL,
    CONSTRAINT [PK_Novice] PRIMARY KEY ([ID])
);

GO

CREATE TABLE [Posta] (
    [ID] int NOT NULL IDENTITY,
    [PostnaStevilka] nvarchar(max) NULL,
    [Kraj] nvarchar(max) NULL,
    [Dodan] datetime2 NOT NULL,
    [Posodobljen] datetime2 NOT NULL,
    CONSTRAINT [PK_Posta] PRIMARY KEY ([ID])
);

GO

CREATE TABLE [TipIzdelka] (
    [ID] int NOT NULL IDENTITY,
    [Ime] nvarchar(max) NULL,
    [Slika] nvarchar(max) NULL,
    CONSTRAINT [PK_TipIzdelka] PRIMARY KEY ([ID])
);

GO

CREATE TABLE [TipOpcije] (
    [ID] int NOT NULL IDENTITY,
    [Ime] nvarchar(max) NULL,
    CONSTRAINT [PK_TipOpcije] PRIMARY KEY ([ID])
);

GO

CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [Naslov] (
    [ID] int NOT NULL IDENTITY,
    [ApplicationUserID] nvarchar(max) NULL,
    [Ulica] nvarchar(max) NULL,
    [HisnaStevilka] nvarchar(max) NULL,
    [Dodan] datetime2 NOT NULL,
    [Posodobljen] datetime2 NOT NULL,
    [PostaID] int NOT NULL,
    CONSTRAINT [PK_Naslov] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_Naslov_Posta_PostaID] FOREIGN KEY ([PostaID]) REFERENCES [Posta] ([ID]) ON DELETE CASCADE
);

GO

CREATE TABLE [Izdelek] (
    [ID] int NOT NULL IDENTITY,
    [Ime] nvarchar(max) NULL,
    [Opis] nvarchar(max) NULL,
    [Podrobnosti] nvarchar(max) NULL,
    [Cena] decimal(18,2) NOT NULL,
    [Izbrisan] bit NOT NULL,
    [PovprecnaOcena] int NOT NULL,
    [SteviloProdanih] int NOT NULL,
    [Dodan] datetime2 NOT NULL,
    [Posodobljen] datetime2 NOT NULL,
    [MaterialID] int NOT NULL,
    [TipIzdelkaID] int NOT NULL,
    CONSTRAINT [PK_Izdelek] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_Izdelek_Material_MaterialID] FOREIGN KEY ([MaterialID]) REFERENCES [Material] ([ID]) ON DELETE CASCADE,
    CONSTRAINT [FK_Izdelek_TipIzdelka_TipIzdelkaID] FOREIGN KEY ([TipIzdelkaID]) REFERENCES [TipIzdelka] ([ID]) ON DELETE CASCADE
);

GO

CREATE TABLE [AspNetUsers] (
    [Id] nvarchar(450) NOT NULL,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    [Ime] nvarchar(100) NULL,
    [Priimek] nvarchar(100) NULL,
    [Aktiven] bit NOT NULL,
    [Dodan] datetime2 NOT NULL,
    [Posodobljen] datetime2 NOT NULL,
    [NaslovID] int NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUsers_Naslov_NaslovID] FOREIGN KEY ([NaslovID]) REFERENCES [Naslov] ([ID]) ON DELETE CASCADE
);

GO

CREATE TABLE [Opcija] (
    [ID] int NOT NULL IDENTITY,
    [Ime] nvarchar(max) NULL,
    [Slika] nvarchar(max) NULL,
    [Zaloga] bit NOT NULL,
    [Dodan] datetime2 NOT NULL,
    [Posodobljen] datetime2 NOT NULL,
    [IzdelekID] int NOT NULL,
    [TipOpcijeID] int NOT NULL,
    CONSTRAINT [PK_Opcija] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_Opcija_Izdelek_IzdelekID] FOREIGN KEY ([IzdelekID]) REFERENCES [Izdelek] ([ID]) ON DELETE CASCADE,
    CONSTRAINT [FK_Opcija_TipOpcije_TipOpcijeID] FOREIGN KEY ([TipOpcijeID]) REFERENCES [TipOpcije] ([ID]) ON DELETE CASCADE
);

GO

CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(128) NOT NULL,
    [ProviderKey] nvarchar(128) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [AspNetUserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    [Discriminator] nvarchar(max) NOT NULL,
    [ApplicationUserID] nvarchar(450) NULL,
    [ApplicationRoleID] nvarchar(450) NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_ApplicationRoleID] FOREIGN KEY ([ApplicationRoleID]) REFERENCES [AspNetRoles] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_ApplicationUserID] FOREIGN KEY ([ApplicationUserID]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [AspNetUserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(128) NOT NULL,
    [Name] nvarchar(128) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [Komentar] (
    [ID] int NOT NULL IDENTITY,
    [Vsebina] nvarchar(max) NULL,
    [SteviloLike] int NOT NULL,
    [SteviloDislike] int NOT NULL,
    [Dodan] datetime2 NOT NULL,
    [Posodobljen] datetime2 NOT NULL,
    [ApplicationUserID] nvarchar(450) NULL,
    [BlogID] int NOT NULL,
    CONSTRAINT [PK_Komentar] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_Komentar_AspNetUsers_ApplicationUserID] FOREIGN KEY ([ApplicationUserID]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Komentar_Blog_BlogID] FOREIGN KEY ([BlogID]) REFERENCES [Blog] ([ID]) ON DELETE CASCADE
);

GO

CREATE TABLE [LajkanjeBlogov] (
    [ID] int NOT NULL IDENTITY,
    [Lajk] bit NULL,
    [Dodan] datetime2 NOT NULL,
    [Posodobljen] datetime2 NOT NULL,
    [ApplicationUserID] nvarchar(450) NULL,
    [BlogID] int NULL,
    CONSTRAINT [PK_LajkanjeBlogov] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_LajkanjeBlogov_AspNetUsers_ApplicationUserID] FOREIGN KEY ([ApplicationUserID]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_LajkanjeBlogov_Blog_BlogID] FOREIGN KEY ([BlogID]) REFERENCES [Blog] ([ID]) ON DELETE NO ACTION
);

GO

CREATE TABLE [OceneIzdelkov] (
    [ID] int NOT NULL IDENTITY,
    [Komentar] nvarchar(max) NULL,
    [Ocena] tinyint NOT NULL,
    [Dodan] datetime2 NOT NULL,
    [Posodobljen] datetime2 NOT NULL,
    [ApplicationUserID] nvarchar(450) NULL,
    [IzdelekID] int NULL,
    CONSTRAINT [PK_OceneIzdelkov] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_OceneIzdelkov_AspNetUsers_ApplicationUserID] FOREIGN KEY ([ApplicationUserID]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_OceneIzdelkov_Izdelek_IzdelekID] FOREIGN KEY ([IzdelekID]) REFERENCES [Izdelek] ([ID]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Transakcija] (
    [ID] int NOT NULL IDENTITY,
    [SkupniZnesek] decimal(18,2) NOT NULL,
    [Dodan] datetime2 NOT NULL,
    [Posodobljen] datetime2 NOT NULL,
    [Zakljuceno] bit NOT NULL,
    [Odposlano] bit NOT NULL,
    [ApplicationUserID] nvarchar(450) NULL,
    CONSTRAINT [PK_Transakcija] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_Transakcija_AspNetUsers_ApplicationUserID] FOREIGN KEY ([ApplicationUserID]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [LajkanjeKomentarjev] (
    [ID] int NOT NULL IDENTITY,
    [Lajk] bit NULL,
    [Dodan] datetime2 NOT NULL,
    [Posodobljen] datetime2 NOT NULL,
    [ApplicationUserID] nvarchar(450) NULL,
    [KomentarID] int NULL,
    CONSTRAINT [PK_LajkanjeKomentarjev] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_LajkanjeKomentarjev_AspNetUsers_ApplicationUserID] FOREIGN KEY ([ApplicationUserID]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_LajkanjeKomentarjev_Komentar_KomentarID] FOREIGN KEY ([KomentarID]) REFERENCES [Komentar] ([ID]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Narocilo] (
    [ID] int NOT NULL IDENTITY,
    [Kolicina] int NOT NULL,
    [Znesek] decimal(18,2) NOT NULL,
    [Opombe] nvarchar(max) NULL,
    [Dodan] datetime2 NOT NULL,
    [Posodobljen] datetime2 NOT NULL,
    [IzdelekID] int NOT NULL,
    [TransakcijaID] int NOT NULL,
    [OpcijaID] int NULL,
    [MaterialID] int NULL,
    CONSTRAINT [PK_Narocilo] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_Narocilo_Izdelek_IzdelekID] FOREIGN KEY ([IzdelekID]) REFERENCES [Izdelek] ([ID]) ON DELETE CASCADE,
    CONSTRAINT [FK_Narocilo_Material_MaterialID] FOREIGN KEY ([MaterialID]) REFERENCES [Material] ([ID]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Narocilo_Opcija_OpcijaID] FOREIGN KEY ([OpcijaID]) REFERENCES [Opcija] ([ID]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Narocilo_Transakcija_TransakcijaID] FOREIGN KEY ([TransakcijaID]) REFERENCES [Transakcija] ([ID]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);

GO

CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;

GO

CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);

GO

CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);

GO

CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);

GO

CREATE INDEX [IX_AspNetUserRoles_ApplicationRoleID] ON [AspNetUserRoles] ([ApplicationRoleID]);

GO

CREATE INDEX [IX_AspNetUserRoles_ApplicationUserID] ON [AspNetUserRoles] ([ApplicationUserID]);

GO

CREATE INDEX [IX_AspNetUsers_NaslovID] ON [AspNetUsers] ([NaslovID]);

GO

CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);

GO

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;

GO

CREATE INDEX [IX_Izdelek_MaterialID] ON [Izdelek] ([MaterialID]);

GO

CREATE INDEX [IX_Izdelek_TipIzdelkaID] ON [Izdelek] ([TipIzdelkaID]);

GO

CREATE INDEX [IX_Komentar_ApplicationUserID] ON [Komentar] ([ApplicationUserID]);

GO

CREATE INDEX [IX_Komentar_BlogID] ON [Komentar] ([BlogID]);

GO

CREATE INDEX [IX_LajkanjeBlogov_ApplicationUserID] ON [LajkanjeBlogov] ([ApplicationUserID]);

GO

CREATE INDEX [IX_LajkanjeBlogov_BlogID] ON [LajkanjeBlogov] ([BlogID]);

GO

CREATE INDEX [IX_LajkanjeKomentarjev_ApplicationUserID] ON [LajkanjeKomentarjev] ([ApplicationUserID]);

GO

CREATE INDEX [IX_LajkanjeKomentarjev_KomentarID] ON [LajkanjeKomentarjev] ([KomentarID]);

GO

CREATE INDEX [IX_Narocilo_IzdelekID] ON [Narocilo] ([IzdelekID]);

GO

CREATE INDEX [IX_Narocilo_MaterialID] ON [Narocilo] ([MaterialID]);

GO

CREATE INDEX [IX_Narocilo_OpcijaID] ON [Narocilo] ([OpcijaID]);

GO

CREATE INDEX [IX_Narocilo_TransakcijaID] ON [Narocilo] ([TransakcijaID]);

GO

CREATE INDEX [IX_Naslov_PostaID] ON [Naslov] ([PostaID]);

GO

CREATE INDEX [IX_OceneIzdelkov_ApplicationUserID] ON [OceneIzdelkov] ([ApplicationUserID]);

GO

CREATE INDEX [IX_OceneIzdelkov_IzdelekID] ON [OceneIzdelkov] ([IzdelekID]);

GO

CREATE INDEX [IX_Opcija_IzdelekID] ON [Opcija] ([IzdelekID]);

GO

CREATE INDEX [IX_Opcija_TipOpcijeID] ON [Opcija] ([TipOpcijeID]);

GO

CREATE INDEX [IX_Transakcija_ApplicationUserID] ON [Transakcija] ([ApplicationUserID]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210129094230_Opombe', N'3.1.8');

GO

