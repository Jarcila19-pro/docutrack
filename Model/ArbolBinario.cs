namespace DocuTrack.Model;

/// <summary>Resultado de una operación de inserción sobre el árbol.</summary>
public enum ResultadoInsercion
{
    Insertado,
    Duplicado,
    Invalido
}

/// <summary>
/// Árbol de búsqueda binario (BST) que modela el repositorio de documentos de DocuTrack.
/// Reglas: clave = Nombre (ordinal, insensible a mayúsculas); menor a la izquierda,
/// mayor a la derecha; no se permiten duplicados.
/// Esta clase NO contiene referencias a Console (capa Model pura).
/// </summary>
public class ArbolBinario
{
    public Nodo? Raiz { get; private set; }

    /// <summary>Número de comparaciones usadas en la última inserción.</summary>
    public int ComparacionesUltimaInsercion { get; private set; }

    /// <summary>Resultado de la última inserción intentada.</summary>
    public ResultadoInsercion UltimoResultadoInsercion { get; private set; }

    /// <summary>Caso de eliminación ejecutado en la última eliminación
    /// ("hoja", "un-hijo", "dos-hijos", "no-existe", "invalido").</summary>
    public string UltimoCasoEliminacion { get; private set; } = string.Empty;

    /// <summary>Compara dos nombres de forma ordinal e insensible a mayúsculas.</summary>
    private static int Comparar(string a, string b) =>
        string.Compare(a, b, StringComparison.OrdinalIgnoreCase);

    // ------------------------------------------------------------------ INSERTAR
    /// <summary>Inserta (nombre, esCarpeta). Rechaza duplicados y nombres vacíos.</summary>
    /// <returns>true si se insertó; false si fue duplicado o inválido.</returns>
    public bool Insertar(string nombre, bool esCarpeta)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            UltimoResultadoInsercion = ResultadoInsercion.Invalido;
            return false;
        }

        ComparacionesUltimaInsercion = 0;
        bool duplicado = false;
        Raiz = InsertarRec(Raiz, nombre.Trim(), esCarpeta, ref duplicado);
        UltimoResultadoInsercion = duplicado
            ? ResultadoInsercion.Duplicado
            : ResultadoInsercion.Insertado;
        return !duplicado;
    }

    private Nodo InsertarRec(Nodo? actual, string nombre, bool esCarpeta, ref bool duplicado)
    {
        if (actual is null) return new Nodo(nombre, esCarpeta);

        ComparacionesUltimaInsercion++;
        int cmp = Comparar(nombre, actual.Nombre);
        if (cmp < 0)
            actual.Izquierdo = InsertarRec(actual.Izquierdo, nombre, esCarpeta, ref duplicado);
        else if (cmp > 0)
            actual.Derecho = InsertarRec(actual.Derecho, nombre, esCarpeta, ref duplicado);
        else
            duplicado = true; // ya existe: se rechaza sin modificar el árbol

        return actual;
    }

    // ------------------------------------------------------------------ BUSCAR
    /// <summary>Busca un nodo por nombre. Entrega el número de comparaciones realizadas.</summary>
    /// <returns>El nodo encontrado o null si no existe.</returns>
    public Nodo? Buscar(string nombre, out int comparaciones)
    {
        comparaciones = 0;
        if (string.IsNullOrWhiteSpace(nombre)) return null;

        Nodo? actual = Raiz;
        while (actual is not null)
        {
            comparaciones++;
            int cmp = Comparar(nombre, actual.Nombre);
            if (cmp < 0) actual = actual.Izquierdo;
            else if (cmp > 0) actual = actual.Derecho;
            else return actual;
        }
        return null;
    }

    // ------------------------------------------------------------------ ACTUALIZAR
    /// <summary>
    /// Actualiza el nombre de un nodo. Estrategia obligatoria: Eliminar(viejo) + Insertar(nuevo),
    /// para conservar la propiedad del BST. Valida que el viejo exista y el nuevo no exista.
    /// </summary>
    public bool Actualizar(string nombreAntiguo, string nombreNuevo, out string motivo)
    {
        motivo = string.Empty;
        if (string.IsNullOrWhiteSpace(nombreAntiguo) || string.IsNullOrWhiteSpace(nombreNuevo))
        {
            motivo = "nombre vacío o nulo";
            return false;
        }

        Nodo? existente = Buscar(nombreAntiguo, out _);
        if (existente is null)
        {
            motivo = $"no existe '{nombreAntiguo}'";
            return false;
        }
        if (Buscar(nombreNuevo.Trim(), out _) is not null)
        {
            motivo = $"el nombre '{nombreNuevo.Trim()}' ya existe";
            return false;
        }

        bool esCarpeta = existente.EsCarpeta;
        Eliminar(nombreAntiguo);
        Insertar(nombreNuevo, esCarpeta);
        return true;
    }

    // ------------------------------------------------------------------ ELIMINAR
    /// <summary>Elimina un nodo por nombre. Informa el caso en UltimoCasoEliminacion.</summary>
    /// <returns>true si se eliminó.</returns>
    public bool Eliminar(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            UltimoCasoEliminacion = "invalido";
            return false;
        }
        if (Buscar(nombre, out _) is null)
        {
            UltimoCasoEliminacion = "no-existe";
            return false;
        }

        bool eliminado = false;
        Raiz = EliminarRec(Raiz, nombre, ref eliminado);
        return eliminado;
    }

    private Nodo? EliminarRec(Nodo? actual, string nombre, ref bool eliminado)
    {
        if (actual is null) return null;

        int cmp = Comparar(nombre, actual.Nombre);
        if (cmp < 0)
            actual.Izquierdo = EliminarRec(actual.Izquierdo, nombre, ref eliminado);
        else if (cmp > 0)
            actual.Derecho = EliminarRec(actual.Derecho, nombre, ref eliminado);
        else
        {
            eliminado = true;

            // Caso 1: hoja (sin hijos) -> se desenlaza.
            if (actual.Izquierdo is null && actual.Derecho is null)
            {
                UltimoCasoEliminacion = "hoja";
                return null;
            }

            // Caso 2: un solo hijo -> se reconecta el hijo con el padre.
            if (actual.Izquierdo is null)
            {
                UltimoCasoEliminacion = "un-hijo";
                return actual.Derecho;
            }
            if (actual.Derecho is null)
            {
                UltimoCasoEliminacion = "un-hijo";
                return actual.Izquierdo;
            }

            // Caso 3: dos hijos -> se reemplaza por el sucesor (mínimo del subárbol derecho).
            Nodo sucesor = Minimo(actual.Derecho);
            actual.Nombre = sucesor.Nombre;
            actual.EsCarpeta = sucesor.EsCarpeta;
            actual.Derecho = EliminarRec(actual.Derecho, sucesor.Nombre, ref eliminado);
            UltimoCasoEliminacion = "dos-hijos";
        }
        return actual;
    }

    private static Nodo Minimo(Nodo nodo)
    {
        while (nodo.Izquierdo is not null) nodo = nodo.Izquierdo;
        return nodo;
    }

    // ------------------------------------------------------------------ RECORRIDOS
    public List<Nodo> Preorden()
    {
        var res = new List<Nodo>();
        PreordenRec(Raiz, res);
        return res;
    }
    private static void PreordenRec(Nodo? n, List<Nodo> acc)
    {
        if (n is null) return;
        acc.Add(n);
        PreordenRec(n.Izquierdo, acc);
        PreordenRec(n.Derecho, acc);
    }

    /// <summary>Inorden (izquierda, raíz, derecho): produce los nombres en orden ascendente.</summary>
    public List<Nodo> Inorden()
    {
        var res = new List<Nodo>();
        InordenRec(Raiz, res);
        return res;
    }
    private static void InordenRec(Nodo? n, List<Nodo> acc)
    {
        if (n is null) return;
        InordenRec(n.Izquierdo, acc);
        acc.Add(n);
        InordenRec(n.Derecho, acc);
    }

    public List<Nodo> Postorden()
    {
        var res = new List<Nodo>();
        PostordenRec(Raiz, res);
        return res;
    }
    private static void PostordenRec(Nodo? n, List<Nodo> acc)
    {
        if (n is null) return;
        PostordenRec(n.Izquierdo, acc);
        PostordenRec(n.Derecho, acc);
        acc.Add(n);
    }

    /// <summary>Recorrido por niveles usando una Queue.</summary>
    public List<Nodo> PorNiveles()
    {
        var res = new List<Nodo>();
        if (Raiz is null) return res;

        var cola = new Queue<Nodo>();
        cola.Enqueue(Raiz);
        while (cola.Count > 0)
        {
            Nodo actual = cola.Dequeue();
            res.Add(actual);
            if (actual.Izquierdo is not null) cola.Enqueue(actual.Izquierdo);
            if (actual.Derecho is not null) cola.Enqueue(actual.Derecho);
        }
        return res;
    }

    // ------------------------------------------------------------------ ALTURA
    /// <summary>Altura del árbol. Árbol vacío = -1; un solo nodo = 0.</summary>
    public int Altura() => AlturaRec(Raiz);

    private static int AlturaRec(Nodo? n)
    {
        if (n is null) return -1;
        return 1 + Math.Max(AlturaRec(n.Izquierdo), AlturaRec(n.Derecho));
    }
}
