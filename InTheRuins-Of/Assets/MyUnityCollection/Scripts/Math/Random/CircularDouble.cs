
/// <summary> Creates a double which loops after the specified maximum value to zero </summary>
public readonly struct CircularDouble {

  public readonly double value;
  public readonly double max;

  public static CircularDouble operator +(CircularDouble a, double b) => new CircularDouble(a.value + b, a.max);
  public static CircularDouble operator -(CircularDouble a, double b) => new CircularDouble(a.value - b, a.max);


  /// <summary> Creates an integer value which loops after the maximum to zero </summary>
  public CircularDouble(double value, double max) {

    if (max <= 0) throw new System.Exception($"Max value for {nameof(CircularDouble)} must be higher than zero");

    if (value > max) value = value % max;
    else if (value < 0) value = max + value % max;

    this.value = value;
    this.max = max;
  }
}