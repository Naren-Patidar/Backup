using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities
{
    [Serializable]
    public abstract class ComparableEntity<T> : IEquatable<T>
    {
        public bool Equals(T other)
        {
            return this.AreInstancesEqual(other);
        }

        public override bool Equals(object obj)
        {
            // Again just optimization
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            // Actually check the type, should not throw exception from Equals override
            if (obj.GetType() != this.GetType()) return false;

            // Call the implementation from IEquatable
            return this.Equals((T)obj);
        }

        public override int GetHashCode()
        {
            return 0;
        }

        internal abstract bool AreInstancesEqual(T target);
    }
}
