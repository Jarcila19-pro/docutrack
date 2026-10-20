using DocuTrack.Model;

namespace DocuTrack.View;

/// <summary>
/// Capa de presentación: únicamente imprime en consola.
/// No contiene lógica estructural del BST ni toca el estado del árbol.
/// </summary>
public class ConsolaView
{
    public void Encabezado(string titulo)
    {
        Console.WriteLine();
        Console.WriteLine("================================================================");
        Console.WriteLine($"  {titulo}");
        Console.WriteLine("================================================================");
    }

    public void Subtitulo(string texto)
    {
        Console.WriteLine();
        Console.WriteLine($"--- {texto}");
    }

    public void Mensaje(string texto) => Console.WriteLine($"    {texto}");

    /// <summary>Etiqueta legible de un nodo: Nombre (carpeta|archivo).</summary>
    public static string Formatear(Nodo n) =>
        $"{n.Nombre} ({(n.EsCarpeta ? "carpeta" : "archivo")})";

    // ------------------------------------------------------------------ ÁRBOL ASCII
    /// <summary>Imprime el árbol en ASCII: raíz arriba, conectores ├── └── │.</summary>
    public void ImprimirArbol(Nodo? raiz)
    {
        if (raiz is null)
        {
            Mensaje("(árbol vacío)");
            return;
        }
        Console.WriteLine("    " + Formatear(raiz));
        ImprimirRama(raiz.Izquierdo, "    ", raiz.Derecho is null);
        ImprimirRama(raiz.Derecho, "    ", true);
    }

    private static void ImprimirRama(Nodo? nodo, string prefijo, bool esUltimo)
    {
        if (nodo is null) return;
        Console.WriteLine($"{prefijo}{(esUltimo ? "└── " : "├── ")}{Formatear(nodo)}");
        string nuevoPrefijo = prefijo + (esUltimo ? "    " : "│   ");
        // El hijo izquierdo cierra con └── solo si el nodo no tiene hermano por la derecha.
        ImprimirRama(nodo.Izquierdo, nuevoPrefijo, nodo.Derecho is null);
        ImprimirRama(nodo.Derecho, nuevoPrefijo, true);
    }

    // ------------------------------------------------------------------ OPERACIONES
    public void ImprimirInsercion(string nombre, bool esCarpeta, ResultadoInsercion caso, int comparaciones)
    {
        switch (caso)
        {
            case ResultadoInsercion.Insertado:
                Mensaje($"Insertar '{nombre}' ({(esCarpeta ? "carpeta" : "archivo")}): OK — {comparaciones} comparaciones.");
                break;
            case ResultadoInsercion.Duplicado:
                Mensaje($"Insertar '{nombre}': RECHAZADO (duplicado, ya existe en el árbol).");
                break;
            default:
                Mensaje($"Insertar: RECHAZADO (nombre vacío o nulo).");
                break;
        }
    }

    public void ImprimirBusqueda(string nombre, Nodo? resultado, int comparaciones)
    {
        if (resultado is not null)
            Mensaje($"Buscar '{nombre}': HALLADO → {Formatear(resultado)} — {comparaciones} comparaciones.");
        else
            Mensaje($"Buscar '{nombre}': NO HALLADO — {comparaciones} comparaciones.");
    }

    public void ImprimirActualizacion(string viejo, string nuevo, bool ok, string motivo)
    {
        if (ok) Mensaje($"Actualizar '{viejo}' → '{nuevo}': EXITOSO (Eliminar + Insertar).");
        else Mensaje($"Actualizar '{viejo}' → '{nuevo}': FALLÓ ({motivo}).");
    }

    public void ImprimirEliminacion(string nombre, bool ok, string caso)
    {
        if (!ok)
        {
            Mensaje($"Eliminar '{nombre}': FALLÓ ({caso}).");
            return;
        }
        string casoTexto = caso switch
        {
            "hoja"      => "caso HOJA (desenlazado)",
            "un-hijo"   => "caso HIJO CON UN SOLO HIJO (hijo reconectado al padre)",
            "dos-hijos" => "caso DOS HIJOS (reemplazado por el SUCESOR = mínimo del subárbol derecho)",
            _           => caso
        };
        Mensaje($"Eliminar '{nombre}': EXITOSO — {casoTexto}.");
    }

    public void ImprimirRecorrido(string nombreRecorrido, List<Nodo> nodos)
    {
        string linea = nodos.Count == 0
            ? "(vacío)"
            : string.Join(" → ", nodos.Select(Formatear));
        Console.WriteLine($"    {nombreRecorrido,-12}: {linea}");
    }

    public void ImprimirAltura(int altura) =>
        Mensaje($"Altura del árbol: {altura}   (vacío = -1, un solo nodo = 0)");
}
