using System.Data.SqlClient;
using ECommon.Components;
using ECommon.Dapper;
using ENode.Eventing;
using ENode.Infrastructure.Exceptions;
using Forum.Commands.Accounts;
using Forum.Domain.Accounts;
using Forum.Infrastructure;

namespace Forum.ProcessManagers
{
    [Component(LifeStyle.Singleton)]
    public class RegistrationProcessManager :
        IEventHandler<RegistrationStartedEvent>,
        IEventHandler<RegistrationConfirmedEvent>,
        IEventHandler<AccountCreatedEvent>
    {
        public void Handle(IEventContext context, RegistrationStartedEvent evnt)
        {
            try
            {
                AddRegistration(evnt.AccountInfo);
                context.AddCommand(new ConfirmRegistrationCommand(context.ProcessId, evnt.AggregateRootId));
            }
            catch (DuplicateAccountNameException)
            {
                context.AddCommand(new CancelRegistrationCommand(context.ProcessId, evnt.AggregateRootId, ErrorCodes.RegistrationDuplicateAccount));
            }
        }
        public void Handle(IEventContext context, RegistrationConfirmedEvent evnt)
        {
            var registration = context.Get<Registration>(evnt.AggregateRootId);
            var command = new CreateAccountCommand(context.ProcessId, registration.AccountInfo.Name, registration.AccountInfo.Password);
            command.Items["RegistrationId"] = evnt.AggregateRootId;
            context.AddCommand(command);
        }
        public void Handle(IEventContext context, AccountCreatedEvent evnt)
        {
            var registrationId = context.Items["RegistrationId"];
            var command = new CompleteRegistrationCommand(context.ProcessId, registrationId);
            command.Items["AccountId"] = evnt.AggregateRootId;
            context.AddCommand(command);
        }

        private void AddRegistration(AccountInfo accountInfo)
        {
            using (var connection = GetConnection())
            {
                try
                {
                    connection.Insert(new
                    {
                        AccountName = accountInfo.Name,
                        AccountPassword = accountInfo.Password
                    }, Constants.RegistrationTable);
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627)
                    {
                        if (ex.Message.Contains(Constants.RegistrationTablePrimaryKeyName))
                        {
                            throw new DuplicateAccountNameException(accountInfo.Name, ex);
                        }
                    }
                    throw;
                }
            }
        }
        private SqlConnection GetConnection()
        {
            return new SqlConnection(ConfigSettings.ConnectionString);
        }
    }
}
