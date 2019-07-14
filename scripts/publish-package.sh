PROJECT_PATH="src/IdentityServerCli.Console"
IS4_VERSION=$(git describe --abbrev=0)

echo "Stage 1: Packing tool"
dotnet pack $PROJECT_PATH

echo "Stage 2: Publishing tool"
dotnet nuget push $PROJECT_PATH/nupkg/IdentityServerCli.$IS4_VERSION.nupkg -k $NUGET_API_KEY -s https://api.nuget.org/v3/index.json