using ACFW.Controllers;
using ACFW.Example.Environment;
using ACFW.Example.Views;
using ACFW.Views;
using System;
using UnityEngine;

namespace ACFW.Example.Controllers
{
    public class TestUIReference : ScriptableReference<TestUIView>
    {
        public Type ViewLoaderFactoryType => typeof(ViewLoaderFactory);
        public Type ContextControllerFactoryType => typeof(ContextControllerFactory);
        public Type MVCContainerFactoryType => typeof(MVCContainerFactory);
        public class ViewLoaderFactory : ViewLoaderFactory<TestUIView, TestUIReference>
        {
            public ViewLoaderFactory(TestUIReference reference) : base(reference)
            {
            }
        }

        public class ContextControllerFactory : IFactory<TestUIController, TestUIView>
        {
            private readonly TestEvents testEvents;
            public ContextControllerFactory(TestEvents testEvents)
            {
                this.testEvents = testEvents;
            }

            public TestUIController Create(TestUIView view)
            {
                return new TestUIController(view, testEvents);
            }
        }

        public class MVCContainer : AbstractMVCContainer<TestUIReference>
        {
            public MVCContainer(IFactory<GameObjectLoader<IView>, Transform> viewLoaderFactory, IFactory<IContextController, IView> contextControllerFactory) :
                base(viewLoaderFactory, contextControllerFactory)
            {
            }

            public override bool IsUI => true;
        }

        public class MVCContainerFactory : AbstractMVCContainerFactory<TestUIReference>
        {
            private readonly ViewLoaderFactory<TestUIView, TestUIReference> viewLoaderFactory;
            private readonly IFactory<TestUIController, TestUIView> contextControllerFactory;

            public MVCContainerFactory(ViewLoaderFactory<TestUIView, TestUIReference> viewLoaderFactory, IFactory<TestUIController, TestUIView> contextControllerFactory)
            {
                this.viewLoaderFactory = viewLoaderFactory;
                this.contextControllerFactory = contextControllerFactory;
            }

            public override IMVCContainer Create()
            {
                return new TestUIReference.MVCContainer(viewLoaderFactory, (IFactory<IContextController, IView>)contextControllerFactory);
            }
        }
    }
}
