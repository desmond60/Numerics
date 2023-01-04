namespace Numerics.Grid;

// % ***** Structure Grid ***** % //
public struct Grid<T> where T : System.Numerics.INumber<T>
{
    public int CountNode  => Nodes.Length;
    public int CountElem  => Elems.Length;
    public int CountBound => Bounds.Length; 
    public int CountEdge  => Edges!.Length;

    public Node<T>[]   Nodes;
    public Elem<T>[]   Elems;
    public Bound[]     Bounds;
    public Edge<T>[]?  Edges;     

    //: Constructor (with edge)
    public Grid(Node<T>[] _nodes, Elem<T>[] _elems, Edge<T>[] _edges, Bound[] _bounds) {
        Nodes  = new Node<T>[_nodes.Length];
        Elems  = new Elem<T>[_elems.Length];
        Edges  = new Edge<T>[_edges.Length];
        Bounds = new Bound[_bounds.Length];
        Array.Copy(_nodes, Nodes, _nodes.Length);
        Array.Copy(_elems, Elems, _elems.Length);
        Array.Copy(_edges, Edges, _edges.Length);
        Array.Copy(_bounds, Bounds, _bounds.Length);
    }

    //: Constructor (without edge)
    public Grid(Node<T>[] _nodes, Elem<T>[] _elems, Bound[] _bounds) {
        Nodes  = new Node<T>[_nodes.Length];
        Elems  = new Elem<T>[_elems.Length];
        Bounds = new Bound[_bounds.Length];
        Array.Copy(_nodes, Nodes, _nodes.Length);
        Array.Copy(_elems, Elems, _elems.Length);
        Array.Copy(_bounds, Bounds, _bounds.Length);
    }

    //: Deconstructor (with edge)
    public void Deconstruct(out Node<T>[]  nodes,
                            out Elem<T>[]  elems,
                            out Edge<T>[]  edges,
                            out Bound[]    bounds) {
        nodes  = new Node<T>[CountNode];
        elems  = new Elem<T>[CountElem];
        edges  = new Edge<T>[CountEdge];
        bounds = new Bound[CountBound];
        Array.Copy(Nodes, nodes, Nodes.Length);
        Array.Copy(Elems, elems, Elems.Length);
        Array.Copy(Edges!, edges, Edges!.Length);
        Array.Copy(Bounds, bounds, Bounds.Length);
    }

    //: Deconstructor (without edge)
    public void Deconstruct(out Node<T>[]  nodes,
                            out Elem<T>[]  elems,
                            out Bound[]    bounds) {
        nodes  = new Node<T>[CountNode];
        elems  = new Elem<T>[CountElem];
        bounds = new Bound[CountBound];
        Array.Copy(Nodes, nodes, Nodes.Length);
        Array.Copy(Elems, elems, Elems.Length);
        Array.Copy(Bounds, bounds, Bounds.Length);
    }
}