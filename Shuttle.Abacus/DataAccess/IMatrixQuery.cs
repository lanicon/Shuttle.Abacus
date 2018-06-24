using System;
using System.Collections.Generic;
using System.Data;

namespace Shuttle.Abacus.DataAccess
{
    public interface IMatrixQuery
    {
        IEnumerable<DataRow> All();
        DataRow Get(Guid id);
        void Registered(Guid id, string name, string columnArgumentName, string rowArgumentName, string dataTypeName);
        void ConstraintAdded(Guid id, int sequenceNumber, string axis, string comparison, string value);
        void ElementAdded(Guid id, int column, int row, string value);
    }
}