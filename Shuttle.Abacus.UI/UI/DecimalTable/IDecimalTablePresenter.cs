using System.Collections.Generic;
using System.Data;
using Shuttle.Abacus.UI.Core.Presentation;
using Shuttle.Abacus.UI.Models;

namespace Shuttle.Abacus.UI.UI.DecimalTable
{
    public interface IDecimalTablePresenter : IPresenter
    {
        IEnumerable<ConstraintTypeModel> ConstraintTypes { get; }
        void DecimalTableNameExited();
        void RowArgumentChanged();
        void ColumnArgumentChanged();
        bool IsDecimal(string value);
        bool IsValidAnswer(ArgumentModel model, object value);
        void ShowInvalidDecimalTableMessage();
        IEnumerable<string> ColumnAnswers();
        IEnumerable<string> RowAnswers();
    }
}
