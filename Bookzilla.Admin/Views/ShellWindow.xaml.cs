﻿using System.Windows.Controls;

using Bookzilla.Admin.Contracts.Views;
using Bookzilla.Admin.ViewModels;

using MahApps.Metro.Controls;

namespace Bookzilla.Admin.Views;

public partial class ShellWindow : MetroWindow, IShellWindow
{
    public ShellWindow(ShellViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    public Frame GetNavigationFrame()
        => shellFrame;

    public void ShowWindow()
        => Show();

    public void CloseWindow()
        => Close();
}
