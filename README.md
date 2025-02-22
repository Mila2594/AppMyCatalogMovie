# 🎬 Aplicación Móvil - Consumo de API TMDB

## 📝 Descripción
Esta aplicación móvil explora el consumo de la API pública de **TMDB**, permitiendo a los usuarios visualizar y filtrar películas de manera interactiva. 

El proyecto cuenta con dos pantallas principales:
1. **Pantalla de Inicio**: Muestra una lista de películas obtenidas de la API y permite aplicar filtros.
2. **Pantalla de Favoritos**: Permite gestionar una lista personalizada de películas favoritas.

Además, hay una pantalla intermedia con información detallada de cada película, incluyendo su título, portada, fecha de estreno, calificación y sinopsis.

## ✨ Características
### 🎥 Vista Principal
- Visualización de una lista de películas obtenidas desde la API TMDB.
- Aplicación de filtros:
  - **Estrenos**: Muestra películas estrenadas en los últimos 30 días.
  - **Categoría**: Muestra un diálogo con todas las categorías disponibles y filtra películas según la selección del usuario.
  - **Búsqueda**: Permite buscar películas ingresando texto en un `Entry`.
- Acceso a la vista de detalles de cada película.
- Posibilidad de agregar películas a favoritos mediante un botón de "corazón".

### ⭐ Vista de Favoritos
- Muestra la lista de películas que han sido marcadas como favoritas.
- Opción para agregar y editar un comentario sobre cada película.
- Posibilidad de eliminar películas de la lista de favoritos.

## 🚀 Requisitos
- **Sistema operativo:** Android.
- **Herramientas necesarias:**
  - Visual Studio con soporte para .NET MAUI.
  - Emulador Android o dispositivo físico para pruebas.
  
## 📂 Instalación
1. Clona este repositorio:
   ```sh
   git clone https://github.com/Mila2594/AppMyCatalogMovie.git
   ```
2. Abre el proyecto en **Visual Studio**.
3. Configura el emulador Android o conecta un dispositivo físico.
4. Compila y ejecuta la aplicación.

## 🖼️ Capturas de Pantalla
Aquí se muestra el ciclo de la app:

1. Vista home.
2. Vista detalle peliculas.
3. Vista favoritos
<br><br>

<img src="https://github.com/Mila2594/AppMyCatalogMovie/blob/main/ScreenApp.png" alt="Captura de pantalla" width="1200"/>

## 🛠️ Tecnologías Utilizadas
- **.NET MAUI**: Framework para el desarrollo de aplicaciones móviles multiplataforma.
- **C#**: Lenguaje principal utilizado para la lógica de la aplicación.
- **TMDB API**: Fuente de datos para obtener la información de las películas.

## 📜 Licencia
Este proyecto es de uso libre y está bajo la **Licencia MIT**. Consulta el archivo LICENSE para más detalles.
