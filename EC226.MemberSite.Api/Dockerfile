#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Set timezone to Taiwan
ENV TZ=Asia/Taipei
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

ENV ASPNETCORE_HTTP_PORTS=80

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# End Install
WORKDIR /
COPY . .

RUN dotnet restore "EC226.MemberSite.Api/EC226.MemberSite.Api.csproj"

WORKDIR "/EC226.MemberSite.Api"
RUN dotnet build "EC226.MemberSite.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EC226.MemberSite.Api.csproj" -c Release -o /app/publish

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "EC226.MemberSite.Api.dll","--environment=Production"]
