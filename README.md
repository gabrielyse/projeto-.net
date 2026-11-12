# PrimeiraApi

## PostgreSQL já integrado

O `compose.yaml` inicia um PostgreSQL 16 com banco `primeiraapi`. A API já tem o provider Npgsql, o `ApplicationDbContext` e a conexão registrados, mas **nenhuma tabela ou migration foi criada**.

### Iniciar apenas o banco enquanto desenvolve localmente

```bash
docker compose up -d postgres
dotnet run --project PrimeiraApi
```

A conexão de desenvolvimento usa `localhost:5433` (a porta interna do container continua `5432`), banco/usuário `primeiraapi`/`postgres` e senha `postgres`. Para mudar a senha do container, defina `POSTGRES_PASSWORD` antes de executar o Compose. Se mudar essa senha, atualize também a connection string local usando User Secrets ou a variável `ConnectionStrings__DefaultConnection`.

### Iniciar API e banco em containers

```bash
docker compose up --build
```

Nesse caso a API usa o host `postgres` internamente; o banco continua acessível no host pela porta `5433`.

## Próximo passo do tutorial

Quando você criar as entidades, adicione os `DbSet`s ao `PrimeiraApi/Data/ApplicationDbContext.cs` e então crie/aplique a migration:

```bash
dotnet tool install --global dotnet-ef --version 10.*
dotnet ef migrations add InitialCreate --project PrimeiraApi
dotnet ef database update --project PrimeiraApi
```

Esses comandos são deliberadamente manuais: este setup não criou a tabela `employees` nem inseriu dados.
