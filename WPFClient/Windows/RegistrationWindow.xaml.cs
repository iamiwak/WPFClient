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
using System.Text.RegularExpressions;

namespace WPFClient.Windows
{
    public partial class RegistrationWndow : Window
    {
        private Base.YachtEntities _db;

        public RegistrationWndow(Base.YachtEntities database)
        {
            InitializeComponent();
            _db = database;
        }

        private void ExitWindow(object sender, RoutedEventArgs e) => Close();

        private void SignUpCLick(object sender, RoutedEventArgs e)
        {
            string login = UserLogin.Text;
            string password = UserPassword.Text;
            string checkPassword = UserPasswordRepeat.Text;

            string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$#~!%*?&])[A-Za-z\d@$!%#~*?&]{8,}$";

            if (_db.Users.SingleOrDefault(u => u.login == login) != null)
            {
                MessageBox.Show("Пользователь с таким логином уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!Regex.IsMatch(password, passwordPattern))
            {
                MessageBox.Show(
                    "Пароль должен состоять минимум из 8 символов, а также содержать заглавные и строчные буквы латинского алфавита, цифры и специальные знаки.",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!password.Equals(checkPassword))
            {
                MessageBox.Show("Пароли не совпадают.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Base.Users user = new Base.Users();
            user.login = login;
            user.password = password;

            _db.Users.Add(user);
            try
            {
                _db.SaveChanges();
            }
            catch (Exception err)
            {
                MessageBox.Show($"Произошла ошибка!\n\n{err.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Close();
        }
    }
}
