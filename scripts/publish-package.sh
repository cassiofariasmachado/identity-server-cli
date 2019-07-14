PROJECT_PATH="src/IdentityServerCli.Console"

echo "Stage 1: Packing tool"
export IS4_VERSION=$(git describe --abbrev=0) && dotnet pack $PROJECT_PATH

echo "Stage 2: Publishing tool"
dotnet nuget push $PROJECT_PATH/nupkg/IdentityServerCli.$IS4_VERSION.nupkg -k $NUGET_API_KEY -s https://api.nuget.org/v3/index.json