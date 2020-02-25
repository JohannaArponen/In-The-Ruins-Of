
/// <summary> Creates a decimal which loops after the specified maximum value to zero </summary>
public readonly struct CircularDecimal {

  public readonly decimal value;
  public readonly decimal max;

  public static CircularDecimal operator +(CircularDecimal a, decimal b) => new CircularDecimal(a.value + b, a.max);
  public static CircularDecimal operator -(CircularDecimal a, decimal b) => new CircularDecimal(a.value - b, a.max);


  /// <summary> Creates an integer value which loops after the maximum to zero </summary>
  public CircularDecimal(decimal value, decimal max) {

    if (max <= 0) throw new System.Exception($"Max value for {nameof(CircularDecimal)} must be higher than zero");

    if (value > max) value = value % max;
    else if (value < 0) value = max + value % max;

    this.value = value;
    this.max = max;
  }
}