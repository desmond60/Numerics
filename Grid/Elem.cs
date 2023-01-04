namespace Numerics.Grid;

// % ***** Structure Final Element ***** % //
public struct Elem<T> where T : System.Numerics.INumber<T>
{

    public Node<T>[] Nodes { get; set; }  /// Nodes of a finite element
    public Edge<T>[]? Edges { get; set; }  /// Edges of a finite element

    //: Constructor
    public Elem(Node<T>[] _nodes)
    {
        Nodes = new Node<T>[_nodes.Length];
        Array.Copy(_nodes, Nodes, _nodes.Length);
    }

    //: Constructor (with edges)
    public Elem(Node<T>[] _nodes, Edge<T>[] _edges)
    {
        Nodes = new Node<T>[_nodes.Length];
        Edges = new Edge<T>[_edges.Length];
        Array.Copy(_nodes, Nodes, _nodes.Length);
        Array.Copy(_edges, Edges, _edges.Length);
    }

    //: Deconstructor
    public void Deconstruct(out Node<T>[] nodes,
                            out Edge<T>[] edges)
    {
        nodes = new Node<T>[Nodes.Length];
        edges = new Edge<T>[Edges!.Length];
        Array.Copy(Nodes, nodes, Nodes.Length);
        Array.Copy(Edges, edges, Edges.Length);
    }

    //: String view structure
    public override string ToString()
    {
        StringBuilder str = new StringBuilder();
        for (int i = 0; i < Nodes.Length; i++)
            str.Append($"{Nodes[i]}\n");

        if (Edges is null) return str.ToString();

        str.Append($"\n");
        for (int i = 0; i < Edges.Length; i++)
            str.Append($"{Edges[i].NodeBegin}\t{Edges[i].NodeEnd}\n");

        return str.ToString();
    }
}