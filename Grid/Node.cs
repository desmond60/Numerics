namespace Numerics.Grid;

// % ***** Structure Node ***** % //
public struct Node<T> where T : System.Numerics.INumber<T>  {

    public T X { get; set; }  /// Coordinate X 
    public T Y { get; set; }  /// Coordinate Y 
    public T Z { get; set; }  /// Coordinate Z
    private bool IsTwoCoordinate { get; set; }

    //: Constructor (three coordinates)
    public Node(T _X, T _Y, T _Z) {
        (this.X, this.Y, this.Z) = (_X, _Y, _Z);
        this.IsTwoCoordinate = false;
    }

    //: Constructor (two coordinates)
    public Node(T _X, T _Y) {
        (this.X, this.Y, this.Z) = (_X, _Y, T.Zero);
        this.IsTwoCoordinate = true;
    }

    //: Deconstructor (three coordinates)
    public void Deconstruct(out T x, 
                            out T y,
                            out T z) 
    {
        (x, y, z) = (this.X, this.Y, this.Z);
    }

    //: Deconstructor (two coordinates)
    public void Deconstruct(out T x, 
                            out T y) 
    {
        (x, y) = (this.X, this.Y);
    }

    //: String view structure
    public override string ToString() {
        return IsTwoCoordinate ? $"{X}\t{Y}"
                               : $"{X}\t{Y}\t{Z}";
    }
}