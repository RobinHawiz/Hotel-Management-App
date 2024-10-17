CREATE PROCEDURE [dbo].[sp_RoomTypes_GetById]
	@id int
AS
begin
	set nocount on;

	select [Id], [Title], [Description], [Price]
	from dbo.RoomTypes
	where Id = @id;

end
