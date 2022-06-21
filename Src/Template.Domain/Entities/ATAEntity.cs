using ATABit.Model.Entities.Contracts;
using Bit.Model.Contracts;

namespace Template.Domain.Entities;

public abstract class ATAEntity : IATAMiniEntity, IArchivableEntity
{
    public int Id { get; set; }

    public bool IsArchived { get; set; }
}