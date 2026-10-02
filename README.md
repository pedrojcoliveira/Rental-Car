# JAP Rent-a-Car

Aplicação web para gestão de uma frota de aluguer de veículos: registo de veículos, clientes e contratos de aluguer, com o estado de cada veículo (Disponível/Alugado) atualizado automaticamente.

Desenvolvida no âmbito do desafio técnico de desenvolvimento de software do Grupo JAP.

## Tecnologias

- ASP.NET Core 8 MVC
- Entity Framework Core 8 (abordagem Code-First)
- SQL Server (LocalDB)
- Bootstrap 5
- xUnit (testes unitários)

## Como executar

### Requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server LocalDB (incluído no Visual Studio 2022, workload "ASP.NET and web development")

### Passos

1. Clonar o repositório:
```bash
   git clone <URL-DO-REPOSITÓRIO>
```
2. Abrir a solução no Visual Studio 2022.
3. Criar a base de dados aplicando as migrations, no **Package Manager Console**:
```
   Update-Database
```
   Ou na linha de comandos, dentro da pasta do projeto web:
```bash
   dotnet ef database update
```
4. Executar a aplicação (F5).

A connection string está em `appsettings.json` e usa por defeito `(localdb)\mssqllocaldb`. Para usar outra instância de SQL Server, basta alterá-la.

### Testes

No Visual Studio: **Test → Test Explorer → Run All**. Ou na linha de comandos, na raiz da solução:
```bash
dotnet test
```

## Funcionalidades

| Área | Funcionalidades | Validações |
|---|---|---|
| **Veículos** | Registar, editar, consultar, eliminar e listar com estado atual | Campos obrigatórios; ano de fabrico entre 1900 e o ano atual; matrícula única |
| **Clientes** | Registar, editar, consultar, eliminar e listar | Campos obrigatórios; email único e com formato válido; telefone com 9 dígitos, apenas números, a começar por 2 ou 9 |
| **Contratos** | Registar, consultar e listar com estado (Agendado, Em curso, Terminado) | Data de início não anterior a hoje; data de fim posterior à de início; quilometragem inicial não negativa; o veículo não pode ter contratos com datas sobrepostas |

## Decisões técnicas

- **Estado do veículo calculado, não guardado.** O estado Disponível/Alugado não é uma coluna da base de dados: é calculado a partir dos contratos sempre que a listagem é aberta. Assim nunca fica dessincronizado, e um veículo volta a ficar disponível automaticamente quando o contrato termina. O cálculo é feito numa única consulta SQL (`EXISTS`), evitando o problema N+1.
- **Unicidade garantida também na base de dados.** A matrícula e o email têm índices únicos. A aplicação valida antes de gravar, para mostrar mensagens claras ao utilizador, e a base de dados funciona como segunda linha de defesa.
- **Normalização dos dados.** As matrículas são guardadas em maiúsculas e sem espaços, e os emails em minúsculas, para que variações da mesma matrícula ou do mesmo email não contornem a regra de unicidade.
- **Contratos imutáveis.** Os contratos podem ser criados e consultados, mas não editados nem eliminados, por serem registos históricos. Pela mesma razão, não é possível eliminar um cliente ou veículo com contratos associados.
- **Prevenção de reservas sobrepostas.** Para além das regras pedidas, a aplicação impede que o mesmo veículo seja alugado em períodos que se sobreponham.
- **Validações no modelo.** As regras de cada campo usam Data Annotations; as regras que comparam vários campos (como as datas do contrato) usam `IValidatableObject`. Assim a mesma validação é aplicada nos formulários e nos testes unitários.
- **`DateOnly` nas datas dos contratos.** Um aluguer trabalha com dias, não com horas, o que evita problemas de fusos horários.
- **ViewModel na listagem de veículos.** A entidade `Vehicle` representa apenas a tabela; o estado calculado é transportado num ViewModel próprio.

## Estrutura do projeto

```
Rental Car Project/
├── Controllers/      # Lógica de cada página e regras que dependem da BD
├── Data/             # AppDbContext (configuração do EF Core)
├── Migrations/       # Histórico de alterações à base de dados
├── Models/           # Entidades e validações
├── ViewModels/       # Dados preparados para as views
└── Views/            # Páginas Razor
Rental_Car_Project.Tests/
└── ...               # Testes unitários das regras de validação
```