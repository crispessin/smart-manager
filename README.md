# Smart Manager

Projeto para cadastro de compradores da SmartHint. 

Foi desenvolvido como uma aplicação ASP.NET Core MVC 8.0 e o Entity Framework para interações com o banco de dados, optou-se pela abordagem "Code-First" para modelagem do banco de dados. O banco de dados escolhido foi o SQLite.

## Tecnologias utilizadas

* Projeto em ASP.NET Core MVC 8.0
* Sqlite
* EF Core

## Como executar

O projeto está configurado com Sqlite para simplificar a execução do projeto.

Para rodar via linha de comando, na raiz do projeto executar:

```
 dotnet run --project .\SmartManager.csproj
```

Acesse a [Aplicação](http://localhost:5041/)

O projeto roda as migrations automaticamente.

## Migrations

### Gerar novas Migrations

Para gerar novas migrations utilizando o Console do Gerenciador de Pacotes execute:

```
Add-Migration "init" -o Persistence\Migrations
```

### Como executar as Migrations manualmente

Executar as migrations com o comando: Update-Database
