using UnityEngine;

public static class RadixSorter
{
    public static int[] SortRadix(int[] array)
    {
        return RadixSort(array, 1);
    }

    private struct KeyValueEntry
    {
        private int key;

        public int Key
        {
            get => key;
            set
            {
                if (key >= 0)
                {
                    key = value;
                }

                else
                {
                    Debug.LogError("Invalid key value");
                }
            }
        }

        public int Value { get; set; }
    }


    private static int[] RadixSort(int[] array, int digit)
    {
        bool empty = true;
        KeyValueEntry[] digits = new KeyValueEntry[array.Length];//array that holds the digits;
        int[] sortedArray = new int[array.Length];//Hold the sorted array

        for (int i = 0; i < array.Length; i++)
        {
            digits[i] = new KeyValueEntry {Key = i, Value = (array[i] / digit) % 10};

            if (array[i] / digit != 0)
            {
                empty = false;
            }
        }

        if (empty)
        {
            return array;
        }

        KeyValueEntry[] sortedDigits = CountingSort(digits);

        for (int i = 0; i < sortedArray.Length; i++)
        {
            sortedArray[i] = array[sortedDigits[i].Key];
        }

        return RadixSort(sortedArray, digit * 10);
    }
    
    private static int MaxValue(KeyValueEntry[] arr)
    {
        int sax = arr[0].Value;

        for (int i = 1; i < arr.Length; i++)
        {
            if (arr[i].Value > sax)
            {
                sax = arr[i].Value;
            }
        }   

        return sax;
    }


    private static KeyValueEntry[] CountingSort(KeyValueEntry[] arrayA)
    {
        int[] arrayB = new int[MaxValue(arrayA) + 1];
        KeyValueEntry[] arrayC = new KeyValueEntry[arrayA.Length];
        for (int i = 0; i < arrayB.Length; i++)
        {
            arrayB[i] = 0;
        }

        for (int i = 0; i < arrayA.Length; i++)
        {
            arrayB[arrayA[i].Value]++;
        }

        for (int i = 1; i < arrayB.Length; i++)
        {
            arrayB[i] += arrayB[i - 1];

        }

        for (int i = arrayA.Length - 1; i >= 0; i--)
        {
            int value = arrayA[i].Value;
            int index = arrayB[value];
            arrayB[value]--;
            arrayC[index - 1] = new KeyValueEntry();
            arrayC[index - 1].Key = i;
            arrayC[index - 1].Value = value;
        }

        return arrayC;
    }
    
    

}
