using System;
using System.Collections.Generic;
using System.Data;
using Shuttle.Abacus.Domain;
using Shuttle.Abacus.DTO;

namespace Shuttle.Abacus.DataAccess
{
    public interface IFormulaQuery
    {
        IEnumerable<DataRow> AllForOwner(Guid ownerId);
        IEnumerable<DataRow> Operations(Guid formulaId);
        DataRow Get(Guid id);
        void PopulateOwner(IFormulaOwner owner);
    }
}
