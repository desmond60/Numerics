namespace Numerics;

// % ***** Class ComplexMatrix ***** % //
public class ComplexMatrix : ICloneable
{
    private Complex[,] matrix;                          /// Matrix
    public int Length  => Rows * Columns;               /// Length Matrix (Count elements in the matrix)
    public int Rows    => matrix.GetUpperBound(0) + 1;  /// Count rows
    public int Columns => matrix.GetUpperBound(1) + 1;  /// Count columns

    /// The determinant of the matrix
    public Complex Determinant => GetDeterminant(this, Rows);

    //: Explicit Conversion
    public static explicit operator Complex[,](ComplexMatrix mat) => mat.matrix;

    //: Implicit Conversion
    public static implicit operator ComplexMatrix(Complex[,] dArray) => new ComplexMatrix(dArray);

    //: Constructor (with dimension)
    public ComplexMatrix(int _rows, int _columns) {
        matrix = new Complex[_rows, _columns];
    }

    //: Constructor (with double array)
    public ComplexMatrix(Complex[,] _mat) {   
        int rows    = _mat.GetUpperBound(0) + 1;
        int columns = _mat.GetUpperBound(1) + 1;     
        matrix = new Complex[rows, columns];
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < columns; j++)
                matrix[i, j] = _mat[i, j];           
    }
 
    //: Indexer
    public Complex this[int i, int j] {
        get { return matrix[i, j];  }
        set { matrix[i, j] = value; }
    }

    //: Filling a matrix
    public static void Fill(ComplexMatrix mat, Complex val) {
        for (int i = 0; i < mat.Rows; i++)
            for (int j = 0; j < mat.Columns; j++)
                mat[i,j] = val;
    }

    //: Clearing the matrix
    public static void Clear(ComplexMatrix mat) {
        for (int i = 0; i < mat.Rows; i++)
            for (int j = 0; j < mat.Columns; j++)
                mat[i,j] = 0;
    }

    //: Matrix transposition
    public ComplexMatrix Transposition() {
        var result = new ComplexMatrix(Columns, Rows);
        for (int i = 0; i < Columns; i++)
            for (int j = 0; j < Rows; j++)
                result[i,j] = matrix[j,i];
        return result;
    }

    //: Calculating the determinant
    private Complex GetDeterminant(ComplexMatrix mat, int dim) {
        
        if (Rows != Columns || dim < 1) return default(Complex);
        if (dim == 1) return mat[0,0];
        if (dim == 2) return mat[0,0] * mat[1,1] - mat[1,0] * mat[0,1];

        Complex det = 0;
        int k = 1;
        if (dim > 2) {
            for (int i = 0; i < dim; i++) {
                var P = mat.GetMinor(i, 0);
                det += k * mat[i,0] * GetDeterminant(P, dim - 1);
                k = -k;
            }
        }
        return det;
    }

    //: Getting the minor of the matrix
    public ComplexMatrix GetMinor(int i, int j) {
        
        if (Rows != Columns) throw new InvalidOperationException();

        var P = new ComplexMatrix(Rows - 1, Rows - 1);
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
    public ComplexMatrix Inverse() {
        
        if (Rows != Columns || Determinant == 0) throw new InvalidOperationException();

        var attached_mat = new ComplexMatrix(Rows, Rows);
        for (int i = 0; i < Rows; i++)
            for (int j = 0; j < Rows; j++)
                attached_mat[i,j] = (i + j) % 2 == 0 ? GetMinor(i,j).Determinant
                                                     : -GetMinor(i,j).Determinant;
        return (1 / Determinant) * attached_mat.Transposition();
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
    public static ComplexMatrix operator -(ComplexMatrix mat) => -1*mat;

    //: Overloading the multiplication of two matrices
    public static ComplexMatrix operator *(ComplexMatrix mat1, ComplexMatrix mat2) {
        
        if (mat1.Columns != mat2.Rows) throw new InvalidOperationException();

        ComplexMatrix result = new ComplexMatrix(mat1.Rows, mat2.Columns);;
        for (int i = 0; i < mat1.Rows; i++)
            for (int j = 0; j < mat2.Columns; j++)
                for (int k = 0; k < mat1.Columns; k++)
                    result[i,j] += mat1[i, k] * mat2[k, j];
        return result;
    }

    //: Overloading the addition of two matrices
    public static ComplexMatrix operator +(ComplexMatrix mat1, ComplexMatrix mat2) {
        
        if (mat1.Rows != mat2.Rows || mat1.Columns != mat2.Columns) throw new InvalidOperationException();

        var result = new ComplexMatrix(mat1.Rows, mat1.Columns);
        for (int i = 0; i < mat1.Rows; i++)
            for (int j = 0; j < mat1.Columns; j++)
                result[i,j] = mat1[i,j] + mat2[i,j];
        return result;
    }

    //: Overloading the subtraction of two matrices
    public static ComplexMatrix operator -(ComplexMatrix mat1, ComplexMatrix mat2) {
        
        if (mat1.Rows != mat2.Rows || mat1.Columns != mat2.Columns) throw new InvalidOperationException();

        var result = new ComplexMatrix(mat1.Rows, mat1.Columns);
        for (int i = 0; i < mat1.Rows; i++)
            for (int j = 0; j < mat1.Columns; j++)
                result[i,j] = mat1[i,j] - mat2[i,j];
        return result;
    }

    //: Overloading multiplication by a constant
    public static ComplexMatrix operator *(double Const, ComplexMatrix mat) {
        var result = new ComplexMatrix(mat.Rows, mat.Columns);
        for (int i = 0; i < mat.Rows; i++)
            for (int j = 0; j < mat.Columns; j++)
                result[i,j] = Const * mat[i,j];
        return result;
    }

    //: Overloading multiplication by a Complex constant
    public static ComplexMatrix operator *(Complex Const, ComplexMatrix mat) {
        var result = new ComplexMatrix(mat.Rows, mat.Columns);
        for (int i = 0; i < mat.Rows; i++)
            for (int j = 0; j < mat.Columns; j++)
                result[i,j] = Const * mat[i,j];
        return result;
    }

    //: Exponentiation of the matrix
    public static ComplexMatrix Pow(ComplexMatrix mat, int degree) {

        if (mat.Columns != mat.Rows) throw new InvalidOperationException();

        var result = new ComplexMatrix(mat.Rows, mat.Columns);
        var mult   = (ComplexMatrix)mat.Clone();
        for (int d = 1; d < degree; d++) {
            ComplexMatrix.Clear(result);
            for (int i = 0; i < mat.Rows; i++)
                for (int j = 0; j < mat.Columns; j++)
                    for (int k = 0; k < mat.Rows; k++)
                        result[i,j] += mult[i,k] * mat[k,j];
            mult = (ComplexMatrix)result.Clone();
        }
        return result;
    }

    //: Copy object Matrix<T>
    public object Clone() => new ComplexMatrix(matrix);

    // % ***** Conversion to other collections ***** % //
    public Complex[,]          ToArray() => matrix;
    public SquareComplexMatrix ToSquareComplexMatrix() {
        if (Rows == Columns) return new SquareComplexMatrix(matrix);
        throw new InvalidCastException();
    }

    // % ***** Interaction with other classes ***** % //

}