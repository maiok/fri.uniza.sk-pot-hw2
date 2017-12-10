using System.Security.Cryptography;
using System.Xml.Linq;
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

        public IQueryable<String> GetClubsName()
        {
            IQueryable<String> clubsQuery =
                from b in this.Clubs
                orderby b.Name
                select b.Name;

            return clubsQuery;
        }


        public IQueryable<Player> GetPlayers()
        {
            IQueryable<Player> players =
                from p in this.Players
                orderby p.LastName
                select p;

            return players;
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

        /*
         * Do filename posielam absolutnu cestu k suboru
         * zdroj: http://www.dotnettricks.com/learn/linq/create-xml-from-database-using-linq
         */
        public void SaveToXml(string fileName)
        {
            XElement mainXML = new XElement("ROOT");

            XElement clubsXml = new XElement("Clubs",
                (from c in this.Clubs
                 select new
                 {
                     c.Id,
                     c.Name,
                     c.Address,
                     c.Url
                 }).ToList().Select(
                    x => new XElement("Club",
                        new XAttribute("ID", x.Id),
                        new XAttribute("Name", x.Name),
                        new XAttribute("Address", x.Address),
                        new XAttribute("Url", x.Url)
                    )));

            XElement playersXml = new XElement("Players",
                (from p in this.Players
                    select new
                    {
                        p.Id,
                        p.FirstName,
                        p.LastName,
                        p.TitleBefore,
                        p.YearOfBirth,
                        p.KrpId,
                        p.AgeCategory,
                        p.Club.Name
                    }).ToList().Select(
                    x => new XElement("Player",
                        new XAttribute("ID", x.Id),
                        new XAttribute("FirstName", x.FirstName),
                        new XAttribute("LastName", x.LastName),
                        new XAttribute("TitleBefore", x.TitleBefore),
                        new XAttribute("YearOfBirth", x.YearOfBirth),
                        new XAttribute("KRP", x.KrpId),
                        new XAttribute("AgeCategory", x.AgeCategory),
                        new XAttribute("Club", x.Name)
                    )));

            mainXML.Add(clubsXml);
            mainXML.Add(playersXml);

            mainXML.Save(fileName);

            Console.WriteLine(mainXML);
        }

        public List<Player> ApplyFilterPlayers(int? krpId, String firstName, String lastName, int? birthFrom,
            int? birthTo, List<AgeCategory> ageCategories, String club)
        {
            // Zakladny full query
            var query = from p in this.Players
                        select p;
            // Where klauzuly
            // KRP
            if (krpId != null)
            {
                query = query.Where(p => p.KrpId == krpId);
            }
            // Meno hraca
            if (firstName != null)
            {
                query = query.Where(p => p.FirstName.Contains(firstName));
            }
            //  Priezvisko
            if (lastName != null)
            {
                query = query.Where(p => p.LastName.Contains(lastName));
            }
            // Rok narodenia OD
            if (birthFrom != null)
            {
                query = query.Where(p => p.YearOfBirth >= birthFrom);
            }
            // Rok narodenia DO
            if (birthTo != null)
            {
                query = query.Where(p => p.YearOfBirth <= birthTo);
            }
            // Vekova kategoria
            if (ageCategories.Count > 0)
            {
                query = query.Where(p => ageCategories.Contains((AgeCategory)p.AgeCategory));
            }
            // Nazov klubu
            if (club != null)
            {
                query = query.Where(p => p.Club.Name.Contains(club));
            }

            // OrderBy Priezviska
            query = query.OrderBy(p => p.LastName);

            return query.ToList();
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

        public void InsertPlayer(Player player)
        {
            Players.Add(player);
            SaveChanges();
        }

        public void UpdatePlayer(Player player)
        {
            if (player != null)
            {
                Player playerToUpdate = GetPlayerByKrp(player.KrpId)[0];
                if (playerToUpdate != null)
                {
                    RemovePlayer(playerToUpdate);
                    // Vymazem a vlozim, ja viem, ugly, ale berte to prosim s rezervou
                    //this.Entry(playerToUpdate).CurrentValues.SetValues(player);
                }
                InsertPlayer(player);
                SaveChanges();
            }
        }

        public void ClearDatabase()
        {
            this.Database.ExecuteSqlCommand("DELETE FROM dbo.Players");
            this.Database.ExecuteSqlCommand("DELETE FROM dbo.Clubs");
            SaveChanges();
        }

        public int? GetClubIdByName(string name)
        {
            var query =
                from b in this.Clubs
                where b.Name.Contains(name)
                select b.Id;

            if (query.ToArray().Length > 0)
            {
                return Int32.Parse(query.ToArray().GetValue(0).ToString());
            }

            return null;
        }

        public List<Player> GetPlayerByKrp(int krpId)
        {
            var query =
                from p in this.Players
                where p.KrpId == krpId
                select p;

            return query.ToList();
        }

        public void RemovePlayer(Player player)
        {
            this.Players.Remove(player);
            SaveChanges();
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public DbSet<Club> Clubs { get; set; }
        public DbSet<Player> Players { get; set; }
    }
}