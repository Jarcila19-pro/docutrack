using DocuTrack.Controller;

namespace DocuTrack;

/// <summary>
/// Punto de entrada. Únicamente invoca al Controller (sin lógica de negocio aquí).
/// </summary>
internal static class Program
{
    private static void Main()
    {
        new ArbolController().Ejecutar();
    }
}
