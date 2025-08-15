# Run

## Linux

### da cartella publish

```bash
../DA.WI.NSGHSM.Api.Api --urls="https://localhost:5001"
../DA.WI.NSGHSM.Web.Web --urls="https://localhost:5002"
../DA.WI.NSGHSM.IdentityServer.IdentityServer --urls="https://localhost:5003;http://localhost:5013"
```

## Windows

### Build

```cmd
cd da-nsghsm
dotnet publish .\DA.WI.NSGHSM.sln --self-contained --runtime win-x64
```

### Start

```cmd

cd da-nsghsm

pushd DA.WI.NSGHSM.Api\bin\Debug\netcoreapp2.2\win-x64\publish
start "Api" ..\DA.WI.NSGHSM.Api.exe --urls="https://localhost:5001"
popd

pushd DA.WI.NSGHSM.Web\bin\Debug\netcoreapp2.2\win-x64\publish
start "Web" ..\DA.WI.NSGHSM.Web.exe --urls="https://localhost:5002"
popd

pushd DA.WI.NSGHSM.IdentityServer\bin\Debug\netcoreapp2.2\win-x64\publish
start "IS" ..\DA.WI.NSGHSM.IdentityServer.exe --urls="https://localhost:5003;http://localhost:5013"
popd

```
