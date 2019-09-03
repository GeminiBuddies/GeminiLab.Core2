using System;
using System.Collections.Generic;

namespace GeminiLab.Core2.Collections.HeapBase {
    /**
     * <summary>This class provides C++-style heap functions.</summary>
     * <remarks>
     * <para>These functions are extension methods which accept an array as "this" parameter.</para>
     * <para>These functions **won't** check whether the array is null or whether the array is large enough.</para>
     * </remarks>
     */
    public static class HeapBase {
        /**
         * <summary>Turn a array to a heap.</summary>
         * <exception cref="ArgumentOutOfRangeException"/>
         * <exception cref="NullReferenceException"/>
         * <exception cref="IndexOutOfRangeException"/>
         */
        public static void MakeHeap<T>(this T[] array, long length) => MakeHeap(array, length, Comparer<T>.Default);

        /**
         * <summary>Turn a array to a heap with a specified comparer.</summary>
         * <exception cref="ArgumentNullException"/>
         * <exception cref="ArgumentOutOfRangeException"/>
         * <exception cref="NullReferenceException"/>
         * <exception cref="IndexOutOfRangeException"/>
         */
        public static void MakeHeap<T>(this T[] array, long length, IComparer<T> comp) {
            if (comp == null) throw new ArgumentNullException(nameof(comp));
            for (int i = 1; i <= length; ++i) array.PushHeap(i, comp);
        }

        // 'length' is the **new** length, which means
        // array[length - 1] is the new element
        /**
         * <summary>Add an item to heap.</summary>
         * <remarks></remarks>
         * <exception cref="ArgumentOutOfRangeException"/>
         * <exception cref="NullReferenceException"/>
         * <exception cref="IndexOutOfRangeException"/>
         */
        public static void PushHeap<T>(this T[] array, long length) => PushHeap(array, length, Comparer<T>.Default);

        /**
         * <summary>Add an item to heap with a specified comparer.</summary>
         * <exception cref="ArgumentNullException"/>
         * <exception cref="ArgumentOutOfRangeException"/>
         * <exception cref="NullReferenceException"/>
         * <exception cref="IndexOutOfRangeException"/>
         */
        public static void PushHeap<T>(this T[] array, long length, IComparer<T> comp) {
            if (comp == null) throw new ArgumentNullException(nameof(comp));
            if (length <= 0) throw new ArgumentOutOfRangeException(nameof(length));
            if (length == 1) return;

            var ptr = length;
            var item = array[length - 1];
            while (ptr > 1 && comp.Compare(item, array[(ptr >> 1) - 1]) < 0) {
                array[ptr - 1] = array[(ptr >> 1) - 1];
                ptr >>= 1;
            }

            array[ptr - 1] = item;
        }

        // 'length' is the **old** length
        // remove T[0], move it to T[length - 1] and return it
        /**
         * <summary>Remove and return the first element of the heap.</summary>
         * <exception cref="ArgumentOutOfRangeException"/>
         * <exception cref="NullReferenceException"/>
         * <exception cref="IndexOutOfRangeException"/>
         */
        public static T PopHeap<T>(this T[] array, long length) => PopHeap(array, length, Comparer<T>.Default);

        /**
         * <summary>Remove and return the first element of the heap with a specified comparer.</summary>
         * <exception cref="ArgumentNullException"/>
         * <exception cref="ArgumentOutOfRangeException"/>
         * <exception cref="NullReferenceException"/>
         * <exception cref="IndexOutOfRangeException"/>
         */
        public static T PopHeap<T>(this T[] array, long length, IComparer<T> comp) {
            if (comp == null) throw new ArgumentNullException(nameof(comp));
            if (length <= 0) throw new ArgumentOutOfRangeException(nameof(length));
            if (length == 1) return array[0];

            --length;
            long ptr = 1;
            T last = array[length];
            array[length] = array[0];
            while (true) {
                if (ptr << 1 > length) break;

                if (comp.Compare(array[(ptr << 1) - 1], last) < 0) {
                    if ((ptr << 1) + 1 <= length && comp.Compare(array[ptr << 1], array[(ptr << 1) - 1]) < 0) {
                        array[ptr - 1] = array[ptr << 1]; ptr = (ptr << 1) + 1;
                    } else {
                        array[ptr - 1] = array[(ptr << 1) - 1]; ptr <<= 1;
                    }
                } else if ((ptr << 1) + 1 <= length && comp.Compare(array[ptr << 1], last) < 0) {
                    array[ptr - 1] = array[ptr << 1]; ptr = (ptr << 1) + 1;
                } else {
                    break;
                }
            }

            array[ptr - 1] = last;
            return array[length];
        }

        // turn heap to a sorted list
        /**
        * <summary>Disassemble a heap into a sorted array.</summary>
        * <exception cref="ArgumentOutOfRangeException"/>
        * <exception cref="NullReferenceException"/>
        * <exception cref="IndexOutOfRangeException"/>
        */
        public static void SortHeap<T>(this T[] array, long length) => SortHeap(array, length, false, Comparer<T>.Default);


        /**
        * <summary>Disassemble a heap into a sorted array with a specified comparer.</summary>
        * <exception cref="ArgumentNullException"/>
        * <exception cref="ArgumentOutOfRangeException"/>
        * <exception cref="NullReferenceException"/>
        * <exception cref="IndexOutOfRangeException"/>
        */
        public static void SortHeap<T>(this T[] array, long length, IComparer<T> comp) => SortHeap(array, length, false, comp);

        /**
        * <summary>Disassemble a heap into a sorted array with a specified comparer.</summary>
        * <exception cref="ArgumentNullException"/>
        * <exception cref="ArgumentOutOfRangeException"/>
        * <exception cref="NullReferenceException"/>
        * <exception cref="IndexOutOfRangeException"/>
        */
        public static void SortHeap<T>(this T[] array, long length, bool reverse, IComparer<T> comp) {
            if (comp == null) throw new ArgumentNullException(nameof(comp));
            if (length <= 0) throw new ArgumentOutOfRangeException(nameof(length));
            if (length == 1) return;

            for (long i = length; i >= 1; --i) array.PopHeap(i, comp);

            if (reverse) return;

            Array.Reverse(array, 0, (int)length);
        }
    }
}
