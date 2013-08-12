using System;
using System.Collections.Generic;
using Dapper;
using ENode.Infrastructure;

namespace Forum.Query {
    [Component]
    public class QueryService {
        protected ISqlQueryDbConnectionFactory ConnectionFactory { get; private set; }

        public QueryService(ISqlQueryDbConnectionFactory connectionFactory) {
            ConnectionFactory = connectionFactory;
        }

        IEnumerable<dynamic> QueryThreads(Guid sectionId) {
            return ConnectionFactory.CreateConnection().TryExecute((connection) => {
                return connection.Query(new { Section = sectionId }, "tb_Thread");
            });
        }
    }
}
