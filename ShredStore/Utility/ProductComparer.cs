using System.Diagnostics.CodeAnalysis;

namespace ShredStore.Models.Utility
{
    public class ProductComparer : IEqualityComparer<ProductViewModel>
    {
        public bool Equals(ProductViewModel? x, ProductViewModel? y)
        {
            if (ReferenceEquals(x, y)) return true;

            if (x == null || y == null) return false;

            if (x.Id != y.Id) return false;

            return x.Id == y.Id;
        }

        public int GetHashCode([DisallowNull] ProductViewModel obj)
        {
            if (ReferenceEquals(obj, null)) return 0;

            int hashProductId = obj.Id.GetHashCode();

            return hashProductId;


        }
    }
}
