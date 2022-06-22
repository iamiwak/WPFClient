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

namespace WPFClient
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuButtonClick(object sender, RoutedEventArgs e)
        {
            string buttonName = ((Button)sender).Name;
            object page = null;

            switch (buttonName)
            {
                case "BoatsButton":
                    page = new Pages.BoatsPage();
                    break;
                case "AccessoriesButton":
                    page = new Pages.AccessoriesPage();
                    break;
                case "ConstractsButton":
                    page = new Pages.ContractsPage();
                    break;
                case "AccessoriesToBoatButton":
                    page = new Pages.AccessoriesToBoatsPage();
                    break;
                case "InvoicesButton":
                    page = new Pages.InvoicesPage();
                    break;
                case "OrdersButton":
                    page = new Pages.OrdersPage();
                    break;
                case "OrderDetailsButton":
                    page = new Pages.OrdersDetailsPage();
                    break;
                case "CustomersButton":
                    page = new Pages.CustomersPage();
                    break;
                case "SalePersonssButton":
                    page = new Pages.SalePeoplesPage();
                    break;
                case "PartnersButton":
                    page = new Pages.PartnersPage();
                    break;
                default:
                    MessageBox.Show("Была нажата неизвестная кнопка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }

            RootFrame.Navigate(page);
        }
    }
}
