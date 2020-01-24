FROM mcr.microsoft.com/dotnet/core/sdk:2.2-alpine AS build
WORKDIR /src

COPY src/SFA.DAS.ToolService.Authentication/*.csproj SFA.DAS.ToolService.Authentication/
COPY src/SFA.DAS.ToolService.Core/*.csproj SFA.DAS.ToolService.Core/
COPY src/SFA.DAS.ToolService.Infrastructure/*.csproj SFA.DAS.ToolService.Infrastructure/
COPY src/SFA.DAS.ToolService.Web/*.csproj SFA.DAS.ToolService.Web/
RUN dotnet restore SFA.DAS.ToolService.Web/SFA.DAS.ToolService.Web.csproj

COPY src/SFA.DAS.ToolService.Authentication/ SFA.DAS.ToolService.Authentication/
COPY src/SFA.DAS.ToolService.Core/ SFA.DAS.ToolService.Core/
COPY src/SFA.DAS.ToolService.Infrastructure/ SFA.DAS.ToolService.Infrastructure/
COPY src/SFA.DAS.ToolService.Web/ SFA.DAS.ToolService.Web/
WORKDIR /src/SFA.DAS.ToolService.Web
RUN dotnet build -c release --no-restore

FROM build AS publish
RUN dotnet publish -c release --no-build -o /app

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-alpine
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SFA.DAS.ToolService.Web.dll"]
