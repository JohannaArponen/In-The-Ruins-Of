using System.Collections.Generic;
using System;

public class ModifierList<T> {

  public int Count { get => filters.Count; }
  public Modifier this[int index] { get => filters[index]; set => filters[index] = value; }
  public int Capacity { get => filters.Capacity; set => filters.Capacity = value; }
  public void Clear() => filters.Clear();
  public bool Contains(Modifier item) => filters.Contains(item);
  public bool Exists(Predicate<Modifier> match) => filters.Exists(match);
  public int FindIndex(int startIndex, int count, Predicate<Modifier> match) => FindIndex(startIndex, count, match);
  public int FindIndex(int startIndex, Predicate<Modifier> match) => FindIndex(startIndex, match);
  public int FindIndex(Predicate<Modifier> match) => FindIndex(match);
  public Modifier FindLast(Predicate<Modifier> match) => FindLast(match);
  public int FindLastIndex(int startIndex, int count, Predicate<Modifier> match) => FindLastIndex(startIndex, count, match);
  public int FindLastIndex(int startIndex, Predicate<Modifier> match) => FindLastIndex(startIndex, match);
  public int FindLastIndex(Predicate<Modifier> match) => FindLastIndex(match);
  public List<Modifier>.Enumerator GetEnumerator() => filters.GetEnumerator();
  public List<Modifier> GetRange(int index, int count) => filters.GetRange(index, count);
  public int IndexOf(Modifier item, int index, int count) => filters.IndexOf(item, index, count);
  public int IndexOf(Modifier item, int index) => filters.IndexOf(item, index);
  public int IndexOf(Modifier item) => filters.IndexOf(item);
  public int LastIndexOf(Modifier item) => filters.LastIndexOf(item);
  public int LastIndexOf(Modifier item, int index) => filters.LastIndexOf(item, index);
  public int LastIndexOf(Modifier item, int index, int count) => filters.LastIndexOf(item);
  public bool Remove(Modifier item) => filters.Remove(item);
  public int RemoveAll(Predicate<Modifier> match) => filters.RemoveAll(match);
  public void RemoveAt(int index) => filters.RemoveAt(index);
  public void RemoveRange(int index, int count) => filters.RemoveRange(index, count);
  public Modifier[] ToArray() => filters.ToArray();
  public void TrimExcess() => filters.TrimExcess();
  public bool TrueForAll(Predicate<Modifier> match) => filters.TrueForAll(match);
  public int BinarySearch(Modifier item) => filters.BinarySearch(item);
  public int BinarySearch(Modifier item, IComparer<Modifier> comparer) => filters.BinarySearch(item, comparer);
  public int BinarySearch(int index, int count, Modifier item, IComparer<Modifier> comparer) => filters.BinarySearch(index, count, item, comparer);


  protected List<Modifier> filters = new List<Modifier>();

  public class Modifier {
    /// <summary>
    /// First parameter passed to function is the modified value and second is the original value. 
    /// Original value is a simple copy of the variable, so a reference type may not keep it's original state!
    /// </summary>
    public Modifier(Func<T, T, T> function, float priority = 0) {
      this.function = function;
      this.priority = priority;
    }
    public Func<T, T, T> function;
    public float priority = 0;
  }

  public T Apply(T value) {
    var orig = value;
    foreach (var filter in filters) value = filter.function(value, orig);
    return value;
  }

  public void AddRange(IEnumerable<Modifier> filters) { foreach (var filter in filters) Add(filter); }

  /// <summary>
  /// First parameter passed to function is the modified value and second is the original value. 
  /// Original value is a simple copy of the variable, so a reference type may not keep it's original state!
  /// </summary>
  public void Add(Func<T, T, T> function, float priority = 0) => Add(new Modifier(function, priority));
  public void Add(Modifier filter) {
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
