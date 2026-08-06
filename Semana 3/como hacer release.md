# Guía de Git en Consola

## ¿Qué buscamos?

- Llevar el código de las ramas `feature` a `develop`, y de `develop` a `main`.
- Teniendo el código en `main`, crear una etiqueta (tag).
- Desde GitHub, crear un release en base a esa etiqueta.

## ¿Cómo se ven nuestras ramas?

```
                Main
                 |
              develop
                 |
             features
             |        |
          Rama 1    Rama 2
```

---

## Paso a paso

### 1. Llevar los cambios de la rama feature a develop

Una vez realizados los últimos cambios en tu rama de feature, la subimos y la fusionamos a `develop`.

```bash
# Verificar en qué rama estamos
git status

# Asegurarnos de tener los últimos cambios locales guardados
git add . #. es todo
git commit -m "Últimos cambios de la feature"

# Subir la rama feature al repositorio
git push origin Rama-1

# Cambiarnos a develop y actualizarla
git checkout develop
git pull origin develop

# Fusionar la rama feature dentro de develop
git merge Rama-1

# Subir develop actualizada al remoto
git push origin develop
```

---

### 2. Llevar los cambios de develop a main

Con los cambios ya integrados y probados en `develop`, los llevamos a `main`.

```bash
# Cambiarnos a main y actualizarla
git checkout main
git pull origin main

# Fusionar develop dentro de main
git merge develop

# Subir main actualizada al remoto
git push origin main
```

---

### 3. Asegurarnos que los cambios están en main y crear una rama de releases

Una vez que confirmamos que `main` tiene los cambios y está actualizada, creamos una rama a partir de `main` llamada `releases` (o con el nombre de la versión, por ejemplo `release/1.0.0`).

```bash
# Confirmar que estamos parados en main y está actualizada
git checkout main
git pull origin main

# Crear la rama de releases a partir de main
git checkout -b releases

# Subir la rama releases al remoto
git push origin releases
```

---

### 4. Crear una tag (etiqueta) desde esa rama

Desde la rama `releases`, creamos la tag que identificará esta versión.

```bash
# Crear una tag anotada
git tag -a v1.0.0 -m "Primera versión estable del proyecto"

# Verificar que la tag se creó correctamente
git tag

# Ver el detalle de la tag
git show v1.0.0

# Subir la tag al repositorio remoto
git push origin v1.0.0
```

---

### 5. Crear el release en GitHub a partir de la tag



---

## Ya hice mi primer release, ¿cómo continúo con mi proyecto?

### ¿Desde dónde sigo trabajando? ¿Creo otra rama?

Sí. Después de publicar un release, el flujo de trabajo continúa así:

1. **Volvemos a trabajar sobre `develop`**, que sigue siendo la rama base para el desarrollo de nuevas funcionalidades.

   ```bash
   git checkout develop
   git pull origin develop
   ```

2. **Creamos una nueva rama feature** a partir de `develop` para la siguiente tarea o funcionalidad.

   ```bash
   git checkout -b feature/nueva-funcionalidad develop
   ```

3. Trabajamos, hacemos commits y, cuando esté lista, hacemos el release en base a ese codigo

