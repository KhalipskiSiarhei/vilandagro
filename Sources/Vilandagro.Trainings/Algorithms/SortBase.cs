﻿namespace Vilandagro.Trainings.Algorithms
{
    public abstract class SortBase : ISort
    {
        public abstract T[] Sort<T>(T[] arrayToSort, bool order);

        public string Sort(string toSort, bool order)
        {
            var arrayToSort = toSort.ToCharArray();
            var sortedArray = Sort(arrayToSort, order);

            return new string(sortedArray);
        }
    }
}