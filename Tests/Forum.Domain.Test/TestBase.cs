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
using Forum.Domain.Accounts.Events;
using Forum.Domain.Posts.Events;
using Forum.Domain.Replies.Events;
using Forum.Domain.Sections.Events;
using NUnit.Framework;

namespace Forum.Domain.Test
{
    [TestFixture]
    public class TestBase
    {
        public static ManualResetEvent EventHandlerWaiter;
        public static ManualResetEvent TestThreadWaiter;
        protected static Random Random = new Random();
        protected ICommandService CommandService;
        protected IMemoryCache MemoryCache;
        private static bool _initialized;

        [TestFixtureSetUp]
        public void SetUp()
        {
            if (!_initialized)
            {
                var assemblies = new[]
                {
                    Assembly.Load("Forum.Domain"),
                    Assembly.Load("Forum.Application"),
                    Assembly.Load("Forum.Domain.Test")
                };
                Configuration
                    .Create()
                    .UseAutofac()
                    .RegisterFrameworkComponents()
                    .RegisterBusinessComponents(assemblies)
                    .UseLog4Net()
                    .UseJsonNet()
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
            return DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Ticks + Random.Next(100);
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
        void IEventHandler<PostReplied>.Handle(PostReplied evnt)
        {
            ReplyTest.ReplyId = evnt.ReplyId;
            TestBase.EventHandlerWaiter.Set();
        }
        void IEventHandler<ReplyBodyChanged>.Handle(ReplyBodyChanged evnt)
        {
            TestBase.EventHandlerWaiter.Set();
        }
    }
}
