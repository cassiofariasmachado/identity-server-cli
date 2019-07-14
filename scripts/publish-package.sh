PROJECT_PATH="src/IdentityServerCli.Console"
IS4_VERSION=$(git describe)

echo "Stage 2: Packing tool"
dotnet pack $PROJECT_PATH

echo "Stage 3: Publishing tool"
dotnet nuget push $PROJECT_PATH/nupkg/IdentityServerCli.$IS4_VERSION.nupkg -k $NUGET_API_KEY -s https://api.nuget.org/v3/index.json