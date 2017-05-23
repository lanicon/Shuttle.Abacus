﻿using System;
using System.Collections.Generic;
using Shuttle.Abacus.Domain;
using Shuttle.Abacus.Invariants;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Abacus
{
    public class ExecutionService
    {
        private readonly Dictionary<string, Formula> _formulas = new Dictionary<string, Formula>();
        private readonly Dictionary<string, Argument> _arguments = new Dictionary<string, Argument>();

        public ExecutionService(IEnumerable<Formula> formulas, IEnumerable<Argument> arguments)
        {
            Guard.AgainstNull(formulas, "formulas");
            Guard.AgainstNull(arguments, "arguments");

            foreach (var formula in formulas)
            {
                if (_formulas.ContainsKey(formula.Name))
                {
                    throw new DuplicateEntryException(string.Format("There is already a formula with name '{0}' registered.", formula.Name));
                }

                _formulas.Add(formula.Name, formula);
            }

            foreach (var argument in arguments)
            {
                if (_arguments.ContainsKey(argument.Name))
                {
                    throw new DuplicateEntryException(string.Format("There is already an argument with name '{0}' registered.", argument.Name));
                }

                _arguments.Add(argument.Name, argument);
            }
        }

        // perhaps something like this
        //public ExecutionEngine AddValueSource(IValueSource valueSource)
        //{
        //}

        public ExecutionContext Execute(string formulaName, IEnumerable<ArgumentValue> values)
        {
            Guard.AgainstNullOrEmptyString(formulaName, "formulaName");
            Guard.AgainstNull(values, "values");

            var context = new ExecutionContext(_arguments.Values, values);

            try
            {
                Execute(formulaName, context, context.FormulaContext());
            }
            catch (Exception ex)
            {
                context.Exception(ex);
            }

            return context;
        }

        private decimal Execute(string formulaName, ExecutionContext executionContext, FormulaContext formulaContext)
        {
            var formula = GetFormula(formulaName);

            if (!formula.IsSatisfiedBy(executionContext))
            {
                return 0;
            }

            foreach (var operation in formula.Operations)
            {
                decimal value = 0;

                switch (operation.ValueSource.ToLower())
                {
                    case "constant":
                        {
                            value = Convert.ToDecimal(operation.ValueSelection);

                            break;
                        }
                    case "argument":
                        {
                            value = Convert.ToDecimal(executionContext.GetArgumentValue(operation.ValueSelection));

                            break;
                        }
                    case "formula":
                        {
                            value = Execute(operation.ValueSelection, executionContext, executionContext.FormulaContext());

                            break;
                        }
                }

                operation.Perform(formulaContext, value);
            }

            return formulaContext.Result;
        }

        private Formula GetFormula(string formulaName)
        {
            if (!_formulas.ContainsKey(formulaName))
            {
                throw new InvalidOperationException(string.Format("There is not formula with name '{0}'.", formulaName));
            }

            return _formulas[formulaName];
        }

        private Argument GetArgument(string name)
        {
            if (!_arguments.ContainsKey(name))
            {
                throw new InvalidOperationException(string.Format("There is no argument with name '{0}'.", name));
            }

            return _arguments[name];
        }


    }
}