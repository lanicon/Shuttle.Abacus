﻿using System;

namespace Shuttle.Abacus.Events.Test.v1
{
    public class ArgumentRegistered
    {
        public Guid ArgumentId { get; set; }
        public string Value { get; set; }
    }
}