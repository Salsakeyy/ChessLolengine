﻿using System;
using System.IO;
using System.Reflection;
using System.Windows;
using Backend.IO;
using Microsoft.Win32;

namespace Frontend.View.FlyoutContent
{
    /// <summary>
    ///     Logique d'interaction pour GameViewFlyout.xaml
    /// </summary>
    public partial class GameViewFlyout
    {
        private GameView _gameView;

        public GameViewFlyout(GameView gameView)
        {
            InitializeComponent();
            _gameView = gameView;
        }

        /// <summary>
        ///     Action effectuée lors d'un click sur la tile sauvegarder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TileSave_OnClick(object sender, RoutedEventArgs e)
        {
            ISaver saver = new BinarySaver();
            var directorySaveName = "Save";
            var fullSavePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" +
                               directorySaveName;
            Console.WriteLine(fullSavePath);
            if (Directory.Exists(fullSavePath) == false) Directory.CreateDirectory(fullSavePath);
            var saveFileDialog = new SaveFileDialog
            {
                Filter = saver.Filter(),
                InitialDirectory = fullSavePath
            };
            if (saveFileDialog.ShowDialog() == true) saver.Save(_gameView.Game.Container, saveFileDialog.FileName);
        }

        private async void TileQuit_OnClick(object sender, RoutedEventArgs e)
        {
            await _gameView.Quit();
        }
    }
}