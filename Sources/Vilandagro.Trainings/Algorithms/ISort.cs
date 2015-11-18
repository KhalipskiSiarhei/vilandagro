namespace Vilandagro.Trainings.Algorithms
{
    public interface ISort
    {
        T[] Sort<T>(T[] arrayToSort, bool order);

        string Sort(string toSort, bool order);
    }
}
