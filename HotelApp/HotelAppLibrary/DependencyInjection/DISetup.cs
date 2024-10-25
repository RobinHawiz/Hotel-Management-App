using HotelAppLibrary.Data;
using HotelAppLibrary.Databases;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HotelAppLibrary.DependencyInjection;

public static class DISetup
{
    public static IServiceCollection AddDatabaseAccess(this IServiceCollection services, IConfiguration config)
    {
        string dbChoice = config.GetValue<string>("DatabaseChoice").ToLower();
        if (dbChoice == "sql")
        {
            services.AddTransient<IDatabaseData, SqlData>();
        }
        else if (dbChoice == "sqlite")
        {
            services.AddTransient<IDatabaseData, SqliteData>();
        }
        else
        {
            // Fallback / Default value
            services.AddTransient<IDatabaseData, SqlData>();
        }

        services.AddTransient<ISqlDataAccess, SqlDataAccess>();
        services.AddTransient<ISqliteDataAccess, SqliteDataAccess>();

        return services;
    }
}
