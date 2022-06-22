using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WPFClient.Pages
{
    public partial class AccessoriesToBoatsPage : Page
    {
        private bool _isAddNewItem = false;
        private Base.AccessoriesToBoat _selectedAccessoryToBoat;

        public AccessoriesToBoatsPage()
        {
            InitializeComponent();
            DataContext = this;
            AccessoriesToBoatGrid.ItemsSource = SourceCore.DataBase.AccessoriesToBoat.ToList();
            Accessory.ItemsSource = SourceCore.DataBase.Accessory.ToList();
            Boat.ItemsSource = SourceCore.DataBase.Boat.ToList();
        }

        private void ChangeDialogFrameState(bool show = true)
        {
            DialogFrame.Width = new GridLength(show ? 400 : 0);
            AccessoriesToBoatGrid.IsHitTestVisible = !show;
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
            string content = "Добавить аксессуар к яхте";
            ChangeDialogFrameState();
            _isAddNewItem = true;
            AccessoriesToBoatGrid.SelectedItem = null;
            DialogTitle.Content = content;
            ApplyButton.Content = content;
            Accessory.Text = "";
            Boat.Text = "";
        }

        private void CopyItem(object sender, RoutedEventArgs e)
        {
            if (!IsSelectedItem(AccessoriesToBoatGrid)) return;

            string content = "Копировать аксессуар к яхте";
            ChangeDialogFrameState();
            _isAddNewItem = true;
            DialogTitle.Content = content;
            ApplyButton.Content = content;

            _selectedAccessoryToBoat = AccessoriesToBoatGrid.SelectedItem as Base.AccessoriesToBoat;
            Accessory.Text = _selectedAccessoryToBoat.Accessory.AccName;
            Boat.Text = _selectedAccessoryToBoat.Boat.Model;
        }

        private void EditItem(object sender, RoutedEventArgs e)
        {
            if (!IsSelectedItem(AccessoriesToBoatGrid)) return;

            string content = "Изменить аксессуар к яхте";
            ChangeDialogFrameState();
            DialogTitle.Content = content;
            ApplyButton.Content = content;

            _selectedAccessoryToBoat = AccessoriesToBoatGrid.SelectedItem as Base.AccessoriesToBoat;
            Accessory.Text = _selectedAccessoryToBoat.Accessory.AccName;
            Boat.Text = _selectedAccessoryToBoat.Boat.Model;
        }

        private void DeleteItem(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("При подтверждении удаления данной записи, Вы не сможете её восстановить.\n\nПродолжить?",
                "Внимание",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning) != MessageBoxResult.Yes) return;

            SourceCore.DataBase.AccessoriesToBoat.Remove((Base.AccessoriesToBoat)AccessoriesToBoatGrid.SelectedItem);
            SourceCore.DataBase.SaveChanges();
        }

        private void ApplyChanges(object sender, RoutedEventArgs e)     
        {
            var accessoryToBoat = (_isAddNewItem) ? new Base.AccessoriesToBoat() : SourceCore.DataBase.AccessoriesToBoat.First(atb => atb.Fit_ID == _selectedAccessoryToBoat.Fit_ID);
            accessoryToBoat.Accessory = Accessory.SelectedItem as Base.Accessory;
            accessoryToBoat.Boat = Boat.SelectedItem as Base.Boat;

            if (_isAddNewItem)
            {
                SourceCore.DataBase.AccessoriesToBoat.Add(accessoryToBoat);
                _selectedAccessoryToBoat = accessoryToBoat;
            }
            try { SourceCore.DataBase.SaveChanges(); } catch (Exception ex) { MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
            UpdateGrid(accessoryToBoat);
            ChangeDialogFrameState(false);

        }

        private void CancelChanges(object sender, RoutedEventArgs e) => ChangeDialogFrameState(false);

        private void UpdateGrid(Base.AccessoriesToBoat accessoryToBoat = null)
        {
            if ((accessoryToBoat == null) && (AccessoriesToBoatGrid.ItemsSource != null)) accessoryToBoat = AccessoriesToBoatGrid.SelectedItem as Base.AccessoriesToBoat;

            AccessoriesToBoatGrid.ItemsSource = new ObservableCollection<Base.AccessoriesToBoat>(SourceCore.DataBase.AccessoriesToBoat.ToList());
            AccessoriesToBoatGrid.SelectedItem = accessoryToBoat;
        }
    }
}
