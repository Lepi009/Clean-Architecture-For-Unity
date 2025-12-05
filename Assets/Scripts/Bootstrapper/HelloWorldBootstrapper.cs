using System.Collections.Generic;
using UnityEngine;
using Domain;
using Domain.EventBus;
using Application;
using Infrastructure;
using Presentation;
using DeviceType = Domain.DeviceType;

namespace Bootstrapper {
    public class HelloWorldBootstrapper : MonoBehaviour {
        //include all fields and properties here (private & public)
        #region Fields and Properties

        private DIContainer _container;

        #endregion

        //include all events here
        #region Events

        #endregion

        #region Unity Callbacks

        private void Awake() {
            _container = new DIContainer();

            // Register concrete instances or services
            _container.Register<IDomainLogger>(
                new UnityLogger()
            );

            _container.Register<ITimeProvider>(new UnityTimeProvider());

            _container.Register<IEventBus>(
                new DecoupledEventBus(_container.Resolve<ITimeProvider>())
            );

            _container.Register<IRandomService>(new UnityRandomAdapter());

            _container.Register<ICoroutineRunner>(UnityCoroutineRunner.Create());

            ServiceLocator.Initialize(
                _container.Resolve<IDomainLogger>(),
                _container.Resolve<IEventBus>(),
                _container.Resolve<ICoroutineRunner>(),
                _container.Resolve<IRandomService>()
            );

            //input
            _container.Register(new InputLayerManager(
                new IInputLayer[] {
                    new UnityUILayer()
                }
            ));
            _container.Register(new InputRouter(
                _container.Resolve<InputLayerManager>()));
            _container.Register(new InputSchemeManager(
                new Dictionary<DeviceType, IInputScheme>{
                    {DeviceType.KeyboardMouse, new KeyboardMouseScheme()}
                },
                DeviceType.KeyboardMouse,
                _container.Resolve<InputRouter>()
            ));

            //view manager
            _container.Register<IPrefabProvider>(new AddressablePrefabProvider());
            _container.Register<IViewManager>(new ViewPipeline(
                _container.Resolve<IPrefabProvider>()
            ));

            //platform-dependent injection
            _container.Register<IFileProvider>(
    #if UNITY_WEBGL
                new WebRequestFileProvider()
    #else
                new ReadAllTextFileProvider()
    #endif
            );


            //example user interface
            _container.Register(new HelloWorldService());

            _container.Register(new HelloWorldPresenter(
                _container.Resolve<IViewManager>(),
                _container.Resolve<HelloWorldService>()
            ));
            _container.Resolve<IViewManager>().Register<HelloWorldView, HelloWorldPresenter>();
            _container.Register(new HelloWorldInputLayer(
                _container.Resolve<HelloWorldPresenter>(),
                _container.Resolve<InputLayerManager>()
            ));
        }

        void Start()
        {
            _container.InitializePending();
        }

        void Update()
        {
            _container.UpdateAll(Time.deltaTime);
        }

        #endregion
    }
}