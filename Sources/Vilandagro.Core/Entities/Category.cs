using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vilandagro.Core.Entities
{
    public partial class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public int Version { get; set; }

        /// <summary>
        /// Navigation property to products
        /// </summary>
        public virtual ICollection<Product> Products { get; set; }
    }
}