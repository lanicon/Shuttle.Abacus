using Shuttle.Abacus.Domain;

namespace Shuttle.Abacus.ApplicationService
{
    public class CreateLimitTask : ICreateLimitTask
    {
        private readonly ILimitRepository repository;

        public CreateLimitTask(ILimitRepository repository)
        {
            this.repository = repository;
        }

        public void Execute(OwnerModel item)
        {
            repository.Add((ILimitOwner) item.Owner, (Limit) item.Entity);
        }
    }
}