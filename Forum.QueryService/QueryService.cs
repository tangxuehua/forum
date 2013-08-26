using System;
using System.Collections.Generic;
using ENode.Infrastructure;
using ENode.Infrastructure.Dapper;
using ENode.Infrastructure.Sql;

namespace Forum.Query
{
    [Component]
    public class QueryService
    {
        protected ISqlQueryDbConnectionFactory ConnectionFactory { get; private set; }

        public QueryService(ISqlQueryDbConnectionFactory connectionFactory)
        {
            ConnectionFactory = connectionFactory;
        }

        IEnumerable<dynamic> QueryThreads(Guid sectionId)
        {
            return ConnectionFactory.CreateConnection().TryExecute(connection => connection.Query(new { Section = sectionId }, "tb_Thread"));
        }
    }
}
