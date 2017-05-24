using Shuttle.Abacus.DataAccess;
using Shuttle.Abacus.Infrastructure;
using Shuttle.Abacus.Invariants.Values;
using Shuttle.Abacus.Localisation;
using Shuttle.Abacus.Shell.Core.Presentation;
using Shuttle.Abacus.Shell.Models;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Abacus.Shell.UI.Test.TestArgument
{
    public class TestArgumentPresenter : Presenter<ITestArgumentView, TestArgumentModel>, ITestArgumentPresenter
    {
        private readonly IArgumentQuery _argumentQuery;
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IValueTypeValidatorProvider _valueTypeValidatorProvider;

        public TestArgumentPresenter(IDatabaseContextFactory databaseContextFactory, IArgumentQuery argumentQuery,
            ITestArgumentView view, IValueTypeValidatorProvider valueTypeValidatorProvider)
            : base(view)
        {
            Guard.AgainstNull(databaseContextFactory, "databaseContextFactory");
            Guard.AgainstNull(argumentQuery, "argumentQuery");
            Guard.AgainstNull(valueTypeValidatorProvider, "valueTypeValidatorProvider");

            _databaseContextFactory = databaseContextFactory;
            _argumentQuery = argumentQuery;
            _valueTypeValidatorProvider = valueTypeValidatorProvider;

            Text = "Test Details";

            Image = Resources.Image_Test;
        }

        public void ArgumentChanged()
        {
            var model = View.ArgumentModel;

            using (_databaseContextFactory.Create())
            {
                View.PopulateArgumentValues(
                    _argumentQuery.GetValues(model.Id).Map(row => ArgumentColumns.ValueColumns.Value.MapFrom(row)));
            }
        }

        public bool ArgumentAnswerOK()
        {
            if (!View.HasArgument)
            {
                View.ShowArgumentError();

                return false;
            }

            if (!View.HasArgumentValue)
            {
                View.ShowArgumentValueError("Please enter a value.");

                return false;
            }

            if (View.ArgumentModel.IsNumber())
            {
                var result =
                    _valueTypeValidatorProvider.Get(View.ArgumentModel.ValueType)
                        .Validate(View.ArgumentName);

                if (!result.OK)
                {
                    View.ShowArgumentValueError(result.ToString());

                    return false;
                }
            }

            return true;
        }

        public override void OnInitialize()
        {
            base.OnInitialize();

            View.PopulateArguments(Model.Arguments);
        }
    }
}