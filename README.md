# TaskFlow 📋✨

Una **API REST moderna** para gestión de tareas desarrollada con **.NET 8** que implementa **Clean Architecture** y patrones **CQRS**. Permite crear, organizar y administrar tareas de manera eficiente con autenticación JWT y validación robusta.

## 🌟 Características principales

### 🚀 Funcionalidades
- ✅ **Gestión completa de tareas**: CRUD con estados y prioridades
- 👤 **Sistema de usuarios**: Registro, login y autenticación JWT
- 👥 **Organización por grupos**: Agrupa tareas por proyectos o equipos
- 🔍 **Filtrado avanzado**: Por estado, prioridad, usuario, fechas
- 🔒 **Seguridad robusta**: JWT + hash de contraseñas
- ✨ **Validación inteligente**: FluentValidation para datos de entrada
- 📚 **Documentación completa**: Swagger/OpenAPI integrado

### 🏗️ Arquitectura
- **DDD** (Domain-Driven Design)
- **Clean Architecture** con separación clara de responsabilidades
- **CQRS Pattern** (Command Query Responsibility Segregation)
- **Mediator Pattern** para desacoplamiento
- **Repository Pattern** para abstracción de datos
- **Minimal APIs** de .NET 8

## 🛠️ Stack tecnológico

| Tecnología | Versión | Propósito |
|------------|---------|-----------|
| .NET | 8.0 | Framework principal |
| ASP.NET Core | 8.0 | Web API |
| FluentValidation | 12.0 | Validación de modelos |
| JWT Bearer | 8.0 | Autenticación |
| Swagger/OpenAPI | 6.6 | Documentación API |
| Scrutor | 6.1 | Inyección de dependencias |

## 📁 Estructura del proyecto

```
src/
├── 🌐 TaskFlow.WebApi/          # Capa de presentación - Endpoints y configuración
├── 💼 TaskFlow.Application/     # Capa de aplicación - Casos de uso y DTOs
├── ⚙️ TaskFlow.Infrastructure/  # Capa de infraestructura - Implementaciones
└── 🏛️ TaskFlow.Domain/         # Capa de dominio - Entidades y lógica de negocio
```

### 🎯 Principios de Clean Architecture
- **Domain Layer**: Entidades, enums y excepciones de negocio
- **Application Layer**: Use cases, comandos, queries y DTOs
- **Infrastructure Layer**: Repositorios, servicios externos y persistencia
- **Presentation Layer**: Controllers, endpoints y configuración web

## 🚀 Inicio rápido

### 📋 Prerrequisitos
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Editor de código (Visual Studio 2022, VS Code, JetBrains Rider)

### 🔧 Instalación

1. **Clona el repositorio:**
```bash
git clone https://github.com/adesduende/TaskFlow
cd TaskFlow
```

2. **Configura los secretos para desarrollo:**
```bash
cd src/TaskFlow.WebApi
dotnet user-secrets init
dotnet user-secrets set "JwtSettings:SecretKey" "mi-clave-super-secreta-de-32-caracteres-minimo-para-jwt-seguro"
```

3. **Restaura dependencias y ejecuta:**
```bash
dotnet restore
dotnet run --project src/TaskFlow.WebApi
```

4. **¡Explora la API! 🎉**
   - **Swagger UI**: `https://localhost:7xxx/swagger`
   - **API Base**: `https://localhost:7xxx/`

## ⚙️ Configuración

### 🌍 Variables de entorno (Producción)
```bash
export JwtSettings__SecretKey="clave-secreta-super-larga-y-segura"
```

### 🔐 User Secrets (Desarrollo)
```bash
dotnet user-secrets set "JwtSettings:SecretKey" "clave-desarrollo"
```

## 📖 API Reference

### 🌐 Endpoints públicos
| Método | Endpoint | Descripción |
|--------|----------|-------------|
| `POST` | `/user` | Registrar nuevo usuario |
| `POST` | `/user/login` | Iniciar sesión |

### 🔒 Endpoints protegidos (Requieren JWT)

#### 👤 Usuarios
| Método | Endpoint | Descripción |
|--------|----------|-------------|
| `GET` | `/user/{id}` | Obtener usuario por ID |

#### ✅ Tareas
| Método | Endpoint | Descripción |
|--------|----------|-------------|
| `GET` | `/tasks` | Listar tareas (con filtros) |
| `GET` | `/task/{id}` | Obtener tarea específica |
| `POST` | `/task` | Crear nueva tarea |
| `PUT` | `/task/{id}/status` | Cambiar estado |
| `PUT` | `/task/{id}/assign` | Asignar a usuario |
| `DELETE` | `/task/{id}` | Eliminar tarea |

#### 👥 Grupos
| Método | Endpoint | Descripción |
|--------|----------|-------------|
| `GET` | `/group/{id}` | Obtener grupo por ID |
| `POST` | `/group` | Crear nuevo grupo |
| `PUT` | `/group` | Actualizar grupo |
| `DELETE` | `/group/{id}` | Eliminar grupo |

### 🎛️ Filtros disponibles para `/tasks`
- **`status`**: `NotStarted`, `InProgress`, `Completed`, `OnHold`, `Cancelled`
- **`priority`**: `Low`, `Medium`, `High`, `Critical`
- **`user`**: GUID del usuario asignado
- **`timeLimit`**: Fecha límite (ISO 8601)
- **`createdAt`**: Fecha de creación (ISO 8601)

## 💡 Ejemplos de uso

### 1️⃣ Registro e inicio de sesión

```bash
# Crear usuario
curl -X POST "https://localhost:7xxx/user" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Ana García",
    "email": "ana@empresa.com",
    "password": "Password123!"
  }'

# Iniciar sesión
curl -X POST "https://localhost:7xxx/user/login" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "ana@empresa.com",
    "password": "Password123!"
  }'

# Respuesta: { "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..." }
```

### 2️⃣ Gestión de tareas

```bash
# Crear tarea
curl -X POST "https://localhost:7xxx/task" \
  -H "Authorization: Bearer <tu-jwt-token>" \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Implementar API de notificaciones",
    "description": "Desarrollar sistema push para móvil",
    "priority": "High",
    "timeLimit": "2025-08-15T17:00:00Z",
    "userId": "guid-del-usuario",
    "groupId": "guid-del-grupo"
  }'

# Obtener tareas con filtros
curl -X GET "https://localhost:7xxx/tasks?status=InProgress&priority=High" \
  -H "Authorization: Bearer <tu-jwt-token>"

# Cambiar estado de tarea
curl -X PUT "https://localhost:7xxx/task/{id}/status" \
  -H "Authorization: Bearer <tu-jwt-token>" \
  -H "Content-Type: application/json" \
  -d '"Completed"'
```

## 🧪 Testing con Swagger UI

1. **Ejecutar aplicación**: `dotnet run --project src/TaskFlow.WebApi`
2. **Abrir Swagger**: Navegar a `https://localhost:7xxx/swagger`
3. **Registrar usuario**: Usar endpoint `POST /user`
4. **Obtener JWT**: Hacer login con `POST /user/login`
5. **Autenticarse**: Clic en 🔓 "Authorize" y pegar el token
6. **Probar endpoints**: Usar cualquier endpoint protegido

## 📊 Estados y prioridades

### 📈 Estados de tareas
- **`NotStarted`** (1): Tarea sin comenzar
- **`InProgress`** (2): En desarrollo
- **`Completed`** (3): Finalizada
- **`OnHold`** (4): En pausa
- **`Cancelled`** (5): Cancelada

### 🚨 Niveles de prioridad
- **`Low`** (1): Prioridad baja
- **`Medium`** (2): Prioridad media
- **`High`** (3): Alta prioridad
- **`Critical`** (4): Urgente

## 🔐 Seguridad implementada

- 🔑 **JWT Authentication** con RS256
- 🔐 **Password hashing** con PBKDF2
- ✅ **Input validation** con FluentValidation
- 🌐 **HTTPS enforcement** en producción
- 🔒 **User Secrets** para desarrollo
- 🌍 **Environment variables** para producción

## 🤝 Contribuir al proyecto

1. **Fork** el repositorio
2. **Crear rama**: `git checkout -b feature/nueva-funcionalidad`
3. **Commit cambios**: `git commit -m 'feat: agregar nueva funcionalidad'`
4. **Push**: `git push origin feature/nueva-funcionalidad`
5. **Pull Request**: Abrir PR con descripción detallada

### 📝 Convenciones de commits
- `feat:` Nueva funcionalidad
- `fix:` Corrección de bugs
- `docs:` Documentación
- `style:` Formato de código
- `refactor:` Refactorización
- `test:` Agregar tests

## 🗺️ Roadmap

### v1.1.0 (Próximo)
- [ ] Notificaciones push
- [ ] Comentarios en tareas
- [ ] Archivos adjuntos
- [ ] Dashboard analytics

### v2.0.0 (Futuro)
- [ ] Base de datos persistente
- [ ] API GraphQL
- [ ] Aplicación móvil
- [ ] Integración con Slack/Teams

## 🐛 Reportar issues

¿Encontraste un bug? [Crea un issue](../../issues) con:
- 📝 Descripción detallada
- 🔄 Pasos para reproducir
- 💻 Información del entorno
- 📸 Screenshots (si aplica)

## 📄 Licencia

Este proyecto está bajo la **Licencia MIT**. Ver [LICENSE](LICENSE) para más información.

---

<div align="center">

**TaskFlow v1.0.0** - Gestión de tareas moderna y eficiente

⭐ Si te gusta el proyecto, ¡dale una estrella! ⭐

[Reportar Bug](../../issues/new?labels=bug&title=fix:) • [Solicitar Feature](../../issues/new?labels=enhancement&title=feat:) • [Documentación](../../wiki)

</div>