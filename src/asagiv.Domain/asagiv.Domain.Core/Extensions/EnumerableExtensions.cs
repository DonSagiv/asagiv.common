using System;
using System.Collections.Generic;
using System.Linq;

namespace asagiv.Domain.Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static void AddRange<T>(this ICollection<T> sourceCollection, IEnumerable<T> itemsToAdd)
        {
            if (sourceCollection is null)
            {
                throw new ArgumentNullException(nameof(sourceCollection));
            }

            if (itemsToAdd is null)
            {
                throw new ArgumentNullException(nameof(itemsToAdd));
            }

            // Iterate through each item to add, and then add them.
            foreach (var item in itemsToAdd)
            {
                sourceCollection.Add(item);
            }
        }

        public static void AddRange<T>(this ICollection<T> sourceCollection, params T[] itemsToAdd)
        {
            if (sourceCollection is null)
            {
                throw new ArgumentNullException(nameof(sourceCollection));
            }

            if (itemsToAdd is null)
            {
                throw new ArgumentNullException(nameof(itemsToAdd));
            }

            sourceCollection.AddRange(itemsToAdd.AsEnumerable());
        }

        public static void RemoveRange<T>(this ICollection<T> sourceCollection, IEnumerable<T> itemsToRemove)
        {
            if (sourceCollection is null)
            {
                throw new ArgumentNullException(nameof(sourceCollection));
            }

            if (itemsToRemove is null)
            {
                throw new ArgumentNullException(nameof(itemsToRemove));
            }

            // Only remove items that are actually in the source collection.
            foreach (var item in sourceCollection.Intersect(itemsToRemove))
            {
                sourceCollection.Remove(item);
            }
        }

        public static void RemoveRange<T>(this ICollection<T> sourceCollection, params T[] itemsToRemove)
        {
            if (sourceCollection is null)
            {
                throw new ArgumentNullException(nameof(sourceCollection));
            }

            if (itemsToRemove is null)
            {
                throw new ArgumentNullException(nameof(itemsToRemove));
            }

            sourceCollection.RemoveRange(itemsToRemove.AsEnumerable());
        }

        public static IEnumerable<T> ConcatParams<T>(this IEnumerable<T> source, params T[] itemsToConcat)
        {
            return source.Concat(itemsToConcat);
        }
    }
}
