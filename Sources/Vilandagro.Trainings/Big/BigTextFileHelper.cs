using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Vilandagro.Trainings.Big
{
    public static class BigTextFileHelper
    {
        public const int OneGbInt = 1073741824;
        public const long OneGbLong = OneGbInt;
        public const int BufferSize = 81920;

        public static string GenerateFile(double sizeGb)
        {
            var fileName = GetFileName();
            var sizeInKb = sizeGb*OneGbLong;
            var currentSize = (long)0;

            using (var writer = File.CreateText(fileName))
            {
                while (currentSize < sizeInKb)
                {
                    var itemToWrite = Guid.NewGuid().ToString();
                    
                    writer.WriteLine(itemToWrite);
                    currentSize += itemToWrite.Length + 2;
                }

                writer.Flush();
            }

            return fileName;
        }

        public static Tuple<string, string> SplitFileInto2Files(string fileToSplit)
        {
            if (!File.Exists(fileToSplit))
            {
                throw new FileNotFoundException(string.Format("The passed file {0} to split does not exist",
                    fileToSplit));
            }

            var secondFile = GetFileName();

            using (var splittedFileWriter = new FileStream(fileToSplit, FileMode.Open, FileAccess.ReadWrite))
            {
                using (var secondFileWritter = File.OpenWrite(secondFile))
                {
                    var middlePosition = GetStartOfSecondFilePosition(splittedFileWriter);

                    splittedFileWriter.Position = middlePosition;
                    splittedFileWriter.CopyTo(secondFileWritter, BufferSize);

                    splittedFileWriter.Position = 0;
                    splittedFileWriter.SetLength(middlePosition);
                }
            }

            return new Tuple<string, string>(fileToSplit, secondFile);
        }

        public static string MergeSortedFiles(List<string> filesToMerge, bool removeFromFile = true, int bufferSize = BufferSize)
        {
            if (filesToMerge.Any(f => !File.Exists(f)))
            {
                throw new FileNotFoundException("One or some of the files to merge do not exist");
            }
            if (filesToMerge.Count == 1)
            {
                return filesToMerge.Single();
            }

            Dictionary<string, ReaderValue> filesToMergeDic =
                new Dictionary<string, ReaderValue>();
            var mergedFile = GetFileName();

            try
            {
                filesToMerge.ForEach(f =>
                {
                    var reader = File.OpenText(f);
                    filesToMergeDic.Add(f, new ReaderValue() { Reader = reader, Value = reader.ReadLine() });
                });
                using (var writer = File.CreateText(mergedFile))
                {
                    while (filesToMergeDic.Any())
                    {
                        var nextSortedItemToMerge = GetSortedItemToMerge(filesToMergeDic, true);

                        if (!string.IsNullOrEmpty(nextSortedItemToMerge))
                        {
                            writer.WriteLine(nextSortedItemToMerge);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            finally
            {
                foreach (var fileToMergeItem in filesToMergeDic)
                {
                    try
                    {
                        if (fileToMergeItem.Value != null && fileToMergeItem.Value.Reader != null)
                        {
                            fileToMergeItem.Value.Reader.Close();
                            fileToMergeItem.Value.Reader.Dispose();
                            fileToMergeItem.Value.Reader = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
                filesToMergeDic.Clear();
            }

            if (removeFromFile)
            {
                filesToMerge.ForEach(f => File.Delete(f));
            }

            return mergedFile;
        }

        private static string GetSortedItemToMerge(Dictionary<string, ReaderValue> filesToMergeDic, bool sortOrder)
        {
            KeyValuePair<string, ReaderValue> nextItemToMergeKeyValue = filesToMergeDic.FirstOrDefault();

            foreach (var fileToMergeKeyValue in filesToMergeDic)
            {
                if (nextItemToMergeKeyValue.Key == fileToMergeKeyValue.Key)
                {
                    continue;
                }
                else
                {
                    var comparisonResult = StringComparer.InvariantCultureIgnoreCase.Compare(nextItemToMergeKeyValue.Value.Value,
                        fileToMergeKeyValue.Value.Value);
                    if (sortOrder && comparisonResult > 0)
                    {
                        nextItemToMergeKeyValue = fileToMergeKeyValue;
                    }
                    else if (!sortOrder && comparisonResult < 0)
                    {
                        nextItemToMergeKeyValue = fileToMergeKeyValue;
                    }
                }
            }

            var sortedItemToMerge = nextItemToMergeKeyValue.Value.Value;
            nextItemToMergeKeyValue.Value.Value = nextItemToMergeKeyValue.Value.Reader.EndOfStream
                ? string.Empty
                : nextItemToMergeKeyValue.Value.Reader.ReadLine();

            if (string.IsNullOrEmpty(nextItemToMergeKeyValue.Value.Value))
            {
                nextItemToMergeKeyValue.Value.Reader.Close();
                nextItemToMergeKeyValue.Value.Reader.Dispose();
                nextItemToMergeKeyValue.Value.Reader = null;
                filesToMergeDic.Remove(nextItemToMergeKeyValue.Key);
            }
            return sortedItemToMerge;
        }

        private static string GetFileName()
        {
            return string.Concat(Guid.NewGuid().ToString(), ".txt");
        }

        private static long GetStartOfSecondFilePosition(FileStream splittedFileWriter)
        {
            var oldPosition = splittedFileWriter.Position;

            try
            {
                var middlePosition = splittedFileWriter.Length/2 + splittedFileWriter.Length%2 - Environment.NewLine.Length;
                var buffer = new byte[BufferSize];

                splittedFileWriter.Position = middlePosition;
                while ((splittedFileWriter.Read(buffer, 0, BufferSize)) > 0)
                {
                    var bufferStr = System.Text.Encoding.Default.GetString(buffer);
                    var index = bufferStr.IndexOf(Environment.NewLine, StringComparison.InvariantCultureIgnoreCase);

                    if (index >= 0)
                    {
                        middlePosition += index + Environment.NewLine.Length;
                        return middlePosition;
                    }
                }
                return -1;
            }
            finally
            {
                splittedFileWriter.Position = oldPosition;
            }
        }
    }
}