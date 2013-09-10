using ENode.Eventing;
using Forum.Events.Post;

namespace Forum.CommandRouters
{
    /// <summary>Command Router用于实现业务流程；响应事件，然后发送命令；
    /// </summary>
    public class SampleCommandRouter : IEventHandler<PostCreated>
    {
        public void Handle(PostCreated evnt)
        {
            throw new System.NotImplementedException();
        }
    }
}
