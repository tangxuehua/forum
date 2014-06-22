using System;
using ENode.Domain;
using Forum.Infrastructure;
using Forum.Infrastructure.Exceptions;

namespace Forum.Domain.Accounts
{
    [Serializable]
    public class Registration : AggregateRoot<string>
    {
        public AccountInfo AccountInfo { get; private set; }
        public RegistrationStatus Status { get; private set; }

        public Registration(string id, AccountInfo accountInfo)
            : base(id)
        {
            Assert.IsNotNull("账号信息", accountInfo);
            Assert.IsNotNullOrWhiteSpace("用户名", accountInfo.Name);
            Assert.IsNotNullOrWhiteSpace("密码", accountInfo.Password);
            RaiseEvent(new RegistrationStartedEvent(Id, accountInfo));
        }

        public void Confirm()
        {
            if (Status != RegistrationStatus.NotConfirmed)
            {
                throw new InvalidRegistrationStatusException("Invalid registration status: {0}, only not confirmed registration can be confirm.", Status);
            }
            RaiseEvent(new RegistrationConfirmedEvent(Id));
        }
        public void Complete()
        {
            if (Status != RegistrationStatus.Confirmed)
            {
                throw new InvalidRegistrationStatusException("Invalid registration status: {0}, only confirmed registration can be complete.", Status);
            }
            RaiseEvent(new RegistrationCompletedEvent(Id));
        }
        public void Cancel(int errorCode)
        {
            if (Status != RegistrationStatus.NotConfirmed)
            {
                throw new InvalidRegistrationStatusException("Invalid registration status: {0}, only not confirmed registration can be cancel.", Status);
            }
            RaiseEvent(new RegistrationCanceledEvent(Id, errorCode));
        }

        private void Handle(RegistrationStartedEvent evnt)
        {
            Id = evnt.AggregateRootId;
            AccountInfo = evnt.AccountInfo;
            Status = RegistrationStatus.NotConfirmed;
        }
        private void Handle(RegistrationConfirmedEvent evnt)
        {
            Status = RegistrationStatus.Confirmed;
        }
        private void Handle(RegistrationCompletedEvent evnt)
        {
            Status = RegistrationStatus.Completed;
        }
        private void Handle(RegistrationCanceledEvent evnt)
        {
            Status = RegistrationStatus.Canceled;
        }
    }
    public enum RegistrationStatus
    {
        NotConfirmed = 1,
        Confirmed,
        Completed,
        Canceled,
    }
}