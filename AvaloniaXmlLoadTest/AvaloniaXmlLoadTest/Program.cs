﻿using System;
using Avalonia;
using Avalonia.Logging.Serilog;

namespace AvaloniaXmlLoadTest
{
    class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        public static void Main(string[] args) => BuildAvaloniaApp().Start(AppMain, args);

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .With(new X11PlatformOptions { UseGpu = true, UseDeferredRendering = false })
                .With(new AvaloniaNativePlatformOptions { UseGpu = true})
                .With(new Win32PlatformOptions { UseDeferredRendering = false, AllowEglInitialization = true})
                .LogToDebug();

        // Your application's entry point. Here you can initialize your MVVM framework, DI
        // container, etc.
        private static void AppMain(Application app, string[] args)
        {
            app.Run(new MainWindow());
        }
    }
}
