echo "Stage 1: Begin sonar scan"
dotnet sonarscanner begin /k:$PROJECT_KEY /d:sonar.organization=$SONAR_ORGANIZATION /d:sonar.host.url=$SONAR_URL /d:sonar.login=$SONAR_SECRET

echo "Stage 2: Analize project"
dotnet build
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover | true

echo "Stage 3: End sonar scan"
dotnet sonarscanner end /d:sonar.login=$SONAR_SECRET