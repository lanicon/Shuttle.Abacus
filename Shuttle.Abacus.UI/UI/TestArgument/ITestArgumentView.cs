﻿using System.Collections.Generic;
using Shuttle.Abacus.Shell.Core.Presentation;
using Shuttle.Abacus.Shell.Models;

namespace Shuttle.Abacus.Shell.UI.TestArgument
{
    public interface ITestArgumentView : IView
    {
        ArgumentModel ArgumentModel { get; }
        string ArgumentValue { get; set; }
        bool HasArgumentValue { get; }
        bool HasArgument { get; }
        void PopulateArguments(IEnumerable<ArgumentModel> items);
        void PopulateArgumentValues(IEnumerable<string> values);
        void ShowArgumentValueError(string message);
        void ShowArgumentError();
    }
}