FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY *.csproj .
RUN dotnet restore
COPY . .
USER root
RUN dotnet publish -c Release -o /app/publish
FROM mcr.microsoft.com/dotnet/aspnet:9.0 as runtime
RUN getent group noroot || addgroup --gid 1001 noroot
RUN id -u deploy || adduser --system --uid 1001 --home /home/deploy deploy
RUN mkdir -p /home/deploy && chown deploy:noroot /home/deploy
ENV HOME=/home/deploy
WORKDIR /app
COPY --from=build --chown=deploy:noroot /app/publish .
USER deploy
EXPOSE 3001
ENTRYPOINT [ "dotnet", "luchito-net.dll" ]