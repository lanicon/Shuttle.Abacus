using System;
using System.Collections.Generic;
using System.Data;
using Shuttle.Abacus.DTO;

namespace Shuttle.Abacus.DataAccess
{
    public interface ICalculationQuery
    {
        IEnumerable<DataRow> AllForOwner(Guid ownerId);
        DataTable AllBeforeCalculation(Guid methodId, Guid calculationId);
        DataRow Get(Guid id);
        DataTable AllForMethod(Guid methodId);
        IEnumerable<CalculationDTO> DTOsBeforeCalculation(Guid methodId, Guid calculationId);
        IEnumerable<CalculationDTO> DTOsForMethod(Guid methodId);
        DataTable AllForMethod(Guid methodId, Guid grabberCalculationId);
        IEnumerable<DataRow> GraphNodeArguments(Guid calculationId);
    }
}