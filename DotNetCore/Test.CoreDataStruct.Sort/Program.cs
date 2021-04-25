using System;

namespace Test.CoreDataStruct.Sort
{
    static class Program
    {
        static void Main(string[] args)
        {
            //var maopaoRes = GetRandomIntArr(20,1,50).BubbleSort();
            //var selectRes = GetRandomIntArr(20, 1, 50).SelectSort();
            //var insertionRes = GetRandomIntArr(20, 1, 50).InsertionSor();
            //var shellRes = GetRandomIntArr(20, 1, 50).ShellSort();
            var mergeRes = GetRandomIntArr(20, 1, 50).MergeSort();
        }



        /// <summary>
        /// 获取随机数
        /// </summary>
        /// <param name="count"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int[] GetRandomIntArr(int count, int min, int max)
        {
            int[] intArr = new int[count];
            var random = new Random();
            for (int i = 0; i < intArr.Length; i++)
            {

                intArr[i] = random.Next(min, max + 1);
            }

            return intArr;
        }


        /// <summary>
        /// 基础冒泡排序
        /// </summary>
        /// <param name="intArr"></param>
        /// <returns></returns>
        public static int[] BubbleSort(this int[] intArr)
        {
            int count = 0;
            for (int i = 0; i < intArr.Length - 1; i++)
            {
                Console.WriteLine($"第{(i + 1).ToSpecialString()}次排序，总共比较{(intArr.Length - (i + 1)).ToSpecialString()}次,数组：{string.Join(",", intArr)}");

                //牺牲空间复杂度优化排序时间
                bool isSort = true;
                for (int j = i + 1; j < intArr.Length; j++)
                {
                    count++;
                    //大的往前面排
                    //可记录下表不进行交换 直到最后交换
                    if (intArr[i] < intArr[j])
                    {
                        isSort = false;
                        int temp = intArr[i];
                        intArr[i] = intArr[j];
                        intArr[j] = temp;
                    }
                }

                if (isSort)
                {
                    Console.WriteLine("已经排好了，不用再排了！");
                    break;
                }
            }

            Console.WriteLine();
            Console.WriteLine($"总共比较次数：{count}");

            return intArr;
        }

        /// <summary>
        /// 基础选择排序
        /// </summary>
        /// <returns></returns>
        public static int[] SelectSort(this int[] intArr)
        {
            int count = 0;
            for (int i = 0; i < intArr.Length - 1; i++)
            {
                Console.WriteLine($"第{(i + 1).ToSpecialString()}次排序，总共比较{(intArr.Length - (i + 1)).ToSpecialString()}次,数组：{string.Join(",", intArr)}");

                int flag = i;
                for (int j = i + 1; j < intArr.Length; j++)
                {
                    //大的往前面排
                    count++;
                    if (intArr[flag] < intArr[j])
                    {
                        flag = j;
                    }
                }

                if (flag != i)
                {
                    int temp = intArr[i];
                    intArr[i] = intArr[flag];
                    intArr[flag] = temp;
                }

            }
            Console.WriteLine();
            Console.WriteLine($"总共比较次数：{count}");
            return intArr;
        }


        /// <summary>
        /// 基础插入排序
        /// </summary>
        /// <returns></returns>
        public static int[] InsertionSor(this int[] intArr)
        {
            int count = 0;
            int index;
            int indexValue;
            for (int i = 1; i < intArr.Length; i++)
            {
                //抽出要往前浮的值
                index = i;
                indexValue = intArr[i];
                for (int j = i - 1; j >= 0; j--)
                {
                    count++;
                    if (indexValue > intArr[j])
                    {
                        //向后退位或者交换位置
                        intArr[j + 1] = intArr[j];
                        index = j;
                    }
                    else
                    {
                        break;
                    }
                }
                intArr[index] = indexValue;
            }

            Console.WriteLine();
            Console.WriteLine($"总共比较次数：{count}");

            return intArr;
        }


        /// <summary>
        /// 希尔排序
        /// </summary>
        /// <param name="intArr"></param>
        /// <returns></returns>
        public static int[] ShellSort(this int[] intArr)
        {
            int count = 0;
            for (int splitLength = intArr.Length / 2; splitLength > 0; splitLength = splitLength / 2)
            {
                for (int i = splitLength; i < intArr.Length; i++)
                {
                    var index = i;
                    var indexValue = intArr[i];

                    for (int j = i - splitLength; j >= 0; j -= splitLength)
                    {
                        count++;
                        if (indexValue > intArr[j])
                        {
                            intArr[j + splitLength] = intArr[j];
                            index = j;
                        }
                        else
                        {
                            break;
                        }
                    }

                    intArr[index] = indexValue;
                }
            }

            Console.WriteLine();
            Console.WriteLine($"总共比较次数：{count}");

            return intArr;
        }

        /// <summary>
        /// 归并排序入口
        /// </summary>
        /// <param name="intArr"></param>
        /// <returns></returns>
        public static int[] MergeSort(this int[] intArr)
        {
            var empty = new int[intArr.Length];
            int count = 0;
            return MergeSortMain(intArr, empty, 0, intArr.Length - 1);
        }

        /// <summary>
        /// 归并排序递归主方法
        /// </summary>
        /// <param name="intArr"></param>
        /// <param name="empty"></param>
        /// <param name="indexStart"></param>
        /// <param name="indexEnd"></param>
        /// <returns></returns>
        public static int[] MergeSortMain(int[] intArr, int[] empty, int indexStart, int indexEnd)
        {
            if (indexStart >= indexEnd)
            {
                return intArr;
            }

            var leftStart = indexStart;
            var leftEnd = (int)Math.Floor((decimal)(indexStart + indexEnd) / 2);
            var rightStart = leftEnd + 1;
            var rightEnd = indexEnd;

            //左叶展开
            MergeSortMain(intArr, empty, leftStart, leftEnd);
            //右叶展开
            MergeSortMain(intArr, empty, rightStart, rightEnd);



            var emptyInsertIndex = indexStart;

            //两个子标移动比较，其中一个子标比较完毕全部加入临时数组时退出
            while (leftStart <= leftEnd && rightStart <= rightEnd)
            {
                if (intArr[leftStart] < intArr[rightStart])
                {
                    empty[emptyInsertIndex++] = intArr[rightStart++];
                }
                else
                {
                    empty[emptyInsertIndex++] = intArr[leftStart++];
                }
            }

            //另外个子标还有没加入临时数组的直接加入
            while (leftStart <= leftEnd)
            {
                empty[emptyInsertIndex++] = intArr[leftStart++];
            }

            while (rightStart <= rightEnd)
            {
                empty[emptyInsertIndex++] = intArr[rightStart++];
            }

            //排序值覆盖到原数组
            while (rightEnd >= indexStart)
            {
                intArr[rightEnd] = empty[rightEnd];
                rightEnd--;
            }

            return intArr;
        }


    }


    public static class Extensions
    {
        public static string ToSpecialString(this int i, int length = 3)
        {
            var join = "";
            for (int j = 0; j < length; j++)
            {
                join += "0";
            }

            return $"{join.Substring(i.ToString().Length)}{i.ToString()}";
        }
    }
}
