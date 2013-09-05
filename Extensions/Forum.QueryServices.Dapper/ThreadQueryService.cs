using System.Collections.Generic;
using ENode.Infrastructure;
using ENode.Infrastructure.Dapper;
using ENode.Infrastructure.Sql;
using Forum.Application.DTOs;
using Forum.Application.QueryServices;

namespace Forum.QueryServices.Dapper
{
    [Component]
    public class ThreadQueryService : BaseQueryService, IThreadQueryService
    {
        public ThreadQueryService(ISqlQueryDbConnectionFactory connectionFactory) : base(connectionFactory) { }

        public IEnumerable<Thread> QueryThreads(ThreadQueryOption option)
        {
            return ConnectionFactory.CreateConnection().TryExecute(connection => connection.Query<Thread>(new { Section = option.SectionId }, "tb_Thread"));
        }
    }
}
