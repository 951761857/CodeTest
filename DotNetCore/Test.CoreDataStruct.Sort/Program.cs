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
            //var mergeRes = GetRandomIntArr(20, 1, 50).MergeSort();
            //var quickRes =  new int[]{72,6,57,88,60,42,83,73,48,85}.QuickSort(0,10-1);
            var countintRes = GetRandomIntArr(50, -50, 50).CountingSort();
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
                //大就往前排
                if (intArr[rightStart] > intArr[leftStart])
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


        /// <summary>
        /// 快速排序
        /// </summary>
        /// <param name="intArr"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static int[] QuickSort(this int[] intArr,int left,int right)
        {
            if (left>= right)
            {
                return intArr;
            }

            //记录  传递到递归
            var leftCopy = left;
            var rightCopy = right;

            //挖坑 基准值
            var indexValue = intArr[left];
            //坑位
            var index = left;

            left++;

            //比基准值大的往左
            while (left <= right)
            {
                //右游标往左移动 left < right = false 结束该次遍历，intArr[right]> indexValue = true跳出循环换坑位
                while (left <= right && !(intArr[right] > indexValue)) right--;

                //上面的循环 intArr[right]> indexValue = true跳出循环换坑位
                if (left <= right)
                {
                    intArr[index] = intArr[right];
                    index = right;
                    right--;
                }

                //左游标右移动 left < right = false 结束该次遍历，intArr[left]< indexValue = true跳出循环换坑位
                while (left <= right && !(intArr[left] < indexValue)) left++;

                //上面的循环 intArr[left]< indexValue = true跳出循环换坑位
                if (left <= right)
                {
                    intArr[index] = intArr[left];
                    index = left;
                    left++;
                }
            }
            
            intArr[index] = indexValue;

            //左右继续分治
            QuickSort(intArr, leftCopy, index - 1);
            QuickSort(intArr, index + 1, rightCopy);

            return intArr;
        }

        /// <summary>
        /// 计数排序
        /// </summary>
        /// <returns></returns>
        public static int[] CountingSort(this int[] intArr)
        {
            int minValue = intArr[0];
            int maxValue = intArr[0];
           

            //偏移量  兼容负数
            int rightPoint = 0;

            for (int i = 0; i < intArr.Length; i++)
            {
                if (intArr[i] < minValue)
                {
                    minValue = intArr[i];
                    continue;
                }

                if (intArr[i] > maxValue)
                {
                    maxValue = intArr[i];
                    continue;
                }
            }

            if (minValue < 0)
            {
                //有负数时进行偏移
                rightPoint = Math.Abs(minValue);
            }

            //+1 兼容0,值即下标
            //+rightPoint 兼容负数
            int[] bucket = new int[maxValue+1+rightPoint];

            for (int i = 0; i < intArr.Length; i++)
            {
                //初始值为0 直接自加
                //下标即为值，进行偏移兼容负数
                bucket[intArr[i] + rightPoint]++;
            }

            int cover = 0;
            //从bucket.Length自减为降序
            for (int i = bucket.Length - 1; i >= 0; i--)
            {
                while (bucket[i]> 0)
                {
                    intArr[cover] = i-rightPoint;
                    cover++;
                    bucket[i]--;
                }
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
