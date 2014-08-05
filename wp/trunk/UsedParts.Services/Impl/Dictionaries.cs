using System.Collections.Generic;
using UsedParts.Domain;
using UsedParts.Localization;

namespace UsedParts.Services.Impl
{
    public class Dictionaries : IDictionaries
    {
        public IEnumerable<Item> Availabilities { get; private set; }
        public IEnumerable<Item> Warranties { get; private set; }
        public IEnumerable<Item> Deliveries { get; private set; }
        public IEnumerable<Item> Conditions { get; private set; }
        public IEnumerable<Item> OrganizationTypes { get; private set; }

        public Manufacturer AllManufacturer { get; private set; }

        public Dictionaries()
        {
            Availabilities = new List<Item>
                                    {
                                        new Item(0, Resources.Empty),
                                        new Item(1, Resources.Now),
                                        new Item(2, Resources.From1To3),
                                        new Item(3, Resources.From4To7),
                                        new Item(4, Resources.MoreThan7)
                                    };
            Warranties = new List<Item>
                                    {
                                        new Item(0, Resources.Empty),
                                        new Item(1, Resources.No),
                                        new Item(2, Resources.D3),
                                        new Item(3, Resources.D7),
                                        new Item(4, Resources.Other)
                                    };
            Deliveries = new List<Item>
                                    {
                                        new Item(0, Resources.Empty),
                                        new Item(1, Resources.No),
                                        new Item(2, Resources.Possible),
                                        new Item(3, Resources.Free),
                                        new Item(4, Resources.NonFree)
                                    };
            Conditions = new List<Item>
                                    {
                                        new Item(0, Resources.Empty),
                                        new Item(1, Resources.VeryBad),
                                        new Item(2, Resources.Bad),
                                        new Item(3, Resources.Normal),
                                        new Item(4, Resources.Good),
                                        new Item(5, Resources.Mint)
                                    };

            OrganizationTypes = new List<Item>
                                    {
                                        new Item(1, Resources.Private), 
                                        new Item(2, Resources.Ooo), 
                                        new Item(3, Resources.Ip)
                                    };

            AllManufacturer = new Manufacturer {Name = Resources.AllManufacturers};
        }
    }
}
