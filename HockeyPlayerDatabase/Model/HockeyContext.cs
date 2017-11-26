using HockeyPlayerDatabase.Interfaces;

namespace HockeyPlayerDatabase.Model
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public class HockeyContext : DbContext
    {
        // Your context has been configured to use a 'HockeyContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'HockeyPlayerDatabase.Model.HockeyContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'HockeyContext' 
        // connection string in the application configuration file.
        public HockeyContext() : base("name=HockeyPlayerDatabase")
        {
        }

        public void InsertClub(Club club)
        {
            Clubs.Add(club);
            SaveChanges();
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public DbSet<Club> Clubs { get; set; }
        public DbSet<Player> Players { get; set; }
    }
}