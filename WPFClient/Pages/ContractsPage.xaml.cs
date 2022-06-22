using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WPFClient.Pages
{
    public partial class ContractsPage : Page
    {
        private bool _isAddNewItem = false;
        private Base.Contract _selectedContract;
        private ValueConverter converter = new ValueConverter();

        public ContractsPage()
        {
            InitializeComponent();
            DataContext = this;
            UpdateGrid();
            Boat.ItemsSource = SourceCore.DataBase.Contract.ToList();
        }

        private void ChangeDialogFrameState(bool show = true)
        {
            DialogFrame.Width = new GridLength(show ? 400 : 0);
            ContractsGrid.IsHitTestVisible = !show;
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
            string content = "Добавить контракт";
            ChangeDialogFrameState();
            _isAddNewItem = true;
            ContractsGrid.SelectedItem = null;
            DialogTitle.Content = content;
            ApplyButton.Content = content;
            Date.Text = "";
            DepositPayed.Text = "";
            Boat.Text = "";
            Price.Text = "";
            VAT.Text = "";
            ProductionProcess.Text = "";
        }

        private void CopyItem(object sender, RoutedEventArgs e)
        {
            if (!IsSelectedItem(ContractsGrid)) return;

            string content = "Копировать контракт";
            ChangeDialogFrameState();
            _isAddNewItem = true;
            DialogTitle.Content = content;
            ApplyButton.Content = content;

            _selectedContract = ContractsGrid.SelectedItem as Base.Contract;

            string date = converter.Convert(_selectedContract.Date.ToString(), typeof(DateTime), "ru", System.Globalization.CultureInfo.CurrentCulture).ToString();
            Date.Text = date;
            DepositPayed.Text = _selectedContract.DepositPayed;
            Boat.Text = _selectedContract.Order.Boat.Model; // Not work?
            Price.Text = _selectedContract.ContractTotalPrice;
            VAT.Text = _selectedContract.ContracTotalPrice_inclVAT;
            ProductionProcess.Text = _selectedContract.ProductionProcess;
        }

        private void EditItem(object sender, RoutedEventArgs e)
        {
            if (!IsSelectedItem(ContractsGrid)) return;

            string content = "Изменить контракт";
            ChangeDialogFrameState();
            DialogTitle.Content = content;
            ApplyButton.Content = content;

            _selectedContract = ContractsGrid.SelectedItem as Base.Contract;
            Date.Text = _selectedContract.Date.ToString();
            DepositPayed.Text = _selectedContract.DepositPayed;
            Boat.Text = _selectedContract.Order.Boat.Model; // Not work?
            Price.Text = _selectedContract.ContractTotalPrice;
            VAT.Text = _selectedContract.ContracTotalPrice_inclVAT;
            ProductionProcess.Text = _selectedContract.ProductionProcess;
        }

        private void DeleteItem(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("При подтверждении удаления данной записи, Вы не сможете её восстановить.\n\nПродолжить?",
                "Внимание",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning) != MessageBoxResult.Yes) return;

            Base.Contract deletingContract = ContractsGrid.SelectedItem as Base.Contract;
            ContractsGrid.SelectedIndex = ContractsGrid.SelectedIndex > 0 ? ContractsGrid.SelectedIndex-- : ContractsGrid.SelectedIndex++;
            try
            {
                SourceCore.DataBase.Contract.Remove(deletingContract);
                SourceCore.DataBase.SaveChanges();
                UpdateGrid(ContractsGrid.SelectedItem as Base.Contract);
            }
            catch (Exception ex) { MessageBox.Show($"Невозможно удалить запись: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void ApplyChanges(object sender, RoutedEventArgs e)
        {
            StringBuilder warnings = new StringBuilder();
            if (string.IsNullOrEmpty(Date.Text)) warnings.Append("Не указана дата;\n");
            if (string.IsNullOrEmpty(DepositPayed.Text)) warnings.Append("Не указан внесенный депозит;\n");
            if (string.IsNullOrEmpty(Price.Text)) warnings.Append("Не указана цена;\n");
            if (string.IsNullOrEmpty(VAT.Text)) warnings.Append("Не указан налог;\n");
            if (string.IsNullOrEmpty(ProductionProcess.Text)) warnings.Append("Не указан процесс работы;\n");
            if (Boat.SelectedItem == null) warnings.Append("Не указана яхта;\n");

            if (warnings.Length != 0)
            {
                MessageBox.Show($"Исправьте следующие недочеты:\n\n{warnings}", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DateTime.TryParse(Date.Text, out DateTime date);
            var contract = (_isAddNewItem) ? new Base.Contract() : SourceCore.DataBase.Contract.First(c => c.Contract_ID == _selectedContract.Contract_ID);
            contract.Date = date;
            contract.DepositPayed = DepositPayed.Text;
            contract.ContractTotalPrice = Price.Text;
            contract.ContracTotalPrice_inclVAT = VAT.Text;
            contract.ProductionProcess = ProductionProcess.Text;
            contract.Order.Boat = Boat.SelectedItem as Base.Boat;

            if (_isAddNewItem)
            {
                SourceCore.DataBase.Contract.Add(contract);
                _selectedContract = contract;
            }
            try { SourceCore.DataBase.SaveChanges(); }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }

            UpdateGrid(contract);
            ChangeDialogFrameState(false);
        }

        private void CancelChanges(object sender, RoutedEventArgs e) => ChangeDialogFrameState(false);

        private void UpdateGrid(Base.Contract contracts = null)
        {
            if ((contracts == null) && (ContractsGrid.ItemsSource != null)) contracts = (Base.Contract)ContractsGrid.SelectedItem;

            ContractsGrid.ItemsSource = new ObservableCollection<Base.Contract>(SourceCore.DataBase.Contract.ToList());
            ContractsGrid.SelectedItem = contracts;
        }

        private void ShowDetailTableClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
