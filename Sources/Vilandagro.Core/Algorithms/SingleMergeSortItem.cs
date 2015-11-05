using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vilandagro.Core.Algorithms
{
    public class SingleMergeSortItem<T> : MergeSortItem<T>
    {
        public T Item { get; set; }
    }
}
