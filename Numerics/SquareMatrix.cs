namespace Numerics;

// % ****** Class SquareMatrix ***** % //
public class SquareMatrix<T> : ICloneable
                where T : System.Numerics.INumber<T>
{
    private T[,] matrix;                            /// Square matrix
    public int Dim => matrix.GetUpperBound(0) + 1;  /// Dimension of the matrix

    /// The determinant of the matrix
    public T? Determinant => GetDeterminant(this, Dim);

    //: Explicit Conversion
    public static explicit operator T[,](SquareMatrix<T> mat) => mat.matrix;

    //: Implicit Conversion
    public static implicit operator SquareMatrix<T>(T[,] dArray) => new SquareMatrix<T>(dArray);

    //: Constructor (with dimension)
    public SquareMatrix(int _dim) {
        matrix = new T[_dim, _dim];
    }

    //: Constructor (with double array)
    public SquareMatrix(T[,] _mat) {        
        int dim = _mat.GetUpperBound(0) + 1;
        matrix = new T[dim, dim];
        for (int i = 0; i < dim; i++)
            for (int j = 0; j < dim; j++)
                matrix[i, j] = _mat[i, j];           
    }

    //: Indexer
    public T this[int i, int j] {
        get { return matrix[i, j];  }
        set { matrix[i, j] = value; }
    }

    //: Filling a matrix
    public static void Fill(SquareMatrix<T> mat, T val) {
        for (int i = 0; i < mat.Dim; i++)
            for (int j = 0; j < mat.Dim; j++)
                mat[i,j] = val;
    }

    //: Clearing the matrix
    public static void Clear(SquareMatrix<T> mat) {
        for (int i = 0; i < mat.Dim; i++)
            for (int j = 0; j < mat.Dim; j++)
                mat[i,j] = T.Zero;
    }

    //: Matrix transposition
    public SquareMatrix<T> Transposition() {
        var result = new SquareMatrix<T>(Dim);
        for (int i = 0; i < Dim; i++)
            for (int j = 0; j < Dim; j++)
                result[i,j] = matrix[j,i];
        return result;
    }

    //: Calculating the determinant
    private T? GetDeterminant(SquareMatrix<T> mat, int dim) {
        
        if (dim <  1) return default(T);
        if (dim == 1) return mat[0,0];
        if (dim == 2) return mat[0,0] * mat[1,1] - mat[1,0] * mat[0,1];

        T det = T.Zero;
        T k = T.One;
        if (dim > 2) {
            for (int i = 0; i < dim; i++) {
                var P = mat.GetMinor(i, 0);
                det += k * mat[i,0] * GetDeterminant(P, dim - 1)!;
                k = -k;
            }
        }
        return det;
    }

    //: Getting the minor of the matrix
    public SquareMatrix<T> GetMinor(int i, int j) {
        var P = new SquareMatrix<T>(Dim - 1);
        int di = 0;
        for (int ki = 0; ki < Dim - 1; ki++) {
            if (ki == i) di = 1;
            int dj = 0;
            for (int kj = 0; kj < Dim - 1; kj++) {
                if (kj == j) dj = 1;
                P[ki,kj] = this[ki + di, kj + dj];
            }
        }
        return P;
    }

    //: Getting the inverse matrix
    public SquareMatrix<T> Inverse() {
        
        if (Determinant == T.Zero) throw new InvalidOperationException();

        var attached_mat = new SquareMatrix<T>(Dim);
        for (int i = 0; i < Dim; i++)
            for (int j = 0; j < Dim; j++)
                attached_mat[i,j] = (i + j) % 2 == 0 ? GetMinor(i,j).Determinant!
                                                     : -GetMinor(i,j).Determinant!;
        return (T.One / Determinant!) * attached_mat.Transposition();
    }

    //: String view of the matrix
    public override string ToString() {
        StringBuilder str = new StringBuilder();
        if (Dim == 0) return $"[ ]";

        for (int i = 0; i < Dim; i++) {
            for (int j = 0; j < Dim; j++)
                str.Append($"{matrix[i,j]}\t");
            str.Append($"\n");
        }

        return str.ToString();
    }

    //: Overloading of the ternary minus
    public static SquareMatrix<T> operator -(SquareMatrix<T> mat) => -T.One*mat;

    //: Overloading the multiplication of two matrices
    public static SquareMatrix<T> operator *(SquareMatrix<T> mat1, SquareMatrix<T> mat2) {
        SquareMatrix<T> result = new SquareMatrix<T>(mat1.Dim);;
        for (int i = 0; i < mat1.Dim; i++)
            for (int j = 0; j < mat1.Dim; j++)
                for (int k = 0; k < mat1.Dim; k++)
                    result[i,j] += mat1[i, k] * mat2[k, j];
        return result;
    }

    //: Overloading the addition of two matrices
    public static SquareMatrix<T> operator +(SquareMatrix<T> mat1, SquareMatrix<T> mat2) {
        var result = new SquareMatrix<T>(mat1.Dim);
        for (int i = 0; i < mat1.Dim; i++)
            for (int j = 0; j < mat1.Dim; j++)
                result[i,j] = mat1[i,j] + mat2[i,j];
        return result;
    }

    //: Overloading the subtraction of two matrices
    public static SquareMatrix<T> operator -(SquareMatrix<T> mat1, SquareMatrix<T> mat2) {
        var result = new SquareMatrix<T>(mat1.Dim);
        for (int i = 0; i < mat1.Dim; i++)
            for (int j = 0; j < mat1.Dim; j++)
                result[i,j] = mat1[i,j] - mat2[i,j];
        return result;
    }

    //: Overloading multiplication by a constant
    public static SquareMatrix<T> operator *(T Const, SquareMatrix<T> mat) {
        var result = new SquareMatrix<T>(mat.Dim);
        for (int i = 0; i < mat.Dim; i++)
            for (int j = 0; j < mat.Dim; j++)
                result[i,j] = Const * mat[i,j];
        return result;
    }

    //: Exponentiation of the matrix
    public static SquareMatrix<T> Pow(SquareMatrix<T> mat, int degree) {
        var result = new SquareMatrix<T>(mat.Dim);
        var mult   = (SquareMatrix<T>)mat.Clone();
        for (int d = 1; d < degree; d++) {
            SquareMatrix<T>.Clear(result);
            for (int i = 0; i < mat.Dim; i++)
                for (int j = 0; j < mat.Dim; j++)
                    for (int k = 0; k < mat.Dim; k++)
                        result[i,j] += mult[i,k] * mat[k,j];
            mult = (SquareMatrix<T>)result.Clone();
        }
        return result;
    }

    //: Copy object SquareMatrix<T>
    public object Clone() => new SquareMatrix<T>(matrix);

    // % ***** Conversion to other collections ***** % //
    public T[,]       ToArray()  => matrix;
    public Matrix<T>  ToMatrix() => new Matrix<T>(matrix);

    // % ***** Interaction with other classes ***** % //
    
}