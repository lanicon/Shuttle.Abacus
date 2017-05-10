using System;
using Shuttle.Abacus.Infrastructure;
using Shuttle.Abacus.Invariants.Values;
using Shuttle.Abacus.Localisation;
using Shuttle.Abacus.UI.Core.Presentation;
using Shuttle.Abacus.UI.Models;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Abacus.UI.UI.FormulaOperation
{
    public class FormulaOperationPresenter : Presenter<IFormulaOperationView, FormulaOperationPresenterModel>, IFormulaOperationPresenter
    {
        private readonly IValueTypeValidatorProvider valueTypeValidatorProvider;

        public FormulaOperationPresenter(IFormulaOperationView view, IValueTypeValidatorProvider valueTypeValidatorProvider)
            : base(view)
        {
            this.valueTypeValidatorProvider = valueTypeValidatorProvider;
            Text = "FormulaOperation Details";
            Image = Resources.Image_FormulaOperation;
        }

        public bool CanAddOperation()
        {
            if (!View.HasOperation)
            {
                View.ShowOperationTypeError();

                return false;
            }

            if (!View.HasValueSource)
            {
                View.ShowValueSourceError();

                return false;
            }

            switch (View.ValueSourceTypeModel.Type.ToLower())
            {
                case "selection":
                    {
                        if (!View.HasValueSelection)
                        {
                            View.ShowValueSelectionError("Please make a selection.");

                            return false;
                        }

                        break;
                    }
                case "fixed":
                    {
                        if (!View.HasValueSelection)
                        {
                            View.ShowValueSelectionError("Please enter a value.");

                            return false;
                        }

                        var result =
                            valueTypeValidatorProvider.Get(DecimalValueTypeValidator.TypeName)
                                .Validate(View.ValueSelectionValue);

                        if (!result.OK)
                        {
                            View.ShowValueSelectionError(result.ToString());

                            return false;
                        }

                        break;
                    }
            }

            return true;
        }

        public override void OnInitialize()
        {
            base.OnInitialize();

            if (Model == null)
            {
                throw new NullDependencyException("Model");
            }

            View.PopulateOperations(Model.OperationTypes);
            View.PopulateValueSources(Model.ValueSourceTypes);

            if (Model.FormulaOperations == null)
            {
                return;
            }

            foreach (var formulaOperation in Model.FormulaOperations)
            {
                View.AddOperation(formulaOperation.OperationType, formulaOperation.ValueSourceType, formulaOperation.ValueSelection, formulaOperation.Text);
            }
        }

        public void ValueSourceChanged()
        {
            var type = Enumeration.Cast<Enumeration.ValueSourceType>(View.ValueSourceTypeModel.Name);

            switch (type)
            {
                case Enumeration.ValueSourceType.ArgumentAnswer:
                    {
                        View.EnableValueSelection("Argument");
                        View.PopulateArguments(Model.Arguments);

                        break;
                    }
                case Enumeration.ValueSourceType.Decimal:
                    {
                        View.EnableValueEntry("Value");

                        break;
                    }
                case Enumeration.ValueSourceType.CalculationResult:
                case Enumeration.ValueSourceType.CalculationSubTotal:
                    {
                        throw new NotImplementedException();
                        View.EnableValueSelection("Calculation");
                        //View.PopulatePrecedingCalculations(Model.PrecedingCalculations);

                        break;
                    }
                case Enumeration.ValueSourceType.DecimalTable:
                    {
                        throw new NotImplementedException();
                        View.EnableValueSelection("Decimal Table");
                        //View.PopulateDecimalTables(Model.DecimalTables);

                        break;
                    }
                case Enumeration.ValueSourceType.MethodResult:
                    {
                        throw new NotImplementedException();
                        View.EnableValueSelection("FormulaOperation");
                        //View.PopulateFormulaOperations(Model.Methods);

                        break;
                    }
                case Enumeration.ValueSourceType.CalculationTotal:
                //case Enumeration.ValueSourceType.FormulaOperationTotal:
                    {
                        View.DisableValues();

                        break;
                    }
            }
        }

    }
}