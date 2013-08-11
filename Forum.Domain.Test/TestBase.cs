using System;
using System.Reflection;
using System.Threading;
using ENode;
using ENode.Autofac;
using ENode.Commanding;
using ENode.Domain;
using ENode.Eventing;
using ENode.Infrastructure;
using ENode.JsonNet;
using ENode.Log4Net;
using Forum.Domain.Events;
using NUnit.Framework;

namespace Forum.Domain.Test
{
    [TestFixture]
    public class TestBase
    {
        public static ManualResetEvent EventHandlerWaiter;
        public static ManualResetEvent TestThreadWaiter;
        protected static Random _random = new Random();
        protected ICommandService _commandService;
        protected IMemoryCache _memoryCache;
        private static bool _initialized = false;

        [TestFixtureSetUp]
        public void SetUp()
        {
            if (!_initialized)
            {
                var querydbConnectionString = "Data Source=.;Initial Catalog=EventDB;Integrated Security=True;Connect Timeout=30;Min Pool Size=10;Max Pool Size=100";
                var assemblies = new Assembly[] { Assembly.Load("Forum.Domain"), Assembly.Load("Forum.Application"), Assembly.Load("Forum.Denormalizer"), Assembly.GetExecutingAssembly() };
                Configuration
                    .Create()
                    .UseAutofac()
                    .RegisterFrameworkComponents()
                    .RegisterBusinessComponents(assemblies)
                    .UseLog4Net()
                    .UseJsonNet()
                    .UseDefaultSqlQueryDbConnectionFactory(querydbConnectionString)
                    .CreateAllDefaultProcessors()
                    .Initialize(assemblies)
                    .Start();
                _initialized = true;
            }
            _commandService = ObjectContainer.Resolve<ICommandService>();
            _memoryCache = ObjectContainer.Resolve<IMemoryCache>();
        }

        protected void ResetWaiters()
        {
            EventHandlerWaiter = new ManualResetEvent(false);
            TestThreadWaiter = new ManualResetEvent(false);
        }
        protected string RandomString()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Ticks + _random.Next(100);
        }
    }

    [Component]
    public class IdSetter :
        IEventHandler<AccountCreated>,
        IEventHandler<SectionCreated>,
        IEventHandler<SectionNameChanged>,
        IEventHandler<PostCreated>,
        IEventHandler<PostBodyChanged> {

        public void Handle(AccountCreated evnt) {
            AccountTest.AccountId = evnt.AccountId;
            TestBase.EventHandlerWaiter.Set();
        }

        public void Handle(SectionCreated evnt) {
            SectionTest.SectionId = evnt.SectionId;
            TestBase.EventHandlerWaiter.Set();
        }
        public void Handle(SectionNameChanged evnt) {
            TestBase.EventHandlerWaiter.Set();
        }

        public void Handle(PostCreated evnt) {
            PostTest.PostId = evnt.PostId;
            TestBase.EventHandlerWaiter.Set();
        }
        public void Handle(PostBodyChanged evnt) {
            TestBase.EventHandlerWaiter.Set();
        }
    }
}
