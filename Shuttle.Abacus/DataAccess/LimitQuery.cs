using System;
using System.Collections.Generic;
using System.Data;
using Shuttle.Core.Data;

namespace Shuttle.Abacus.DataAccess
{
    public class LimitQuery :ILimitQuery
    {
        private readonly IDatabaseGateway _databaseGateway;
        private readonly ILimitQueryFactory _limitQueryFactory;

        public LimitQuery(IDatabaseGateway databaseGateway, ILimitQueryFactory limitQueryFactory)
        {
            _databaseGateway = databaseGateway;
            _limitQueryFactory = limitQueryFactory;
        }

        public IEnumerable<DataRow> AllForOwner(Guid ownerId)
        {
            return _databaseGateway.GetRowsUsing(_limitQueryFactory.AllForOwner(ownerId));
        }

        public DataRow Get(Guid limitId)
        {
            return _databaseGateway.GetSingleRowUsing(_limitQueryFactory.Get(limitId));
        }
    }
}