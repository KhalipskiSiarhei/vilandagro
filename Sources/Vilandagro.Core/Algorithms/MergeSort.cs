using System;


using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vilandagro.Core.Algorithms
{
    public class MergeSort : SortBase
    {
        public override T[] Sort<T>(T[] arrayToSort, bool order)
        {
            var mergeSortItem = Devide(arrayToSort);

            return Sort(mergeSortItem, order);
        }

        /// <summary>
        /// Build tree of items from the sorted array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arrayToSort"></param>
        /// <returns></returns>
        private MergeSortItem<T> Devide<T>(T[] arrayToSort)
        {
            MergeSortItem<T> item = null;

            if (arrayToSort.Length > 1)
            {
                var leftArray = new T[arrayToSort.Length/2 + (arrayToSort.Length%2 == 0 ? 0 : 1)];
                var rightArray = new T[arrayToSort.Length/2];
                item = new MergeSortItem<T>();

                Array.Copy(arrayToSort, leftArray, leftArray.Length);
                Array.Copy(arrayToSort, leftArray.Length, rightArray, 0, rightArray.Length);

                item.Left = Devide(leftArray);
                item.Right = Devide(rightArray);
            }
            else
            {
                var singleItem = new SingleMergeSortItem<T>();

                singleItem.Left = null;
                singleItem.Right = null;
                singleItem.Item = arrayToSort[0];
                item = singleItem;
            }

            return item;
        }

        /// <summary>
        /// Sort/Build value from the tree of values
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        private T[] Sort<T>(MergeSortItem<T> item, bool order)
        {
            if (item is SingleMergeSortItem<T>)
            {
                return new T[] { ((SingleMergeSortItem<T>)item).Item };
            }
            else
            {
                return MergeArraysByOrder(Sort(item.Left, order), Sort(item.Right, order), order);
            }
        }

        private T[] MergeArraysByOrder<T>(T[] sortedArray1, T[] sortedArray2, bool order)
        {
            var mergedSortedArray =
                new T[
                    (sortedArray1 != null ? sortedArray1.Length : 0) + (sortedArray2 != null ? sortedArray2.Length : 0)];
            var comparer = Comparer<T>.Default;
            var index = 0;

            while (true)
            {
                if (IsEmptyArray(sortedArray1) && IsEmptyArray(sortedArray2))
                {
                    break;
                }
                if (IsEmptyArray(sortedArray1) && !IsEmptyArray(sortedArray2))
                {
                    mergedSortedArray[index] = Pop(ref sortedArray2);
                }
                if (!IsEmptyArray(sortedArray1) && IsEmptyArray(sortedArray2))
                {
                    mergedSortedArray[index] = Pop(ref sortedArray1);
                }
                if (!IsEmptyArray(sortedArray1) && !IsEmptyArray(sortedArray2))
                {
                    var comparationResult = comparer.Compare(sortedArray1[0], sortedArray2[0]);

                    if (order)
                    {
                        mergedSortedArray[index] = comparationResult <= 0 ? Pop(ref sortedArray1) : Pop(ref sortedArray2);
                    }
                    else
                    {
                        mergedSortedArray[index] = comparationResult >= 0 ? Pop(ref sortedArray1) : Pop(ref sortedArray2);
                    }
                }

                index++;
            }
            return mergedSortedArray;
        }

        private bool IsEmptyArray<T>(T[] array)
        {
            return (array == null || array.Length == 0);
        }

        private T Pop<T>(ref T[] array)
        {
            var item = array[0];

            if (array.Length > 1)
            {
                var newArray = new T[array.Length - 1];
                Array.Copy(array, 1, newArray, 0, array.Length - 1);
                array = newArray;
            }
            else
            {
                array = null;
            }

            return item;
        }
    }
}