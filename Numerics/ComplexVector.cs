namespace Numerics;

// % *****  Class ComplexVector ***** //
public class ComplexVector : IEnumerable, ICloneable
{
    public Complex[] vector;            /// Vector
    public int Length => vector.Length; /// Length vector

    /// Calculating the norm of a vector
    public double Norm => GetNorm(this);

    //: Реализация IEnumerable
    public IEnumerator GetEnumerator() => vector.GetEnumerator();

    //: Explicit Conversion
    public static explicit operator Complex[](ComplexVector vec) => vec.vector;

    //: Implicit Conversion
    public static implicit operator ComplexVector(Complex[] array) => new ComplexVector(array);

    //: Constructor (with dimension)
    public ComplexVector(int length) => vector = new Complex[length];

    //: Constructor (with array)
    public ComplexVector(Complex[] array) {
        vector = new Complex[array.Length];
        Array.Copy(array, vector, array.Length);
    }

    //: Indexer
    public Complex this[int index] {
        get => vector[index];
        set => vector[index] = value;
    }

    //: Overloading the multiplication of two vectors
    public static Complex operator *(ComplexVector vec1, ComplexVector vec2) {
        Complex result = 0;
        for (int i = 0; i < vec1.Length; i++)
            result += vec1[i] * vec2[i];
        return result;
    }

    //: Overloading multiplication by a constant
    public static ComplexVector operator *(double Const, ComplexVector vec) {
        var result = new ComplexVector(vec.Length);
        for (int i = 0; i < vec.Length; i++)
            result[i] = Const * vec[i];
        return result;
    }
    public static ComplexVector operator *(ComplexVector vec, double Const) => Const * vec;

    //: Overloading multiplication by a Complex constant
    public static ComplexVector operator *(Complex Const, ComplexVector vec) {
        var result = new ComplexVector(vec.Length);
        for (int i = 0; i < vec.Length; i++)
            result[i] = Const * vec[i];
        return result;
    }
    public static ComplexVector operator *(ComplexVector vec, Complex Const) => Const * vec;

    //: Overloading division by constant
    public static ComplexVector operator /(ComplexVector vec, double Const) {
        var result = new ComplexVector(vec.Length);
        for (int i = 0; i < vec.Length; i++)
            result[i] = vec[i] / Const;
        return result;
    }

    //: Overloading division by Complex constant
    public static ComplexVector operator /(ComplexVector vec, Complex Const) {
        var result = new ComplexVector(vec.Length);
        for (int i = 0; i < vec.Length; i++)
            result[i] = vec[i] / Const;
        return result;
    }

    //: Overloading the addition of two vectors
    public static ComplexVector operator +(ComplexVector vec1, ComplexVector vec2) {
        var result = new ComplexVector(vec1.Length);
        for (int i = 0; i < vec1.Length; i++)
            result[i] = vec1[i] + vec2[i];
        return result;
    }

    //: Overloading the subtraction of two vectors
    public static ComplexVector operator -(ComplexVector vec1, ComplexVector vec2) {
        var result = new ComplexVector(vec1.Length);
        for (int i = 0; i < vec1.Length; i++)
            result[i] = vec1[i] - vec2[i];
        return result;
    }

    //: Overloading of the ternary minus
    public static ComplexVector operator -(ComplexVector vec) => -1*vec;

    //: Scalar product of vectors
    public static Complex Scalar(ComplexVector vec1, ComplexVector vec2) => vec1 * vec2;

    //: The module of a complex vector
    public static double GetNorm(ComplexVector vec) {
        double norm = 0;
        for (int i = 0; i < vec.Length; i++)
            norm += vec[i].Real*vec[i].Real + vec[i].Imaginary*vec[i].Imaginary;
        return Sqrt(norm);
    }

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
    public object Clone() => new ComplexVector(vector);

    public void CopyTo(Complex[] array, int index) {
        vector.CopyTo(array, index);
    }

    public void CopyTo(int index, Complex[] array, int arrayIndex, int count) {
        vector.ToList().CopyTo(index, array, arrayIndex, count);
    }

    // % ***** Conversion to other collections ***** % //
    public Complex[]        ToArray()   => vector;
    public List<Complex>    ToList()    => vector.ToList();
    public HashSet<Complex> ToHashSet() => vector.ToHashSet();
    public ImmutableArray<Complex> ToImmutableArray() => ImmutableArray.Create(vector);

    // % ****** Array methods ***** //
    //: Filling a vector
    public static void Fill(ComplexVector vec, Complex val) {
        for (int i = 0; i < vec.Length; i++)
            vec[i] = val;
    }

    //: Clearing the vector
    public static void Clear(ComplexVector vec) {
        for (int i = 0; i < vec.Length; i++)
            vec[i] = 0;
    }

    //: Increasing the dimension of the vector
    public static void Resize(ref ComplexVector vec, int lenght) {
        var vecClone = (ComplexVector)vec.Clone();
        vec = new ComplexVector(lenght);
        for (int i = 0; i < vecClone.Length; i++)
            vec[i] = vecClone[i];            
    }

    //: Binary search in sorted array
    public static int BinarySearch(ComplexVector vec, Complex val) {
        return Array.BinarySearch((Complex[])vec, val);
    }

    //: Sorting vector
    public static void Sort(ComplexVector vec) {
        Array.Sort((Complex[])vec);
    }

    //: Checking the vector contains element that satisfy delegate
    public static bool Exists(ComplexVector vec, Predicate<Complex> match) {
        return Array.Exists((Complex[])vec, match);
    }

    //: Search first element the vector that satisfy delegate
    public static Complex? Find(ComplexVector vec, Predicate<Complex> match) {
        return Array.Find((Complex[])vec, match);
    }

    //: Search last element the vector that satisfy delegate
    public static Complex? FindLast(ComplexVector vec, Predicate<Complex> match) {
        return Array.FindLast((Complex[])vec, match);
    }

    //: Search index element the vector that satisfy delegate
    public static int FindIndex(ComplexVector vec, Predicate<Complex> match) {
        return Array.FindIndex((Complex[])vec, match);
    }

    //: Search last index element the vector that satisfy delegate
    public static int FindLastIndex(ComplexVector vec, Predicate<Complex> match) {
        return Array.FindLastIndex((Complex[])vec, match);
    }

    //: Search all element the vector that satisfy delegate
    public static ComplexVector FindAll(ComplexVector vec, Predicate<Complex> match) {
        return new ComplexVector(Array.FindAll((Complex[])vec, match));
    }

    //: Search index element the vector
    public static int IndexOf(ComplexVector vec, Complex val) {
        return Array.IndexOf((Complex[])vec, val);
    }

    //: Search last index element the vector
    public static int LastIndexOf(ComplexVector vec, Complex val) {
        return Array.LastIndexOf((Complex[])vec, val);
    }

    // % ***** Interaction with other classes ***** % //
    
}