﻿using System;

namespace Shuttle.Abacus.Domain
{
    public class FormulaConstraint
    {
        public FormulaConstraint(int sequenceNumber, string argumentName, string comparison, string value)
        {
            SequenceNumber = sequenceNumber;
            ArgumentName = argumentName;
            Comparison = comparison;
            Value = value;
        }

        public int SequenceNumber { get; }
        public string ArgumentName { get; }
        public string Comparison { get; }
        public string Value { get; }
    }
}