namespace Numerics;

// % ****** Class SquareComplexMatrix ***** % //
public class SquareComplexMatrix : ICloneable
{
    private Complex[,] matrix;                      /// Square matrix
    public int Dim => matrix.GetUpperBound(0) + 1;  /// Dimension of the matrix

    /// The determinant of the matrix
    public Complex Determinant => GetDeterminant(this, Dim);

    //: Explicit Conversion
    public static explicit operator Complex[,](SquareComplexMatrix mat) => mat.matrix;

    //: Implicit Conversion
    public static implicit operator SquareComplexMatrix(Complex[,] dArray) => new SquareComplexMatrix(dArray);

    //: Constructor (with dimension)
    public SquareComplexMatrix(int _dim) {
        matrix = new Complex[_dim, _dim];
    }

    //: Constructor (with double array)
    public SquareComplexMatrix(Complex[,] _mat) {        
        
        if (_mat.GetUpperBound(0) + 1 != _mat.GetUpperBound(1) + 1) throw new InvalidOperationException();

        int dim = _mat.GetUpperBound(0) + 1;
        matrix = new Complex[dim, dim];
        for (int i = 0; i < dim; i++)
            for (int j = 0; j < dim; j++)
                matrix[i, j] = _mat[i, j];           
    }

    //: Indexer
    public Complex this[int i, int j] {
        get { return matrix[i, j];  }
        set { matrix[i, j] = value; }
    }

    //: Filling a matrix
    public static void Fill(SquareComplexMatrix mat, Complex val) {
        for (int i = 0; i < mat.Dim; i++)
            for (int j = 0; j < mat.Dim; j++)
                mat[i,j] = val;
    }

    //: Clearing the matrix
    public static void Clear(SquareComplexMatrix mat) {
        for (int i = 0; i < mat.Dim; i++)
            for (int j = 0; j < mat.Dim; j++)
                mat[i,j] = 0;
    }

    //: Matrix transposition
    public SquareComplexMatrix Transposition() {
        var result = new SquareComplexMatrix(Dim);
        for (int i = 0; i < Dim; i++)
            for (int j = 0; j < Dim; j++)
                result[i,j] = matrix[j,i];
        return result;
    }

    //: Calculating the determinant
    private Complex GetDeterminant(SquareComplexMatrix mat, int dim) {
        
        if (dim <  1) return default(Complex);
        if (dim == 1) return mat[0,0];
        if (dim == 2) return mat[0,0] * mat[1,1] - mat[1,0] * mat[0,1];

        Complex det = 0;
        int k = 1;
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
    public SquareComplexMatrix GetMinor(int i, int j) {
        var P = new SquareComplexMatrix(Dim - 1);
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
    public SquareComplexMatrix Inverse() {
        
        if (Determinant == 0) throw new InvalidOperationException();

        var attached_mat = new SquareComplexMatrix(Dim);
        for (int i = 0; i < Dim; i++)
            for (int j = 0; j < Dim; j++)
                attached_mat[i,j] = (i + j) % 2 == 0 ? GetMinor(i,j).Determinant
                                                     : -GetMinor(i,j).Determinant;
        return (1 / Determinant) * attached_mat.Transposition();
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
    public static SquareComplexMatrix operator -(SquareComplexMatrix mat) => -1*mat;

    //: Overloading the multiplication of two matrices
    public static SquareComplexMatrix operator *(SquareComplexMatrix mat1, SquareComplexMatrix mat2) {
        SquareComplexMatrix result = new SquareComplexMatrix(mat1.Dim);
        for (int i = 0; i < mat1.Dim; i++)
            for (int j = 0; j < mat1.Dim; j++)
                for (int k = 0; k < mat1.Dim; k++)
                    result[i,j] += mat1[i, k] * mat2[k, j];
        return result;
    }

    //: Overloading the addition of two matrices
    public static SquareComplexMatrix operator +(SquareComplexMatrix mat1, SquareComplexMatrix mat2) {
        var result = new SquareComplexMatrix(mat1.Dim);
        for (int i = 0; i < mat1.Dim; i++)
            for (int j = 0; j < mat1.Dim; j++)
                result[i,j] = mat1[i,j] + mat2[i,j];
        return result;
    }

    //: Overloading the subtraction of two matrices
    public static SquareComplexMatrix operator -(SquareComplexMatrix mat1, SquareComplexMatrix mat2) {
        var result = new SquareComplexMatrix(mat1.Dim);
        for (int i = 0; i < mat1.Dim; i++)
            for (int j = 0; j < mat1.Dim; j++)
                result[i,j] = mat1[i,j] - mat2[i,j];
        return result;
    }

    //: Overloading multiplication by a constant
    public static SquareComplexMatrix operator *(double Const, SquareComplexMatrix mat) {
        var result = new SquareComplexMatrix(mat.Dim);
        for (int i = 0; i < mat.Dim; i++)
            for (int j = 0; j < mat.Dim; j++)
                result[i,j] = Const * mat[i,j];
        return result;
    }

    //: Overloading multiplication by a Complex constant
    public static SquareComplexMatrix operator *(Complex Const, SquareComplexMatrix mat) {
        var result = new SquareComplexMatrix(mat.Dim);
        for (int i = 0; i < mat.Dim; i++)
            for (int j = 0; j < mat.Dim; j++)
                result[i,j] = Const * mat[i,j];
        return result;
    }

    //: Exponentiation of the matrix
    public static SquareComplexMatrix Pow(SquareComplexMatrix mat, int degree) {
        var result = new SquareComplexMatrix(mat.Dim);
        var mult   = (SquareComplexMatrix)mat.Clone();
        for (int d = 1; d < degree; d++) {
            SquareComplexMatrix.Clear(result);
            for (int i = 0; i < mat.Dim; i++)
                for (int j = 0; j < mat.Dim; j++)
                    for (int k = 0; k < mat.Dim; k++)
                        result[i,j] += mult[i,k] * mat[k,j];
            mult = (SquareComplexMatrix)result.Clone();
        }
        return result;
    }

    //: Copy object SquareMatrix<T>
    public object Clone() => new SquareComplexMatrix(matrix);

    // % ***** Conversion to other collections ***** % //
    public Complex[,]     ToArray()         => matrix;
    public ComplexMatrix  ToComplexMatrix() => new ComplexMatrix(matrix);
    
    // % ***** Interaction with other classes ***** % //
    //: Overloading multiplication by a ComplexVector
    public static ComplexVector operator *(SquareComplexMatrix mat, ComplexVector vec) {

        if (vec.Length != mat.Dim) throw new InvalidOperationException();

        var result = new ComplexVector(vec.Length); 
        for (int i = 0; i < mat.Dim; i++)
            for (int j = 0; j < mat.Dim; j++)
                result[i] += (dynamic)mat[i,j] * vec[j];
        return result;
    }

    //: Overloading multiplication by a ComplexMatrix
    public static ComplexMatrix operator *(SquareComplexMatrix mat1, ComplexMatrix mat2) {

        if (mat1.Dim != mat2.Rows) throw new InvalidOperationException();

        var result = new ComplexMatrix(mat1.Dim, mat2.Columns);
        for (int i = 0; i < mat1.Dim; i++)
            for (int j = 0; j < mat2.Columns; j++)
                for (int k = 0; k < mat1.Dim; k++)
                    result[i,j] += mat1[i, k] * mat2[k, j];
        return result;
    }

    //: Overloading addition by a ComplexMatrix
    public static SquareComplexMatrix operator +(SquareComplexMatrix mat1, ComplexMatrix mat2) => mat2 + mat1;

    //: Overloading subtraction by a SquareComplexMatrix
    public static SquareComplexMatrix operator -(SquareComplexMatrix mat1, ComplexMatrix mat2) {

        if (mat1.Dim != mat2.Rows || mat2.Columns != mat1.Dim) throw new InvalidOperationException();

        var result = new SquareComplexMatrix(mat1.Dim);
        for (int i = 0; i < mat1.Dim; i++)
            for (int j = 0; j < mat1.Dim; j++)
                    result[i,j] = mat1[i, j] - mat2[i, j];
        return result;
    }
}