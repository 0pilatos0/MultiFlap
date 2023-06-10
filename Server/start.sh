export DOTNET_ROOT=$HOME/.dotnet
export PATH=$PATH:$HOME/.dotnet:$HOME/.dotnet/tools
dotnet build
#dotnet ef database drop
dotnet ef database update
dotnet run
