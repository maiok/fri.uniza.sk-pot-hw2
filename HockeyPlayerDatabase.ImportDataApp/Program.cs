using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FileHelpers;
using HockeyPlayerDatabase.Model;

namespace HockeyPlayerDatabase.ImportDataApp
{
    class Program
    {

        private static string _clubsCsvPath, _playersCsvPath;
        private static bool _clearDatabase = false;

        static void Main(string[] args)
        {
            Console.WriteLine("HockeyPlayerDatabase - import dat");

            try
            {
                // nacitanie argumentov, na poradi nezalezi
                var order = 0;
                foreach (var arg in args)
                {
                    if (arg == "-clubs")
                    {
                        _clubsCsvPath = args[order + 1];
                        var engine = new FileHelperEngine<ClubsHeader>();
                        ImportClubs(engine, arg);
                    }
                    if (arg == "-players")
                    {
                        _playersCsvPath = args[order + 1];
                        //ProcessCsvFile(_playersCsvPath, arg);
                    }
                    if (arg == "-clearDatabase")
                    {
                        _clearDatabase = true;
                    }
                    order++;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Nastala chyba pri citani argumentov.");
            }

            // sqlconnection tu? namiesto hockeycontext alebo nejaky controller
            using (var db = new HockeyContext())
            {
                //Club club = new Club("Testovaci klub", "Kukucinova 2, Zilina", "http://url");
                //db.InsertClub(club);


                // lezie do bezparametrickeho konstruktora
                // LINQ
                var query = db.GetClubs();

                // vypis zo selectu
                foreach (var item in query)
                {
                    Console.WriteLine(item.ToString());
                }

                Console.WriteLine("Koniec");
            }


            //Console.WriteLine(club.ToString());

            //Player player = new Player("Mario", "Kemen", "Hromoblesk", 1991, 1548975, AgeCategory.Midgest, null);
            //Console.WriteLine(player.ToString());

            Console.ReadLine();
        }

        /*
         * zdroj: http://www.filehelpers.net/example/QuickStart/ReadFileDelimited/
         */

        private void ImportClubs(var engine, String path)
        {
            var records = engine.ReadFile(path);

            foreach (var record in records)
            {
                if (objType == "-clubs")
                {
                    Console.WriteLine(record.Priezvisko);
                    Console.WriteLine(record.Meno);
                    Console.WriteLine(record.TitulPred);
                    Console.WriteLine(record.RokNarodenia);
                    Console.WriteLine(record.Krp);
                    Console.WriteLine(record.MaterskyKlub);
                    Console.WriteLine(record.VekovaKategoria);
                }
                else
                {
                    Console.WriteLine(record.Priezvisko);
                    Console.WriteLine(record.Meno);
                    Console.WriteLine(record.TitulPred);
                    Console.WriteLine(record.RokNarodenia);
                    Console.WriteLine(record.Krp);
                    Console.WriteLine(record.MaterskyKlub);
                    Console.WriteLine(record.VekovaKategoria);
                }
            }
        }
    }

    [DelimitedRecord(";")]
    public class ClubsHeader
    {
        public String Nazov;
        public String Adresa;
        public String Url;
        public String Empty; // kvoli poslednej bodkociarke
    }

    [DelimitedRecord(";")]
    public class PlayersHeader
    {
        public String Priezvisko;
        public String Meno;
        public String TitulPred;
        public String RokNarodenia;
        public String Krp;
        public String MaterskyKlub;
        public String VekovaKategoria;
        public String Empty;

        //[FieldConverter(ConverterKind.Date, "ddMMyyyy")]
        //public DateTime OrderDate;

        //[FieldConverter(ConverterKind.Decimal, ".")] // The decimal separator is .
        //public decimal Freight;
    }
}
