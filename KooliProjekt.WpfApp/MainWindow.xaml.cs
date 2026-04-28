using System.Windows;
using KooliProjekt.WpfApp.Api;

namespace KooliProjekt.WpfApp;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var vm = new MainWindowViewModel(new ApiClient());
        vm.OnError = message => MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        vm.ConfirmDelete = () => MessageBox.Show("Delete selected item?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes;

        DataContext = vm;
    }
}
