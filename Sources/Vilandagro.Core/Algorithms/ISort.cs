using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vilandagro.Core.Algorithms
{
    public interface ISort
    {
        T[] Sort<T>(T[] arrayToSort, bool order);

        string Sort(string toSort, bool order);
    }
}
