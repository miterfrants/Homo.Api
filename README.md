# How To Use

## Development

- 在開發環境執行 `dotnet watch run --launch-profile "dev"`

## Deployment

### Build Docker Image

```
docker build -t ziip-erp-api ./ --rm --network=host
```

### Run Docker Container

```
docker run -d \
-e ASPNETCORE_URLS=http://\*:8080 -e ASPNETCORE_ENVIRONMENT=staging \
-p 8091:8080 \
--name ziip-website-api \
-v /var/project/ziip/static:/static \
-v /var/project/ziip/website/api/secrets.json:/app/secrets.json \
ziip-website-api
```
