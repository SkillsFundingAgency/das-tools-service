FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build

ENV PROJECT_PATH=SFA.DAS.ToolService.Web/SFA.DAS.ToolService.Web.csproj
COPY ./src ./src
WORKDIR /src

RUN dotnet restore $PROJECT_PATH
RUN dotnet build $PROJECT_PATH -c release --no-restore
RUN dotnet publish $PROJECT_PATH -c release --no-build -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine
WORKDIR /app
COPY --from=build /app .
RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
ENTRYPOINT ["dotnet", "SFA.DAS.ToolService.Web.dll"]
