FROM registry.cn-shenzhen.aliyuncs.com/gmmy/dotnetcore-runtime-base
WORKDIR /app
COPY src/ZCYX.FRMSCore.Web.Host/publish/. .
ENTRYPOINT ["dotnet", "ZCYX.FRMSCore.Web.Host.dll"]