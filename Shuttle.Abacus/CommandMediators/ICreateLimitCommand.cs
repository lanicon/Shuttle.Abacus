using System;

namespace Shuttle.Abacus
{
    public interface ICreateLimitCommand
    {
        string OwnerName { get; set; }
        Guid OwnerId { get; set; }
        string Name { get; set; }
        string Type { get; set; }
    }
}