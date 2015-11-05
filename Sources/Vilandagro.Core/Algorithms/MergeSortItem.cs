using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vilandagro.Core.Algorithms
{
    public class MergeSortItem<T>
    {
        public MergeSortItem<T> Left { get; set; }

        public MergeSortItem<T> Right { get; set; }
    }
}