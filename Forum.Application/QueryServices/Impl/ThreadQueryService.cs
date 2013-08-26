using System;
using System.Collections.Generic;
using ENode.Infrastructure;
using ENode.Infrastructure.Dapper;
using ENode.Infrastructure.Sql;

namespace Forum.Application.QueryServices.Impl
{
    [Component]
    public class ThreadQueryService : BaseQueryService, IThreadQueryService
    {
        public ThreadQueryService(ISqlQueryDbConnectionFactory connectionFactory) : base(connectionFactory) { }

        public IEnumerable<dynamic> QueryThreads(Guid sectionId)
        {
            return ConnectionFactory.CreateConnection().TryExecute(connection => connection.Query(new { Section = sectionId }, "tb_Thread"));
        }
    }
}
