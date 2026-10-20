# PruebaDevsecops — API de Contactos

API REST desarrollada con **ASP.NET Core y Entity Framework Core**, utilizando PostgreSQL como base de datos y Docker Compose para la contenerización del entorno.

El proyecto está orientado a demostrar buenas prácticas de configuración, persistencia de datos, gestión de secretos mediante variables de entorno y despliegue de aplicaciones en contenedores.

## Tecnologías utilizadas

- **C# / ASP.NET Core .NET 10** — desarrollo de la API REST.
- **Entity Framework Core** — acceso a datos y migraciones.
- **PostgreSQL 17** — base de datos relacional.
- **Npgsql** — proveedor de PostgreSQL para Entity Framework Core.
- **Docker** — construcción y ejecución de la aplicación en contenedores.
- **Docker Compose** — orquestación local de la API y la base de datos.
- **Git / GitHub** — control de versiones.

## Arquitectura

```text
Cliente HTTP
     |
     v
ASP.NET Core API
  Contenedor Docker
     |
     v
PostgreSQL 17
  Contenedor Docker
     |
     v
Volumen persistente
```

Docker Compose administra los servicios y permite que la API se comunique con PostgreSQL mediante el nombre del servicio `db` dentro de la red interna.

## Estructura del proyecto

```text
PruebaDevsecops/
├── Controllers/
├── Data/
├── Migrations/
├── Models/
├── Properties/
├── .env.example
├── .gitignore
├── Dockerfile
├── docker-compose.yml
├── Program.cs
├── prueba.csproj
└── prueba.http
```

## Requisitos previos

Para ejecutar el proyecto necesitas:

- Docker Engine.
- Docker Compose v2.
- Git, para clonar el repositorio.
- .NET SDK 10, si deseas compilar, ejecutar o gestionar migraciones fuera de Docker.

## Instalación y ejecución

### 1. Clonar el repositorio

```bash
git clone https://github.com/BrayanPacheco05/PruebaDevsecops.git
cd PruebaDevsecops
```

### 2. Configurar las variables de entorno

Crea tu archivo `.env` a partir del ejemplo:

```bash
cp .env.example .env
```

Edita el archivo y configura tus credenciales:

```dotenv
POSTGRES_DB=contactos
POSTGRES_USER=appuser
POSTGRES_PASSWORD=CAMBIA_ESTA_CLAVE
```

Utiliza una contraseña propia y segura. El archivo `.env` contiene información sensible y no debe subirse al repositorio.

### 3. Construir y levantar los contenedores

```bash
docker compose up -d --build
```

Este comando construye la imagen de la API e inicia los servicios definidos en Docker Compose.

### 4. Verificar los contenedores

```bash
docker compose ps
```

Consultar los registros de la aplicación:

```bash
docker compose logs -f api
```

Consultar los registros de PostgreSQL:

```bash
docker compose logs -f db
```

### 5. Acceder a PostgreSQL

Para conectarte al CLI de PostgreSQL dentro del contenedor:

```bash
docker exec -it contactos-db \
  sh -c 'psql -U "$POSTGRES_USER" -d "$POSTGRES_DB"'
```

Una vez conectado, puedes listar las tablas con:

```sql
\dt
```

Para salir:

```sql
\q
```

## Migraciones de Entity Framework Core

Las migraciones permiten mantener la estructura de PostgreSQL sincronizada con los modelos de la aplicación.

Si tienes instalado el SDK de .NET 10 y la herramienta `dotnet-ef`, puedes ejecutar las migraciones desde el proyecto.

Cuando trabajes desde el host, la variable `DB_CONNECTION` debe apuntar al puerto publicado de PostgreSQL, normalmente `localhost:5432`, y no al nombre interno `db`.

Ejemplo para una terminal Linux, utilizando las credenciales de tu archivo `.env`:

```bash
set -a
. ./.env
set +a

export DB_CONNECTION="Host=localhost;Port=5432;Database=${POSTGRES_DB};Username=${POSTGRES_USER};Password=${POSTGRES_PASSWORD}"

dotnet ef migrations list
dotnet ef database update
```

**Importante:** ejecuta la actualización cuando PostgreSQL esté disponible y revisa las migraciones existentes antes de crear otras nuevas. Si la migración falla por un índice único, revisa los datos duplicados y las restricciones correspondientes antes de volver a ejecutarla.

## Pruebas de la API

El proyecto incluye el archivo `prueba.http`, que puedes utilizar como base para probar las solicitudes desde un cliente HTTP compatible.

También puedes utilizar `curl` o Postman.

La API está configurada para escuchar en el puerto `8080` del contenedor, publicado en el puerto `8080` del host.

```bash
curl -i http://localhost:8080
```

Esta solicitud sirve para comprobar la respuesta de la ruta raíz si está definida. Para probar las operaciones de contactos, utiliza las rutas reales configuradas en los controladores.

## Seguridad y buenas prácticas

- Configurar las credenciales mediante variables de entorno.
- Excluir `.env` del control de versiones.
- Mantener una plantilla `.env.example` sin secretos reales.
- Evitar incluir contraseñas en el código fuente o en los archivos de configuración versionados.
- Utilizar validaciones y restricciones en los modelos de datos.
- Revisar los logs y errores de la aplicación.
- Mantener actualizadas las dependencias y las imágenes de Docker.

Para un entorno productivo se recomienda utilizar un gestor de secretos, limitar la exposición de puertos, configurar HTTPS y agregar análisis automatizados de vulnerabilidades.

## Detener el entorno

Para detener los servicios sin eliminar los datos persistidos:

```bash
docker compose down
```

Para volver a iniciarlos:

```bash
docker compose up -d
```

El volumen `postgres_data` conserva los datos de PostgreSQL entre reinicios.

**Advertencia:** `docker compose down -v` elimina los volúmenes asociados y puede ocasionar la pérdida de los datos almacenados.

## Mejoras futuras

- Incorporar un pipeline CI/CD para compilación y pruebas automatizadas.
- Agregar análisis de vulnerabilidades de dependencias e imágenes mediante herramientas como Trivy.
- Implementar pruebas unitarias y de integración.
- Incorporar documentación OpenAPI/Swagger cuando corresponda.
- Configurar monitoreo, logging estructurado y manejo centralizado de errores.

## Autor

**Brayan Pacheco**

Repositorio: https://github.com/BrayanPacheco05/PruebaDevsecops
