```
cd compiler
dotnet publish -c Release
cd ..
vi neon # Update the path to neon.dll
cp neon ~/bin
cd project
dotnet build
```
