using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vilandagro.Core.Entities
{
    public class ProductPrice
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        /// <summary>
        /// Foreign key to the UnitOfPrice
        /// </summary>
        public int UnitOfPriceId { get; set; }

        public virtual Product Product { get; set; }

        /// <summary>
        /// Navigation property to UnitOfPrice
        /// </summary>
        public virtual UnitOfPrice UnitOfPrice { get; set; }
    }
}