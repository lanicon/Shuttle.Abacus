﻿using System;

namespace Shuttle.Abacus.Events.Formula.v1
{
    public class OperationAdded
    {
        public Guid Id { get; set; }
        public int SequenceNumber { get; set; }
        public string Operation { get; set; }
        public string ValueProvider { get; set; }
        public string Input { get; set; }
    }
}