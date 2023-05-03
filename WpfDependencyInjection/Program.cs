using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using WpfDependencyInjection.ViewModels;
using WpfDependencyInjection.Views;
using WpfLibrary;

namespace WpfDependencyInjection;

class Program
{
    private static IHost? _host;

    [STAThread]
    static void Main(string[] args)
    {
        ILogger<Program>? logger;

        using (var host = _host = CreateHostBuilder(args).Build())
        {
            host.Start();

            logger = host.Services.GetRequiredService<ILogger<Program>>();

            // VSColorOutput64 で色分けすると捗ります。 コメントは {Level:u3} の文字列です。
            logger.LogCritical("Fatal");            // FTL
            logger.LogError("Error");               // ERR
            logger.LogWarning("Warning");           // WRN
            logger.LogInformation("Information");   // INF
            logger.LogDebug("Debug");               // DBG
            logger.LogTrace("Verbose");             // VRB

            logger.LogInformation("--- Start Main ---");

            var mainWindow = host.Services.GetRequiredService<MainWindow>();
            mainWindow.Closing += (_, e) =>
            {
                if (e.Cancel) return;
                Debug.WriteLine("MainWindowClosing()");
            };

            var app = host.Services.GetRequiredService<App>();
            app.InitializeComponent();
            app.Run(mainWindow);
        }

        logger.LogInformation("--- End Main ---");
    }

#if false
    // Mainメソッドを非同期化すると、[STAThread] を付けていてもUIコンポーネントの生成時に Thread で怒られます。
    // 以下のように自分で指定すれば対応できます。
    static async Task Main(string[] args)
    {
        if (!Thread.CurrentThread.TrySetApartmentState(ApartmentState.STA))
        {
            Thread.CurrentThread.SetApartmentState(ApartmentState.Unknown);
            Thread.CurrentThread.SetApartmentState(ApartmentState.STA);
        }
        using var host = _host = CreateHostBuilder(args).Build();
        await host.StartAsync().ConfigureAwait(false);
        ...
    }
#endif

    // ViewModelLocator から使用されます。(直で DataContext を設定する場合は必要ありません)
    internal static object GetViewModel(Type viewType) => _host!.Services.GetRequiredViewModel(viewType);

    private static IHostBuilder CreateHostBuilder(string[] args) => Host
        .CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((context, configBuilder) =>
        {
            configBuilder.AddCommandLine(args);
            configBuilder.SetBasePath(context.HostingEnvironment.ContentRootPath);  // ローカルでは同じPATHでした。 System.IO.Directory.GetCurrentDirectory()
            configBuilder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);
        })
        .UseSerilog((hostingContext, services, loggerConfiguration) =>
        {
            loggerConfiguration
#if true
                // "appsettings.json" からログ設定を読み取ります
                .ReadFrom.Configuration(hostingContext.Configuration);
#else
                // デフォルトのログレベル設定
                .MinimumLevel.Information()
                // Microsoft 名前空間下のログレベルを Warning にする（Serilog でデフォルトは Information)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Error)
                // ログに追加のコンテキスト情報を提供します
                .Enrich.FromLogContext()
                // ログの出力先と自動ローテーションの設定。 RollingInterval.Day なら1日ごとに新しいファイルが作成されます
                .WriteTo.File(@"Logs\log.txt", rollingInterval: RollingInterval.Day);
#endif
        })
        .ConfigureServices((hostContext, services) =>
        {
            services.Configure<CommandLineArgs>(hostContext.Configuration);
            services.Configure<AppSettings>(hostContext.Configuration.GetSection(nameof(AppSettings)));

            services.AddSingleton<App>();

            // ViewModelLocator のサンプルとして AddViewAndViewModel() を使用していますが、
            // コードビハインドで(View.Ctor から) DataContext を差し込めばよい気がしてきました。
            services.AddViewAndViewModel<MainWindow, MainWindowViewModel>();

            services.AddSingleton<IExternalObject, ExternalObject>();

            services.AddSingleton<IPageIndexCounter, PageIndexCounter>();
            services.AddIndexedFactory<ParentPage>();
            services.AddTransient<ParentPageViewModel>();
            services.AddTransient<ChildView>();
            services.AddTransient<ChildViewModel>();

        });
}
