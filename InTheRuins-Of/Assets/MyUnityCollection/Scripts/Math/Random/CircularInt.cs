
/// <summary> Creates an integer which loops after the specified maximum value to zero </summary>
public readonly struct CircularInt {

  public readonly int value;
  public readonly int max;

  public static CircularInt operator +(CircularInt a, int b) => new CircularInt(a.value + b, a.max);
  public static CircularInt operator -(CircularInt a, int b) => new CircularInt(a.value - b, a.max);


  /// <summary> Creates an integer value which loops after the maximum to zero </summary>
  public CircularInt(int value, int max) {

    if (max <= 0) throw new System.Exception($"Max value for {nameof(CircularInt)} must be higher than zero");

    if (value > max) value = value % max;
    else if (value < 0) value = max + value % max;

    this.value = value;
    this.max = max;
  }
}