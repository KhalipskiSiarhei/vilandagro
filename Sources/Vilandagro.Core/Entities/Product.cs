using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vilandagro.Core.Entities
{
    public partial class Product : IEntity, IVersion
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public int Version { get; set; }

        /// <summary>
        /// Navigation property to the prices of products
        /// </summary>
        public virtual ICollection<ProductPrice> ProductPrices { get; set; }
    }
}