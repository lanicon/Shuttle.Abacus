using System;
using System.Collections.Generic;

namespace Shuttle.Abacus
{
    public interface ICreateMethodTestCommand
    {
        Guid MethodTestId { get; set; }
        Guid MethodId { get; set; }
        string Description { get; set; }
        decimal ExpectedResult { get; set; }
        List<ArgumentAnswerDTO> ArgumentAnswers { get; }
    }
}