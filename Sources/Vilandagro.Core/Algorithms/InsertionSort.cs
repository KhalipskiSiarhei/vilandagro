using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vilandagro.Core.Algorithms
{
    public class InsertionSort : SortBase
    {
        public override T[] Sort<T>(T[] arrayToSort, bool order)
        {
            if (arrayToSort == null)
            {
                throw new NullReferenceException();
            }

            var sortedArray = new T[arrayToSort.Length];
            var comparer = Comparer<T>.Default;

            for (int i = 0; i < arrayToSort.Length; i++)
            {
                var indexToInsert = i;

                for (int j = 0; j < i; j++)
                {
                    // order=true (ASC): sortedArray[j] > arrayToSort[i]
                    // order=false (DESC): sortedArray[j] < arrayToSort[i]
                    if (order ? comparer.Compare(sortedArray[j], arrayToSort[i]) > 0 : comparer.Compare(sortedArray[j], arrayToSort[i]) < 0)
                    {
                        SwapToRight(sortedArray, j, i - 1);
                        indexToInsert = j;
                        break;
                    }
                }

                sortedArray[indexToInsert] = arrayToSort[i];
            }

            return sortedArray;
        }

        /// <summary>
        /// Swap sortedArray of size [fromIndex..toIndex ] to right in one element
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sortedArray"></param>
        /// <param name="fromIndex"></param>
        /// <param name="toIndex"></param>
        private void SwapToRight<T>(T[] sortedArray, int fromIndex, int toIndex)
        {
            for (int i = toIndex; i >= fromIndex; i--)
            {
                sortedArray[i + 1] = sortedArray[i];
            }
            sortedArray[fromIndex] = default(T);
        }
    }
}