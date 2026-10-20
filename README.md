# DocuTrack S.A. — Gestión jerárquica de documentos

**Árbol Binario de Búsqueda (BST) · MVC en C# · Consola**

 Actividad colaborativa I — *Programación III* · Universidad de Manizales · Unidad 1

Módulo interno para gestionar jerárquicamente el repositorio de documentos de DocuTrack S.A.
Cada nodo representa una **carpeta** o un **archivo**, modelado con un **Árbol Binario de
Búsqueda** cuya clave de ordenamiento es el `Nombre` (comparación **ordinal**, insensible a
mayúsculas). Regla del BST: *menor a la izquierda, mayor a la derecha; sin duplicados*.

> La elección del **BST simple (no balanceado)** es deliberada con fines pedagógicos de la
> Unidad 1. El balanceo se aborda en unidades posteriores.

---

## Integrantes y responsabilidades

| Integrante | Área principal |
|------------|----------------|
| **Juan Esteban Arcila** | Modelo — `Nodo`, `ArbolBinario` (BST, CRUD, recorridos, métricas) |
| **Juan Martín López** | Vista — `ConsolaView` (impresión ASCII del árbol, mensajes y métricas) |
| **Juan Alejandro Cardona** | Controller — `ArbolController` (orquestación de casos de uso) y repositorio |

> Los tres integrantes participan en el diseño, las pruebas y la revisión del código.

---

## Requisitos para ejecutar

- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0) (LTS) o superior.
- Un terminal / símbolo del sistema.

### Compilar y ejecutar

```bash
cd docutrack
dotnet run
```

> También puede abrirse la carpeta en **Visual Studio** y ejecutar el proyecto `DocuTrack`.
> No requiere dependencias externas ni pasos adicionales: compila y ejecuta directamente.

---

## Arquitectura (MVC estricto)

```
docutrack/
├── Program.cs                  # Punto de entrada: SOLO invoca al Controller
├── Model/
│   ├── Nodo.cs                 # Nombre, EsCarpeta, Izquierdo, Derecho
│   └── ArbolBinario.cs         # BST: CRUD, recorridos, altura (cero referencias a Console)
├── View/
│   └── ConsolaView.cs          # Solo presentación: árbol ASCII, resultados y métricas
└── Controller/
    └── ArbolController.cs      # Orquesta y secuencia los casos de uso
```

- **Model:** clases de dominio. *Cero referencias a `Console`.*
- **View:** únicamente presenta (imprime árbol, resultados y métricas).
- **Controller:** secuencia y orquesta; *no contiene lógica estructural del BST*.
- **Main (`Program.cs`):** invoca exclusivamente al Controller; sin lógica de negocio.

---

## Casos de uso implementados (en orden de ejecución)

1. **Construcción del árbol** con 14 nombres (mezcla de carpetas y archivos) que produce:
   una raíz con dos subárboles, hojas en ambos lados y nodos con un solo hijo.
   Se imprime el árbol ASCII inicial y se prueba la **política de duplicados** (rechazo).
2. **Búsquedas rápidas:** 6 búsquedas (2 en el subárbol izquierdo, 2 en el derecho y
   2 inexistentes), mostrando hallado/no hallado y **número de comparaciones**.
3. **Actualizaciones selectivas** (Estrategia `Eliminar + Insertar`): en **hoja**, en
   **hijo con un hijo** y en la **raíz**. Se imprime el árbol tras cada una.
4. **Eliminaciones selectivas**: **hoja**, **hijo con un hijo** y **raíz** (caso de dos
   hijos resuelto con **sucesor** = mínimo del subárbol derecho, de forma consistente).
   Se imprime el árbol tras cada una e informa qué caso se ejecutó.
5. **Recorridos de verificación:** Preorden, Inorden, Postorden y Por niveles (con `Queue`).
6. **Altura del árbol final.**

---

## ¿Por qué el recorrido **Inorden** confirma que el BST sigue ordenado?

En un BST, el recorrido **inorden** visita los nodos en el orden *izquierda → raíz → derecho*.
Como la rama izquierda siempre contiene claves **menores** que la raíz y la rama derecha claves
**mayores**, recorrer inorden emitirá las claves en **orden ascendente**.

Por lo tanto, si la secuencia producida por el **Inorden** resulta estrictamente ordenada
(insensible a mayúsculas), queda demostrado que **la propiedad del BST se conserva** después de
las inserciones, búsquedas, actualizaciones y eliminaciones. Cualquier violación (una clave fuera
de orden) delataría inmediatamente una estructura incorrecta. Los demás recorridos
(preorden, postorden y por niveles) sirven para verificar la estructura, pero **no** garantizan
por sí solos el orden de las claves.

---

## Verificación (checklist de ejecución)

- [x] Construcción con ≥ 14 nombres variados (carpetas y archivos).
- [x] Impresión ASCII del árbol inicial (conectores `├──`, `└──`, `│`).
- [x] 6 búsquedas (2 izquierda, 2 derecha, 2 inexistentes) con comparaciones.
- [x] 3 actualizaciones (hoja, un hijo, raíz) con impresión tras cada una.
- [x] 3 eliminaciones (hoja, un hijo, raíz) con impresión tras cada una.
- [x] Recorridos: preorden, inorden, postorden y por niveles.
- [x] Altura del árbol final.

---

## Etiqueta del commit final

`release-unidad1`
