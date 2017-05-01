namespace Shuttle.Abacus.Domain
{
    public class SubtractionOperationFactory : IOperationFactory
    {
        public Operation Create(IValueSource valueSource)
        {
            return new SubtractionOperation(valueSource);
        }

        public string Name
        {
            get { return "Subtraction"; }
        }

        public string Text
        {
            get { return "Subtraction"; }
        }
    }
}
