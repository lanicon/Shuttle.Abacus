using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Shuttle.Abacus.Infrastructure;

namespace Shuttle.Abacus.Shell.Core.Validation
{
    public class ControlValidatorProvider : IControlValidatorProvider
    {
        private readonly List<IControlValidator> validators = new List<IControlValidator>();

        public ControlValidatorProvider()
        {
            foreach (var validator in DependencyResolver.Resolver.ResolveAll<IControlValidator>())
            {
                Register(validator);
            }
        }

        public IControlValidatorProvider Register(IControlValidator validator)
        {
            if (GetFor(validator.HandlesType) != null)
            {
                return this;
            }

            validators.Add(validator);

            return this;
        }

        public IControlValidator GetFor<T>()
        {
            return GetFor(typeof (T));
        }

        public IControlValidator GetFor(Type type)
        {
            return validators.Find(validator => validator.HandlesType.Equals(type));
        }

        public IEnumerable<IControlValidator> Validators => new ReadOnlyCollection<IControlValidator>(validators);
    }
}
