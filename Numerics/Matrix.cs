namespace Numerics;

// % ****** Class Matrix ***** % //
public class Matrix<T> : ICloneable
                where T : System.Numerics.INumber<T>
{
    private T[,] matrix;                                /// Matrix
    public int Length  => Rows * Columns;               /// Lenght Matrix (Count elements in the matrix)
    public int Rows    => matrix.GetUpperBound(0) + 1;  /// Count rows
    public int Columns => matrix.GetUpperBound(1) + 1;  /// Count columns

    /// The determinant of the matrix
    public T? Determinant => GetDeterminant(this, Rows);

    //: Explicit Conversion
    public static explicit operator T[,](Matrix<T> mat) => mat.matrix;

    //: Implicit Conversion
    public static implicit operator Matrix<T>(T[,] dArray) => new Matrix<T>(dArray);

    //: Constructor (with dimension)
    public Matrix(int _rows, int _columns) {
        matrix = new T[_rows, _columns];
    }

    //: Constructor (with double array)
    public Matrix(T[,] _mat) {   
        int rows    = _mat.GetUpperBound(0) + 1;
        int columns = _mat.GetUpperBound(1) + 1;     
        matrix = new T[rows, columns];
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < columns; j++)
                matrix[i, j] = _mat[i, j];           
    }

    //: Indexer
    public T this[int i, int j] {
        get { return matrix[i, j];  }
        set { matrix[i, j] = value; }
    }

    //: Filling a matrix
    public static void Fill(Matrix<T> mat, T val) {
        for (int i = 0; i < mat.Rows; i++)
            for (int j = 0; j < mat.Columns; j++)
                mat[i,j] = val;
    }

    //: Clearing the matrix
    public static void Clear(Matrix<T> mat) {
        for (int i = 0; i < mat.Rows; i++)
            for (int j = 0; j < mat.Columns; j++)
                mat[i,j] = T.Zero;
    }

    //: Matrix transposition
    public Matrix<T> Transposition() {

        var result = new Matrix<T>(Columns, Rows);
        for (int i = 0; i < Columns; i++)
            for (int j = 0; j < Rows; j++)
                result[i,j] = matrix[j,i];
        return result;
    }

    //: Calculating the determinant
    private T? GetDeterminant(Matrix<T> mat, int dim) {
        
        if (Rows != Columns || dim < 1) return default(T);
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
    public Matrix<T> GetMinor(int i, int j) {
        
        if (Rows != Columns) throw new InvalidOperationException();

        var P = new Matrix<T>(Rows - 1, Rows - 1);
        int di = 0;
        for (int ki = 0; ki < Rows - 1; ki++) {
            if (ki == i) di = 1;
            int dj = 0;
            for (int kj = 0; kj < Rows - 1; kj++) {
                if (kj == j) dj = 1;
                P[ki,kj] = this[ki + di, kj + dj];
            }
        }
        return P;
    }

    //: Getting the inverse matrix
    public Matrix<T> Inverse() {
        
        if (Rows != Columns || Determinant == T.Zero) throw new InvalidOperationException();

        var attached_mat = new Matrix<T>(Rows, Rows);
        for (int i = 0; i < Rows; i++)
            for (int j = 0; j < Rows; j++)
                attached_mat[i,j] = (i + j) % 2 == 0 ? GetMinor(i,j).Determinant!
                                                     : -GetMinor(i,j).Determinant!;
        return (T.One / Determinant!) * attached_mat.Transposition();
    }

    //: String view of the matrix
    public override string ToString() {
        StringBuilder str = new StringBuilder();
        if (Length == 0) return $"[ ]";

        for (int i = 0; i < Rows; i++) {
            for (int j = 0; j < Columns; j++)
                str.Append($"{matrix[i,j]}\t");
            str.Append($"\n");
        }

        return str.ToString();
    }

    //: Overloading of the ternary minus
    public static Matrix<T> operator -(Matrix<T> mat) => -T.One*mat;

    //: Overloading the multiplication of two matrices
    public static Matrix<T> operator *(Matrix<T> mat1, Matrix<T> mat2) {
        
        if (mat1.Columns != mat2.Rows) throw new InvalidOperationException();

        Matrix<T> result = new Matrix<T>(mat1.Rows, mat2.Columns);;
        for (int i = 0; i < mat1.Rows; i++)
            for (int j = 0; j < mat2.Columns; j++)
                for (int k = 0; k < mat1.Columns; k++)
                    result[i,j] += mat1[i, k] * mat2[k, j];
        return result;
    }

    //: Overloading the addition of two matrices
    public static Matrix<T> operator +(Matrix<T> mat1, Matrix<T> mat2) {
        
        if (mat1.Rows != mat2.Rows || mat1.Columns != mat2.Columns) throw new InvalidOperationException();

        var result = new Matrix<T>(mat1.Rows, mat1.Columns);
        for (int i = 0; i < mat1.Rows; i++)
            for (int j = 0; j < mat1.Columns; j++)
                result[i,j] = mat1[i,j] + mat2[i,j];
        return result;
    }

    //: Overloading the subtraction of two matrices
    public static Matrix<T> operator -(Matrix<T> mat1, Matrix<T> mat2) {
        
        if (mat1.Rows != mat2.Rows || mat1.Columns != mat2.Columns) throw new InvalidOperationException();

        var result = new Matrix<T>(mat1.Rows, mat1.Columns);
        for (int i = 0; i < mat1.Rows; i++)
            for (int j = 0; j < mat1.Columns; j++)
                result[i,j] = mat1[i,j] - mat2[i,j];
        return result;
    }

    //: Overloading multiplication by a constant
    public static Matrix<T> operator *(T Const, Matrix<T> mat) {
        var result = new Matrix<T>(mat.Rows, mat.Columns);
        for (int i = 0; i < mat.Rows; i++)
            for (int j = 0; j < mat.Columns; j++)
                result[i,j] = Const * mat[i,j];
        return result;
    }

    //: Exponentiation of the matrix
    public static Matrix<T> Pow(Matrix<T> mat, int degree) {

        if (mat.Columns != mat.Rows) throw new InvalidOperationException();

        var result = new Matrix<T>(mat.Rows, mat.Columns);
        var mult   = (Matrix<T>)mat.Clone();
        for (int d = 1; d < degree; d++) {
            Matrix<T>.Clear(result);
            for (int i = 0; i < mat.Rows; i++)
                for (int j = 0; j < mat.Columns; j++)
                    for (int k = 0; k < mat.Rows; k++)
                        result[i,j] += mult[i,k] * mat[k,j];
            mult = (Matrix<T>)result.Clone();
        }
        return result;
    }

    //: Copy object Matrix<T>
    public object Clone() => new Matrix<T>(matrix);

    // % ***** Conversion to other collections ***** % //
    public T[,]            ToArray() => matrix;
    public SquareMatrix<T> ToSquareMatrix() {
        if (Rows == Columns) return new SquareMatrix<T>(matrix);
        throw new InvalidCastException();
    }

    // % ***** Interaction with other classes ***** % //
    
}