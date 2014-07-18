using ECommon.Components;
using ENode.Domain;
using Forum.Domain.Accounts;
using Forum.Domain.Posts;
using Forum.Domain.Replies;
using Forum.Domain.Sections;
using Forum.Infrastructure;

namespace Forum.Domain
{
    [Component(LifeStyle.Singleton)]
    public class AggregateRootFactory
    {
        private readonly IIdentityGenerator _identityGenerator;
        private readonly IRepository _repository;
        private readonly IAccountIndexStore _accountIndexStore;

        public AggregateRootFactory(IIdentityGenerator identityGenerator, IRepository repository, IAccountIndexStore accountIndexStore)
        {
            _identityGenerator = identityGenerator;
            _repository = repository;
            _accountIndexStore = accountIndexStore;
        }

        public Account CreateAccount(string accountIndexId, string name, string password)
        {
            var account = new Account(_identityGenerator.GetNextIdentity(), new AccountInfo(name, password));
            var accountIndex = _accountIndexStore.FindByAccountName(name);
            if (accountIndex == null)
            {
                _accountIndexStore.Add(new AccountIndex(accountIndexId, account.Id, name));
            }
            else if (accountIndex.IndexId != accountIndexId)
            {
                throw new DuplicateAccountException(name);
            }
            return account;
        }
        public Section CreateSection(string name)
        {
            return new Section(_identityGenerator.GetNextIdentity(), name);
        }
        public Post CreatePost(string subject, string body, string sectionId, string authorId)
        {
            return new Post(_identityGenerator.GetNextIdentity(), subject, body, sectionId, authorId);
        }
        public Reply CreateReply(string postId, string parentId, string authorId, string body)
        {
            Reply parent = null;
            if (!string.IsNullOrEmpty(parentId))
            {
                parent = _repository.Get<Reply>(parentId);
            }
            return new Reply(_identityGenerator.GetNextIdentity(), postId, parent, authorId, body);
        }
    }
}
