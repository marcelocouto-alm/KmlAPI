# KML Placemark Filter API

## Descrição do Projeto
Esta é uma API .NET Core para filtrar e exportar placemarks de um arquivo KML, oferecendo recursos de busca e exportação flexíveis.

## Funcionalidades
- Filtrar placemarks por Cliente, Situação, Bairro, Referência e Rua/Cruzamento
- Exportar placemarks filtrados para arquivo KML
- Obter valores únicos para filtros disponíveis

## Pré-requisitos
- .NET 8.0
- SharpKml NuGet Package

## Instalação
1. Clone o repositório
2. Restaure os pacotes NuGet:
   ```
   dotnet restore
   ```
3. Adicione o arquivo KML na pasta `Data/DIRECIONADORES1.kml`

## Endpoints da API

### Filtrar Placemarks
`GET /api/placemarks`
- Retorna placemarks filtrados
- Parâmetros de query: Cliente, Situacao, Bairro, Referencia, RuaCruzamento

### Exportar KML
`POST /api/placemarks/export`
- Exporta placemarks filtrados para arquivo KML
- Envie filtros no corpo da requisição

### Obter Filtros Disponíveis
`GET /api/placemarks/filters`
- Retorna valores únicos para Cliente, Situação e Bairro

```

## Tratamento de Erros
- Retorna BadRequest se nenhum resultado for encontrado
- Validações para filtros:
  - Pelo menos um filtro deve ser preenchido
  - Campos "REFERENCIA" e "RUA/CRUZAMENTO" precisam ter pelo menos 3 caracteres
  - Filtros devem corresponder a valores existentes

## Configuração
Configurar serviços no `Startup.cs` ou `Program.cs`:
```csharp
services.AddSingleton<IKmlService, KmlService>();
services.AddScoped<IPlacemarkService, PlacemarkService>();
```

## Dependências
- SharpKml
- Microsoft.AspNetCore.Mvc
