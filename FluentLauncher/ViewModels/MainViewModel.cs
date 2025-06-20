using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentLauncher.Contracts.Services;
using FluentLauncher.Core.Commands;
using FluentLauncher.Core.Models;
using FluentLauncher.Core.Utils;
using FluentLauncher.Helpers;
using FluentLauncher.Services;
using Microsoft.Windows.ApplicationModel.Resources;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Windows.System;


namespace FluentLauncher.ViewModels;

public partial class MainViewModel : ObservableRecipient
{
    [ObservableProperty]
    private ObservableCollection<GameConfig> _gameList = new();

    [ObservableProperty]
    private GameConfig? _selectedGame;
    public string PageHash { get; private set; }
    private readonly ShellViewModel _shellViewModel;
    private GameConfig? _currentGame;

    public MainViewModel()
    {
        _shellViewModel = App.GetService<ShellViewModel>();

        PageHash = Guid.NewGuid().ToString();
    }

    public void InitializeViewModel()
    {
        ResourceLoader resourceLoader = new();
        var a = Properties.Settings.Default.LastSelectGame;
        //TODO 修复System.NullReferenceException
        //      HResult = 0x80004003
        //Message = Object reference not set to an instance of an object.
        //Source = FluentLauncher
        //StackTrace:
        //      在 FluentLauncher.ViewModels.MainViewModel.InitializeViewModel() 在 C:\Users\123\Desktop\启动器\FluentLauncher\ViewModels\MainViewModel.cs 中: 第 39 行

        _shellViewModel.MainSelectGame = string.IsNullOrEmpty(Properties.Settings.Default.LastSelectGame)
            ? resourceLoader.GetString("Shell_Main")
            : SelectedGame.GameName;
    }

    partial void OnSelectedGameChanged(GameConfig? value)
    {
        if (value is not null)
        {
            _shellViewModel.MainSelectGame = value.GameName;
            _currentGame = value;
            Properties.Settings.Default.LastSelectGame = value.GamePath;
            Properties.Settings.Default.Save();
        }
        else
        {
            ResourceLoader resourceLoader = new();
            _shellViewModel.MainSelectGame = resourceLoader.GetString("Shell_Main");
        }
    }
    [RelayCommand]
    internal async Task LauncherGameAsync()
    {
        LaunchCommand.LaunchGameAsync(SelectedGame);
    }

    [RelayCommand]
    internal async Task LoadGameListAsync()
    {
        var config = await JsonConfigUtils.LoadFromFile();
        GameList.Clear();
        foreach (var game in config.GameConfig)
        {
            GameList.Add(game);
        }

        RestoreSelectedGame();
    }

    private void RestoreSelectedGame()
    {
        if (_currentGame != null)
        {
            SelectedGame = GameList.FirstOrDefault(g => g.GamePath == _currentGame.GamePath);
        }
    }
}