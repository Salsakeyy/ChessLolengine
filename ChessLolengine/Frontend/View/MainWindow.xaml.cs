using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Backend.IO;
using ControlzEx.Theming;
using MahApps.Metro.Controls.Dialogs;
using Container = Backend.Model.Container;

namespace Frontend.View
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            this.AccentColors = ThemeManager.Current.Themes
                .GroupBy(x => x.ColorScheme)
                .OrderBy(a => a.Key)
                .Select(a => new AccentColorMenuData { Name = a.Key, ColorBrush = a.First().ShowcaseBrush })
                .ToList();
        }

        public List<AccentColorMenuData> AccentColors { get; set; }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            //TODO has to do with the logger
            if (File.Exists("log.temp"))
                File.Delete("log.temp");
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (Environment.GetCommandLineArgs().Length != 1)
            {
                ILoader loader = new BinaryLoader();
                Container container = null;

                try
                {
                    container = loader.Load(Environment.GetCommandLineArgs()[1]);
                }
                catch (Exception)
                {
                    this.ShowMessageAsync("Its not possible to read the selected file",
                        Environment.GetCommandLineArgs()[1]);
                }

                MainControl.Content = container != null ? new Home(this, container) : new Home(this, new Container());
            }
            else
                MainControl.Content = new Home(this, new Container());
        }
    }

    public class AccentColorMenuData
    {
        public string Name { get; set; }
        public Brush ColorBrush { get; set; }

        public ICommand ChangeAccentCommand
        {
            get
            {
                return _changeAccentCommand ?? (_changeAccentCommand = 
                    new SimpleCommand { CanExecuteDelegate = x => true, ExecuteDelegate = x => DoChangeTheme(x) });
            }
        }
        private ICommand _changeAccentCommand;

        protected virtual void DoChangeTheme(object sender)
        {
            ThemeManager.Current.ChangeThemeColorScheme(Application.Current, Name);
        }
    }

    public class SimpleCommand : ICommand
    {
        public Predicate<object> CanExecuteDelegate { get; set; }
        public Action<object> ExecuteDelegate { get; set; }

        public bool CanExecute(object parameter)
        {
            return CanExecuteDelegate == null || CanExecuteDelegate(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public void Execute(object parameter)
        {
            ExecuteDelegate?.Invoke(parameter);
        }
    }
}