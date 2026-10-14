# Analizador de Mensajes — ASP.NET Core + Razor Pages

Aplicación web que recibe mensajes en XML (estilo red social) y permite consultar,
por rango de fechas:

- los **hashtags** usados (`#ejemplo#`),
- los **usuarios mencionados** (`@usuario`),
- cuántas palabras **positivas y negativas** tiene cada mensaje, según un diccionario
  que también se carga en XML.

---

## 1. Cómo ejecutarlo

**Requisito:** [.NET SDK 10](https://dotnet.microsoft.com/download).

La aplicación son **dos programas** que deben correr al mismo tiempo. Abre dos terminales
en la carpeta `Ejemplo P3`:

```bash
# Terminal 1: backend (API) en http://localhost:5080
dotnet run --project src/Proyecto3.Api

# Terminal 2: frontend (sitio web) en http://localhost:5090
dotnet run --project src/Proyecto3.Web
```

Abre <http://localhost:5090> y sigue este orden:

1. **Cargar diccionario** → `ejemplos/diccionarios.xml`
2. **Cargar mensajes** → `ejemplos/mensajes.xml` o `ejemplos/mensajesSentimientos.xml`
3. **Hashtags / Menciones / Sentimientos** → elige un rango de fechas de 2023.

Para probar la API sola (como con Postman), usa
[`src/Proyecto3.Api/Proyecto3.Api.http`](src/Proyecto3.Api/Proyecto3.Api.http).

> **En Visual Studio:** clic derecho en la solución → *Configure Startup Projects* →
> *Multiple startup projects* → `Proyecto3.Api` y `Proyecto3.Web` en *Start* → F5.

---

## 2. Arquitectura

```
 ┌────────────┐  formularios  ┌─────────────────┐   HTTP (POST)   ┌─────────────────┐   lee/escribe   ┌────────────────────┐
 │ Navegador  │ ◀───────────▶ │ Proyecto3.Web   │ ◀─────────────▶ │ Proyecto3.Api   │ ◀─────────────▶ │ App_Data/          │
 │            │     HTML      │ Razor Pages     │      JSON       │ .NET            │                 │  db.xml            │
 └────────────┘               └─────────────────┘                 └─────────────────┘                 │  dbDiccionario.xml │
                                                                                                      └────────────────────┘
```

---

## 3. Archivos del proyecto

### Backend (`src/Proyecto3.Api`)

| Archivo | Qué hace |
|---|---|
| [`Program.cs`](src/Proyecto3.Api/Program.cs) | Define las rutas de la API |
| [`Bd.cs`](src/Proyecto3.Api/Bd.cs) | Guarda mensajes y diccionario en los XML; limpia los datos |
| [`ER.cs`](src/Proyecto3.Api/ER.cs) | Lee los XML y analiza cada mensaje carácter por carácter |
| [`GETS.cs`](src/Proyecto3.Api/GETS.cs) | Clases `Hastags`, `Menciones` y `Sentimientos`: filtran por fecha |
| [`Unidecode.cs`](src/Proyecto3.Api/Unidecode.cs) | Quita tildes |

**Rutas de la API:**

| Ruta | Campos del formulario | Respuesta |
|---|---|---|
| `GET /` | — | `{"message": "Funcionando Jefe"}` |
| `POST /entradaMensaje` | `archivoMsg` (contenido del XML) | XML `MENSAJES_RECIBIDOS` |
| `POST /CargaArchivosConfiguracion` | `archivoDicc` (contenido del XML) | `{"message": "..."}` |
| `POST /LimpiarDatos` | — | `{"message": "..."}` |
| `POST /devolverHastags` | `PrimerRango`, `SegundoRango` (yyyy-mm-dd) | `{"dd/mm/yyyy": ["#a#", ...]}` |
| `POST /devolverMenciones` | `PrimerRango`, `SegundoRango` | `{"dd/mm/yyyy": ["@a", ...]}` |
| `POST /devolverSentimientos` | `PrimerRango`, `SegundoRango` | `{"positivos": {...}, "negativos": {...}}` |

### Frontend (`src/Proyecto3.Web`)

| Página | URL | Qué hace |
|---|---|---|
| [`Program.cs`](src/Proyecto3.Web/Program.cs) | — | Configuración del sitio |
| [`Pages/Index`](src/Proyecto3.Web/Pages/Index.cshtml.cs) | `/` | Carga el XML de mensajes |
| [`Pages/PedirDiccionario`](src/Proyecto3.Web/Pages/PedirDiccionario.cshtml.cs) | `/PedirDiccionario` | Carga el XML del diccionario |
| [`Pages/ResetearDatos`](src/Proyecto3.Web/Pages/ResetearDatos.cshtml.cs) | `/ResetearDatos` | Borra los datos guardados |
| [`Pages/DevolverHashtags`](src/Proyecto3.Web/Pages/DevolverHashtags.cshtml.cs) | `/DevolverHashtags` | Consulta hashtags por fecha |
| [`Pages/DevolverMenciones`](src/Proyecto3.Web/Pages/DevolverMenciones.cshtml.cs) | `/DevolverMenciones` | Consulta menciones por fecha |
| [`Pages/DevolverSentimientos`](src/Proyecto3.Web/Pages/DevolverSentimientos.cshtml.cs) | `/DevolverSentimientos` | Consulta sentimientos por fecha |
| [`Pages/Ayuda.cshtml`](src/Proyecto3.Web/Pages/Ayuda.cshtml) | `/Ayuda` | Página de ayuda |
| `wwwroot/css/` | `/css/...` | Hojas de estilo |

---

## 4. Cómo funciona una página de Razor Pages

Cada página tiene **dos archivos**:

- **`Pagina.cshtml`**: el HTML de la página.
- **`Pagina.cshtml.cs`**: una clase con la lógica que se ejecuta al abrir o enviar la página.

```csharp
public class DevolverHashtagsModel : PageModel
{
    public Dictionary<string, List<string>>? Fechas { get; set; } // dato que usa el HTML

    public void OnGet() { }                       // GET: mostrar el formulario

    public async Task OnPostAsync(string PrimerRango, string SegundoRango)  // POST: enviar
    {
        // ... llamar a la API y llenar Fechas
    }
}
```

| Concepto | En Razor Pages |
|---|---|
| Distinguir GET de POST | métodos separados `OnGet()` y `OnPost()` / `OnPostAsync()` |
| Leer un campo del formulario | parámetro `string PrimerRango` (mismo nombre que el `<input name>`) |
| Recibir un archivo | parámetro `IFormFile archivoMsg` |
| Pasar datos al HTML | propiedad `Fechas`, leída en el HTML como `Model.Fechas` |
| Mostrar un valor | `@fecha` |
| Recorrer una colección | `@foreach (var (fecha, lista) in Model.Fechas)` |
| Condicional | `@if (x) { }` |
| Protección CSRF | automática en todo `<form method="post">` |
| Enlazar un CSS | `~/css/x.css` |
| HTML compartido entre páginas | `Shared/_Layout.cshtml` + `@RenderBody()` |

Archivos especiales de la carpeta `Pages/`:

- **`_ViewImports.cshtml`**: configuración común de todas las páginas (activa los Tag Helpers como `asp-page`).
- **`_ViewStart.cshtml`**: indica que todas usan el layout.
- **`Shared/_Layout.cshtml`**: el HTML compartido (menú). Cada página indica su CSS con `@section Estilos { ... }`.
