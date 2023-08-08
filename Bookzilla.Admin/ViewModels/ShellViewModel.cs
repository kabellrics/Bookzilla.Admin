﻿using System.Collections.ObjectModel;
using System.Windows.Input;

using Bookzilla.Admin.Contracts.Services;
using Bookzilla.Admin.Properties;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using MahApps.Metro.Controls;

namespace Bookzilla.Admin.ViewModels;

public class ShellViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;
    private HamburgerMenuItem _selectedMenuItem;
    private HamburgerMenuItem _selectedOptionsMenuItem;
    private RelayCommand _goBackCommand;
    private ICommand _menuItemInvokedCommand;
    private ICommand _optionsMenuItemInvokedCommand;
    private ICommand _loadedCommand;
    private ICommand _unloadedCommand;

    public HamburgerMenuItem SelectedMenuItem
    {
        get { return _selectedMenuItem; }
        set { SetProperty(ref _selectedMenuItem, value); }
    }

    public HamburgerMenuItem SelectedOptionsMenuItem
    {
        get { return _selectedOptionsMenuItem; }
        set { SetProperty(ref _selectedOptionsMenuItem, value); }
    }

    // TODO: Change the icons and titles for all HamburgerMenuItems here.
    public ObservableCollection<HamburgerMenuItem> MenuItems { get; } = new ObservableCollection<HamburgerMenuItem>()
    {
        new HamburgerMenuGlyphItem() { Label = Resources.ShellMainPage, Glyph = "\uE8A5", TargetPageType = typeof(MainViewModel) },
        new HamburgerMenuGlyphItem() { Label = Resources.ShellCollectionListPage, Glyph = "\uE8A5", TargetPageType = typeof(CollectionListViewModel) },
        new HamburgerMenuGlyphItem() { Label = Resources.ShellPublicationListPage, Glyph = "\uE8A5", TargetPageType = typeof(PublicationListViewModel) },
        new HamburgerMenuGlyphItem() { Label = Resources.ShellTomeListPage, Glyph = "\uE8A5", TargetPageType = typeof(TomeListViewModel) },
        //new HamburgerMenuGlyphItem() { Label = Resources.ShellCollectionDetailPage, Glyph = "\uE8A5", TargetPageType = typeof(CollectionDetailViewModel) },
        //new HamburgerMenuGlyphItem() { Label = Resources.ShellPublicationDetailPage, Glyph = "\uE8A5", TargetPageType = typeof(PublicationDetailViewModel) },
        //new HamburgerMenuGlyphItem() { Label = Resources.ShellTomeDetailPage, Glyph = "\uE8A5", TargetPageType = typeof(TomeDetailViewModel) },
        //new HamburgerMenuGlyphItem() { Label = Resources.ShellCollectionCreatePage, Glyph = "\uE8A5", TargetPageType = typeof(CollectionCreateViewModel) },
        //new HamburgerMenuGlyphItem() { Label = Resources.ShellPublicationCreatePage, Glyph = "\uE8A5", TargetPageType = typeof(PublicationCreateViewModel) },
        //new HamburgerMenuGlyphItem() { Label = Resources.ShellTomeImportPage, Glyph = "\uE8A5", TargetPageType = typeof(TomeImportViewModel) },
    };

    public ObservableCollection<HamburgerMenuItem> OptionMenuItems { get; } = new ObservableCollection<HamburgerMenuItem>()
    {
        new HamburgerMenuGlyphItem() { Label = Resources.ShellSettingsPage, Glyph = "\uE713", TargetPageType = typeof(SettingsViewModel) }
    };

    public RelayCommand GoBackCommand => _goBackCommand ?? (_goBackCommand = new RelayCommand(OnGoBack, CanGoBack));

    public ICommand MenuItemInvokedCommand => _menuItemInvokedCommand ?? (_menuItemInvokedCommand = new RelayCommand(OnMenuItemInvoked));

    public ICommand OptionsMenuItemInvokedCommand => _optionsMenuItemInvokedCommand ?? (_optionsMenuItemInvokedCommand = new RelayCommand(OnOptionsMenuItemInvoked));

    public ICommand LoadedCommand => _loadedCommand ?? (_loadedCommand = new RelayCommand(OnLoaded));

    public ICommand UnloadedCommand => _unloadedCommand ?? (_unloadedCommand = new RelayCommand(OnUnloaded));

    public ShellViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    private void OnLoaded()
    {
        _navigationService.Navigated += OnNavigated;
    }

    private void OnUnloaded()
    {
        _navigationService.Navigated -= OnNavigated;
    }

    private bool CanGoBack()
        => _navigationService.CanGoBack;

    private void OnGoBack()
        => _navigationService.GoBack();

    private void OnMenuItemInvoked()
        => NavigateTo(SelectedMenuItem.TargetPageType);

    private void OnOptionsMenuItemInvoked()
        => NavigateTo(SelectedOptionsMenuItem.TargetPageType);

    private void NavigateTo(Type targetViewModel)
    {
        if (targetViewModel != null)
        {
            _navigationService.NavigateTo(targetViewModel.FullName);
        }
    }

    private void OnNavigated(object sender, string viewModelName)
    {
        var item = MenuItems
                    .OfType<HamburgerMenuItem>()
                    .FirstOrDefault(i => viewModelName == i.TargetPageType?.FullName);
        if (item != null)
        {
            SelectedMenuItem = item;
        }
        else
        {
            SelectedOptionsMenuItem = OptionMenuItems
                    .OfType<HamburgerMenuItem>()
                    .FirstOrDefault(i => viewModelName == i.TargetPageType?.FullName);
        }

        GoBackCommand.NotifyCanExecuteChanged();
    }
}
