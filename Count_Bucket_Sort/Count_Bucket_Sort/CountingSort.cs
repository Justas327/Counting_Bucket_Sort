﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Count_Bucket_Sort
{
    class CountingSort
    {
        /// <summary>
        /// Tests counting sort performance with array in operative memory
        /// </summary>
        /// <param name="n">Element count</param>
        /// <param name="seed"Seed></param>
        public void TestArray_OP(int seed, int range)
        {
            Console.Clear();
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("COUNTING SORT OP");
            builder.AppendLine("===================================================");
            builder.AppendLine("| Number of elements |        |    Runtime time   |");
            builder.AppendLine("========================ARRAY======================");

            foreach(int count in Program.KIEKIAI)
            {
                //Array sorting
                DataArray data = new MyArray(count, seed, range);
                //data.Print(data.Length);

                Stopwatch t1 = new Stopwatch();
                t1.Start();
                CountSort(data);
                t1.Stop();

                //data.Print(data.Length);
                //Console.WriteLine("Time: " + t1.ElapsedMilliseconds + "ms");
                builder.AppendLine(string.Format("|{0,-20}|        |{1} ms|", count, t1.Elapsed.ToString()));

                //Clears memory
                data = null;
                System.GC.Collect();
            }

            Console.Write(builder.ToString());
        }

        /// <summary>
        /// Tests counting sort performance with linked list in operative memory
        /// </summary>
        /// <param name="n">Elements count</param>
        /// <param name="seed">Generating seed</param>
        /// <param name="range">Generated element range</param>
        public void TestList_OP(int seed, int range)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("========================List=======================");

            foreach (int count in Program.KIEKIAI)
            {
                //Linked list sorting
                DataList listData = new MyList(count, seed, range);
                //listData.Print(listData.Length);

                Stopwatch t2 = new Stopwatch();
                t2.Start();
                CountSort(listData);
                t2.Stop();

                //listData.Print(listData.Length);
                builder.AppendLine(string.Format("|{0,-20}|        |{1} ms|", count, t2.Elapsed.ToString()));
                //Clears memory
                listData = null;
                System.GC.Collect();
            }

            builder.AppendLine("===================================================");
            Console.Write(builder.ToString());
        }

        /// <summary>
        /// Tests counting sort performance with array in disk memory
        /// </summary>
        /// <param name="n">Elements count</param>
        /// <param name="seed">Seed</param>
        /// <param name="fileName">File name</param>
        public void TestArray_D(int seed, int range, string fileName)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("COUNTING SORT D");
            builder.AppendLine("===================================================");
            builder.AppendLine("| Number of elements |        |    Runtime time   |");
            builder.AppendLine("========================ARRAY======================");

            foreach (int count in Program.KIEKIAI)
            {
                MyFileArray dataArray = new MyFileArray(fileName, count, seed, range);
                using (dataArray.fs = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite))
                {
                    //dataArray.Print(n);

                    Stopwatch t1 = new Stopwatch();
                    t1.Start();
                    CountSort(dataArray);
                    t1.Stop();

                    //dataArray.Print(n);
                    builder.AppendLine(string.Format("|{0,-20}|        |{1} ms|", count, t1.Elapsed.ToString()));

                    //Clears memory
                    dataArray = null;
                    System.GC.Collect();
                }
            }

            Console.Write(builder.ToString());
        }
       
        /// <summary>
        /// Tests counting sort performance with list in disk memory
        /// </summary>
        /// <param name="n">Elements count</param>
        /// <param name="seed">Generating seed</param>
        /// <param name="range">Element range</param>
        /// <param name="fileName">File name</param>
        public void TestList_D(int seed, int range, string fileName)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("========================List=======================");

            foreach (int count in Program.KIEKIAI)
            {
                MyFileList dataList = new MyFileList(fileName, count, seed, range);
                using (dataList.fs = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite))
                {
                    //dataList.Print(n);

                    Stopwatch t2 = new Stopwatch();
                    t2.Start();
                    CountSort(dataList);
                    t2.Stop();

                    //dataList.Print(n);
                    builder.AppendLine(string.Format("|{0,-20}|        |{1} ms|", count, t2.Elapsed.ToString()));
                }
            }

            builder.AppendLine("===================================================");
            Console.Write(builder.ToString());
        }
        /// <summary>
        /// Counting sort for array
        /// </summary>
        /// <param name="items">Array</param>
        public void CountSort(DataArray array)
        {
            if (array == null)
                return;

            //Finds min and max
            int[] output = new int[array.Length];
            int minValue = array.Min();
            int maxValue = array.Max();

            //Counts frequencies
            int[] counts = new int[maxValue - minValue + 1];
            for (int i = 0; i < array.Length; i++)
            {
                counts[array[i] - minValue]++;
            }

            //Each element stores the sum of previous counts
            counts[0]--;
            for (int i = 1; i < counts.Length; i++)
            {
                counts[i] += counts[i - 1];
            }

            //Puts each element from items array to the right place using counts saved index
            for (int i = 0; i < array.Length; i++)
            {
                output[counts[array[i] - minValue]--] = array[i];
            }

            //Copies output to object
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = output[i];
            }
        }

        /// <summary>
        /// Counting sort for list
        /// </summary>
        /// <param name="list">List</param>
        public void CountSort(DataList list)
        {
            if (list == null)
                return;

            //Finds min and max values
            int[] output = new int[list.Length];
            int minValue = list.Min();
            int maxValue = list.Max();

            //Counts frequencies
            int[] counts = new int[maxValue - minValue + 1];
            for(list.Head(); list.Exists(); list.Next())
            {
                counts[list.Current() - minValue]++;
            }

            //Each element stores the sum of previous counts
            counts[0]--;
            for (int i = 1; i < counts.Length; i++)
            {
                counts[i] += counts[i - 1];
            }

            for (list.Head(); list.Exists(); list.Next())
            {
                output[counts[list.Current() - minValue]--] = list.Current();
            }

            //list = new MyLinkedList(output);
            list.Head();
            for(int i = 0; i < list.Length; i++)
            {
                list.ChangeData(output[i]);
                list.Next();

            }
        }
    }
}
