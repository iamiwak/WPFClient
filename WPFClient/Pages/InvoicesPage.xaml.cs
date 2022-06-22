using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPFClient.Pages
{
    public partial class InvoicesPage : Page
    {
        private bool _isAddNewItem = false;

        public InvoicesPage()
        {
            InitializeComponent();
            DataContext = this;
            UpdateGrid();
            Boat.ItemsSource = SourceCore.DataBase.Invoice.ToList();

            //InvoicesGrid.ItemsSource = SourceCore.DataBase.Invoice.ToList();
        }

        private void ChangeDialogFrameState(bool show = true)
        {
            DialogFrame.Width = new GridLength(show ? 400 : 0);
            InvoicesGrid.IsHitTestVisible = !show;
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
            string content = "Добавить счет";
            ChangeDialogFrameState();
            _isAddNewItem = true;
            InvoicesGrid.SelectedItem = null;
            DialogTitle.Content = content;
            ApplyButton.Content = content;
            Boat.Text = "";
            Settled.Text = "";
            Sum.Text = "";
            SumInclVAT.Text = "";
            Date.Text = "";
        }

        private void CopyItem(object sender, RoutedEventArgs e)
        {
            if (!IsSelectedItem(InvoicesGrid)) return;

            string content = "Копировать яхту";
            ChangeDialogFrameState();
            _isAddNewItem = true;
            DialogTitle.Content = content;
            ApplyButton.Content = content;
            string boat = Boat.Text;
            string settled = Settled.Text;
            string sum = Sum.Text;
            string vat = SumInclVAT.Text;
            string date = Date.Text;

            InvoicesGrid.SelectedItem = null;
            Boat.Text = boat;
            Settled.Text = settled;
            Sum.Text = sum;
            SumInclVAT.Text = vat;
            Date.Text = date;
        }

        private void EditItem(object sender, RoutedEventArgs e)
        {
            if (!IsSelectedItem(InvoicesGrid)) return;

            string content = "Изменить счет";
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

            Base.Invoice deletingBoat = InvoicesGrid.SelectedItem as Base.Invoice;
            InvoicesGrid.SelectedIndex = InvoicesGrid.SelectedIndex > 0 ? InvoicesGrid.SelectedIndex-- : InvoicesGrid.SelectedIndex++;
            try
            {
                SourceCore.DataBase.Invoice.Remove(deletingBoat);
                SourceCore.DataBase.SaveChanges();
                UpdateGrid(InvoicesGrid.SelectedItem as Base.Invoice);
            }
            catch (Exception ex) { MessageBox.Show($"Невозможно удалить запись: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void ApplyChanges(object sender, RoutedEventArgs e)
        {
            StringBuilder warnings = new StringBuilder();
            if (string.IsNullOrEmpty(Settled.Text)) warnings.Append("Не указано settled;\n");
            if (string.IsNullOrEmpty(Sum.Text)) warnings.Append("Не указана сумма;\n");
            if (string.IsNullOrEmpty(SumInclVAT.Text)) warnings.Append("Не указан налог;\n");
            if (string.IsNullOrEmpty(Date.Text)) warnings.Append("Не указана дата;\n");
            if (Boat.SelectedItem == null) warnings.Append("Не указана яхта;\n");

            if (warnings.Length != 0)
            {
                MessageBox.Show($"Исправьте следующие недочеты:\n\n{warnings}", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            double.TryParse(SumInclVAT.Text, out double vat);
            DateTime.TryParse(Date.Text, out DateTime date);
            Base.Invoice invoice = new Base.Invoice
            {
                Settled = Settled.Text,
                Sum = Sum.Text,
                Sum_inclVAT = vat,
                Date = date
            };
            invoice.Contract.Order.Boat = Boat.SelectedItem as Base.Boat;

            if (_isAddNewItem) SourceCore.DataBase.Invoice.Add(invoice);
            try { SourceCore.DataBase.SaveChanges(); }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }

            UpdateGrid(invoice);
            ChangeDialogFrameState(false);
        }

        private void CancelChanges(object sender, RoutedEventArgs e) => ChangeDialogFrameState(false);

        private void UpdateGrid(Base.Invoice invoices = null)
        {
            if ((invoices == null) && (InvoicesGrid.ItemsSource != null)) invoices = (Base.Invoice)InvoicesGrid.SelectedItem;

            InvoicesGrid.ItemsSource = new ObservableCollection<Base.Invoice>(SourceCore.DataBase.Invoice.ToList());
            InvoicesGrid.SelectedItem = invoices;
        }
    }
}
