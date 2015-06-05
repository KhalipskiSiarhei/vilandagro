using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vilandagro.Core.Entities
{
    public partial class UnitOfPrice
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Version { get; set; }

        /// <summary>
        /// Navigation property to prices of products
        /// </summary>
        public virtual ICollection<ProductPrice> ProductPrices { get; set; }
    }
}