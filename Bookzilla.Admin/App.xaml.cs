using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;

using Bookzilla.Admin.Activation;
using Bookzilla.Admin.Contracts.Activation;
using Bookzilla.Admin.Contracts.Services;
using Bookzilla.Admin.Contracts.Views;
using Bookzilla.Admin.Core.Contracts.Services;
using Bookzilla.Admin.Core.Services;
using Bookzilla.Admin.Dialogs.DialogService;
using Bookzilla.Admin.Models;
using Bookzilla.Admin.Services;
using Bookzilla.Admin.ViewModels;
using Bookzilla.Admin.Views;

using CommunityToolkit.WinUI.Notifications;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Bookzilla.Admin;

// For more information about application lifecycle events see https://docs.microsoft.com/dotnet/framework/wpf/app-development/application-management-overview

// WPF UI elements use language en-US by default.
// If you need to support other cultures make sure you add converters and review dates and numbers in your UI to ensure everything adapts correctly.
// Tracking issue for improving this is https://github.com/dotnet/wpf/issues/1946
public partial class App : Application
{
    private IHost _host;

    public T GetService<T>()
        where T : class
        => _host.Services.GetService(typeof(T)) as T;

    public App()
    {
    }

    private async void OnStartup(object sender, StartupEventArgs e)
    {
        // https://docs.microsoft.com/windows/apps/design/shell/tiles-and-notifications/send-local-toast?tabs=desktop
        ToastNotificationManagerCompat.OnActivated += (toastArgs) =>
        {
            Current.Dispatcher.Invoke(async () =>
            {
                var config = GetService<IConfiguration>();
                config[ToastNotificationActivationHandler.ActivationArguments] = toastArgs.Argument;
                await _host.StartAsync();
            });
        };

        // TODO: Register arguments you want to use on App initialization
        var activationArgs = new Dictionary<string, string>
        {
            { ToastNotificationActivationHandler.ActivationArguments, string.Empty }
        };
        var appLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

        // For more information about .NET generic host see  https://docs.microsoft.com/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-3.0
        _host = Host.CreateDefaultBuilder(e.Args)
                .ConfigureAppConfiguration(c =>
                {
                    c.SetBasePath(appLocation);
                    c.AddInMemoryCollection(activationArgs);
                })
                .ConfigureServices(ConfigureServices)
                .Build();

        if (ToastNotificationManagerCompat.WasCurrentProcessToastActivated())
        {
            // ToastNotificationActivator code will run after this completes and will show a window if necessary.
            return;
        }

        await _host.StartAsync();
    }

    private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        // TODO: Register your services, viewmodels and pages here

        // App Host
        services.AddHostedService<ApplicationHostService>();

        // Activation Handlers
        services.AddSingleton<IActivationHandler, ToastNotificationActivationHandler>();

        // Core Services
        services.AddSingleton<IFileService, FileService>();

        // Services
        services.AddSingleton<IToastNotificationsService, ToastNotificationsService>();
        services.AddSingleton<IApplicationInfoService, ApplicationInfoService>();
        services.AddSingleton<ISystemService, SystemService>();
        services.AddSingleton<ICollectionAPIClient, CollectionAPIClient>();
        services.AddSingleton<IPublicationAPIClient, PublicationAPIClient>();
        services.AddSingleton<ITomeAPIClient, TomeAPIClient>();
        services.AddSingleton<IParamAPIClient, ParamAPIClient>();
        services.AddSingleton<IGoogleBookAPIClient, GoogleBookAPIClient>();
        services.AddSingleton<ICoverExtractor, CoverExtractor>();
        services.AddSingleton<IPersistAndRestoreService, PersistAndRestoreService>();
        services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
        services.AddSingleton<ISampleDataService, SampleDataService>();
        services.AddSingleton<IPageService, PageService>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<DialogService>();

        // Views and ViewModels
        services.AddTransient<IShellWindow, ShellWindow>();
        services.AddTransient<ShellViewModel>();

        services.AddTransient<MainViewModel>();
        services.AddTransient<MainPage>();

        services.AddTransient<CollectionListViewModel>();
        services.AddTransient<CollectionListPage>();

        services.AddTransient<CollectionListDetailViewModel>();
        services.AddTransient<CollectionListDetailPage>();

        services.AddTransient<PublicationListViewModel>();
        services.AddTransient<PublicationListPage>();

        services.AddTransient<PublicationListDetailViewModel>();
        services.AddTransient<PublicationListDetailPage>();

        services.AddTransient<TomeListViewModel>();
        services.AddTransient<TomeListPage>();

        services.AddTransient<TomeListDetailViewModel>();
        services.AddTransient<TomeListDetailPage>();

        services.AddTransient<SettingsViewModel>();
        services.AddTransient<SettingsPage>();

        services.AddTransient<TomeGoogleSynchroSearchViewModel>();
        services.AddTransient<TomeGoogleSynchroSearch>();

        services.AddTransient<TomeGooglereconcileViewModel>();
        services.AddTransient<TomeGooglereconcilePage>();


        // Configuration
        services.Configure<AppConfig>(context.Configuration.GetSection(nameof(AppConfig)));
    }

    private async void OnExit(object sender, ExitEventArgs e)
    {
        await _host.StopAsync();
        _host.Dispose();
        _host = null;
    }

    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        // TODO: Please log and handle the exception as appropriate to your scenario
        // For more info see https://docs.microsoft.com/dotnet/api/system.windows.application.dispatcherunhandledexception?view=netcore-3.0
    }
}
