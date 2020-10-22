create table Item
(
	ItemId int not null identity(1, 1),
	ItemText nvarchar(200) not null,
	CreatedDate datetime2 not null default getdate(),
	CreatedBy varchar(200) not null default system_user
);

insert into Item(ItemText) values ('the'), ('actual'), ('items'), ('are'), ('irrelevant');