using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HockeyPlayerDatabase.Interfaces;
using HockeyPlayerDatabase.Model;

namespace HockeyPlayerDatabase.ImportDataApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("HockeyPlayerDatabase");

            Club club = new Club("Prvy klub", "Kukucinova 2, Zilina", "http://url");
            Console.WriteLine(club.ToString());



            Console.ReadLine();
        }
    }
}
