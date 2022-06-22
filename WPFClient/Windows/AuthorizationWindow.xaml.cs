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
using WPFClient.Base;

namespace WPFClient.Windows
{
    public partial class AuthorizationWindow : Window
    {
        private YachtEntities _db;

        public AuthorizationWindow()
        {
            InitializeComponent();
            try
            {
                _db = new YachtEntities();
            }
            catch (Exception e)
            {
                MessageBox.Show($"Не удалось подключиться к базе данных из-за непредвиденной ошибки.\n\nОшибка: {e.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }

        private void SignInAccount(string login, string password)
        {
            if (_db.Users.SingleOrDefault(user => user.login == login && user.password == password) != null)
            {
                new MainWindow().Show();
                Close();
            }
            else MessageBox.Show("Логин или пароль введён неверно.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void LoginClick(object sender, RoutedEventArgs e) => SignInAccount(UserLogin.Text, UserPassword.Password);

        private void ExitFromProgramm(object sender, RoutedEventArgs e) => Close();

        private void CreateAccount(object sender, RoutedEventArgs e)
        {
            RegistrationWndow window = new RegistrationWndow(_db);
            window.Show();
        }

        private void ChangePasswordState(object sender, MouseEventArgs e)
        {
            string password = UserPassword.Password;
            Visibility visibility = UserPassword.Visibility;
            double width = UserPassword.ActualWidth;

            UserPassword.Password = UserPasswordText.Text;
            UserPassword.Visibility = UserPasswordText.Visibility;
            UserPassword.Width = UserPasswordText.Width;

            UserPasswordText.Text = password;
            UserPasswordText.Visibility = visibility;
            UserPasswordText.Width = width;
        }
    }
}
