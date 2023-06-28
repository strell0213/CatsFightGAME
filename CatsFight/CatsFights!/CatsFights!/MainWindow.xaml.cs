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

namespace CatsFights_
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AppContext DB;
        public MainWindow()
        {
            InitializeComponent();
            DB = new AppContext();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DB.Users.Where(c => c.Nickname.Contains(NickText.Text)).FirstOrDefault() != null)
                {
                    NowClass.Now = NickText.Text;
                    MainCat mainCat1 = new MainCat();
                    mainCat1.Show();
                    this.Close();
                }
                else
                {

                    User user = new User()
                    {
                        Nickname = NickText.Text,
                        level = 0,
                        coin = 0,
                        hp = 100,
                        slaughter = 20,
                    };
                    DB.Users.Add(user);
                    DB.SaveChanges();
                    NowClass.Now = NickText.Text;
                    MainCat mainCat = new MainCat();
                    mainCat.Show();
                    this.Close();
                }
            }
            catch (Exception ex) { 
                MessageBox.Show("Ошибка: "+ex.Message);
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
