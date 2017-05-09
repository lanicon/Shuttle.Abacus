using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Abacus.Domain
{
    public class Formula
    {
        private readonly List<FormulaConstraint> _constraints = new List<FormulaConstraint>();
        private readonly List<FormulaOperation> _operations = new List<FormulaOperation>();

        public Formula(Guid id, string name, string executionType)
        {
            Id = id;
            Name = name;
            ExecutionType = executionType;
        }

        public IEnumerable<FormulaOperation> Operations => new ReadOnlyCollection<FormulaOperation>(_operations);

        public Guid Id { get; }
        public string Name { get; }
        public string ExecutionType { get; }
        public string MaximumFormulaName { get; private set; }
        public string MinimumFormulaName { get; private set; }

        public Formula WithMaximum(string formulaName)
        {
            Guard.AgainstNullOrEmptyString(formulaName, "formulaName");

            MaximumFormulaName = formulaName;

            return this;
        }

        public Formula WithMinimum(string formulaName)
        {
            Guard.AgainstNullOrEmptyString(formulaName, "formulaName");

            MinimumFormulaName = formulaName;

            return this;
        }

        public IEnumerable<FormulaConstraint> Constraints => new ReadOnlyCollection<FormulaConstraint>(_constraints);

        public void AddConstraint(FormulaConstraint item)
        {
            Guard.AgainstNull(item, "item");

            _constraints.Add(item);
        }

        public string OwnerName => "Formula";
        public bool HasOperations => _operations.Count > 0;

        //public bool IsSatisfiedBy(IMethodContext collectionMethodContext)
        //{
        //    return
        //        OperationsSatisfied(collectionMethodContext)
        //        &&
        //        ConstraintSatisfied(collectionMethodContext);
        //}

        //public IEnumerable<Guid> RequiredCalculationIds()
        //{
        //    var result = new List<Guid>();

        //    _operations.ForEach(operation =>
        //    {
        //        var source = operation.ValueSource as ICalculationValueSource;

        //        if (source != null)
        //        {
        //            var id = new Guid(source.ValueSelection);

        //            if (!result.Contains(id))
        //            {
        //                result.Add(id);
        //            }
        //        }
        //    });

        //    return result;
        //}

        //private bool ConstraintSatisfied(IMethodContext collectionContext)
        //{
        //    foreach (var constraint in constraints)
        //    {
        //        if (!constraint.IsSatisfiedBy(collectionContext))
        //        {
        //            return false;
        //        }
        //    }

        //    return true;
        //}

        //private bool OperationsSatisfied(IMethodContext collectionContext)
        //{
        //    var result = true;

        //    //foreach (var operation in _operations)
        //    //{
        //    //    if (!operation.IsSatisfiedBy(collectionContext))
        //    //    {
        //    //        result = false;
        //    //    }
        //    //}

        //    return result;
        //}

        public Formula AddOperation(FormulaOperation operation)
        {
            _operations.Add(operation);

            return this;
        }

        //public decimal Execute(IMethodContext methodContext, IFormulaCalculationContext calculationContext)
        //{
        //    Guard.AgainstNull(calculationContext, "context");

        //    throw new NotImplementedException();

        //    //if (methodContext.LoggerEnabled)
        //    //{
        //    //    methodContext.Log("Executing formula:");

        //    //    if (constraints.Count > 0)
        //    //    {
        //    //        constraints.ForEach(
        //    //            constraint => methodContext.Log("\t{0}", constraint.Description()));
        //    //    }
        //    //    else
        //    //    {
        //    //        methodContext.Log("\t(no contraints)");
        //    //    }

        //    //    methodContext.Log();
        //    //}

        //    //calculationContext.ZeroFormulaTotal();

        //    //foreach (var operation in _operations)
        //    //{
        //    //    var operand = operation.Operand(methodContext, calculationContext);

        //    //    if (methodContext.LoggerEnabled)
        //    //    {
        //    //        methodContext.Log("{0}\t{1}", operation.Symbol,
        //    //            operation.ValueSource.Description(operand, methodContext));
        //    //    }

        //    //    calculationContext.SetFormulaTotal(operation.Execute(calculationContext.FormulaTotal, operand));

        //    //    if (!methodContext.OK)
        //    //    {
        //    //        return 0;
        //    //    }
        //    //}

        //    //if (methodContext.LoggerEnabled)
        //    //{
        //    //    methodContext.Log();
        //    //}

        //    //return calculationContext.FormulaTotal;
        //}

        public Formula Copy()
        {
            throw new NotImplementedException();
            //var result = new Formula();

            //_constraints.ForEach(constraint => result.AddConstraint(constraint));
            //_operations.ForEach(operation => result.AddOperation(operation));

            //return result;
        }
    }
}