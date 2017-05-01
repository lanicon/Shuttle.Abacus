using System;
using System.Collections.Generic;
using System.Data;
using Shuttle.Abacus.Domain;
using Shuttle.Abacus.DTO;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Abacus.DataAccess
{
    public class FormulaQuery :IFormulaQuery
    {
        private readonly IDatabaseGateway _databaseGateway;
        private readonly IFormulaQueryFactory _formulaQueryFactory;
        private readonly IDataTableRepository<OperationDTO> _operationRepository;

        public FormulaQuery(IDatabaseGateway databaseGateway, IFormulaQueryFactory formulaQueryFactory, IDataTableRepository<OperationDTO> operationRepository)
        {
            _databaseGateway = databaseGateway;
            _formulaQueryFactory = formulaQueryFactory;
            _operationRepository = operationRepository;
        }

        public IEnumerable<DataRow> AllForOwner(Guid ownerId)
        {
            return _databaseGateway.GetRowsUsing(_formulaQueryFactory.AllForOwner(ownerId));
        }

        public IEnumerable<OperationDTO> OperationDTOs(Guid formulaId)
        {
                return _operationRepository.FetchAllUsing(_formulaQueryFactory.GetOperations(formulaId));
        }

        public IEnumerable<DataRow> Operations(Guid formulaId)
        {
            return _databaseGateway.GetRowsUsing(_formulaQueryFactory.GetOperations(formulaId));
        }

        public DataRow Get(Guid id)
        {
            return _databaseGateway.GetSingleRowUsing(_formulaQueryFactory.Get(id));
        }

        public void PopulateOwner(IFormulaOwner owner)
        {
            Guard.AgainstNull(owner, "owner");

            foreach (var row in _databaseGateway.GetRowsUsing(_formulaQueryFactory.AllForOwner(owner.Id)))
            {
                owner.AddFormula(
                    new OwnedFormula(
                        FormulaColumns.Id.MapFrom(row),
                        FormulaColumns.SequenceNumber.MapFrom(row),
                        FormulaColumns.Description.MapFrom(row)));
            }
        }
    }
}
