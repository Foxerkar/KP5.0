using KP.Pages;
using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace KP
{
    /// <summary>
    /// Логика взаимодействия для MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        private bool isPanelVisible = true;
        public MainMenu(bool isGuest)
        {
            InitializeComponent();

            MainContent.Content = new LoadMenu();

            if (isGuest)
            {
                //Скрываем кнопки редактирования  расписания для гостей
                //RaspicsanieManagment_Button.IsEnabled = false
            }

        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            if (isPanelVisible) //Анимация боковой панельки
            {
                AnimatePanel(0);
            }
            else
            {
                AnimatePanel(150);
            }
            isPanelVisible = !isPanelVisible;

        }

        private void AnimatePanel(double targetWidth)
        {
            DoubleAnimation animation = new DoubleAnimation()
            {
                To = targetWidth,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new QuarticEase { EasingMode = EasingMode.EaseInOut }
            };

            SidePanel.BeginAnimation(WidthProperty, animation);
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void RaspisanieManagment(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new RaspisanieControl();
            TextMenu.Text = "Расписание";
        }

        private void AnalizManagment(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new AnalizControl(false);
            TextMenu.Text = "Анализ";
        }

        private void GruppaManagment(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new GruppaControl(false);
            TextMenu.Text = "Группы";
        }

        private void EditManagment(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new EditControl();
            TextMenu.Text = "Редактор";
        }

        private void LoadMenu(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new LoadMenu();
            TextMenu.Text = "Главное меню";
        }

        private void EDIT_Prepodavateli(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new EDIT_Prepodavateli(false);
            TextMenu.Text = "Преподаватели";
        }

        private void EDIT_Auditorii(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new EDIT_Auditorii(false);
            TextMenu.Text = "Аудитории";
        }

        private void EDIT_Disciplina(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new EDIT_Disciplina(false);
            TextMenu.Text = "Дисциплины";
        }
    }
}
