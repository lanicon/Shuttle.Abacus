﻿namespace Shuttle.Abacus.Messages.v1.TransferObjects
{
    public class FormulaConstraint
    {
        public int SequenceNumber { get; set; }
        public string ArgumentName { get; set; }
        public string Comparison { get; set; }
        public string Value { get; set; }
    }
}