using DocuTrack.Model;
using DocuTrack.View;

namespace DocuTrack.Controller;

/// <summary>
/// Orquesta y secuencia los casos de uso del taller (CRUD, recorridos y pruebas).
/// No contiene lógica estructural del BST; delega en el Model y delega la
/// presentación en la View.
/// </summary>
public class ArbolController
{
    public void Ejecutar()
    {
        var arbol = new ArbolBinario();
        var vista = new ConsolaView();

        Caso1_Construccion(arbol, vista);
        Caso2_Busquedas(arbol, vista);
        Caso3_Actualizaciones(arbol, vista);
        Caso4_Eliminaciones(arbol, vista);
        Caso5_Recorridos(arbol, vista);
        Caso6_Altura(arbol, vista);
        Caso7_Robustez(arbol, vista);

        vista.Encabezado("FIN DE LA EJECUCIÓN");
    }

    // ---------------------------------------------------- 1. Construcción del árbol
    private static void Caso1_Construccion(ArbolBinario arbol, ConsolaView vista)
    {
        vista.Encabezado("CASO 1 - CONSTRUCCIÓN DEL ÁRBOL (14 nombres)");

        // Orden de inserción cuidadosamente elegido para producir un BST balanceado:
        // raíz con dos subárboles, hojas en ambos lados y nodos con un solo hijo.
        (string Nombre, bool EsCarpeta)[] semilla =
        {
            ("Documentos",          true),   // raíz
            ("Certificados",        true),
            ("Guias",               true),
            ("Backup",              true),
            ("Contratos",           true),
            ("FacturaEnero.pdf",    false),
            ("GuiaUsuario.pdf",     false),
            ("Avances.docx",        false),
            ("Catalogo.xlsx",       false),
            ("Boletin.pdf",         false),
            ("Encuestas.xlsx",      false),
            ("Manuales",            true),
            ("ContratoVigente.pdf", false),
            ("Facturas.xlsx",       false),
        };

        foreach (var (nombre, esCarpeta) in semilla)
        {
            arbol.Insertar(nombre, esCarpeta);
            vista.ImprimirInsercion(nombre, esCarpeta, arbol.UltimoResultadoInsercion, arbol.ComparacionesUltimaInsercion);
        }

        vista.Subtitulo("Prueba de la política de duplicados (se debe rechazar)");
        arbol.Insertar("Documentos", false);
        vista.ImprimirInsercion("Documentos", false, arbol.UltimoResultadoInsercion, arbol.ComparacionesUltimaInsercion);

        vista.Subtitulo("Árbol inicial");
        vista.ImprimirArbol(arbol.Raiz);
    }

    // ---------------------------------------------------- 2. Búsquedas rápidas
    private static void Caso2_Busquedas(ArbolBinario arbol, ConsolaView vista)
    {
        vista.Encabezado("CASO 2 - BÚSQUEDAS RÁPIDAS (con nº de comparaciones)");

        (string Nombre, string Lado)[] busquedas =
        {
            ("Avances.docx",     "subárbol izquierdo"),
            ("Catalogo.xlsx",    "subárbol izquierdo"),
            ("Facturas.xlsx",    "subárbol derecho"),
            ("Manuales",         "subárbol derecho"),
            ("Presupuesto.xlsx", "inexistente"),
            ("Zonas.zip",        "inexistente"),
        };

        foreach (var (nombre, lado) in busquedas)
        {
            Nodo? res = arbol.Buscar(nombre, out int comp);
            vista.Mensaje($"[{lado}]");
            vista.ImprimirBusqueda(nombre, res, comp);
        }
    }

    // ---------------------------------------------------- 3. Actualizaciones selectivas
    private static void Caso3_Actualizaciones(ArbolBinario arbol, ConsolaView vista)
    {
        vista.Encabezado("CASO 3 - ACTUALIZACIONES SELECTIVAS (Eliminar + Insertar)");

        vista.Subtitulo("3.1 Actualizar en HOJA: Avances.docx -> AvancesFinales.docx");
        arbol.Actualizar("Avances.docx", "AvancesFinales.docx", out string m1);
        vista.ImprimirActualizacion("Avances.docx", "AvancesFinales.docx", m1 == "", m1);
        vista.ImprimirArbol(arbol.Raiz);

        vista.Subtitulo("3.2 Actualizar en HIJO CON UN HIJO: GuiaUsuario.pdf -> GuiaManual.pdf");
        arbol.Actualizar("GuiaUsuario.pdf", "GuiaManual.pdf", out string m2);
        vista.ImprimirActualizacion("GuiaUsuario.pdf", "GuiaManual.pdf", m2 == "", m2);
        vista.ImprimirArbol(arbol.Raiz);

        vista.Subtitulo("3.3 Actualizar en la RAÍZ: Documentos -> ArchivoCentral");
        arbol.Actualizar("Documentos", "ArchivoCentral", out string m3);
        vista.ImprimirActualizacion("Documentos", "ArchivoCentral", m3 == "", m3);
        vista.ImprimirArbol(arbol.Raiz);
    }

    // ---------------------------------------------------- 4. Eliminaciones selectivas
    private static void Caso4_Eliminaciones(ArbolBinario arbol, ConsolaView vista)
    {
        vista.Encabezado("CASO 4 - ELIMINACIONES SELECTIVAS");

        vista.Subtitulo("4.1 Eliminar HOJA: Boletin.pdf");
        bool e1 = arbol.Eliminar("Boletin.pdf");
        vista.ImprimirEliminacion("Boletin.pdf", e1, arbol.UltimoCasoEliminacion);
        vista.ImprimirArbol(arbol.Raiz);

        vista.Subtitulo("4.2 Eliminar HIJO CON UN HIJO: Contratos");
        bool e2 = arbol.Eliminar("Contratos");
        vista.ImprimirEliminacion("Contratos", e2, arbol.UltimoCasoEliminacion);
        vista.ImprimirArbol(arbol.Raiz);

        vista.Subtitulo("4.3 Eliminar la RAÍZ (dos hijos, se usa sucesor): Encuestas.xlsx");
        bool e3 = arbol.Eliminar("Encuestas.xlsx");
        vista.ImprimirEliminacion("Encuestas.xlsx", e3, arbol.UltimoCasoEliminacion);
        vista.ImprimirArbol(arbol.Raiz);
    }

    // ---------------------------------------------------- 5. Recorridos de verificación
    private static void Caso5_Recorridos(ArbolBinario arbol, ConsolaView vista)
    {
        vista.Encabezado("CASO 5 - RECORRIDOS DE VERIFICACIÓN");
        vista.ImprimirRecorrido("Preorden", arbol.Preorden());
        vista.ImprimirRecorrido("Inorden", arbol.Inorden());
        vista.ImprimirRecorrido("Postorden", arbol.Postorden());
        vista.ImprimirRecorrido("Por niveles", arbol.PorNiveles());
        vista.Mensaje("Nota: el INORDEN confirma que el BST sigue ordenado (ascendente por Nombre).");
    }

    // ---------------------------------------------------- 6. Altura final
    private static void Caso6_Altura(ArbolBinario arbol, ConsolaView vista)
    {
        vista.Encabezado("CASO 6 - MÉTRICA FINAL");
        vista.ImprimirAltura(arbol.Altura());
    }

    // ---------------------------------------------------- 7. Validaciones y robustez
    // Verifica casos límite (nulos, vacíos, duplicados, nombres inexistentes y
    // búsqueda insensible a mayúsculas). Estas operaciones NO modifican el árbol.
    private static void Caso7_Robustez(ArbolBinario arbol, ConsolaView vista)
    {
        vista.Encabezado("CASO 7 - VALIDACIONES Y ROBUSTEZ (casos límite)");
        vista.Mensaje("Ninguna de estas operaciones modifica el árbol; solo comprueban las validaciones.");

        vista.Subtitulo("Insertar nombre NULO");
        arbol.Insertar(null!, false);
        vista.ImprimirInsercion("(nulo)", false, arbol.UltimoResultadoInsercion, arbol.ComparacionesUltimaInsercion);

        vista.Subtitulo("Insertar nombre VACÍO (solo espacios)");
        arbol.Insertar("   ", true);
        vista.ImprimirInsercion("   ", true, arbol.UltimoResultadoInsercion, arbol.ComparacionesUltimaInsercion);

        vista.Subtitulo("Insertar DUPLICADO ('Guias' ya existe)");
        arbol.Insertar("Guias", false);
        vista.ImprimirInsercion("Guias", false, arbol.UltimoResultadoInsercion, arbol.ComparacionesUltimaInsercion);

        vista.Subtitulo("Actualizar a un nombre que YA EXISTE ('Backup' -> 'Guias')");
        arbol.Actualizar("Backup", "Guias", out string m1);
        vista.ImprimirActualizacion("Backup", "Guias", m1 == "", m1);

        vista.Subtitulo("Actualizar un nodo INEXISTENTE ('NoExiste.txt' -> 'Nuevo.txt')");
        arbol.Actualizar("NoExiste.txt", "Nuevo.txt", out string m2);
        vista.ImprimirActualizacion("NoExiste.txt", "Nuevo.txt", m2 == "", m2);

        vista.Subtitulo("Eliminar un nodo INEXISTENTE ('Fantasma.zip')");
        bool e1 = arbol.Eliminar("Fantasma.zip");
        vista.ImprimirEliminacion("Fantasma.zip", e1, arbol.UltimoCasoEliminacion);

        vista.Subtitulo("Búsqueda insensible a MAYÚSCULAS ('guias' debe hallar 'Guias')");
        Nodo? res = arbol.Buscar("guias", out int comp);
        vista.ImprimirBusqueda("guias", res, comp);

        vista.Subtitulo("Árbol tras las validaciones (debe ser idéntico al del Caso 4)");
        vista.ImprimirArbol(arbol.Raiz);
    }
}
