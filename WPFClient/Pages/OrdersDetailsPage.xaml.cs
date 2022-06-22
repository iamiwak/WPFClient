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
    public partial class OrdersDetailsPage : Page
    {
        private bool _isAddNewItem = false;

        public OrdersDetailsPage()
        {
            InitializeComponent();
            DataContext = this;
            OrdersDetailsGrid.ItemsSource = SourceCore.DataBase.OrderDetails.ToList();
        }

        private void ChangeDialogFrameState(bool show = true)
        {
            DialogFrame.Width = new GridLength(show ? 400 : 0);
            OrdersDetailsGrid.IsHitTestVisible = !show;
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
            string content = "Добавить детали заказа";
            ChangeDialogFrameState();
            _isAddNewItem = true;
            OrdersDetailsGrid.SelectedItem = null;
            DialogTitle.Content = content;
            ApplyButton.Content = content;
        }

        private void CopyItem(object sender, RoutedEventArgs e)
        {
            if (!IsSelectedItem(OrdersDetailsGrid)) return;

            string content = "Копировать детали заказа";
            ChangeDialogFrameState();
            _isAddNewItem = true;
            DialogTitle.Content = content;
            ApplyButton.Content = content;

            OrdersDetailsGrid.SelectedItem = null;
        }

        private void EditItem(object sender, RoutedEventArgs e)
        {
            if (!IsSelectedItem(OrdersDetailsGrid)) return;

            string content = "Изменить детали заказа";
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

            SourceCore.DataBase.OrderDetails.Remove((Base.OrderDetails)OrdersDetailsGrid.SelectedItem);
            SourceCore.DataBase.SaveChanges();
        }

        private void ApplyChanges(object sender, RoutedEventArgs e)
        {
            var orderDetails = new Base.OrderDetails();

            if (_isAddNewItem) SourceCore.DataBase.OrderDetails.Add(orderDetails);
            SourceCore.DataBase.SaveChanges();
            ChangeDialogFrameState(false);
        }

        private void CancelChanges(object sender, RoutedEventArgs e) => ChangeDialogFrameState(false);
    }
}

