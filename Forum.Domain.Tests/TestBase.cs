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
using Forum.Events.Account;
using Forum.Events.Post;
using Forum.Events.Reply;
using Forum.Events.Section;
using NUnit.Framework;

namespace Forum.Domain.Tests
{
    [TestFixture]
    public class TestBase
    {
        public static ManualResetEvent EventHandlerWaiter;
        public static ManualResetEvent TestThreadWaiter;
        protected static Random Random = new Random();
        protected ICommandService CommandService;
        protected IMemoryCache MemoryCache;
        private const string QuerySideConnectionString = "Data Source=(localdb)\\Projects;Initial Catalog=ForumDB;Integrated Security=True;Connect Timeout=30;Min Pool Size=10;Max Pool Size=100";
        private static bool _initialized;

        [TestFixtureSetUp]
        public void SetUp()
        {
            if (!_initialized)
            {
                var assemblies = new[]
                {
                    Assembly.Load("Forum.Domain"),
                    Assembly.Load("Forum.CommandHandlers"),
                    Assembly.Load("Forum.Denormalizers.Dapper"),
                    Assembly.Load("Forum.Domain.Tests")
                };
                Configuration
                    .Create()
                    .UseAutofac()
                    .RegisterFrameworkComponents()
                    .RegisterBusinessComponents(assemblies)
                    .UseLog4Net()
                    .UseJsonNet()
                    .UseDefaultSqlQueryDbConnectionFactory(QuerySideConnectionString)
                    .CreateAllDefaultProcessors()
                    .Initialize(assemblies)
                    .Start();
                _initialized = true;
            }
            CommandService = ObjectContainer.Resolve<ICommandService>();
            MemoryCache = ObjectContainer.Resolve<IMemoryCache>();
        }

        protected void ResetWaiters()
        {
            EventHandlerWaiter = new ManualResetEvent(false);
            TestThreadWaiter = new ManualResetEvent(false);
        }
        protected string RandomString()
        {
            return Guid.NewGuid().ToString();
        }
    }

    [Component]
    public class IdSetter :
        IEventHandler<AccountCreated>,
        IEventHandler<SectionCreated>,
        IEventHandler<SectionNameChanged>,
        IEventHandler<PostCreated>,
        IEventHandler<PostSubjectAndBodyChanged>,
        IEventHandler<PostReplied>,
        IEventHandler<ReplyReplied>,
        IEventHandler<ReplyBodyChanged>
    {
        public void Handle(AccountCreated evnt)
        {
            AccountTest.AccountId = evnt.AccountId;
            TestBase.EventHandlerWaiter.Set();
        }

        public void Handle(SectionCreated evnt)
        {
            SectionTest.SectionId = evnt.SectionId;
            TestBase.EventHandlerWaiter.Set();
        }
        public void Handle(SectionNameChanged evnt)
        {
            TestBase.EventHandlerWaiter.Set();
        }

        public void Handle(PostCreated evnt)
        {
            PostTest.PostId = evnt.PostId;
            TestBase.EventHandlerWaiter.Set();
        }
        public void Handle(PostSubjectAndBodyChanged evnt)
        {
            TestBase.EventHandlerWaiter.Set();
        }

        public void Handle(PostReplied evnt)
        {
            ReplyTest.ReplyId = evnt.Info.ReplyId;
            TestBase.EventHandlerWaiter.Set();
        }
        public void Handle(ReplyBodyChanged evnt)
        {
            TestBase.EventHandlerWaiter.Set();
        }
        public void Handle(ReplyReplied evnt)
        {
            ReplyTest.ReplyId = evnt.Info.ReplyId;
            TestBase.EventHandlerWaiter.Set();
        }
    }
}
