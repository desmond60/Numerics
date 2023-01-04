namespace Numerics.Grid;

// % ***** Structure Bound ***** % //
public struct Bound
{
    public int?   NumEdge  { get; set; } /// Number Edge
    public int[]? NumNodes { get; set; } /// Number Nodes
    public int    NumBound { get; set; } /// Number Bound
    public int    NumSide  { get; set; } /// Number Side
    public object Value    { get; set; } /// Value Bound

    //: Constructor (number edge)
    public Bound(int _numBound, int _numSide, object _value, int _numEdge) {
        this.NumBound = _numBound;
        this.NumSide  = _numSide;
        this.Value    = _value;     
        this.NumEdge  = _numEdge;
    }

    //: Constructor (number nodes)
    public Bound(int _numBound, int _numSide, object _value, params int[] _numNodes) {
        this.NumBound = _numBound;
        this.NumSide  = _numSide;
        this.Value    = _value;  
        NumNodes      = new int[_numNodes.Length];
        Array.Copy(_numNodes, NumNodes, _numNodes.Length);
    }

    //: String view structure
    public override string ToString() {
        StringBuilder str = new StringBuilder();
        
        if (NumEdge is null) {
            str.Append($"{NumBound}\t{NumSide}\t{Value}\t\t{NumNodes![0]}\t");
            for (int i = 1; i < NumNodes.Count(); i++)
                str.Append($"{NumNodes[i]}\t");
            return str.ToString();
        } else {
            str.Append($"{NumBound}\t{NumSide}\t{Value}\t{NumEdge}");
            return str.ToString();
        }
    }
}