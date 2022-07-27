using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Windows;
using Grace.DependencyInjection;
using HLab.Bugs.Wpf;
using HLab.Core;
using HLab.Core.Annotations;
using HLab.Core.DebugTools;
using HLab.Erp.Acl;
using HLab.Erp.Acl.AuditTrails;
using HLab.Erp.Acl.LoginServices;
using HLab.Erp.Base.Data;
using HLab.Erp.Base.Wpf.Entities.Customers;
using HLab.Erp.Core;
using HLab.Erp.Core.DragDrops;
using HLab.Erp.Core.EntityLists;
using HLab.Erp.Core.Wpf.EntityLists;
using HLab.Erp.Core.WebService;
using HLab.Erp.Core.Wpf.DragDrops;
using HLab.Erp.Core.Wpf.Localization;
using HLab.Erp.Core.Wpf.WebService;
using HLab.Erp.Data;
using HLab.Erp.Data.Observables;
using HLab.Erp.Lims.Analysis.Data;
using HLab.Erp.Lims.Analysis.Data.Workflows;
using HLab.Erp.Lims.Analysis.Module.Manufacturers;
using HLab.Erp.Lims.Monographs.Data;
using HLab.Icons.Annotations.Icons;
using HLab.Icons.Wpf.Icons;
using HLab.Mvvm;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Application;
using HLab.Mvvm.Application.Wpf;
using HLab.Mvvm.Flowchart;
using HLab.Mvvm.Flowchart.Models;
using HLab.Mvvm.Wpf;
using HLab.Notify.Annotations;
using HLab.Notify.PropertyChanged;
using HLab.Notify.Wpf;
using HLab.Options;
using HLab.Options.Wpf;


namespace HLab.Erp.Lims.Analysis.Loader
{



    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //private T Locate<T>() => Locator<T>.Locate();

        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                base.OnStartup(e);
                var container = new DependencyInjectionContainer();

                //Locator.Configure();

                container.Configure(c =>
                {
                    c.Export<OptionsServices>().As<IOptionsService>().Lifestyle.Singleton();
                    c.Export<EventHandlerServiceWpf>().As<IEventHandlerService>().Lifestyle.Singleton();
                    c.Export<MessageBus>().As<IMessageBus>().Lifestyle.Singleton();

                    c.Export<AclService>().As<IAclService>().Lifestyle.Singleton();
                    c.Export<AclHelperWpf>().As<IAclHelper>().Lifestyle.Singleton();
                    c.Export<DebugLogger>().As<IDebugLogger>().Lifestyle.Singleton();
                    c.Export<DataService>().As<IDataService>().Lifestyle.Singleton();

                    c.Export<DragDropServiceWpf>().As<IDragDropService>().Lifestyle.Singleton();
                    c.Export<DialogService>().As<IDialogService>().Lifestyle.Singleton();
                    c.Export<CurrencyService>().As<ICurrencyService>().Lifestyle.Singleton();

                    c.Export<UnitService>().As<IUnitService>().Lifestyle.Singleton();

                    c.Export<GraphService>().As<IGraphService>().Lifestyle.Singleton();
                    c.Export<BrowserViewModel>().As<IBrowserService>().Lifestyle.Singleton();
                    c.Export<IconService>().As<IIconService>().Lifestyle.Singleton();
                    c.Export<LocalizationService>().As<ILocalizationService>().Lifestyle.Singleton();
                    c.Export<MvvmServiceWpf>().As<IMvvmService>().Lifestyle.Singleton();
                    c.Export<ErpServices>().As<IErpServices>().Lifestyle.Singleton();
                    c.Export<ApplicationInfoService>().As<IApplicationInfoService>().Lifestyle.Singleton();
                    c.Export<DocumentServiceWpf>().As<IDocumentService>().Lifestyle.Singleton();
                    c.Export<MenuService>().As<IMenuService>().Lifestyle.Singleton();
                    c.Export<LocalizeFromDb>().As<LocalizeFromDb>().Lifestyle.Singleton();

                    c.Export<MainWpfViewModel>().As<MainWpfViewModel>().Lifestyle.Singleton();

                    c.Export<LoginViewModel>().As<ILoginViewModel>();
                    c.Export<AuditTrailMotivationViewModel>().As<IAuditTrailProvider>();
                    c.Export<SelectedMessage>().As<ISelectedMessage>();

                    c.Export(typeof(EntityListHelper<>)).As(typeof(IEntityListHelper<>));
                    c.Export(typeof(ColumnsProvider<>)).As(typeof(IColumnsProvider<>));
                    c.Export(typeof(ObservableQuery<>)).As(typeof(IObservableQuery<>));
                    c.Export(typeof(DataLocker<>)).As(typeof(IDataLocker<>));
                    //c.ExportWrapper(typeof(DataLocker<>)).As(typeof(IDataLocker<>));

                    //c.ExportDecorator(typeof(DataLocker<>)).As(typeof(IDataLocker<>));
                    var parser = new AssemblyParser();

                    //var a0 = boot.LoadDll("HLab.Erp.Core.Wpf");
                    var a01 = parser.LoadDll("HLab.Options.Wpf");
                    var a3 = parser.LoadDll("HLab.Notify.Wpf");
                    var a2 = parser.LoadDll("HLab.Erp.Base.Wpf");
                    //  var b0 = boot.LoadDll("HLab.Mvvm");
                    //  var c0 = boot.LoadDll("HLab.Mvvm.Wpf");
                    //  var d0 = boot.LoadDll("HLab.Erp.Data");
                    var d1 = parser.LoadDll("HLab.Erp.Data.Wpf");
                    var e0 = parser.LoadDll("HLab.Erp.Acl.Wpf");
                    var a1 = parser.LoadDll("HLab.Erp.Workflows.Wpf");
                    var g0 = parser.LoadDll("HLab.Erp.Lims.Analysis.Data");
                    var g2 = parser.LoadDll("HLab.Erp.Lims.Analysis.Module");
                    //var g1 = boot.LoadDll("HLab.Erp.Lims.Monographs.Module");

                    parser.LoadModules();

                    parser.Add<IView>(t => c.Export(t).As(typeof(IView)));
                    parser.Add<IViewModel>(t => c.Export(t).As(typeof(IViewModel)));
                    parser.Add<IBootloader>(t => c.Export(t).As(typeof(IBootloader)));

                    parser.Add<IToolGraphBlock>(t => c.Export(t).As(typeof(IToolGraphBlock)));
                    parser.Add<IEntityListViewModel>(listType =>
                    {
                        if (listType.IsGenericType) return;

                        foreach (var interfaceType in listType.GetInterfaces().Where(i => i.IsGenericType))
                        {
                            if (interfaceType.GetGenericTypeDefinition() != typeof(IEntityListViewModel<>)) continue;
                            var entityType = interfaceType.GetGenericArguments()[0];

                            c.Export(listType).As(interfaceType);
                        }
                    });

                    parser.Parse();

                    c.ExportInitialize<Entity>(o => o.DataService = container.Locate<IDataService>());
                    c.ExportInitialize<GraphElement>(o => o.MvvmService = container.Locate<IMvvmService>());
                    c.ExportInitialize<NestedBootloader>(o => o.Erp = container.Locate<IErpServices>());
                });

//                SingletonLocator<IOptionsService>.Set<OptionsServices>();

//                SingletonLocator<IEventHandlerService>.Set<EventHandlerServiceWpf>();
//                SingletonLocator<IAclService>.Set<AclService>();
//                SingletonLocator<IAclHelper>.Set<AclHelperWpf>();
//                SingletonLocator<IDebugLogger>.Set<DebugLogger>();
//                SingletonLocator<IDataService>.Set<DataService>();
//                SingletonLocator<IMessageBus>.Set<MessageBus>();
                //SingletonLocator<IDragDropService>.Set<DragDropServiceWpf>();
                //SingletonLocator<IDialogService>.Set<DialogService>();
                //SingletonLocator<ICurrencyService>.Set<CurrencyService>();

                //SingletonLocator<IUnitService>.Set<UnitService>();

                //SingletonLocator<IGraphService>.Set<GraphService>();
                //SingletonLocator<IBrowserService>.Set<BrowserViewModel>();
                //SingletonLocator<IIconService>.Set<IconService>();
                //SingletonLocator<ILocalizationService>.Set<LocalizationService>();
                //SingletonLocator<IMvvmService>.Set<MvvmServiceWpf>();
                //SingletonLocator<IErpServices>.Set<ErpServices>();
                //SingletonLocator<IApplicationInfoService>.Set<ApplicationInfoService>();
                //SingletonLocator<IDocumentService>.Set<DocumentServiceWpf>();
                //SingletonLocator<IMenuService>.Set<MenuService>();

                //SingletonLocator<LocalizeFromDb>.Set<LocalizeFromDb>();

                //Locator<ILoginViewModel>.Set<LoginViewModel>();
                //Locator<IAuditTrailProvider>.Set<AuditTrailMotivationViewModel>();
                //Locator<ISelectedMessage>.Set<SelectedMessage>();

                //SingletonLocator<MainWpfViewModel>.Set<MainWpfViewModel>();

                //Locator.SetOpenGenericFactory(typeof(IEntityListHelper<>), typeof(EntityListHelper<>));
                //Locator.SetOpenGenericFactory(typeof(IColumnsProvider<>), typeof(ColumnsProvider<>));
                //Locator.SetOpenGenericFactory(typeof(IObservableQuery<>), typeof(ObservableQuery<>));
                ////Locator.SetOpenGenericFactory(typeof(IEntityListViewModel<>),typeof(EntityListViewModel<>));
                //Locator.SetOpenGenericFactory(typeof(IDataLocker<>), typeof(DataLocker<>));

                NotifyHelper.EventHandlerService = container.Locate<IEventHandlerService>();


                _ = SampleWorkflow.Production;
                _ = SampleTestWorkflow.ValidatedResults;
                _ = SampleTestResultWorkflow.Checked;


                //Locator.SetFactory(typeof(IDataLocker<>),);

                /*            


                            .Export(typeof(DataLocker<>)).As(typeof(IDataLocker<>))

                              .Export(typeof(ColumnsProvider<>)).As(typeof(IColumnsProvider<>))
                             .Export(typeof(ListableEntityListViewModel<>)).As(typeof(IListableEntityListViewModel<>))
                             .Export(typeof(ColumnConfigurator<,,>)).As(typeof(IColumnConfigurator<,,>))
                             .Export(typeof(ColumnHelper<>)).As(typeof(IColumn<>.IHelper))
                             .Export(typeof(ObservableQuery<>)).As(typeof(ObservableQuery<>))
                             .Export(typeof(EntityListHelper<>)).As(typeof(IEntityListHelper<>))

                             .Export<MvvmContext>().As<IMvvmContext>()
                             .Export<SelectedMessage>().As<ISelectedMessage>()
                             .Export<DetailsPanelViewModel>().As<DetailsPanelViewModel>()

                             );
                ;

                */
                //boot.Scope.ExportInitialize<BootLoaderErpWpf>(b => b.SetMainViewMode(typeof(ViewModeKiosk)));





                //Locator.InitSingletons();

                var options = container.Locate<IOptionsService>();
                options.OptionsPath = "HLab.Erp";
                options.AddProvider(new OptionsProviderRegistry());

                //STATIC IMPORTS//
                NotifyHelper.EventHandlerService = container.Locate<IEventHandlerService>();
                WorkflowAnalysisExtension.Acl = container.Locate<IAclService>();

                var mvvm = container.Locate<IMvvmService>();

                mvvm.Register(typeof(Customer), typeof(CustomerViewModel), typeof(IViewClassDocument), typeof(ViewModeDefault));
                mvvm.Register(typeof(Manufacturer), typeof(ManufacturerViewModel), typeof(IViewClassDocument), typeof(ViewModeDefault));
                mvvm.Register();

                var doc = container.Locate<IDocumentService>();
                doc.MainViewModel = container.Locate<MainWpfViewModel>();

                var boot = new Bootstrapper(container.Locate<IEnumerable<IBootloader>>);

                

                boot.Boot();
            }
            catch (Exception ex)
            {
                var view = new ExceptionView {
                    Exception = ex 
                    //, Token = "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"
                    , Token = "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"
                    };
                view.ShowDialog();
#if DEBUG
                //throw;
                ExceptionDispatchInfo.Capture(ex).Throw();
#endif        
            }
        }
    }
}
