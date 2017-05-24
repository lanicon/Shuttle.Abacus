﻿using System;
using System.Collections.Generic;
using NUnit.Framework;
using Shuttle.Abacus.Domain;

namespace Shuttle.Abacus.Tests
{
    [TestFixture]
    public class ExecutionEngineFixture
    {
        [Test]
        public void Should_be_able_to_perform_simple_addition()
        {
            var formulas = new List<Formula>();

            var formula = new Formula(Guid.NewGuid());

            formula.Register("Test");

            formula.AddOperation(1, "Addition", "Argument", "Operand1");
            formula.AddOperation(1, "Addition", "Argument", "Operand2");

            formulas.Add(formula);

            var operand1 = new Argument(Guid.NewGuid());

            operand1.Register("Operand1", "Integer");

            var operand2 = new Argument(Guid.NewGuid());

            operand2.Register("Operand2", "Decimal");

            var arguments = new List<Argument> {operand1, operand2};

            var context = new ExecutionService(new ConstraintComparison(new ValueTypeFactory()),  formulas, arguments);

            context.Execute("Test", new List<ArgumentValue>
            {
                new ArgumentValue {Name = "Operand1", Value = "2"},
                new ArgumentValue {Name = "Operand2", Value = "3"}
            });
        }
    }
}