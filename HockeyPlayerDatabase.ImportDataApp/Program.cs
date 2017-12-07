using System;
using System.Globalization;
using System.Linq;
using FileHelpers;
using HockeyPlayerDatabase.Interfaces;
using HockeyPlayerDatabase.Model;

namespace HockeyPlayerDatabase.ImportDataApp
{

    /// <summary>
    /// 
    /// ZDROJE
    /// 
    /// Prve pismeno velke - https://msdn.microsoft.com/en-us/library/system.globalization.textinfo.totitlecase.aspx
    /// CSVParser - http://www.filehelpers.net/example/QuickStart/ReadFileDelimited/
    /// </summary>

    class Program
    {
        private static HockeyContext dbContext;
        private static string clubsCsvPath;
        private static string playersCsvPath;

        static void Main(string[] args)
        {
            Console.WriteLine("HockeyPlayerDatabase - import dat");
            dbContext = new HockeyContext();

            try
            {
                // nacitanie argumentov, na poradi nezalezi
                var order = 0;
                foreach (var arg in args)
                {
                    if (arg == "-clubs")
                    {
                        clubsCsvPath = args[order + 1];
                    }
                    else if (arg == "-players")
                    {
                        playersCsvPath = args[order + 1];
                    }
                    order++;
                }

                // zmazat data ak je nastaveny argument
                if (args.Contains("-clearDatabase"))
                {
                    dbContext.ClearDatabase();
                }

                FileHelperEngine<ClubsHeader> engineClubs = new FileHelperEngine<ClubsHeader>();
                ImportClubs(engineClubs, clubsCsvPath);
                FileHelperEngine<PlayersHeader> enginePlayers = new FileHelperEngine<PlayersHeader>();
                ImportPlayers(enginePlayers, playersCsvPath);

            }
            catch (Exception e)
            {
                Console.WriteLine($"Neocakavana chyba pri spracovani importu: {e}");
            }

            Console.WriteLine("Import bol uspesne dokonceny.");
            Console.ReadLine();

        }

        private static void ImportClubs(FileHelperEngine<ClubsHeader> engine, String csvPath)
        {
            var records = engine.ReadFile(csvPath);

            foreach (var record in records.Skip(1))
            {
                Club club = new Club();
                club.Name = record.Nazov;
                club.Address = record.Adresa;
                club.Url = record.Url;

                dbContext.InsertClub(club);
            }
            
        }

        private static void ImportPlayers(FileHelperEngine<PlayersHeader> engine, String csvPath)
        {
            var records = engine.ReadFile(csvPath);

            // pouzijem pre pracu so stylom textu
            TextInfo textInfo = new CultureInfo("sk-SK", false).TextInfo;

            // rozparsujem data okrem 1. riadku (hlavicka)
            foreach (var record in records.Skip(1))
            {
                string priezvisko = record.Priezvisko.ToLower();
                priezvisko = textInfo.ToTitleCase(priezvisko); // potom capitalize

                string meno = record.Meno.ToLower();
                meno = textInfo.ToTitleCase(meno);

                int? materskyKlub = dbContext.GetClubIdByName(record.MaterskyKlub);

                AgeCategory vekovaKategoria;
                if (record.VekovaKategoria.ToLower().Contains("kadet"))
                {
                    vekovaKategoria = AgeCategory.Cadet;
                }
                else if (record.VekovaKategoria.ToLower().Contains("dorast"))
                {
                    vekovaKategoria = AgeCategory.Midgest;
                }
                else if (record.VekovaKategoria.ToLower().Contains("junior"))
                {
                    vekovaKategoria = AgeCategory.Junior;
                }
                else
                {
                    vekovaKategoria = AgeCategory.Senior;
                }

                Player player = new Player();
                player.LastName = priezvisko;
                player.FirstName = meno;
                player.TitleBefore = record.TitulPred;
                player.YearOfBirth = Int32.Parse(record.RokNarodenia);
                player.KrpId = Int32.Parse(record.Krp);
                player.ClubId = materskyKlub;
                player.AgeCategory = vekovaKategoria;

                dbContext.InsertPlayer(player);

            }
        }
    }

    [DelimitedRecord(";")]
    public class ClubsHeader
    {
        public string Nazov;
        public string Adresa;
        public string Url;
    }

    [DelimitedRecord(";")]
    public class PlayersHeader
    {
        public string Priezvisko;
        public string Meno;
        public string TitulPred;
        public string RokNarodenia;
        public string Krp;
        public string MaterskyKlub;
        public string VekovaKategoria;
        public string Empty;

        //[FieldConverter(ConverterKind.Date, "ddMMyyyy")]
        //public DateTime OrderDate;

        //[FieldConverter(ConverterKind.Decimal, ".")] // The decimal separator is .
        //public decimal Freight;
    }
}
