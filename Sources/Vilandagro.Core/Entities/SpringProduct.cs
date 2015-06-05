using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vilandagro.Core.Entities
{
    public partial class SpringProduct : Product
    {
        public decimal Diametr { get; set; }

        public decimal Weight { get; set; }
    }
}