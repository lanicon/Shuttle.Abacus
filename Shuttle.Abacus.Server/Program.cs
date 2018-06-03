﻿using Castle.Windsor;
using log4net;
using Shuttle.Core.Castle;
using Shuttle.Core.Container;
using Shuttle.Core.Data;
using Shuttle.Core.Log4Net;
using Shuttle.Core.Logging;
using Shuttle.Core.ServiceHost;
using Shuttle.Esb;
using Shuttle.Recall;

namespace Shuttle.Abacus.Server
{
    public class Program
    {
        private static void Main()
        {
            ServiceHost.Run<Host>();
        }
    }

    public class Host : IServiceHost
    {
        private IServiceBus _bus;
        private WindsorContainer _container;

        public void Start()
        {
            Log.Assign(new Log4NetLog(LogManager.GetLogger(typeof(Host))));

            _container = new WindsorContainer();

            var container = new WindsorComponentContainer(_container);

            container.RegisterSuffixed("Shuttle.Abacus");

            EventStore.Register(container);
            ServiceBus.Register(container);

            container.Resolve<IDatabaseContextFactory>().ConfigureWith("Abacus");

            _bus = ServiceBus.Create(container).Start();
        }

        public void Stop()
        {
            _bus?.Dispose();
        }
    }
}