using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WPFClient.Pages
{
    public partial class AccessoriesPage : Page
    {
        private bool _isAddNewItem;
        private Base.Accessory _selectedAccessory;

        public AccessoriesPage()
        {
            InitializeComponent();
            DataContext = this;
            UpdateGrid();
            PartnerId.ItemsSource = SourceCore.DataBase.Partner.ToList();
        }

        private void ChangeDialogFrameState(bool show = true)
        {
            DialogFrame.Width = new GridLength(show ? 400 : 0);
            AccessoriesGrid.IsHitTestVisible = !show;
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
            string content = "Добавить аксессуар";
            ChangeDialogFrameState();
            _isAddNewItem = true;
            AccessoriesGrid.SelectedItem = null;
            DialogTitle.Content = content;
            ApplyButton.Content = content;
            AccName.Text = "";
            DescriptionOfAccessory.Text = "";
            Price.Text = "";
            VAT.Text = "";
            Inventory.Text = "";
            OrderLevel.Text = "";
            OrderBatch.Text = "";
            PartnerId.Text = "";
        }

        private void CopyItem(object sender, RoutedEventArgs e)
        {
            if (!IsSelectedItem(AccessoriesGrid)) return;

            string content = "Копировать аксессуар";
            ChangeDialogFrameState();
            _isAddNewItem = true;
            DialogTitle.Content = content;
            ApplyButton.Content = content;

            _selectedAccessory = AccessoriesGrid.SelectedItem as Base.Accessory;
            AccName.Text = _selectedAccessory.AccName;
            DescriptionOfAccessory.Text = _selectedAccessory.DescriptionOfAccessory;
            Price.Text = _selectedAccessory.Price;
            VAT.Text = _selectedAccessory.VAT;
            Inventory.Text = _selectedAccessory.Inventory.ToString();
            OrderLevel.Text = _selectedAccessory.OrderLevel.ToString();
            OrderBatch.Text = _selectedAccessory.OrderBatch.ToString();
            PartnerId.Text = _selectedAccessory.Partner.Name;
        }

        private void EditItem(object sender, RoutedEventArgs e)
        {
            if (!IsSelectedItem(AccessoriesGrid)) return;

            string content = "Изменить аксессуар";
            ChangeDialogFrameState();
            DialogTitle.Content = content;
            ApplyButton.Content = content;

            _selectedAccessory = AccessoriesGrid.SelectedItem as Base.Accessory;
            AccName.Text = _selectedAccessory.AccName;
            DescriptionOfAccessory.Text = _selectedAccessory.DescriptionOfAccessory;
            Price.Text = _selectedAccessory.Price;
            VAT.Text = _selectedAccessory.VAT;
            Inventory.Text = _selectedAccessory.Inventory.ToString();
            OrderLevel.Text = _selectedAccessory.OrderLevel.ToString();
            OrderBatch.Text = _selectedAccessory.OrderBatch.ToString();
            PartnerId.Text = _selectedAccessory.Partner.Name;
        }

        private void DeleteItem(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("При подтверждении удаления данной записи, Вы не сможете её восстановить.\n\nПродолжить?",
                "Внимание",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning) != MessageBoxResult.Yes) return;

            Base.Accessory deletingAccessory = AccessoriesGrid.SelectedItem as Base.Accessory;
            AccessoriesGrid.SelectedIndex = AccessoriesGrid.SelectedIndex > 0 ? AccessoriesGrid.SelectedIndex-- : AccessoriesGrid.SelectedIndex++;

            try
            {
                SourceCore.DataBase.Accessory.Remove(deletingAccessory);
                SourceCore.DataBase.SaveChanges();
                UpdateGrid(AccessoriesGrid.SelectedItem as Base.Accessory);
            }
            catch (Exception ex) { MessageBox.Show($"Невозможно удалить запись: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void ApplyChanges(object sender, RoutedEventArgs e)
        {
            StringBuilder warnings = new StringBuilder();
            if (string.IsNullOrEmpty(AccName.Text)) warnings.Append("Не указано название аксессуара;\n");
            if (string.IsNullOrEmpty(DescriptionOfAccessory.Text)) warnings.Append("Не указано описание аксессуара;\n");
            if (string.IsNullOrEmpty(Price.Text)) warnings.Append("Не указана цена;\n");
            if (string.IsNullOrEmpty(VAT.Text)) warnings.Append("Не указан налог;\n");
            if (PartnerId.SelectedItem as Base.Partner == null) warnings.Append("Не указан партнер;\n");

            if (warnings.Length != 0)
            {
                MessageBox.Show($"Исправьте следующие недочеты:\n\n{warnings}", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int.TryParse(Inventory.Text, out int inventory);
            int.TryParse(OrderLevel.Text, out int orderLevel);
            int.TryParse(OrderBatch.Text, out int orderBatch);
            Base.Accessory accessory = (_isAddNewItem) ? new Base.Accessory() : SourceCore.DataBase.Accessory.First(a => a.Accessory_ID == _selectedAccessory.Accessory_ID);
            accessory.AccName = AccName.Text;
            accessory.DescriptionOfAccessory = DescriptionOfAccessory.Text;
            accessory.Price = Price.Text;
            accessory.VAT = VAT.Text;
            accessory.Inventory = inventory;
            accessory.OrderLevel = orderLevel;
            accessory.OrderBatch = orderBatch;
            accessory.Partner = (Base.Partner)PartnerId.SelectedItem;

            if (_isAddNewItem) SourceCore.DataBase.Accessory.Add(accessory);
            try { SourceCore.DataBase.SaveChanges(); } catch (Exception ex) { MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }

            UpdateGrid(accessory);
            ChangeDialogFrameState(false);
        }

        private void CancelChanges(object sender, RoutedEventArgs e) => ChangeDialogFrameState(false);

        private void UpdateGrid(Base.Accessory accessories = null)
        {
            if ((accessories == null) && (AccessoriesGrid.ItemsSource != null)) accessories = AccessoriesGrid.SelectedItem as Base.Accessory;

            AccessoriesGrid.ItemsSource = new ObservableCollection<Base.Accessory>(SourceCore.DataBase.Accessory.ToList());
            AccessoriesGrid.SelectedItem = accessories;
        }

        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            List<string> filters = new List<string>();
            foreach (DataGridColumn column in AccessoriesGrid.Columns)
            {
                column.CanUserSort = false;
                filters.Add(column.Header.ToString());
            }

            FilterComboBox.ItemsSource = filters;
            FilterComboBox.SelectedIndex = 0;
        }

        private void UpdateFilterText(object sender, TextChangedEventArgs e)
        {
            List<Base.Accessory> getFiltredList(int selectedIndex)
            {
                List<Base.Accessory> result = new List<Base.Accessory>(SourceCore.DataBase.Accessory.ToList());
                string filterText = FilterTextBox.Text;

                switch (selectedIndex)
                {
                    case 0:
                        result = SourceCore.DataBase.Accessory.Where(a => a.AccName.Contains(filterText)).ToList();
                        break;
                    case 1:
                        result = SourceCore.DataBase.Accessory.Where(a => a.DescriptionOfAccessory.Contains(filterText)).ToList();
                        break;
                    case 2:
                        result = SourceCore.DataBase.Accessory.Where(a => a.Price.Contains(filterText)).ToList();
                        break;
                    case 3:
                        result = SourceCore.DataBase.Accessory.Where(a => a.VAT.Contains(filterText)).ToList();
                        break;
                    case 4:
                        result = SourceCore.DataBase.Accessory.Where(a => a.Inventory.ToString().Contains(filterText)).ToList();
                        break;
                    case 5:
                        result = SourceCore.DataBase.Accessory.Where(a => a.OrderLevel.ToString().Contains(filterText)).ToList();
                        break;
                    case 6:
                        result = SourceCore.DataBase.Accessory.Where(a => a.OrderBatch.ToString().Contains(filterText)).ToList();
                        //if (int.TryParse(filterText, out int orderBatch))
                        //    result = SourceCore.DataBase.Accessory.Where(a => a.OrderBatch == orderBatch).ToList();
                        break;
                }

                return result;
            }

            AccessoriesGrid.ItemsSource = getFiltredList(FilterComboBox.SelectedIndex);
        }
    }
}
