﻿using Bookzilla.Admin.Models;

namespace Bookzilla.Admin.Contracts.Services;

public interface IThemeSelectorService
{
    void InitializeTheme();

    void SetTheme(AppTheme theme);

    AppTheme GetCurrentTheme();
}
