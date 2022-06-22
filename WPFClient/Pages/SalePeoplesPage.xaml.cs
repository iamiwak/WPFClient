using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class SalePeoplesPage : Page
    {
        private bool _isAddNewItem = false;
        private Base.Sales_Person _selectedSalePeople;

        public SalePeoplesPage()
        {
            InitializeComponent();
            DataContext = this;
            UpdateGrid();
        }

        private void ChangeDialogFrameState(bool show = true)
        {
            DialogFrame.Width = new GridLength(show ? 400 : 0);
            SalePeoplersGrid.IsHitTestVisible = !show;
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
            string content = "Добавить продавца";
            ChangeDialogFrameState();
            _isAddNewItem = true;
            SalePeoplersGrid.SelectedItem = null;
            DialogTitle.Content = content;
            ApplyButton.Content = content;
            FirstName.Text = "";
            FamilyName.Text = "";
        }

        private void CopyItem(object sender, RoutedEventArgs e)
        {
            if (!IsSelectedItem(SalePeoplersGrid)) return;

            string content = "Копировать продавца";
            ChangeDialogFrameState();
            _isAddNewItem = true;
            DialogTitle.Content = content;
            ApplyButton.Content = content;

            _selectedSalePeople = SalePeoplersGrid.SelectedItem as Base.Sales_Person;
            FirstName.Text = _selectedSalePeople.FirstName;
            FamilyName.Text = _selectedSalePeople.FamilyName;
        }

        private void EditItem(object sender, RoutedEventArgs e)
        {
            if (!IsSelectedItem(SalePeoplersGrid)) return;

            string content = "Изменить продавца";
            ChangeDialogFrameState();
            DialogTitle.Content = content;
            ApplyButton.Content = content;
            _selectedSalePeople = SalePeoplersGrid.SelectedItem as Base.Sales_Person;
            FirstName.Text = _selectedSalePeople.FirstName;
            FamilyName.Text = _selectedSalePeople.FamilyName;
        }

        private void DeleteItem(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("При подтверждении удаления данной записи, Вы не сможете её восстановить.\n\nПродолжить?",
                "Внимание",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning) != MessageBoxResult.Yes) return;

            Base.Sales_Person deletingSalePeople = SalePeoplersGrid.SelectedItem as Base.Sales_Person;
            SalePeoplersGrid.SelectedIndex = SalePeoplersGrid.SelectedIndex > 0 ? SalePeoplersGrid.SelectedIndex-- : SalePeoplersGrid.SelectedIndex++;
            try
            {
                SourceCore.DataBase.Sales_Person.Remove(deletingSalePeople);
                SourceCore.DataBase.SaveChanges();
                UpdateGrid(SalePeoplersGrid.SelectedItem as Base.Sales_Person);
            }
            catch (Exception ex) { MessageBox.Show($"Невозможно удалить запись: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void ApplyChanges(object sender, RoutedEventArgs e)
        {
            StringBuilder warnings = new StringBuilder();
            if (string.IsNullOrEmpty(FirstName.Text)) warnings.Append("Не указано имя;\n");
            if (string.IsNullOrEmpty(FamilyName.Text)) warnings.Append("Не указана фамилия;\n");

            if (warnings.Length != 0)
            {
                MessageBox.Show($"Исправьте следующие недочеты:\n\n{warnings}", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Base.Sales_Person salePerson = (_isAddNewItem) ? new Base.Sales_Person() : SourceCore.DataBase.Sales_Person.First(sp => sp.SalesPerson_ID == _selectedSalePeople.SalesPerson_ID);
            salePerson.FirstName = FirstName.Text;
            salePerson.FamilyName = FamilyName.Text;

            if (_isAddNewItem)
            {
                SourceCore.DataBase.Sales_Person.Add(salePerson);
                _selectedSalePeople = salePerson;
            }
            try { SourceCore.DataBase.SaveChanges(); }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }

            UpdateGrid(salePerson);
            ChangeDialogFrameState(false);
        }

        private void CancelChanges(object sender, RoutedEventArgs e) => ChangeDialogFrameState(false);

        private void UpdateGrid(Base.Sales_Person salePeoples = null)
        {
            if ((salePeoples == null) && (SalePeoplersGrid.ItemsSource != null)) salePeoples = (Base.Sales_Person)SalePeoplersGrid.SelectedItem;

            SalePeoplersGrid.ItemsSource = new ObservableCollection<Base.Sales_Person>(SourceCore.DataBase.Sales_Person.ToList());
            SalePeoplersGrid.SelectedItem = salePeoples;
        }

        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            List<string> filters = new List<string>();
            foreach (DataGridColumn column in SalePeoplersGrid.Columns)
            {
                column.CanUserSort = false;
                filters.Add(column.Header.ToString());
            }

            FilterComboBox.ItemsSource = filters;
            FilterComboBox.SelectedIndex = 0;
        }

        private void UpdateFilterText(object sender, TextChangedEventArgs e)
        {
            List<Base.Sales_Person> getFiltredList(int selectedIndex)
            {
                List<Base.Sales_Person> result = new List<Base.Sales_Person>(SourceCore.DataBase.Sales_Person.ToList());
                string filterText = FilterTextBox.Text;

                switch (selectedIndex)
                {
                    case 0:
                        result = SourceCore.DataBase.Sales_Person.Where(a => a.FirstName.Contains(filterText)).ToList();
                        break;
                    case 1:
                        result = SourceCore.DataBase.Sales_Person.Where(a => a.FamilyName.Contains(filterText)).ToList();
                        break;
                }

                return result;
            }

            SalePeoplersGrid.ItemsSource = getFiltredList(FilterComboBox.SelectedIndex);
        }
    }
}
