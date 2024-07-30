# Smart Manager

Projeto para cadastro de compradores da SmartHint. 

Foi desenvolvido como uma aplicação ASP.NET Core MVC 8.0 e o Entity Framework para interações com o banco de dados, optou-se pela abordagem "Code-First" para modelagem do banco de dados. O banco de dados escolhido foi o SQLite.

## Tecnologias utilizadas

* Projeto em ASP.NET Core MVC 8.0
* Sqlite
* EF Core
* Bogus para geração de daos aleatórios par testes
* X.PagedList para paginação
* BCrypt.Net para criptografar a senha

### Decisões arquiteturais (Architecture Decision Records)

- Foi utilizado o o SQLLite por ser um banco leve e não ser necessário subir uma imagem docker local para rodar o projeto localmente;
- O projeto utiliza o estilo clássico MVC, sem uso de camadas como Repository ou Services devido ao tamanho do projeto e simplicidade, como exemplo de um projeto seguindo padrão de camadas orientado ao DDD, temos esse do portifolio: https://github.com/crispessin/ProductManagerAPI;
- Optei utilizar "Client" para representar o nome de dominio do modelo de negócio do cliente da Loja, poderias ter utilizado Buyer ou Customer;
- Optei por incluir ao iniciar o projeto o carregamento de dados para facilitar os testes de paginação e pesquisas, para isso utilizei o Bogus para gerar dados "Fakes";
- A senha sempre é salva com criptografia;
- Quando o cadastro é editado não é necessário informar a senha, só se o usuário quiser alterar.

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
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Como executar as Migrations manualmente

Executar as migrations com o comando: Update-Database

## Telas

Tela inicial da aplicação:

![Tela_Inicial.png](Docs%2FTela_Inicial.png)

Cadastro de Cliente:

![Cadastro_Cliente.png](Docs%2FCadastro_Cliente.png)

Edição de Cliente:

![Edicao_Cliente.png](Docs%2FEdicao_Cliente.png)

## Contato

[Linkedin](https://www.linkedin.com/in/cristianepfernandes/)