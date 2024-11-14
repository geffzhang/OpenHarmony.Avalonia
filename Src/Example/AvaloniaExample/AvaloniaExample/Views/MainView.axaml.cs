using Avalonia.Controls;
using Avalonia.Interactivity;
using AvaloniaExample.ViewModels;
using System.Threading.Tasks;

namespace AvaloniaExample.Views
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
            Test();
        }

        async Task Test()
        {
            await Task.Yield();
            var vm = this.DataContext as MainViewModel;
            for (int i = 0; i < 10; i++)
            {
                await Task.Delay(1000);
                vm.Greeting = "Welcome to Avalonia!" + i;
                
            }
        }

        public void Next(object source, RoutedEventArgs args)
        {
            slides.Next();
        }

        public void Previous(object source, RoutedEventArgs args)
        {
            slides.Previous();
        }
    }
}