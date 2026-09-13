FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["Directory.Build.props", "./"]
COPY ["src/Directory.Build.props", "src/"]
COPY ["src/Home.Blog.Mvc/Home.Blog.Mvc.csproj", "src/Home.Blog.Mvc/"]
COPY ["src/Home.Blog.Core/Home.Blog.Core.csproj", "src/Home.Blog.Core/"]
COPY ["src/Home.Blog.Data/Home.Blog.Data.csproj", "src/Home.Blog.Data/"]
COPY ["src/Home.Blog.Common/Home.Blog.Common.csproj", "src/Home.Blog.Common/"]
RUN dotnet restore "src/Home.Blog.Mvc/Home.Blog.Mvc.csproj"
COPY . .
WORKDIR "/src/src/Home.Blog.Mvc"
RUN dotnet build "Home.Blog.Mvc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Home.Blog.Mvc.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=prod
COPY --from=publish /app/publish .
COPY ["src/Home.Blog.Mvc/appsettings.prod.json", "./appsettings.prod.json"]
ENTRYPOINT ["dotnet", "Home.Blog.Mvc.dll"]