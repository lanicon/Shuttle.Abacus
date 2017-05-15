using System;
using System.Collections.Generic;
using Shuttle.Abacus.Messages.v1.TransferObjects;

namespace Shuttle.Abacus.Messages.v1
{
    public class ChangeTestCommand 
    {
        public ChangeTestCommand()
        {
            ArgumentAnswers = new List<ArgumentAnswer>();
        }

        public Guid MethodTestId { get; set; }
        public Guid MethodId { get; set; }
        public string Description { get; set; }
        public string ExpectedResult { get; set; }
        public List<ArgumentAnswer> ArgumentAnswers { get; private set; }
        public string ExpectedResultType { get; set; }
        public string Comparison { get; set; }
    }
}
