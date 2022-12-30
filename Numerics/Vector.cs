namespace Numerics;

// % ****** Class Vector ***** % //
public class Vector<T> : ICloneable, IEnumerable
                where T : System.Numerics.INumber<T>
{
    private T[] vector;                                                /// Vector
    public int Length => vector.Length;                                /// Lenght vector
    public int PositiveElem => vector.Where(n => n > T.Zero).Count();  /// The count positive elem in the vector
    public int NegativeElem => vector.Where(n => n < T.Zero).Count();  /// The count negative elem in the vector
    
    /// Calculating the norm of a vector
    public double Norm => Sqrt(Double.Parse(Vector<T>.Scalar(this, this).ToString()!));

    //: Enumerator
    public IEnumerator GetEnumerator() => vector.GetEnumerator();

    //: Explicit Conversion
    public static explicit operator T[](Vector<T> vec) => vec.vector;

    //: Implicit Conversion
    public static implicit operator Vector<T>(T[] array) => new Vector<T>(array);

    //: Constructor (with dimension)
    public Vector(int lenght) => vector = new T[lenght];

    //: Constructor (with array)
    public Vector(T[] array) {
        vector = new T[array.Length];
        Array.Copy(array, vector, array.Length);
    }

    //: Indexer
    public T this[int index] {
        get => vector[index];
        set => vector[index] = value;
    }

    //: Overloading the multiplication of two vectors
    public static T operator *(Vector<T> vec1, Vector<T> vec2) {
        T result = T.Zero;
        for (int i = 0; i < vec1.Length; i++)
            result += vec1[i] * vec2[i];
        return result;
    }

    //: Overloading multiplication by a constant
    public static Vector<T> operator *(T Const, Vector<T> vec) {
        var result = new Vector<T>(vec.Length);
        for (int i = 0; i < vec.Length; i++)
            result[i] = Const * vec[i];
        return result;
    }
    public static Vector<T> operator *(Vector<T> vec, T Const) => Const * vec;

    //: Overloading division by constant
    public static Vector<T> operator /(Vector<T> vec, T Const) {
        var result = new Vector<T>(vec.Length);
        for (int i = 0; i < vec.Length; i++)
            result[i] = vec[i] / Const;
        return result;
    }

    //: Overloading the addition of two vectors
    public static Vector<T> operator +(Vector<T> vec1, Vector<T> vec2) {
        var result = new Vector<T>(vec1.Length);
        for (int i = 0; i < vec1.Length; i++)
            result[i] = vec1[i] + vec2[i];
        return result;
    }

    //: Overloading the subtraction of two vectors
    public static Vector<T> operator -(Vector<T> vec1, Vector<T> vec2) {
        var result = new Vector<T>(vec1.Length);
        for (int i = 0; i < vec1.Length; i++)
            result[i] = vec1[i] - vec2[i];
        return result;
    }

    //: Overloading of the ternary minus
    public static Vector<T> operator -(Vector<T> vec) => -T.One*vec;

    //: Scalar product of vectors
    public static T Scalar(Vector<T> vec1, Vector<T> vec2) => vec1 * vec2;

    //: String view of the vector
    public override string ToString() {
        StringBuilder str = new StringBuilder();
        if (Length == 0) return $"[ ]";

        str.Append("[");
        for (int i = 0; i < Length - 1; i++)
            str.Append(vector[i].ToString() + "\t");
        str.Append($"{vector[^1]}]");

        return str.ToString();
    }


    // % ***** Copy object ***** % //
    public object Clone() => new Vector<T>(vector);

    public void CopyTo(T[] array, int index) {
        vector.CopyTo(array, index);
    }

    public void CopyTo(int index, T[] array, int arrayIndex, int count) {
        vector.ToList().CopyTo(index, array, arrayIndex, count);
    }


    // % ***** Conversion to other collections ***** % //
    public T[]        ToArray()   => vector;
    public List<T>    ToList()    => vector.ToList();
    public HashSet<T> ToHashSet() => vector.ToHashSet();
    public ImmutableArray<T> ToImmutableArray() => ImmutableArray.Create(vector);

    // % ****** Array methods ***** //
    //: Filling a vector
    public static void Fill(Vector<T> vec, T val) {
        for (int i = 0; i < vec.Length; i++)
            vec[i] = val;
    }

    //: Clearing the vector
    public static void Clear(Vector<T> vec) {
        for (int i = 0; i < vec.Length; i++)
            vec[i] = T.Zero;
    }

    //: Increasing the dimension of the vector
    public static void Resize(ref Vector<T> vec, int lenght) {
        var vecClone = (Vector<T>)vec.Clone();
        vec = new Vector<T>(lenght);
        for (int i = 0; i < vecClone.Length; i++)
            vec[i] = vecClone[i];            
    }

    //: Binary search in sorted array
    public static int BinarySearch(Vector<T> vec, T val) {
        return Array.BinarySearch((T[])vec, val);
    }

    //: Sorting vector
    public static void Sort(Vector<T> vec) {
        Array.Sort((T[])vec);
    }

    //: Checking the vector contains element that satisfy delegate
    public static bool Exists(Vector<T> vec, Predicate<T> match) {
        return Array.Exists((T[])vec, match);
    }

    //: Search first element the vector that satisfy delegate
    public static T? Find(Vector<T> vec, Predicate<T> match) {
        return Array.Find((T[])vec, match);
    }

    //: Search last element the vector that satisfy delegate
    public static T? FindLast(Vector<T> vec, Predicate<T> match) {
        return Array.FindLast((T[])vec, match);
    }

    //: Search index element the vector that satisfy delegate
    public static int FindIndex(Vector<T> vec, Predicate<T> match) {
        return Array.FindIndex((T[])vec, match);
    }

    //: Search last index element the vector that satisfy delegate
    public static int FindLastIndex(Vector<T> vec, Predicate<T> match) {
        return Array.FindLastIndex((T[])vec, match);
    }

    //: Search all element the vector that satisfy delegate
    public static Vector<T> FindAll(Vector<T> vec, Predicate<T> match) {
        return new Vector<T>(Array.FindAll((T[])vec, match));
    }

    //: Search index element the vector
    public static int IndexOf(Vector<T> vec, T val) {
        return Array.IndexOf((T[])vec, val);
    }

    //: Search last index element the vector
    public static int LastIndexOf(Vector<T> vec, T val) {
        return Array.LastIndexOf((T[])vec, val);
    }

    // % ***** Interaction with other classes ***** % //

}