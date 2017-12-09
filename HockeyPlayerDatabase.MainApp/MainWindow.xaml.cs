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
using System.Windows.Navigation;
using System.Windows.Shapes;
using HockeyPlayerDatabase.Model;

namespace HockeyPlayerDatabase.MainApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public HockeyContext DbContext;
        public List<Player> ListPlayers { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            // Zalozim si db kontext
            DbContext = new HockeyContext();

            // Ako prve si selektnem vsetkych hracov (nefiltrovanych)
            ListPlayers = DbContext.GetPlayers().ToList();
            // ListViewPlayers je nazov ListView do ktoreho si nabindujem zdroj
            ListViewPlayers.ItemsSource = ListPlayers;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_2(object sender, TextChangedEventArgs e)
        {

        }


        private void MenuExport(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ExitApp(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
