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
    /// <summary>
    /// Interaction logic for PartnersPage.xaml
    /// </summary>
    public partial class PartnersPage : Page
    {
        private bool _isAddNewItem = false;
        private Base.Partner _selectedPartner;

        public PartnersPage()
        {
            InitializeComponent();
            DataContext = this;
            UpdateGrid();
        }

        private void ChangeDialogFrameState(bool show = true)
        {
            DialogFrame.Width = new GridLength(show ? 400 : 0);
            PartnersGrid.IsHitTestVisible = !show;
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
            string content = "Добавить партнера";
            ChangeDialogFrameState();
            _isAddNewItem = true;
            PartnersGrid.SelectedItem = null;
            DialogTitle.Content = content;
            ApplyButton.Content = content;
            PartnerName.Text = "";
            Address.Text = "";
            City.Text = "";
        }

        private void CopyItem(object sender, RoutedEventArgs e)
        {
            if (!IsSelectedItem(PartnersGrid)) return;

            string content = "Копировать партнера";
            ChangeDialogFrameState();
            _isAddNewItem = true;
            DialogTitle.Content = content;
            ApplyButton.Content = content;

            _selectedPartner = PartnersGrid.SelectedItem as Base.Partner;
            PartnerName.Text = _selectedPartner.Name;
            Address.Text = _selectedPartner.Address;
            City.Text = _selectedPartner.City;
        }

        private void EditItem(object sender, RoutedEventArgs e)
        {
            if (!IsSelectedItem(PartnersGrid)) return;

            string content = "Изменить партнера";
            ChangeDialogFrameState();
            DialogTitle.Content = content;
            ApplyButton.Content = content;

            _selectedPartner = PartnersGrid.SelectedItem as Base.Partner;
            PartnerName.Text = _selectedPartner.Name;
            Address.Text = _selectedPartner.Address;
            City.Text = _selectedPartner.City;
        }

        private void DeleteItem(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("При подтверждении удаления данной записи, Вы не сможете её восстановить.\n\nПродолжить?",
                "Внимание",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning) != MessageBoxResult.Yes) return;

            Base.Partner deletingPartner = PartnersGrid.SelectedItem as Base.Partner;
            PartnersGrid.SelectedIndex = PartnersGrid.SelectedIndex > 0 ? PartnersGrid.SelectedIndex-- : PartnersGrid.SelectedIndex++;
            try
            {
                SourceCore.DataBase.Partner.Remove(deletingPartner);
                SourceCore.DataBase.SaveChanges();
                UpdateGrid(PartnersGrid.SelectedItem as Base.Partner);
            }
            catch (Exception ex) { MessageBox.Show($"Невозможно удалить запись: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void ApplyChanges(object sender, RoutedEventArgs e)
        {
            StringBuilder warnings = new StringBuilder();
            if (string.IsNullOrEmpty(PartnerName.Text)) warnings.Append("Не указано название;\n");
            if (string.IsNullOrEmpty(Address.Text)) warnings.Append("Не указан адрес;\n");
            if (string.IsNullOrEmpty(City.Text)) warnings.Append("Не указан город;\n");

            if (warnings.Length != 0)
            {
                MessageBox.Show($"Исправьте следующие недочеты:\n\n{warnings}", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var partner = (_isAddNewItem) ? new Base.Partner() : SourceCore.DataBase.Partner.First(p => p.Partner_ID == _selectedPartner.Partner_ID);
            partner.Name = PartnerName.Text;
            partner.Address = Address.Text;
            partner.City = City.Text;

            if (_isAddNewItem)
            {
                SourceCore.DataBase.Partner.Add(partner);
                _selectedPartner = partner;
            }
            try { SourceCore.DataBase.SaveChanges(); }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }

            UpdateGrid(partner);
            ChangeDialogFrameState(false);
        }

        private void CancelChanges(object sender, RoutedEventArgs e) => ChangeDialogFrameState(false);

        private void UpdateGrid(Base.Partner partners = null)
        {
            if ((partners == null) && (PartnersGrid.ItemsSource != null)) partners = (Base.Partner)PartnersGrid.SelectedItem;

            PartnersGrid.ItemsSource = new ObservableCollection<Base.Partner>(SourceCore.DataBase.Partner.ToList());
            PartnersGrid.SelectedItem = partners;
        }

        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            List<string> filters = new List<string>();
            foreach (DataGridColumn column in PartnersGrid.Columns)
            {
                column.CanUserSort = false;
                filters.Add(column.Header.ToString());
            }

            FilterComboBox.ItemsSource = filters;
            FilterComboBox.SelectedIndex = 0;
        }

        private void UpdateFilterText(object sender, TextChangedEventArgs e)
        {
            List<Base.Partner> getFiltredList(int selectedIndex)
            {
                List<Base.Partner> result = new List<Base.Partner>(SourceCore.DataBase.Partner.ToList());
                string filterText = FilterTextBox.Text;

                switch (selectedIndex)
                {
                    case 0:
                        result = SourceCore.DataBase.Partner.Where(a => a.Name.Contains(filterText)).ToList();
                        break;
                    case 1:
                        result = SourceCore.DataBase.Partner.Where(a => a.Address.Contains(filterText)).ToList();
                        break;
                    case 2:
                        result = SourceCore.DataBase.Partner.Where(a => a.City.Contains(filterText)).ToList();
                        break;
                }

                return result;
            }

            PartnersGrid.ItemsSource = getFiltredList(FilterComboBox.SelectedIndex);
        }
    }
}
