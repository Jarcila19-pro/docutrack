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
| **Juan Alejandro Cardona** | Controller — `ArbolController` (orquestación y secuenciación de los 6 casos de uso: construcción, búsquedas, actualizaciones, eliminaciones, recorridos y altura) |

> Los tres integrantes participan en el diseño, las pruebas y la revisión del código.

### Nota sobre el trabajo colaborativo

Este proyecto se desarrolló de forma **conjunta** por los tres integrantes, compañeros de la
carrera desde hace 3 años. El trabajo se coordinó en **llamadas en tiempo real**, donde se diseñó
la estructura, se implementaron las capas y se probaron los casos de uso de manera colaborativa
(enfrentando juntos los problemas de compilación y de ejecución que surgieron).

Aunque cada integrante lideró un área del patrón MVC —y así aparece la autoría de los commits—,
las decisiones de diseño, la implementación, las pruebas y la revisión del código se realizaron
**en equipo**: cada decisión se discutió y validó entre los tres antes de quedar en el repositorio.
La autoría por commit refleja la **repartición de responsabilidades**, no una división del trabajo
por separado.

---

## 🚀 Paso a paso: clonar y ejecutar desde cero

> Esta guía está pensada para **cualquier persona que llegue al proyecto por primera vez**, o
> que **no recuerde los pasos** (incluidos nosotros mismos si retomamos el proyecto más adelante).
> No importa el nivel de experiencia: sigue los pasos en orden y el proyecto quedará funcionando.

### Requisitos previos
1. **Git** — para clonar el repositorio.
2. **.NET SDK 8.0** (o superior) — para compilar y ejecutar.
3. *(Opcional)* **Visual Studio 2022** — para abrirlo con interfaz gráfica.

#### Instalar Git
- **Windows:** https://git-scm.com/download/win
- **macOS:** `brew install git` (o viene incluido con Xcode)
- **Linux (Debian/Ubuntu):** `sudo apt install git`

#### Instalar .NET SDK 8
- Descargar de https://dotnet.microsoft.com/download/dotnet/8.0
- Instalar con las opciones por defecto (siguiente → siguiente → finalizar).

---

### Opción A — Ejecutar desde la terminal (recomendado)

#### 1. Verificar que quedaron instalados
Abre una **terminal NUEVA** (importante, para que reconozca la instalación) y ejecuta:
```bash
dotnet --version    # debe mostrar 8.x.x o superior
git --version
```

#### 2. Clonar el repositorio
```bash
git clone https://github.com/Jarcila19-pro/docutrack.git
```

#### 3. Entrar a la carpeta del proyecto
```bash
cd docutrack
```

#### 4. Compilar y ejecutar
```bash
dotnet run
```

Deberías ver los encabezados de los **Casos 1 a 7** y el árbol ASCII en consola.

#### 5. (Opcional) Verificar solo la compilación
```bash
dotnet build
```

> Si aparece `Compilación correcta` con **0 errores y 0 advertencias**, todo está bien.

---

### Opción B — Abrir y ejecutar desde Visual Studio

1. **Instalar Visual Studio 2022** (Community es gratuita): https://visualstudio.microsoft.com/
   - En el instalador, marca la carga de trabajo **".NET desktop development"**
     (Desarrollo de escritorio de .NET).
2. Abre Visual Studio y elige **"Abrir un proyecto o solución"**.
3. Selecciona el archivo **`DocuTrack.csproj`** (en la raíz de la carpeta `docutrack`).
   - Si no aparece, cambia el filtro del diálogo a **"Todos los archivos (*.*)"**.
   - Visual Studio generará la solución y restaurará las dependencias automáticamente.
4. Verifica la barra superior:
   - Proyecto de inicio: **DocuTrack**
   - Configuración: **Debug**
5. Presiona **`F5`** (con depuración) o **`Ctrl + F5`** (sin depuración).
6. Se abrirá la consola con la salida del programa (Casos 1 a 7).

> 💡 Si usas **Visual Studio Code**, abre la carpeta y ejecuta con `dotnet run` desde la terminal
> integrada (la extensión "C# Dev Kit" es opcional).

---

### ¿Problemas comunes?

| Síntoma | Causa probable | Solución |
|---|---|---|
| `dotnet` no se reconoce | La terminal se abrió antes de instalar el SDK | Cerrar y abrir una terminal nueva; si persiste, reinstalar el SDK. |
| `git` no se reconoce | Git no está instalado | Instalar Git y reabrir la terminal. |
| "No se encontró ningún proyecto" | Estás en la carpeta equivocada | Asegúrate de estar dentro de `docutrack` (donde está `DocuTrack.csproj`). |
| Visual Studio no muestra el archivo | Filtro de archivos activo | Cambia el filtro a "Todos los archivos (*.*)". |
| La consola se cierra muy rápido | Se ejecutó sin pausa o desde un entorno sin consola | Usa `Ctrl + F5`, o coloca un punto de interrupción con `F9`. |

---

> **Nota:** el proyecto no requiere dependencias externas, paquetes NuGet adicionales ni
> configuración especial. Con el SDK de .NET instalado, compila y ejecuta directamente.

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
