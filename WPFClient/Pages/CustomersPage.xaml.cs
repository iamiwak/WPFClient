using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WPFClient.Pages
{
    public partial class CustomersPage : Page
    {
        private ValueConverter converter = new ValueConverter();
        private bool _isAddNewItem = false;
        private Base.Customers _selectedCustomer;

        public CustomersPage()
        {
            InitializeComponent();
            DataContext = this;
            UpdateGrid();
        }

        private void ChangeDialogFrameState(bool show = true)
        {
            DialogFrame.Width = new GridLength(show ? 400 : 0);
            CustomersGrid.IsHitTestVisible = !show;
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
            string content = "Добавить покупателя";
            ChangeDialogFrameState();
            _isAddNewItem = true;
            CustomersGrid.SelectedItem = null;
            DialogTitle.Content = content;
            ApplyButton.Content = content;
            FistName.Text = "";
            FamilyName.Text = "";
            DateOfBirth.Text = "";
            OrganisationName.Text = "";
            Address.Text = "";
            City.Text = "";
            email.Text = "";
            Phone.Text = "";
            DocumentNumber.Text = "";
            DocumentName.Text = "";
        }

        private void CopyItem(object sender, RoutedEventArgs e)
        {
            if (!IsSelectedItem(CustomersGrid)) return;

            string content = "Копировать покупателя";
            ChangeDialogFrameState();
            _isAddNewItem = true;
            DialogTitle.Content = content;
            ApplyButton.Content = content;

            _selectedCustomer = CustomersGrid.SelectedItem as Base.Customers;

            string date = converter.Convert(_selectedCustomer.DateOfBirth.ToString(), typeof(DateTime), "ru", CultureInfo.CurrentCulture).ToString();
            FistName.Text = _selectedCustomer.FistName;
            FamilyName.Text = _selectedCustomer.FamilyName;
            DateOfBirth.Text = date;
            OrganisationName.Text = _selectedCustomer.OrganisationName;
            Address.Text = _selectedCustomer.Address;
            City.Text = _selectedCustomer.City;
            email.Text = _selectedCustomer.email;
            Phone.Text = _selectedCustomer.Phone;
            DocumentNumber.Text = _selectedCustomer.IDNumber;
            DocumentName.Text = _selectedCustomer.IDDocumentName;
        }

        private void EditItem(object sender, RoutedEventArgs e)
        {
            if (!IsSelectedItem(CustomersGrid)) return;

            string content = "Изменить покупателя";
            ChangeDialogFrameState();
            DialogTitle.Content = content;
            ApplyButton.Content = content;

            _selectedCustomer = CustomersGrid.SelectedItem as Base.Customers;

            string date = converter.Convert(_selectedCustomer.DateOfBirth.ToString(), typeof(DateTime), "ru", CultureInfo.CurrentCulture).ToString();
            FistName.Text = _selectedCustomer.FistName;
            FamilyName.Text = _selectedCustomer.FamilyName;
            DateOfBirth.Text = date;
            OrganisationName.Text = _selectedCustomer.OrganisationName;
            Address.Text = _selectedCustomer.Address;
            City.Text = _selectedCustomer.City;
            email.Text = _selectedCustomer.email;
            Phone.Text = _selectedCustomer.Phone;
            DocumentNumber.Text = _selectedCustomer.IDNumber;
            DocumentName.Text = _selectedCustomer.IDDocumentName;
        }

        private void DeleteItem(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("При подтверждении удаления данной записи, Вы не сможете её восстановить.\n\nПродолжить?",
                "Внимание",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning) != MessageBoxResult.Yes) return;

            Base.Customers deletingCustomer = CustomersGrid.SelectedItem as Base.Customers;
            CustomersGrid.SelectedIndex = CustomersGrid.SelectedIndex > 0 ? CustomersGrid.SelectedIndex-- : CustomersGrid.SelectedIndex++;
            try
            {
                SourceCore.DataBase.Customers.Remove(deletingCustomer);
                SourceCore.DataBase.SaveChanges();
                UpdateGrid(CustomersGrid.SelectedItem as Base.Customers);
            }
            catch (Exception ex) { MessageBox.Show($"Невозможно удалить запись: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void ApplyChanges(object sender, RoutedEventArgs e)
        {
            StringBuilder warnings = new StringBuilder();
            if (string.IsNullOrEmpty(FistName.Text)) warnings.Append("Не указано имя;\n");
            if (string.IsNullOrEmpty(FamilyName.Text)) warnings.Append("Не указана фамилия;\n");
            if (string.IsNullOrEmpty(DateOfBirth.Text)) warnings.Append("Не указана дата рождения;\n");
            if (string.IsNullOrEmpty(OrganisationName.Text)) warnings.Append("Не указано имя организации;\n");
            if (string.IsNullOrEmpty(Address.Text)) warnings.Append("Не указан адрес;\n");
            if (string.IsNullOrEmpty(City.Text)) warnings.Append("Не указан город;\n");
            if (string.IsNullOrEmpty(email.Text)) warnings.Append("Не указана адрес электронной почты;\n");
            if (string.IsNullOrEmpty(Phone.Text)) warnings.Append("Не указан номер телефона;\n");
            if (string.IsNullOrEmpty(DocumentNumber.Text)) warnings.Append("Не указан номер документа;\n");
            if (string.IsNullOrEmpty(DocumentName.Text)) warnings.Append("Не указано название документа;\n");

            if (warnings.Length != 0)
            {
                MessageBox.Show($"Исправьте следующие недочеты:\n\n{warnings}", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DateTime.TryParse(DateOfBirth.Text, out DateTime date);
            var customer = (_isAddNewItem) ? new Base.Customers() : SourceCore.DataBase.Customers.First(c => c.Customer_ID == _selectedCustomer.Customer_ID);
            customer.FistName = FistName.Text;
            customer.FamilyName = FamilyName.Text;
            customer.DateOfBirth = date;
            customer.OrganisationName = OrganisationName.Text;
            customer.Address = Address.Text;
            customer.City = City.Text;
            customer.email = email.Text;
            customer.Phone = Phone.Text;
            customer.IDNumber = DocumentNumber.Text;
            customer.IDDocumentName = DocumentName.Text;

            if (_isAddNewItem)
            {
                SourceCore.DataBase.Customers.Add(customer);
                _selectedCustomer = customer;
            }
            try { SourceCore.DataBase.SaveChanges(); }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }

            UpdateGrid(customer);
            ChangeDialogFrameState(false);
        }

        private void CancelChanges(object sender, RoutedEventArgs e) => ChangeDialogFrameState(false);

        private void UpdateGrid(Base.Customers customers = null)
        {
            if ((customers == null) && (CustomersGrid.ItemsSource != null)) customers = (Base.Customers)CustomersGrid.SelectedItem;

            CustomersGrid.ItemsSource = new ObservableCollection<Base.Customers>(SourceCore.DataBase.Customers.ToList());
            CustomersGrid.SelectedItem = customers;
        }

        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            List<string> filters = new List<string>();
            foreach (DataGridColumn column in CustomersGrid.Columns)
            {
                column.CanUserSort = false;
                filters.Add(column.Header.ToString());
            }

            FilterComboBox.ItemsSource = filters;
            FilterComboBox.SelectedIndex = 0;
        }

        private void UpdateFilterText(object sender, TextChangedEventArgs e)
        {
            List<Base.Customers> getFiltredList(int selectedIndex)
            {
                List<Base.Customers> result = new List<Base.Customers>(SourceCore.DataBase.Customers.ToList());
                string filterText = FilterTextBox.Text;

                switch (selectedIndex)
                {
                    case 0:
                        result = SourceCore.DataBase.Customers.Where(a => a.FistName.Contains(filterText)).ToList();
                        break;
                    case 1:
                        result = SourceCore.DataBase.Customers.Where(a => a.FamilyName.Contains(filterText)).ToList();
                        break;
                    case 2:
                        string[] dates = filterText.Split('.');
                        if (dates.Length == 0)
                            break;

                        StringBuilder date = new StringBuilder(10);
                        if (dates.Length >= 3)
                        {
                            int.TryParse(dates[2], out int year);
                            if (year != 0) date.Append($"{dates[2]}-");
                        }

                        if (dates.Length >= 2)
                        {
                            int.TryParse(dates[1], out int month);
                            if (month != 0) date.Append($"{dates[1]}-");
                        }

                        int.TryParse(dates[0], out int day);
                        if (day != 0) date.Append(dates[0]);

                        string dts = date.ToString();
                        result = SourceCore.DataBase.Customers.Where(a => a.DateOfBirth.ToString().Contains(dts)).ToList(); 
                        break;
                    case 3:
                        result = SourceCore.DataBase.Customers.Where(a => a.OrganisationName.Contains(filterText)).ToList();
                        break;
                    case 4:
                        result = SourceCore.DataBase.Customers.Where(a => a.Address.ToString().Contains(filterText)).ToList();
                        break;
                    case 5:
                        result = SourceCore.DataBase.Customers.Where(a => a.City.ToString().Contains(filterText)).ToList();
                        break;
                    case 6:
                        result = SourceCore.DataBase.Customers.Where(a => a.email.ToString().Contains(filterText)).ToList();
                        break;
                    case 7:
                        result = SourceCore.DataBase.Customers.Where(a => a.Phone.Contains(filterText)).ToList();
                        break;
                    case 8:
                        result = SourceCore.DataBase.Customers.Where(a => a.IDNumber.ToString().Contains(filterText)).ToList();
                        break;
                    case 9:
                        result = SourceCore.DataBase.Customers.Where(a => a.IDDocumentName.ToString().Contains(filterText)).ToList();
                        break;
                }

                return result;
            }

            CustomersGrid.ItemsSource = getFiltredList(FilterComboBox.SelectedIndex);
        }
    }
}
