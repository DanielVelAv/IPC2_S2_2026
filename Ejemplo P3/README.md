# Analizador de Mensajes — ASP.NET Core + Razor Pages

Aplicación web que recibe mensajes en XML (estilo red social) y permite consultar,
por rango de fechas:

- los **hashtags** usados (`#ejemplo#`),
- los **usuarios mencionados** (`@usuario`),
- cuántas palabras **positivas y negativas** tiene cada mensaje, según un diccionario
  que también se carga en XML.

Es la traducción a C# de un proyecto hecho originalmente en Python: backend con **Flask** y
frontend con **Django**. Se respetaron la misma lógica, los mismos nombres de clases y
métodos y los mismos algoritmos, para que se pueda comparar cada archivo con su versión en Python.

---

## 1. Cómo ejecutarlo

**Requisito:** [.NET SDK 10](https://dotnet.microsoft.com/download).

La aplicación son **dos programas** que deben correr al mismo tiempo. Abre dos terminales
en la carpeta `Proyecto3.NET`:

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
 └────────────┘               │ (antes Django)  │                 │ (antes Flask)   │                 │  dbDiccionario.xml │
                              └─────────────────┘                 └─────────────────┘                 └────────────────────┘
```

---

## 3. Cada archivo de Python y su equivalente en C#

### Backend (`src/Proyecto3.Api`)

| Python | C# | Qué hace |
|---|---|---|
| `app.py` | [`Program.cs`](src/Proyecto3.Api/Program.cs) | Define las rutas de la API |
| `main.py` (clase `Bd`) | [`Bd.cs`](src/Proyecto3.Api/Bd.cs) | Guarda mensajes y diccionario en los XML; limpia los datos |
| `ER.py` | [`ER.cs`](src/Proyecto3.Api/ER.cs) | Lee los XML y analiza cada mensaje carácter por carácter |
| `GETS.py` | [`GETS.cs`](src/Proyecto3.Api/GETS.cs) | Clases `Hastags`, `Menciones` y `Sentimientos`: filtran por fecha |
| librería `unidecode` | [`Unidecode.cs`](src/Proyecto3.Api/Unidecode.cs) | Quita tildes |

**Rutas de la API** (las mismas que en Flask):

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

| Django | Razor Pages | URL |
|---|---|---|
| `settings.py` + `urls.py` | [`Program.cs`](src/Proyecto3.Web/Program.cs) | — |
| `pedirEntrada` + `Principal.html` | [`Pages/Index`](src/Proyecto3.Web/Pages/Index.cshtml.cs) | `/` |
| `pedirDiccionario` + `EnviarDiccionarios.html` | [`Pages/PedirDiccionario`](src/Proyecto3.Web/Pages/PedirDiccionario.cshtml.cs) | `/PedirDiccionario` |
| `resetearDatos` + `ResetearDatos.html` | [`Pages/ResetearDatos`](src/Proyecto3.Web/Pages/ResetearDatos.cshtml.cs) | `/ResetearDatos` |
| `consultarHashtags` + `DevolverHashtags.html` / `RecibeHashtags.html` | [`Pages/DevolverHashtags`](src/Proyecto3.Web/Pages/DevolverHashtags.cshtml.cs) | `/DevolverHashtags` |
| `consultarMenciones` + `ConsultarMenciones.html` / `RecibeMenciones.html` | [`Pages/DevolverMenciones`](src/Proyecto3.Web/Pages/DevolverMenciones.cshtml.cs) | `/DevolverMenciones` |
| `consultarSentimientos` + `ConsultarSentimientos.html` / `RecibeSentimientos.html` | [`Pages/DevolverSentimientos`](src/Proyecto3.Web/Pages/DevolverSentimientos.cshtml.cs) | `/DevolverSentimientos` |
| `ayuda` + `ayuda.html` | [`Pages/Ayuda.cshtml`](src/Proyecto3.Web/Pages/Ayuda.cshtml) | `/Ayuda` |
| carpeta `static/` | `wwwroot/css/` (los mismos CSS) | `/css/...` |

---

## 4. Equivalencias de Python a C#

| Python | C# |
|---|---|
| `@app.route('/x', methods=['POST'])` | `app.MapPost("/x", ...)` |
| `request.form['PrimerRango']` | parámetro `[FromForm] string PrimerRango` |
| `jsonify({...})` | `Results.Json(new { ... })` |
| `send_file(ruta)` | `Results.File(ruta, "application/xml")` |
| `ET.parse(ruta).getroot()` | `XDocument.Load(ruta).Root` |
| `ET.fromstring(texto)` | `XDocument.Parse(texto).Root` |
| `raiz.findall('MENSAJE')` | `raiz.Elements("MENSAJE")` |
| `elemento.text` | `elemento.Value` |
| `ET.Element("palabra")` + `.text = x` | `new XElement("palabra", x)` |
| `padre.append(hijo)` / `arbol.write(ruta)` | `padre.Add(hijo)` / `documento.Save(ruta)` |
| `open(ruta).read()` | `File.ReadAllText(ruta)` |
| `open(ruta, 'w').write(x)` | `File.WriteAllText(ruta, x)` |
| `texto[a:b]` | `texto.Substring(a, b - a)` |
| `texto.find('#', j)` (devuelve -1) | `texto.IndexOf('#', j)` (devuelve -1) |
| `c.isalpha()` / `c.isdigit()` | `char.IsLetter(c)` / `char.IsDigit(c)` |
| `s.strip()` / `s.lower()` | `s.Trim()` / `s.ToLower()` |
| `lista = []` / `lista.append(x)` | `var lista = new List<string>();` / `lista.Add(x)` |
| `dicc = {}` / `x in dicc` | `new Dictionary<string, string>()` / `dicc.ContainsKey(x)` |
| `len(lista)` / `len(texto)` | `lista.Count` / `texto.Length` |
| `return a, b` | `return (a, b);` (tupla) |
| `time.strptime(s, '%d/%m/%Y')` | `DateTime.ParseExact(s, "dd/MM/yyyy", CultureInfo.InvariantCulture)` |
| `self.atributo` | `this.atributo` (o solo `atributo`) |
| `def __init__(self, x):` | constructor `public Clase(string x) { ... }` |

---

## 5. Cómo funciona una página de Razor Pages

Cada página tiene **dos archivos**:

- **`Pagina.cshtml`**: el HTML, como un template de Django.
- **`Pagina.cshtml.cs`**: una clase con la lógica, como la función de `view.py`.

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

| Django | Razor Pages |
|---|---|
| `if request.method == 'POST':` | métodos separados `OnGet()` y `OnPost()` / `OnPostAsync()` |
| `request.POST['PrimerRango']` | parámetro `string PrimerRango` (mismo nombre que el `<input name>`) |
| `request.FILES['archivoMsg']` | parámetro `IFormFile archivoMsg` |
| `loader.get_template(...).render({'fechas': x})` | propiedad `Fechas`, leída en el HTML como `Model.Fechas` |
| `{{ fecha }}` | `@fecha` |
| `{% for fecha, lista in fechas.items %}` | `@foreach (var (fecha, lista) in Model.Fechas)` |
| `{% if x %}` | `@if (x) { }` |
| `{% csrf_token %}` / `@csrf_exempt` | automático en todo `<form method="post">` |
| `{% static 'x.css' %}` | `~/css/x.css` |
| HTML repetido en cada template | `Shared/_Layout.cshtml` + `@RenderBody()` |

Archivos especiales de la carpeta `Pages/`:

- **`_ViewImports.cshtml`**: configuración común de todas las páginas (activa los Tag Helpers como `asp-page`).
- **`_ViewStart.cshtml`**: indica que todas usan el layout.
- **`Shared/_Layout.cshtml`**: el HTML compartido (menú). Cada página indica su CSS con `@section Estilos { ... }`.
