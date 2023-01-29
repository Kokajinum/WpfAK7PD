using MongoDB.Bson;
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
using WpfAK7PD.Managers;
using WpfAK7PD.Models;

namespace WpfAK7PD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MongoDBManager Manager = new MongoDBManager("mongodb+srv://utb:barbari@knihovna.cmf7vc6.mongodb.net/?retryWrites=true&w=majority");
        public User LoggedUser { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            User newUser = new User()
            {
                Login = "test1",
                Password = BCrypt.Net.BCrypt.HashPassword("test1"),
                FirstName = "Petr",
                LastName = "Kozak",
                Address = new Address()
                {
                     City = "Kroměříž",
                     Street = "Zborovská 4180",
                     ZipCode = "76701"
                }
            };
            bool res = await Manager.CreateOrdinaryUserAsync(newUser);
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            LoggedUser = await Manager.LoginUserAsync("test1", "test1");
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var userBooks = Manager.GetUserBooks(LoggedUser);
            var books = Manager.GetBooks();
        }
    }
}
