CREATE PROCEDURE [dbo].[spRoomTypes_GetAvailableTypes]
	@startDate date,
	@endDate date
AS
begin
set nocount on;

	select t.Id, t.Title, t.Description, T.Price
	from dbo.Rooms r
	inner join dbo.RoomTypes t on t.Id = r.RoomTypeId
	where r.Id not in (
	select b.RoomId
	from dbo.Bookings b
	where (@startDate >= b.StartDate and @startDate < b.EndDate)
	or (@endDate >= b.StartDate and @endDate <= b.EndDate)
	or(b.StartDate >= @startDate and b.StartDate <= @endDate)
	)
	group by t.Id, t.Title, t.Description, T.Price;
end
