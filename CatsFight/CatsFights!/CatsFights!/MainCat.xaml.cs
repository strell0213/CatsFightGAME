using System;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace CatsFights_
{
    /// <summary>
    /// Логика взаимодействия для MainCat.xaml
    /// </summary>
    public partial class MainCat : Window
    {
        AppContext DB;
        DoubleAnimation teAnim;
        System.Windows.Threading.DispatcherTimer timer;
        MediaPlayer player;
        string mainsound = @"D:\Колледж\CatsFight\CatsFights!\CatsFights!\MainSoundTrack.mp3";
        public MainCat()
        {
            InitializeComponent();
            DB = new AppContext();
            timer = new System.Windows.Threading.DispatcherTimer();
            updateperson();
            teAnim = new DoubleAnimation();
            player = new MediaPlayer();
            player.Open(new Uri(mainsound));
            player.Play();
            VolumeCount.Content = player.Volume;
        }
        public void updateperson() {
            var r = DB.Users.Where(c => c.Nickname == NowClass.Now).FirstOrDefault();
            MainNickNLevel.Content = r.Nickname + "\nУровень: " + r.level;
            CoinCount.Content = r.coin.ToString();
            HPCount.Content = r.hp.ToString();
            SlaughterCount.Content = r.slaughter.ToString();

        }
        public void showplusone() {
            teAnim.From = 0;
            teAnim.To = 26;
            teAnim.Duration = TimeSpan.FromMilliseconds(800);
            plusone.BeginAnimation(Label.HeightProperty, teAnim);
            teAnim.From = 26;
            teAnim.To = 0;
            teAnim.Duration = TimeSpan.FromMilliseconds(800);
            plusone.BeginAnimation(Label.HeightProperty, teAnim);
        }

        public void ShowplusoneHP() {
            teAnim.From = 0;
            teAnim.To = 26;
            teAnim.Duration = TimeSpan.FromMilliseconds(800);
            plusoneHP.BeginAnimation(Label.HeightProperty, teAnim);
            teAnim.From = 26;
            teAnim.To = 0;
            teAnim.Duration = TimeSpan.FromMilliseconds(800);
            plusoneHP.BeginAnimation(Label.HeightProperty, teAnim);
        }

        public void ShowplusoneSL() {
            teAnim.From = 0;
            teAnim.To = 26;
            teAnim.Duration = TimeSpan.FromMilliseconds(800);
            plusoneSl.BeginAnimation(Label.HeightProperty, teAnim);
            teAnim.From = 26;
            teAnim.To = 0;
            teAnim.Duration = TimeSpan.FromMilliseconds(800);
            plusoneSl.BeginAnimation(Label.HeightProperty, teAnim);
        }
        public void catsitadnjump() {
            if (CatJump.Visibility == Visibility.Hidden)
            {
                CatJump.Visibility = Visibility.Visible;
                CatSit.Visibility = Visibility.Hidden;
            }
            else {
                CatJump.Visibility = Visibility.Hidden;
                CatSit.Visibility = Visibility.Visible;
            }
        }

        private void ClickButton_Click(object sender, RoutedEventArgs e)
        {
            plusone.Content = "+1";
            showplusone();
            CoinCount.Content = Convert.ToInt32(CoinCount.Content) + 1;
            var r = DB.Users.Where(c => c.Nickname == NowClass.Now).FirstOrDefault();
            r.coin = Convert.ToInt32(CoinCount.Content);
            DB.SaveChanges();
            catsitadnjump();
            updateperson();
        }
        

        private void ShopButton_Click(object sender, RoutedEventArgs e)
        {
            if (ShopGrid.Visibility == Visibility.Hidden)
            {
                ShopGrid.Visibility = Visibility.Visible;
                FightGrid.Visibility = Visibility.Hidden;
                OptionGrid.Visibility = Visibility.Hidden;
            }
            else { 
                ShopGrid.Visibility = Visibility.Hidden;
                FightGrid.Visibility = Visibility.Hidden;
                OptionGrid.Visibility = Visibility.Hidden;
            }
        }

        private void HPbuy_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(CoinCount.Content) >= 150)
            {
                plusone.Content = "-150";
                showplusone();
                CoinCount.Content = Convert.ToInt32(CoinCount.Content) - 150;    
                var r = DB.Users.Where(c => c.Nickname == NowClass.Now).FirstOrDefault();
                r.coin = Convert.ToInt32(CoinCount.Content);
                plusoneHP.Content = "+30";
                ShowplusoneHP();
                HPCount.Content = Convert.ToInt32(HPCount.Content) + 30;
                r.hp = Convert.ToInt32(HPCount.Content);
                DB.SaveChanges();
            }
            else { 
                MessageBox.Show("У вас недостаточно монет!", "CatFight!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SlaughterBuy_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(CoinCount.Content) >= 200)
            {
                plusone.Content = "-200";
                showplusone();
                CoinCount.Content = Convert.ToInt32(CoinCount.Content) - 200;
                var r = DB.Users.Where(c => c.Nickname == NowClass.Now).FirstOrDefault();
                r.coin = Convert.ToInt32(CoinCount.Content);
                plusoneSl.Content = "+10";
                ShowplusoneSL();
                SlaughterCount.Content = Convert.ToInt32(SlaughterCount.Content) + 10;
                r.slaughter = Convert.ToInt32(SlaughterCount.Content);
                DB.SaveChanges();

            }
            else {
                MessageBox.Show("У вас недостаточно монет!", "CatFight!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FightButton_Click(object sender, RoutedEventArgs e)
        {
            var r = DB.Enemies.Select(c => c.id + ". "+c.Name+" Уровень - "+c.levelE+"\nХарактеристики: Здоровье - "+c.hpE+"\nСила - "+c.slaughterE).ToList();
            EnemyList.ItemsSource = r;
            if (FightGrid.Visibility == Visibility.Hidden)
            {
                FightGrid.Visibility = Visibility.Visible;
                ShopGrid.Visibility = Visibility.Hidden;
                OptionGrid.Visibility = Visibility.Hidden;
            }
            else
            {
                ShopGrid.Visibility = Visibility.Hidden;
                FightGrid.Visibility = Visibility.Hidden;
                OptionGrid.Visibility = Visibility.Hidden;
            }
        }

        private void FightWithEnemyButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int d1;
                if (EnemyList.SelectedItem.ToString()[1].ToString() == " " || EnemyList.SelectedItem.ToString()[1].ToString() == ".")
                {
                    d1 = Convert.ToInt32(EnemyList.SelectedItem.ToString()[0].ToString());
                }
                else
                {
                    d1 = Convert.ToInt32(EnemyList.SelectedItem.ToString()[0].ToString() + EnemyList.SelectedItem.ToString()[1].ToString());
                }
                NowClass.nowenemy = d1.ToString();
                player.Stop();
                FightWindow fightWindow = new FightWindow();
                fightWindow.Show();
                this.Close();
            }
            catch {
                MessageBox.Show("Вы не выбрали соперника!", "CatFight!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void minusVolumeButton_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToDouble(VolumeCount.Content.ToString()) == 0)
            {
                VolumeCount.Content = "1";
                player.Volume = Convert.ToDouble(VolumeCount.Content);
            }
            else {
                if (Convert.ToDouble(VolumeCount.Content.ToString()) == 0.1)
                {
                    VolumeCount.Content = "0";
                    player.Volume = Convert.ToDouble(VolumeCount.Content);
                }
                else
                {
                    VolumeCount.Content = Convert.ToDouble(VolumeCount.Content) - 0.1;
                    player.Volume = Convert.ToDouble(VolumeCount.Content);
                }
            }   
        }

        private void plusVolumeButton_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToDouble(VolumeCount.Content.ToString()) == 1)
            {
                VolumeCount.Content = "0";
                player.Volume = Convert.ToDouble(VolumeCount.Content);
            }
            else
            {
                if (Convert.ToDouble(VolumeCount.Content.ToString()) == 0.9)
                {
                    VolumeCount.Content = "1";
                    player.Volume = Convert.ToDouble(VolumeCount.Content);
                }
                else
                {
                    VolumeCount.Content = Convert.ToDouble(VolumeCount.Content) + 0.1;
                    player.Volume = Convert.ToDouble(VolumeCount.Content);
                }
            }
        }

        private void OptionsButton_Click(object sender, RoutedEventArgs e)
        {
            if (OptionGrid.Visibility == Visibility.Hidden)
            {
                OptionGrid.Visibility = Visibility.Visible;
                FightGrid.Visibility = Visibility.Hidden;
                ShopGrid.Visibility = Visibility.Hidden;
            }
            else {
                OptionGrid.Visibility = Visibility.Hidden;
                FightGrid.Visibility = Visibility.Hidden;
                ShopGrid.Visibility = Visibility.Hidden;
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
