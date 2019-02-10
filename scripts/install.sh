PROJECT_PATH="src/IdentityServerCli.Console"
PROJECT_NAME="IdentityServerCli.Console"

echo "Stage 1: Uninstalling previous version"
dotnet tool uninstall -g $PROJECT_NAME

echo "Stage 2: Packing tool"
dotnet pack $PROJECT_PATH

echo "Stage 3: Installing tool"
dotnet tool install --global --add-source $PROJECT_PATH/nupkg $PROJECT_NAME 