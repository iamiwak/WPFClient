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

namespace WPFClient.Pages
{
    public partial class OrdersPage : Page
    {
        private bool _isAddNewItem = false;

        public OrdersPage()
        {
            InitializeComponent();
            DataContext = this;
            OrdersGrid.ItemsSource = SourceCore.DataBase.Order.ToList();
            Boat.ItemsSource = SourceCore.DataBase.Boat.ToList();
            SalePerson.ItemsSource = SourceCore.DataBase.Sales_Person.ToList();
            Customer.ItemsSource = SourceCore.DataBase.Customers.ToList();
        }

        private void ChangeDialogFrameState(bool show = true)
        {
            DialogFrame.Width = new GridLength(show ? 400 : 0);
            OrdersGrid.IsHitTestVisible = !show;
            _isAddNewItem = false;
        }

        private bool IsSelectedItem(DataGrid item)
        {
            if (item.SelectedItem == null)
            {
                MessageBox.Show("Вы не выбрали ни одну запись.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private void AddItem(object sender, RoutedEventArgs e)
        {
            string content = "Добавить заказ";
            ChangeDialogFrameState();
            _isAddNewItem = true;
            OrdersGrid.SelectedItem = null;
            DialogTitle.Content = content;
            ApplyButton.Content = content;
            Date.Text = "";
            SalePerson.Text = "";
            Customer.Text = "";
            Boat.Text = "";
            DeliveryAddress.Text = "";
            City.Text = "";
        }

        private void CopyItem(object sender, RoutedEventArgs e)
        {
            if (!IsSelectedItem(OrdersGrid)) return;

            string content = "Копировать заказ";
            ChangeDialogFrameState();
            _isAddNewItem = true;
            DialogTitle.Content = content;
            ApplyButton.Content = content;
            string date = Date.Text;
            string salePerson = SalePerson.Text;
            string customer = Customer.Text;
            string boat = Boat.Text;
            string address = DeliveryAddress.Text;
            string city = City.Text;

            OrdersGrid.SelectedItem = null;
            Date.Text = date;
            SalePerson.Text = salePerson;
            Customer.Text = customer;
            Boat.Text = boat;
            DeliveryAddress.Text = address;
            City.Text = city;
        }

        private void EditItem(object sender, RoutedEventArgs e)
        {
            if (!IsSelectedItem(OrdersGrid)) return;

            string content = "Изменить заказ";
            ChangeDialogFrameState();
            DialogTitle.Content = content;
            ApplyButton.Content = content;
        }

        private void DeleteItem(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("При подтверждении удаления данной записи, Вы не сможете её восстановить.\n\nПродолжить?",
                "Внимание",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning) != MessageBoxResult.Yes) return;

            SourceCore.DataBase.Order.Remove((Base.Order)OrdersGrid.SelectedItem);
            SourceCore.DataBase.SaveChanges();
        }

        private void ApplyChanges(object sender, RoutedEventArgs e)
        {
            var order = new Base.Order();

            DateTime.TryParse(Date.Text, out DateTime date);

            order.Date = date;
            order.Sales_Person = (Base.Sales_Person)SalePerson.SelectedItem;
            order.Customers = (Base.Customers)Customer.SelectedItem;
            order.Boat = (Base.Boat)Boat.SelectedItem;
            order.DeliveryAddress = DeliveryAddress.Text;
            order.City = City.Text;
            if (_isAddNewItem) SourceCore.DataBase.Order.Add(order);
            SourceCore.DataBase.SaveChanges();
            ChangeDialogFrameState(false);
        }

        private void CancelChanges(object sender, RoutedEventArgs e) => ChangeDialogFrameState(false);
    }
}
