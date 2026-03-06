using Prog4GodMAUI.Views;
using System;

namespace Prog4GodMAUI
{
    public partial class App : Application
    {
        public App(AppShell appShell)
        {
            InitializeComponent();
            MainPage = appShell;
        }

        //public App()
        //{
        //    InitializeComponent();
        //    MainPage = new MainPage();
        //}
    }
}
