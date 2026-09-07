# Sistema de Gestão de Consultas UVV

Aplicação web desenvolvida em ASP.NET Core MVC para gerenciar usuários e consultas médicas/profissionais.

## Tecnologias
- ASP.NET Core MVC
- C#
- Entity Framework Core
- SQL Server
- Bootstrap

## Requisitos
- .NET SDK 8.0
- SQL Server Express com a instância `SQLEXPRESS` (ou ajuste a connection string para sua instância)

## Configuração do Banco de Dados
1. Ajuste a string de conexão no arquivo `appsettings.json`.
2. Crie o banco e aplique as migrações. Pela linha de comando:

```bash
dotnet ef database update
```

No Console do Gerenciador de Pacotes do Visual Studio, use:

```powershell
Update-Database
```

3. Inicie a aplicação:

```bash
dotnet run --project .\UVVConsultas.csproj
```

Se o terminal estiver na pasta acima de `UVVConsultas`, use:

```bash
dotnet run --project .\UVVConsultas\UVVConsultas.csproj
```

4. Acesse no navegador usando a URL exibida no terminal. Com o perfil padrão, use:

```text
http://localhost:5234
```

Para executar em uma porta específica:

```bash
dotnet run --project .\UVVConsultas.csproj --urls http://localhost:5010
```

> Em instalações com autenticação Windows, a string padrão utiliza o usuário do Windows que executa a aplicação. Ajuste `Server` em `appsettings.json` caso sua instância tenha outro nome.

## Estrutura da Aplicação
- `Models`: entidades `Usuario` e `Consulta`
- `Controllers`: `AccountController` e `ConsultasController`
- `Data`: `AppDbContext`
- `ViewModels`: validação e dados de entrada
- `Views`: telas de cadastro, login e consultas

## Funcionalidades
- Cadastro de usuário com senha criptografada
- Login com autenticação por cookies
- Registro, visualização, edição e exclusão de consultas
- Validação com Data Annotations
- Proteção de rotas autenticadas com `[Authorize]`

## Vídeo Demonstrativo
- Link do vídeo: [Demonstração do sistema no YouTube](https://youtu.be/Z3jSboZmoHs)

O vídeo deve demonstrar: cadastro de usuário, login, cadastro de consulta e consulta da listagem protegida.
