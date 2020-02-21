using System.Collections.Generic;
using System;

public class FilterList<T> {

  public int Count { get => filters.Count; }
  public Filter this[int index] { get => filters[index]; set => filters[index] = value; }
  public int Capacity { get => filters.Capacity; set => filters.Capacity = value; }
  public void Clear() => filters.Clear();
  public bool Contains(Filter item) => filters.Contains(item);
  public bool Exists(Predicate<Filter> match) => filters.Exists(match);
  public int FindIndex(int startIndex, int count, Predicate<Filter> match) => FindIndex(startIndex, count, match);
  public int FindIndex(int startIndex, Predicate<Filter> match) => FindIndex(startIndex, match);
  public int FindIndex(Predicate<Filter> match) => FindIndex(match);
  public Filter FindLast(Predicate<Filter> match) => FindLast(match);
  public int FindLastIndex(int startIndex, int count, Predicate<Filter> match) => FindLastIndex(startIndex, count, match);
  public int FindLastIndex(int startIndex, Predicate<Filter> match) => FindLastIndex(startIndex, match);
  public int FindLastIndex(Predicate<Filter> match) => FindLastIndex(match);
  public List<Filter>.Enumerator GetEnumerator() => filters.GetEnumerator();
  public List<Filter> GetRange(int index, int count) => filters.GetRange(index, count);
  public int IndexOf(Filter item, int index, int count) => filters.IndexOf(item, index, count);
  public int IndexOf(Filter item, int index) => filters.IndexOf(item, index);
  public int IndexOf(Filter item) => filters.IndexOf(item);
  public int LastIndexOf(Filter item) => filters.LastIndexOf(item);
  public int LastIndexOf(Filter item, int index) => filters.LastIndexOf(item, index);
  public int LastIndexOf(Filter item, int index, int count) => filters.LastIndexOf(item);
  public bool Remove(Filter item) => filters.Remove(item);
  public int RemoveAll(Predicate<Filter> match) => filters.RemoveAll(match);
  public void RemoveAt(int index) => filters.RemoveAt(index);
  public void RemoveRange(int index, int count) => filters.RemoveRange(index, count);
  public Filter[] ToArray() => filters.ToArray();
  public void TrimExcess() => filters.TrimExcess();
  public bool TrueForAll(Predicate<Filter> match) => filters.TrueForAll(match);
  public int BinarySearch(Filter item) => filters.BinarySearch(item);
  public int BinarySearch(Filter item, IComparer<Filter> comparer) => filters.BinarySearch(item, comparer);
  public int BinarySearch(int index, int count, Filter item, IComparer<Filter> comparer) => filters.BinarySearch(index, count, item, comparer);

  protected List<Filter> filters;

  public class Filter {
    public Filter(Func<T, T> function, float priority = 0) {
      this.function = function;
      this.priority = priority;
    }
    public Func<T, T> function;
    public float priority = 0;
  }

  public T Apply(T value) {
    foreach (var filter in filters) value = filter.function(value);
    return value;
  }

  public void AddRange(IEnumerable<Filter> filters) { foreach (var filter in filters) Add(filter); }

  public void Add(Func<T, T> function, float priority = 0) => Add(new Filter(function, priority));
  public void Add(Filter filter) {
    for (int i = 0; i < filters.Count; i++) {
      var other = filters[i];
      if (other.priority <= filter.priority) {
        filters.Insert(i, filter);
        return;
      }
    }
    filters.Add(filter);
  }

}
