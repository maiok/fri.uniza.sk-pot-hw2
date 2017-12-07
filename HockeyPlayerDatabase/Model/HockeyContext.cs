using HockeyPlayerDatabase.Interfaces;

namespace HockeyPlayerDatabase.Model
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public class HockeyContext : DbContext, IHockeyReport<Club, Player>
    {
        public IQueryable<Club> GetClubs()
        {
            // todo methodChain
            IQueryable<Club> clubsQuery =
                from b in this.Clubs
                orderby b.Name
                select b;

            return clubsQuery;
        }

        public IQueryable<Player> GetPlayers()
        {
            throw new NotImplementedException();
        }

        // todo toList
        public IEnumerable<Club> GetSortedClubs(int maxResultCount)
        {
			IQueryable<Club> clubsQuery =
				from b in this.Clubs
				orderby b.Name
				select b;

            return clubsQuery.ToList();
        }

        public IEnumerable<Player> GetSortedPlayers(int maxResultCount)
        {
            throw new NotImplementedException();
        }

        public double GetPlayerAverageAge()
        {
            throw new NotImplementedException();
        }

        public Player GetYoungestPlayer()
        {
            throw new NotImplementedException();
        }

        public Player GetOldestPlayer()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<int> GetClubPlayerAges()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Player> GetPlayersByAge(int minAge, int maxAge)
        {
            throw new NotImplementedException();
        }

        public ReportResult GetReportByClub(int clubId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the report by age category.
        /// </summary>
        /// <returns>The report by age category.</returns>
        public IDictionary<AgeCategory, ReportResult> GetReportByAgeCategory()
        {
            throw new NotImplementedException();
            // todo zlozity linq vyraz
            // pozriet groupby mozno vrati dictionary
            // cize grupovanie
        }

        public void SaveToXml(string fileName)
        {
            throw new NotImplementedException();
        }

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