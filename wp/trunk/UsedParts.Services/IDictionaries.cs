using System.Collections.Generic;
using UsedParts.Domain;

namespace UsedParts.Services
{
    public interface IDictionaries
    {
        IEnumerable<Item> Availabilities { get; }
        IEnumerable<Item> Warranties { get; }
        IEnumerable<Item> Deliveries { get; }
        IEnumerable<Item> Conditions { get; }
        IEnumerable<Item> OrganizationTypes { get; }

        Manufacturer AllManufacturer { get; }
    }
}
