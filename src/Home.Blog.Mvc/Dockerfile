FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Home.Blog.Mvc/Home.Blog.Mvc.csproj", "src/Home.Blog.Mvc/"]
COPY . .
RUN ls -alR
RUN dotnet restore "src/Home.Blog.Mvc/Home.Blog.Mvc.csproj"
WORKDIR "/src/src/Home.Blog.Mvc"
RUN dotnet build "Home.Blog.Mvc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Home.Blog.Mvc.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Home.Blog.Mvc.dll"]