using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vilandagro.Trainings.Algorithms;

namespace Vilandagro.Trainings.Big
{
    public class BigFileSorting
    {
        private long _requriedChunkSize;
        private double _allowableInfelicity;

        private List<string> _chunks;

        public BigFileSorting(long requriedChunkSize, double allowableInfelicity)
        {
            _requriedChunkSize = requriedChunkSize;
            _allowableInfelicity = allowableInfelicity;

            _chunks = new List<string>();
        }

        public void SortSync(string fileToSort)
        {
            var commonTimer = new Stopwatch();
            var timer = new Stopwatch();

            commonTimer.Start();
            timer.Start();
            // Create copy of the file to sort
            var fileToSortCopy = CreateCopyFile(fileToSort);
            timer.Stop();
            Console.WriteLine("CreateCopyFile total time: {0} s{1}", timer.Elapsed.TotalSeconds, Environment.NewLine);
            timer.Reset();

            timer.Start();
            // Split file into small chunks
            SplitFileIntoSmallChunks(fileToSortCopy, new FileInfo(fileToSortCopy).Length);
            timer.Stop();
            Console.WriteLine("SplitFileIntoSmallChunks total time: {0} s{1}", timer.Elapsed.TotalSeconds, Environment.NewLine);
            timer.Reset();

            timer.Start();
            SortAllChunks();
            timer.Stop();
            Console.WriteLine("SortAllChunks total time: {0} s{1}", timer.Elapsed.TotalSeconds, Environment.NewLine);
            timer.Reset();

            timer.Start();
            MergeAllChunksIntoOneFile();
            timer.Stop();
            Console.WriteLine("MergeAllChunksIntoOneFile total time: {0} s{1}", timer.Elapsed.TotalSeconds, Environment.NewLine);
            timer.Reset();

            commonTimer.Stop();
            Console.WriteLine("Total sorting time: {0} s", commonTimer.Elapsed.TotalSeconds);
        }

        private string CreateCopyFile(string fileToSort)
        {
            if (!File.Exists(fileToSort))
            {
                throw new FileNotFoundException(string.Format("The file {0} does not exist", fileToSort));
            }

            var copyFileToSort = GetFileName();
            using (var writer = File.OpenWrite(copyFileToSort))
            {
                using (var reader = File.OpenRead(fileToSort))
                {
                    reader.CopyTo(writer, BigTextFileHelper.BufferSize);
                }
            }
            return copyFileToSort;
        }

        private void SplitFileIntoSmallChunks(string fileToSplit, long currentSize)
        {
            if (currentSize > _requriedChunkSize * (1 + _allowableInfelicity))
            {
                var splittedFiles = BigTextFileHelper.SplitFileInto2Files(fileToSplit);

                SplitFileIntoSmallChunks(splittedFiles.Item1, currentSize / 2);
                SplitFileIntoSmallChunks(splittedFiles.Item2, currentSize / 2);
            }
            else
            {
                _chunks.Add(fileToSplit);
            }
        }

        private void SortAllChunks()
        {
            var commonTimer = new Stopwatch();
            var timer = new Stopwatch();

            foreach (var chunk in _chunks)
            {
                commonTimer.Start();
                var itemsToMerge = new List<string>();

                timer.Start();
                using (var reader = File.OpenText(chunk))
                {
                    while (!reader.EndOfStream)
                    {
                        var item = reader.ReadLine();

                        if (!string.IsNullOrEmpty(item))
                        {
                            itemsToMerge.Add(item);
                        }
                    }
                }
                timer.Stop();
                Console.WriteLine("Chunk {0} was readed, there are {1} items to sort, time to read: {2} s", chunk, itemsToMerge.Count, timer.Elapsed.TotalSeconds);
                timer.Reset();

                timer.Start();
                itemsToMerge.Sort(StringComparer.InvariantCultureIgnoreCase);
                timer.Stop();
                Console.WriteLine("Sorting chunk {0} time: {1} s", chunk, timer.Elapsed.TotalSeconds);
                timer.Reset();

                timer.Start();
                File.Delete(chunk);
                using (var writer = File.CreateText(chunk))
                {
                    foreach (var mergedItem in itemsToMerge)
                    {
                        writer.WriteLine(mergedItem);
                    }
                    writer.Flush();
                }
                timer.Stop();
                Console.WriteLine("Saving sorted chunk {0} time: {1} s", chunk, timer.Elapsed.TotalSeconds);
                timer.Reset();

                commonTimer.Stop();
                Console.WriteLine("Total time to sort chunk {0}: {1} s{2}", chunk, commonTimer.Elapsed.TotalSeconds, Environment.NewLine);
                commonTimer.Reset();
            }
        }

        private string MergeAllChunksIntoOneFile()
        {
            return BigTextFileHelper.MergeSortedFiles(_chunks);
        }

        private string GetFileName()
        {
            return string.Concat(Guid.NewGuid().ToString(), ".txt");
        }
    }
}
