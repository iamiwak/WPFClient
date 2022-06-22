using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WPFClient.Pages
{
    public partial class BoatsPage : Page
    {
        private bool _isAddNewItem = false;
        private bool _isDetailTableShow = false;
        private Base.Boat _selectedBoat;

        public BoatsPage()
        {
            InitializeComponent();
            DataContext = this;
            UpdateGrid();
        }

        private void ChangeDialogFrameState(bool show = true)
        {
            DialogFrame.Width = new GridLength(show ? 400 : 0);
            MainDialogFrame.Visibility = show ? Visibility.Visible : Visibility.Hidden;
            DialogFrameGridSplitter.IsEnabled = show;
            BoatsGrid.IsHitTestVisible = !show;
            _isAddNewItem = false;
        }

        private void ChangeDetailTableFrameState(bool show = true)
        {
            DetailTableFrame.Width = new GridLength(show ? DetailTableFrame.MaxWidth : 0);
            _isDetailTableShow = show;
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
            string content = "Добавить яхту";
            ChangeDialogFrameState();
            _isAddNewItem = true;
            DialogTitle.Content = content;
            ApplyButton.Content = content;
            ModelName.Text = "";
            Type.Text = "";
            NumberOfRowers.Text = "";
            Mast.Text = "";
            Color.Text = "";
            Wood.Text = "";
            BasePrice.Text = "";
            VAT.Text = "";
        }

        private void CopyItem(object sender, RoutedEventArgs e)
        {
            if (!IsSelectedItem(BoatsGrid)) return;

            string content = "Копировать яхту";
            ChangeDialogFrameState();
            _isAddNewItem = true;
            DialogTitle.Content = content;
            ApplyButton.Content = content;

            _selectedBoat = BoatsGrid.SelectedItem as Base.Boat;
            ModelName.Text = _selectedBoat.Model;
            Type.Text = _selectedBoat.BoatType;
            NumberOfRowers.Text = _selectedBoat.NumberOfRowers.ToString();
            Mast.Text = _selectedBoat.Mast;
            Color.Text = _selectedBoat.Colour;
            Wood.Text = _selectedBoat.Wood;
            BasePrice.Text = _selectedBoat.BasePrice;
            VAT.Text = _selectedBoat.VAT;
        }

        private void EditItem(object sender, RoutedEventArgs e)
        {
            if (!IsSelectedItem(BoatsGrid)) return;

            string content = "Изменить яхту";
            ChangeDialogFrameState();
            DialogTitle.Content = content;
            ApplyButton.Content = content;

            _selectedBoat = BoatsGrid.SelectedItem as Base.Boat;
            ModelName.Text = _selectedBoat.Model;
            Type.Text = _selectedBoat.BoatType;
            NumberOfRowers.Text = _selectedBoat.NumberOfRowers.ToString();
            Mast.Text = _selectedBoat.Mast;
            Color.Text = _selectedBoat.Colour;
            Wood.Text = _selectedBoat.Wood;
            BasePrice.Text = _selectedBoat.BasePrice;
            VAT.Text = _selectedBoat.VAT;
        }

        private void DeleteItem(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("При подтверждении удаления данной записи, Вы не сможете её восстановить.\n\nПродолжить?",
                "Внимание",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning) != MessageBoxResult.Yes) return;

            Base.Boat deletingBoat = BoatsGrid.SelectedItem as Base.Boat;
            BoatsGrid.SelectedIndex = BoatsGrid.SelectedIndex > 0 ? BoatsGrid.SelectedIndex-- : BoatsGrid.SelectedIndex++;
            try
            {
                SourceCore.DataBase.Boat.Remove(deletingBoat);
                SourceCore.DataBase.SaveChanges();
                UpdateGrid(BoatsGrid.SelectedItem as Base.Boat);
            }
            catch (Exception ex) { MessageBox.Show($"Невозможно удалить запись: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void ApplyChanges(object sender, RoutedEventArgs e)
        {
            StringBuilder warnings = new StringBuilder();
            if (string.IsNullOrEmpty(ModelName.Text)) warnings.Append("Не указано название яхты;\n");
            if (string.IsNullOrEmpty(Type.Text)) warnings.Append("Не указано тип яхты;\n");
            if (string.IsNullOrEmpty(NumberOfRowers.Text)) warnings.Append("Не указано максимальное количество гребцов;\n");
            if (string.IsNullOrEmpty(Mast.Text)) warnings.Append("Не указано наличие мачты;\n");
            if (string.IsNullOrEmpty(Color.Text)) warnings.Append("Не указан цвет яхты;\n");
            if (string.IsNullOrEmpty(Wood.Text)) warnings.Append("Не указан тип дерева;\n");
            if (string.IsNullOrEmpty(BasePrice.Text)) warnings.Append("Не указана базовая цена;\n");
            if (string.IsNullOrEmpty(VAT.Text)) warnings.Append("Не указан налог;\n");

            if (warnings.Length != 0)
            {
                MessageBox.Show($"Исправьте следующие недочеты:\n\n{warnings}", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int.TryParse(NumberOfRowers.Text, out int numberOfRowers);
            Base.Boat boat = (_isAddNewItem) ? new Base.Boat() : SourceCore.DataBase.Boat.First(b => b.boat_ID == _selectedBoat.boat_ID);
            boat.Model = ModelName.Text;
            boat.BoatType = Type.Text;
            boat.NumberOfRowers = numberOfRowers;
            boat.Mast = Mast.Text;
            boat.Colour = Color.Text;
            boat.Wood = Wood.Text;
            boat.BasePrice = BasePrice.Text;
            boat.VAT = VAT.Text;

            if (_isAddNewItem)
            {
                SourceCore.DataBase.Boat.Add(boat);
                _selectedBoat = boat;
            }

            try { SourceCore.DataBase.SaveChanges(); }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }

            UpdateGrid(boat);
            ChangeDialogFrameState(false);
        }

        private void CancelChanges(object sender, RoutedEventArgs e) => ChangeDialogFrameState(false);

        private void UpdateGrid(Base.Boat boats = null)
        {
            if ((boats == null) && (BoatsGrid.ItemsSource != null)) boats = (Base.Boat)BoatsGrid.SelectedItem;

            BoatsGrid.ItemsSource = new ObservableCollection<Base.Boat>(SourceCore.DataBase.Boat.ToList());
            BoatsGrid.SelectedItem = boats;
        }

        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            List<string> filters = new List<string>();
            foreach (DataGridColumn column in BoatsGrid.Columns)
            {
                column.CanUserSort = false;
                filters.Add(column.Header.ToString());
            }

            FilterComboBox.ItemsSource = filters;
            FilterComboBox.SelectedIndex = 0;
        }

        private void UpdateFilterText(object sender, TextChangedEventArgs e)
        {
            List<Base.Boat> getFiltredList(int selectedIndex)
            {
                List<Base.Boat> result = new List<Base.Boat>(SourceCore.DataBase.Boat.ToList());
                string filterText = FilterTextBox.Text;

                switch (selectedIndex)
                {
                    case 0:
                        result = SourceCore.DataBase.Boat.Where(a => a.Model.Contains(filterText)).ToList();
                        break;
                    case 1:
                        result = SourceCore.DataBase.Boat.Where(a => a.BoatType.Contains(filterText)).ToList();
                        break;
                    case 2:
                        result = SourceCore.DataBase.Boat.Where(a => a.NumberOfRowers.ToString().Contains(filterText)).ToList();
                        break;
                    case 3:
                        result = SourceCore.DataBase.Boat.Where(a => a.Mast.Contains(filterText)).ToList();
                        break;
                    case 4:
                        result = SourceCore.DataBase.Boat.Where(a => a.Colour.Contains(filterText)).ToList();
                        break;
                    case 5:
                        result = SourceCore.DataBase.Boat.Where(a => a.Wood.Contains(filterText)).ToList();
                        break;
                    case 6:
                        result = SourceCore.DataBase.Boat.Where(a => a.BasePrice.Contains(filterText)).ToList();
                        break;
                    case 7:
                        result = SourceCore.DataBase.Boat.Where(a => a.VAT.Contains(filterText)).ToList();
                        break;
                }

                return result;
            }

            BoatsGrid.ItemsSource = getFiltredList(FilterComboBox.SelectedIndex);
        }

        private void ShowDetailTablClick(object sender, RoutedEventArgs e)
        {
            ShowDetailTableButton.Content = _isDetailTableShow ? "Показать detail-таблицу" : "Скрыть detail-таблицу";
            ChangeDetailTableFrameState(!_isDetailTableShow);
        }
    }
}
