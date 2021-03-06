using System;
using System.Collections.Generic;
using System.Data;
using Shuttle.Abacus.Events.Argument.v1;
using Shuttle.Recall;

namespace Shuttle.Abacus.DataAccess
{
    public interface IArgumentQuery
    {
        IEnumerable<DataRow> Search(ArgumentSearchSpecification specification);
        DataRow Get(Guid id);
        IEnumerable<DataRow> Values(Guid id);
        void Registered(PrimitiveEvent primitiveEvent, Registered registered);
        void Removed(PrimitiveEvent primitiveEvent, Removed removed);
        void Renamed(PrimitiveEvent primitiveEvent, Renamed renamed);
        void ValueAdded(PrimitiveEvent primitiveEvent, ValueAdded valueAdded);
        void ValueRemoved(PrimitiveEvent primitiveEvent, ValueRemoved valueRemoved);
    }
}