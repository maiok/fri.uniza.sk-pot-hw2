using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HockeyPlayerDatabase.Model;

namespace HockeyPlayerDatabase.ImportDataApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("HockeyPlayerDatabase");

            using (var db = new HockeyContext())
            {
                Club club = new Club("Treti klub", "Kukucinova 2, Zilina", "http://url");
                db.InsertClub(club);

                // lezie do bezparametrickeho konstruktora, why?
                var query = from b in db.Clubs
                    orderby b.Name
                    select b;

                foreach (var item in query)
                {
                    Console.WriteLine(item.Name);
                }

                Console.WriteLine("Koniec");
            }

            
            //Console.WriteLine(club.ToString());

            //Player player = new Player("Mario", "Kemen", "Hromoblesk", 1991, 1548975, AgeCategory.Midgest, null);
            //Console.WriteLine(player.ToString());

            Console.ReadLine();
        }
    }
}
