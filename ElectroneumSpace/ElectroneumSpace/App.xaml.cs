﻿using ElectroneumSpace.Services;
using ElectroneumSpace.Views;

using Microsoft.Practices.Unity;

using Prism.Logging;
using Prism.Unity;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace ElectroneumSpace
{
    public partial class App : PrismApplication
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("MainPage");
        }

        protected override void RegisterTypes()
        {
            Container.RegisterType<ILoggerFacade, LoggerFacade>(new ContainerControlledLifetimeManager());
            Container.RegisterTypeForNavigation<MainPage>();
        }
    }
}
