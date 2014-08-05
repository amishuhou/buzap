using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UsedParts.Domain.Converters;

namespace UsedParts.Domain
{
    public class Order
    {
        [JsonProperty(PropertyName = "ix")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "head")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "body")]
        public string Body { get; set; }

        [JsonProperty(PropertyName = "offers")]
        public int OffersCount { get; set; }

        [JsonProperty(PropertyName = "advDate")]
        [JsonConverter(typeof(DateConverter))]
        public DateTime Posted { get; set; }

        [JsonProperty(PropertyName = "imgs")]
        public IEnumerable<string> Images { get; set; }

        #region equality

        protected bool Equals(Order other)
        {
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Order) obj);
        }

        #endregion
    }
}
