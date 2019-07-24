FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-alpine AS runtime
WORKDIR /src
EXPOSE 80
EXPOSE 443
COPY /src/SFA.DAS.ToolService.Web/publish ./
ENTRYPOINT ["dotnet", "SFA.DAS.ToolService.Web.dll"]