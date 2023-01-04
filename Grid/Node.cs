namespace Numerics.Grid;

// % ***** Structure Node ***** % //
public struct Node<T> where T : System.Numerics.INumber<T>
{
    public T  X { get; set; }  /// Coordinate X 
    public T  Y { get; set; }  /// Coordinate Y 
    public T? Z { get; set; }  /// Coordinate Z

    //: Constructor (three coordinates)
    public Node(T _X, T _Y, T _Z) {
        (this.X, this.Y, this.Z) = (_X, _Y, _Z);
    }

    //: Constructor (two coordinates)
    public Node(T _X, T _Y) {
        (this.X, this.Y) = (_X, _Y);
    }

    //: Deconstructor (three coordinates)
    public void Deconstruct(out T x, 
                            out T y,
                            out T z) 
    {
        (x, y, z) = (this.X, this.Y, this.Z!);
    }

    //: Deconstructor (two coordinates)
    public void Deconstruct(out T x, 
                            out T y) 
    {
        (x, y) = (this.X, this.Y);
    }

    //: String view structure
    public override string ToString() {
        return Z is null ? $"{X}\t{Y}"
                         : $"{X}\t{Y}\t{Z}";
    }
}