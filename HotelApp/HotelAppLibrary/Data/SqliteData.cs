﻿using HotelAppLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelAppLibrary.Data;
public class SqliteData : IDatabaseData
{
    public void BookGuest(string firstName, string lastName, DateTime startDate, DateTime endDate, int roomTypeId)
    {
        throw new NotImplementedException();
    }

    public void CheckInGuest(int bookingId)
    {
        throw new NotImplementedException();
    }

    public List<RoomTypeModel> GetAvailableRoomTypes(DateTime startDate, DateTime endDate)
    {
        throw new NotImplementedException();
    }

    public RoomTypeModel GetRoomTypeModelById(int id)
    {
        throw new NotImplementedException();
    }

    public List<BookingFullModel> SearchBookings(string lastName)
    {
        throw new NotImplementedException();
    }
}
