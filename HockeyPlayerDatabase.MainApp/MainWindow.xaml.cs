using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xaml;
using System.Xml.Linq;
using HockeyPlayerDatabase.Interfaces;
using HockeyPlayerDatabase.Model;
using Microsoft.Win32;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

namespace HockeyPlayerDatabase.MainApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly HockeyContext _dbContext;
        private List<Player> ListPlayers { get; set; }
        private int _selectPlayerKrp;

        public MainWindow()
        {
            InitializeComponent();

            // Zalozim si db kontext
            _dbContext = new HockeyContext();

            // Ako prve si selektnem vsetkych hracov (nefiltrovanych)
            RefreshListPlayers();
        }

        // ************* METODY PRE OBSLUHU GUI ************************************

        private void SetFilteredItems(int count)
        {
            TextBlockFilteredItems.Text = count.ToString();
        }

        private void SetFilteredItemsAll()
        {
            TextBlockFilteredItemsAll.Text = _dbContext.GetPlayers().ToList().Count.ToString();
        }

        public void RefreshListPlayers()
        {
            SetListPlayers(_dbContext.GetPlayers().ToList());
        }

        public void SetListPlayers(List<Player> listPlayers)
        {
            ListPlayers = listPlayers;
            // ListViewPlayers je nazov ListView do ktoreho si nabindujem zdroj
            try { ListViewPlayers.ItemsSource = ListPlayers; } catch (Exception) { }
            // Aktualizujem pocty filtrovanych poloziek
            SetFilteredItems(ListPlayers.Count);
            SetFilteredItemsAll();
        }


        // **********************************************************************************

        private void ExitApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ListViewPlayers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Nacitam a ulozim si KRP hraca (v celom systeme by mal byt unikatny)
            Player selectedPlayer = (Player)ListViewPlayers.SelectedItem;
            _selectPlayerKrp = selectedPlayer.KrpId;
            // Zapnem tlacidla Remove a Edit
            ButtonRemovePlayer.IsEnabled = true;
            ButtonEditPlayer.IsEnabled = true;
            ButtonOpenClubsUrl.IsEnabled = true;
        }

        private void ButtonApply_Click(object sender, RoutedEventArgs e)
        {
            String firstName = (FilterFirstName.Text.Trim() != "") ? FilterFirstName.Text : null;
            String lastName = (FilterLastName.Text.Trim() != "") ? FilterLastName.Text : null;

            int? krpId = null;
            int? birthFrom = null;
            int? birthTo = null;

            try { krpId = (FilterKrp.Text.Trim() != "") ? Int32.Parse(FilterKrp.Text) : (int?)null; } catch (Exception ex) { }
            try { birthFrom = (FilterBirthFrom.Text.Trim() != "") ? Int32.Parse(FilterBirthFrom.Text) : (int?)null; } catch (Exception ex) { }
            try { birthTo = (FilterBirthTo.Text.Trim() != "") ? Int32.Parse(FilterBirthTo.Text) : (int?)null; } catch (Exception ex) { }

            // Do tohoto pola si dam ID kategori, ktore su zaskrtnute
            List<AgeCategory> ageCategories = new List<AgeCategory>();
            if ((bool)FilterCadet.IsChecked)
            {
                ageCategories.Add(AgeCategory.Cadet);
            }
            if ((bool)FilterJunior.IsChecked)
            {
                ageCategories.Add(AgeCategory.Junior);
            }
            if ((bool)FilterMidgest.IsChecked)
            {
                ageCategories.Add(AgeCategory.Midgest);
            }
            if ((bool)FilterSenior.IsChecked)
            {
                ageCategories.Add(AgeCategory.Senior);
            }

            // Zo zadania som pochopil ze sa jedna o string nazvu klubu
            String club = (FilterClub.Text.Trim() != "") ? FilterClub.Text : null;

            // Zavolanie filtrovania a aktualizovat zoznam hracov
            SetListPlayers(_dbContext.ApplyFilterPlayers(krpId, firstName, lastName, birthFrom, birthTo, ageCategories, club));
        }

        private void ButtonAddPlayer_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new PlayerWindow(_dbContext, this, null);
            dialog.ShowDialog();
        }

        private void ButtonRemovePlayer_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show($@"Are you sure to remove player with KRP ID {_selectPlayerKrp}?", "Remove player", MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel);
            if (result == MessageBoxResult.OK)
            {
                try
                {
                    Player player = _dbContext.GetPlayerByKrp(_selectPlayerKrp)[0];
                    _dbContext.RemovePlayer(player);
                    RefreshListPlayers();
                    // Po vymazani je potrebne znova vypnut tlacidla Remove a Edit
                    ButtonRemovePlayer.IsEnabled = false;
                    ButtonEditPlayer.IsEnabled = false;
                    ButtonOpenClubsUrl.IsEnabled = false;

                }
                catch (Exception exception)
                {
                    MessageBox.Show("Unexpected error, the player wasn't removed!");
                    Console.WriteLine(exception);
                }
            }
        }

        private void ButtonEditPlayer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Player player = _dbContext.GetPlayerByKrp(_selectPlayerKrp)[0];
                var dialog = new PlayerWindow(_dbContext, this, player);
                dialog.ShowDialog();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        private void ButtonOpenClubsUrl_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Player player = _dbContext.GetPlayerByKrp(_selectPlayerKrp)[0];
                if (player.Club.Url != "")
                {
                    System.Diagnostics.Process.Start(player.Club.Url);
                }
                else
                {
                    MessageBox.Show("The club doesn't have a website.");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }


        // zdroj: https://msdn.microsoft.com/en-us/library/system.windows.forms.openfiledialog(v=vs.110).aspx
        private void ExportDbtoXml(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Please, choose directory to save a XML file.";

            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string selectedPath = fbd.SelectedPath;
                _dbContext.SaveToXml($"{selectedPath}//Hockey-export.xml");
            }
        }
    }
}
