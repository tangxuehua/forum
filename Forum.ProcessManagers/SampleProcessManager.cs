using ENode.Eventing;
using Forum.Events.Post;

namespace Forum.ProcessManagers
{
    /// <summary>一个Sample流程管理器；
    /// 目前论坛的业务比较简单，所以不需要流程管理器。流程管理器一般用于在一个业务场景涉及多个聚合根的修改的时候使用；
    /// 我们会把整个业务场景设计为一个流程（也是领域内的一个聚合根），然后流程管理器的职责就是响应事件，
    /// 然后发送命令给流程聚合根，然后由流程聚合根决定下一步该怎么走；
    /// </summary>
    public class SampleProcessManager : IEventHandler<PostCreated>
    {
        public void Handle(PostCreated evnt)
        {
            throw new System.NotImplementedException();
        }
    }
}
