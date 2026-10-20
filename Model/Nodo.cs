namespace DocuTrack.Model;

/// <summary>
/// Representa un nodo del repositorio de documentos: una carpeta o un archivo.
/// Un archivo es siempre hoja; una carpeta puede tener 0, 1 o 2 subnodos.
/// </summary>
public class Nodo
{
    /// <summary>Clave del BST. Se compara de forma ordinal e insensible a mayúsculas.</summary>
    public string Nombre { get; set; }

    /// <summary>true = carpeta, false = archivo.</summary>
    public bool EsCarpeta { get; set; }

    public Nodo? Izquierdo { get; set; }
    public Nodo? Derecho { get; set; }

    public Nodo(string nombre, bool esCarpeta)
    {
        Nombre = nombre;
        EsCarpeta = esCarpeta;
        Izquierdo = null;
        Derecho = null;
    }
}
