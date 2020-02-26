
/// <summary> Creates a float which loops after the specified maximum value to zero </summary>
public readonly struct CircularFloat {

  public readonly float value;
  public readonly float max;

  public static CircularFloat operator +(CircularFloat a, float b) => new CircularFloat(a.value + b, a.max);
  public static CircularFloat operator -(CircularFloat a, float b) => new CircularFloat(a.value - b, a.max);

  public static explicit operator int(CircularFloat a) => (int)a.value;
  public static implicit operator float(CircularFloat a) => a.value;
  public static implicit operator double(CircularFloat a) => a.value;
  public static explicit operator decimal(CircularFloat a) => (decimal)a.value;

  /// <summary> Creates an integer value which loops after the maximum to zero </summary>
  public CircularFloat(float value, float max) {

    if (max <= 0) throw new System.Exception($"Max value for {nameof(CircularFloat)} must be higher than zero");

    if (value > max) value = value % max;
    else if (value < 0) value = max + value % max;

    this.value = value;
    this.max = max;
  }
}