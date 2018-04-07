using System;
using System.Collections.Generic;

namespace GeminiLab.Core2.Collections.HeapBase {
    public static class HeapBase {
        public static void MakeHeap<T>(this T[] array, long length) => MakeHeap(array, length, Comparer<T>.Default);

        public static void MakeHeap<T>(this T[] array, long length, IComparer<T> comp) {
            for (int i = 1; i <= length; ++i) array.PushHeap(i, comp);
        }

        // array[length - 1] is the new element
        public static void PushHeap<T>(this T[] array, long length) => PushHeap(array, length, Comparer<T>.Default);

        public static void PushHeap<T>(this T[] array, long length, IComparer<T> comp) {
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

        // remove T[0], move it to T[length - 1] and return it
        public static T PopHeap<T>(this T[] array, long length) => PopHeap(array, length, Comparer<T>.Default);

        public static T PopHeap<T>(this T[] array, long length, IComparer<T> comp) {
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
        public static void SortHeap<T>(this T[] array, long length) => SortHeap(array, length, false, Comparer<T>.Default);

        public static void SortHeap<T>(this T[] array, long length, bool reverse) => SortHeap(array, length, reverse, Comparer<T>.Default);

        public static void SortHeap<T>(this T[] array, long length, IComparer<T> comp) => SortHeap(array, length, false, comp);

        public static void SortHeap<T>(this T[] array, long length, bool reverse, IComparer<T> comp) {
            if (length <= 0) throw new ArgumentOutOfRangeException(nameof(length));
            if (length == 1) return;

            for (long i = length; i >= 1; --i) array.PopHeap(i, comp);

            if (reverse) return;

            for (long i = length / 2; i >= 0; --i) {
                T v = array[i];
                array[i] = array[length - i - 1];
                array[length - i - 1] = v;
            }
        }
    }
}
