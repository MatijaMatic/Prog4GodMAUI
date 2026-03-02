using Prog4GodMAUI.Views;

namespace Prog4GodMAUI
{
    public partial class App : Application
    {
        public App(AppShell appShell)
        {
            InitializeComponent();
            MainPage = appShell;
        }
    }
}