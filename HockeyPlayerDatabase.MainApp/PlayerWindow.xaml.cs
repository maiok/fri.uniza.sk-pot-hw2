using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using HockeyPlayerDatabase.Interfaces;
using HockeyPlayerDatabase.Model;

namespace HockeyPlayerDatabase.MainApp
{
    /// <summary>
    /// Interaction logic for PlayerWindow.xaml
    /// </summary>
    public partial class PlayerWindow : Window
    {
        private HockeyContext _dbContext;
        private MainWindow _mainWindow;
        public Array DataAgeCategories { get; set; }
        public List<String> DataClubs { get; set; }

        public PlayerWindow(HockeyContext dbContext, MainWindow mainWindow, Player player)
        {
            // Predana instancia db kontextu
            _dbContext = dbContext;
            _mainWindow = mainWindow;
            InitializeComponent();

            // Naplnim si hodnoty vekovej kategorie do combobox
            DataAgeCategories = Enum.GetValues(typeof(AgeCategory));
            ComboBoxAgeCategories.ItemsSource = DataAgeCategories;

            // Naplnim si hodnoty nazvov klubov do combobox
            DataClubs = _dbContext.GetClubsName().ToList();
            ComboBoxClubs.ItemsSource = DataClubs;

            // Ak je nastaveny player, je to editaca
            if (player != null)
            {
                TextBoxKrp.Text = player.KrpId.ToString();
                TextBoxTitleBefore.Text = player.TitleBefore;
                TextBoxFirstName.Text = player.FirstName;
                TextBoxLastName.Text = player.LastName;
                TextBoxYearOfBirth.Text = player.YearOfBirth.ToString();
                ComboBoxAgeCategories.SelectedItem = player.AgeCategory;
                ComboBoxClubs.SelectedItem = player.Club.Name;
            }
        }

        private void ButtonCancelPlayer_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonOkPlayer_Click(object sender, RoutedEventArgs e)
        {
            String titleBefore = TextBoxTitleBefore.Text.Trim();
            String firstName = TextBoxFirstName.Text.Trim();
            String lastName = TextBoxLastName.Text.Trim();

            int krpId = 0;
            int yearOfBirth = 0;

            bool krpIdEx = false;
            bool yearOfBirthEx = false;

            try { krpId = (TextBoxKrp.Text.Trim() != "") ? Int32.Parse(TextBoxKrp.Text) : 0; } catch (Exception ex) { krpIdEx = true; }
            try { yearOfBirth = (TextBoxYearOfBirth.Text.Trim() != "") ? Int32.Parse(TextBoxYearOfBirth.Text) : 0; } catch (Exception ex) { yearOfBirthEx = true; }

            var ageCategory = ComboBoxAgeCategories.SelectedItem;
            var club = ComboBoxClubs.SelectedItem;

            int? clubId = _dbContext.GetClubIdByName((string)club);

            // V pripade ze bol zadany nespravny tvar pre ciselne hodnoty vyskoci warning
            if (krpIdEx || yearOfBirthEx)
            {
                MessageBox.Show("KRP or YearOfBirth has incorrect value!");
            }
            // Ked je vsetko OK, idem pridat hraca do DB
            else
            {
                Player player = new Player();
                player.KrpId = krpId;
                player.TitleBefore = titleBefore;
                player.FirstName = firstName;
                player.LastName = lastName;
                player.YearOfBirth = yearOfBirth;
                player.AgeCategory = (AgeCategory?)ageCategory;
                player.ClubId = clubId;

                // Overenie ci sa KRP v systeme nenachadza
                if (_dbContext.GetPlayerByKrp(krpId).Count > 0)
                {
                    MessageBoxResult result = MessageBox.Show($@"The player is already exists, do you wish continue?",
                        "Edit player", MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel);
                    if (result == MessageBoxResult.OK)
                    {
                        // Aktualizovanie existujuceho hraca
                        _dbContext.UpdatePlayer(player);
                    }
                }
                else
                {
                    // Vlozenie noveho hraca
                    _dbContext.InsertPlayer(player);
                }
                _mainWindow.RefreshListPlayers();
                Close();
            }
        }
    }
}
