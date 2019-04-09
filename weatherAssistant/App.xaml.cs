using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using weatherAssistant.ViewModels;

namespace weatherAssistant
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private CultureInfo cultureOverride = new CultureInfo("pl-PL");
        //private CultureInfo cultureOverride = new CultureInfo("en-US");


        public App()
        {
            if (Debugger.IsAttached == true && cultureOverride != null)
            {
                Thread.CurrentThread.CurrentUICulture = cultureOverride;
                Thread.CurrentThread.CurrentCulture = cultureOverride;
            }
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            cultureOverride.NumberFormat.NumberDecimalSeparator = ".";
            MainWindow app = new MainWindow();
            MainWindowViewModel context = new MainWindowViewModel();
            app.DataContext = context;
            app.Show();
        }


    }
}
