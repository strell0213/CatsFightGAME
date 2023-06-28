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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CatsFights_
{
    /// <summary>
    /// Логика взаимодействия для FightWindow.xaml
    /// </summary>
    public partial class FightWindow : Window
    {
        ThicknessAnimation teAnim;
        ThicknessAnimation beAnim;
        AppContext DB;
        MediaPlayer playersound;
        System.Windows.Threading.DispatcherTimer timer;
        string mainsound = @"D:\Колледж\CatsFight\CatsFights!\CatsFights!\FightSound.mp3";
        string winsound = @"D:\Колледж\CatsFight\CatsFights!\CatsFights!\WinSound.mp3";
        int ti = 0;
        int win = 0;
        int Count = 0;
        public FightWindow()
        {
            InitializeComponent();
            //таймер
            timer = new System.Windows.Threading.DispatcherTimer();
            //музыка
            playersound = new MediaPlayer();
            playersound.Open(new Uri(mainsound));
            playersound.Volume = 0.2;
            playersound.Play();
            //анимации
            teAnim = new ThicknessAnimation();
            beAnim = new ThicknessAnimation();
            //база данных
            DB = new AppContext();
            //взятие данных из базы данных
            int en;
            var r = DB.Users.Where(c => c.Nickname == NowClass.Now).FirstOrDefault();
            NowClass.washpuser = r.hp.ToString();
            HPCountUser.Content = r.hp;
            SLCountUser.Content = r.slaughter;
            en = Convert.ToInt32(NowClass.nowenemy);
            var w = DB.Enemies.Where(c => c.id == en).FirstOrDefault();
            NowClass.washpenemy = w.hpE.ToString();
            HPCountEnemy.Content = w.hpE;
            SLCountEnemy.Content = w.slaughterE;
        }
        public void catfightenemyTO() {
            teAnim = new ThicknessAnimation();
            //воспроизведение элементов
            ClickButton.Visibility = Visibility.Hidden;
            CatJump.Visibility = Visibility.Visible;
            CatSit.Visibility = Visibility.Hidden;
            //анимация 1
            int l = 50;
            teAnim.To = new Thickness(l + 400, 0, 0, 67);
            teAnim.Duration = TimeSpan.FromSeconds(1);
            teAnim.AutoReverse = true;
            teAnim.Completed += new EventHandler(teAnim_complited); 
            CatJump.BeginAnimation(Image.MarginProperty, teAnim);
        }

        private void teAnim_complited(object sender, EventArgs e)
        {
            //воспроизведение элементов
            ClickButton.Visibility = Visibility.Visible;
            CatJump.Visibility = Visibility.Hidden;
            CatSit.Visibility = Visibility.Visible;
            //взятие элементов с бд
            int en = Convert.ToInt32(NowClass.nowenemy);
            var w = DB.Enemies.Where(c => c.id == en).FirstOrDefault();
            var r = DB.Users.Where(c => c.Nickname == NowClass.Now).FirstOrDefault();
            //изменение значений бд
            w.hpE -= r.slaughter;
            HPCountEnemy.Content = w.hpE;
            //проверка
            if (w.hpE <= 0)
            {
                //музыка
                playersound.Stop();
                playersound.Open(new Uri(winsound));
                playersound.Play();
                //изменение элементов
                r.level = r.level + 1;
                r.coin = r.coin + 200;
                //вывод победы
                NewLevelLabel.Content = "Ваш уровень: " + r.level;
                WINGRID.Visibility = Visibility.Visible;
                ClickButton.Visibility = Visibility.Hidden;
                win++;
                //восстановление данных
                r.hp = Convert.ToInt32(NowClass.washpuser);
                w.hpE = Convert.ToInt32(NowClass.washpenemy);
                DB.SaveChanges();
            }
            else
            {
                DB.SaveChanges();
            }
        }

        public void enemyfightcat()
        {
            beAnim = new ThicknessAnimation();
            //воспроизведение элементов
            ClickButton.Visibility = Visibility.Hidden;
            //анимация
            int l = 62;
            beAnim.To = new Thickness(0, 0, l + 400, 67);
            beAnim.Duration = TimeSpan.FromSeconds(1);
            beAnim.AutoReverse = true;
            beAnim.Completed += delegate
            {
                //воспроизведение элементов
                ClickButton.Visibility = Visibility.Visible;
                //взятие значений из бд
                int en = Convert.ToInt32(NowClass.nowenemy);
                var w = DB.Enemies.Where(c => c.id == en).FirstOrDefault();
                var r = DB.Users.Where(c => c.Nickname == NowClass.Now).FirstOrDefault();
                //изменение значений бд
                r.hp -= w.slaughterE;
                HPCountUser.Content = r.hp;
                if (r.hp <= 0)
                {
                    //музыка
                    playersound.Stop();
                    //изменение значений бд
                    if (r.level <= 1)
                    {
                        //воспроизведение элементов
                        ClickButton.Visibility = Visibility.Hidden;
                        LoseGrid.Visibility = Visibility.Visible;
                        //изменение значений бд
                        r.level = r.level - 1;
                        r.hp = Convert.ToInt32(NowClass.washpuser);
                        w.hpE = Convert.ToInt32(NowClass.washpenemy);
                        DB.SaveChanges();
                        LoseLevelLabel.Content = "Ваш уровень: "+r.level;
                    }
                    else
                    {
                        ClickButton.Visibility = Visibility.Hidden;
                        LoseGrid.Visibility = Visibility.Visible;
                        r.hp = Convert.ToInt32(NowClass.washpuser);
                        w.hpE = Convert.ToInt32(NowClass.washpenemy);
                        DB.SaveChanges();
                        LoseLevelLabel.Content = "Ваш уровень: " + r.level;
                    }

                }
                else {
                    DB.SaveChanges();
                }
            };
            EnemySit.BeginAnimation(Image.MarginProperty, beAnim);
        }
        private void ClickButton_Click(object sender, RoutedEventArgs e)
        {
            catfightenemyTO();
            if (win >= 1)
            {

            }
            else
            {
                timer.Tick += new EventHandler(timertick);
                timer.Interval = new TimeSpan(0, 0, 1);
                timer.Start();
            }
        }

        private void timertick(object sender, EventArgs e)
        {
            int en = Convert.ToInt32(NowClass.nowenemy);
            var w = DB.Enemies.Where(c => c.id == en).FirstOrDefault();
            try
            {
                if (w.levelE == 1)
                {
                    if (ti == 5)
                    {
                        timer.Stop();
                        ti = 0;
                        enemyfightcat(); 
                    }
                    else
                    {
                        ti++;
                    }
                }
                else if(w.levelE > 2 && w.levelE < 5){

                    if (ti == 3)
                    {
                        timer.Stop();
                        ti = 0;
                        enemyfightcat();
                    }
                    else
                    {
                        ti++;
                    }
                }
                else
                {

                }
            }
            catch { }
        }

        private void ShopButton_Click(object sender, RoutedEventArgs e)
        {
            playersound.Stop();
            MainCat mainCat = new MainCat();
            mainCat.Show();
            this.Close();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            playersound.Stop();
            int en = Convert.ToInt32(NowClass.nowenemy);
            var w = DB.Enemies.Where(c => c.id == en).FirstOrDefault();
            var r = DB.Users.Where(c => c.Nickname == NowClass.Now).FirstOrDefault();
            r.hp = Convert.ToInt32(NowClass.washpuser);
            w.hpE = Convert.ToInt32(NowClass.washpenemy);
            DB.SaveChanges();
            MainCat mainCat = new MainCat();
            mainCat.Show();
            this.Close();
        }
    }
}
