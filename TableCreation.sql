CREATE TABLE ToDoItem(
	ID int Primary KEY Identity(1,1),
	OwnerID nvarchar(450),
	Title nvarchar(75) NOT NULL,
	[Description] nvarchar(max),
	DueDate DateTime NOT NULL,
	CreatedDate DateTime NOT NULL,
	Complete bit NOT NULL
	CONSTRAINT FK_TaskOwner
	FOREIGN KEY(OwnerID)
	REFERENCES AspNetUsers(Id)
);

INSERT INTO ToDoItem
VALUES('df1564c9-bc18-492c-b084-456d4b0abbb5',
'This is a task',
12/12/2020,
11/20/2020,
0)