﻿using HotelAppLibrary.Databases;
using HotelAppLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelAppLibrary.Data;
public class SqliteData : IDatabaseData
{
    private readonly ISqliteDataAccess _db;
    private const string connectionStringName = "SqliteDb";

    public SqliteData(ISqliteDataAccess db)
    {
        _db = db;
    }

    public void BookGuest(string firstName, string lastName, DateTime startDate, DateTime endDate, int roomTypeId)
    {
        string sql = @"select 1 from Guests where FirstName = @firstName and LastName = @lastName";

        var result = _db.LoadData<dynamic, dynamic>(sql, new { firstName, lastName }, connectionStringName).Count();

        if (result == 0)
        {
            sql = @"insert into Guests (FirstName, LastName)
		            values (@firstName, @lastName);";

            _db.SaveData(sql, new { firstName, lastName }, connectionStringName);
        }

        sql = @"select Id, FirstName, LastName
	            from Guests
	            where FirstName = @firstName and LastName = @lastName LIMIT 1";

        GuestModel guest = _db.LoadData<GuestModel, dynamic>(sql,
                                                             new { firstName, lastName },
                                                             connectionStringName).First();

        sql = @"select * from RoomTypes where Id = @Id";

        RoomTypeModel roomType = _db.LoadData<RoomTypeModel, dynamic>(sql,
                                                                      new { Id = roomTypeId },
                                                                      connectionStringName).First();

        TimeSpan timeStaying = endDate.Date.Subtract(startDate.Date);

        sql = @"select r.*
                from Rooms r
                inner join RoomTypes t on t.Id = r.RoomTypeId
                where r.RoomTypeId = @roomTypeId
                and
                r.Id not in (
                select b.RoomId
                from Bookings b
                where (@startDate >= b.StartDate and @startDate < b.EndDate)
                or (@endDate >= b.StartDate and @endDate <= b.EndDate)
                or(b.StartDate >= @startDate and b.StartDate <= @endDate)
                )";

        List<RoomModel> availableRooms = _db.LoadData<RoomModel, dynamic>(sql,
                                                                          new { startDate, endDate, roomTypeId },
                                                                          connectionStringName);

        sql = @"insert into Bookings(RoomId, GuestId, StartDate, EndDate, TotalCost)
	            values(@roomId, @guestId, @startDate, @endDate, @totalCost)";

        _db.SaveData(sql,
              new
              {
                  roomId = availableRooms.First().Id,
                  guestId = guest.Id,
                  startDate = startDate,
                  endDate = endDate,
                  totalCost = timeStaying.Days * roomType.Price
              },
              connectionStringName);

    }

    public void CheckInGuest(int bookingId)
    {
        string sql = @"update Bookings
	                   set CheckedIn = 1
	                   where Id = @Id;";

        _db.SaveData(sql, new { Id = bookingId }, connectionStringName);
    }

    public List<RoomTypeModel> GetAvailableRoomTypes(DateTime startDate, DateTime endDate)
    {
        string sql = @"select t.Id, t.Title, t.Description, T.Price
	                   from Rooms r
	                   inner join RoomTypes t on t.Id = r.RoomTypeId
	                   where r.Id not in (
	                   select b.RoomId
	                   from Bookings b
	                   where (@startDate >= b.StartDate and @startDate < b.EndDate)
	                   or (@endDate >= b.StartDate and @endDate <= b.EndDate)
	                   or(b.StartDate >= @startDate and b.StartDate <= @endDate)
	                   )
	                   group by t.Id, t.Title, t.Description, T.Price;";

        var output = _db.LoadData<RoomTypeModel, dynamic>(sql, new { startDate, endDate }, connectionStringName);

        output.ForEach(x => { x.Price /= 100; });

        return output;
    }

    public RoomTypeModel GetRoomTypeModelById(int id)
    {
        string sql = @"select [Id], [Title], [Description], [Price]
	                   from RoomTypes
	                   where Id = @id;";

        return _db.LoadData<RoomTypeModel, dynamic>(sql,
                                                    new { id }, connectionStringName).FirstOrDefault();
    }

    public List<BookingFullModel> SearchBookings(string lastName)
    {
        string sql = @"select [b].[Id], [b].[RoomId], [b].[GuestId], [b].[StartDate], [b].[EndDate], [b].[CheckedIn], [b].[TotalCost], 
	                   [g].[FirstName], [g].[LastName], 
	                   [r].[RoomNumber], [r].[RoomTypeId], 
	                   [rt].[Title], [rt].[Description], [rt].[Price]
	                   from Bookings b
	                   inner join Guests g on g.id = b.GuestId
	                   inner join Rooms r on r.id = b.RoomId
	                   inner join RoomTypes rt on rt.id = r.RoomTypeId
	                   where b.StartDate = @startDate and g.LastName = @lastName";


        var output = _db.LoadData<BookingFullModel, dynamic>(sql,
                                                     new { lastName, startDate = DateTime.Now.Date },
                                                     connectionStringName);

        output.ForEach(x => { x.TotalCost /= 100; });

        return output;
    }
}
