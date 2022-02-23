# weather.io_GRPC

A .net core 5.0 gRPC weather mock for playing with HTTPS on local IIS, local docker, and remote k8s 

## build & run

From powershell run
 ```
 dotnet run
 ```

## test

You can only test the connection using the weather.io.grpc.console

 for http use reroute (this will reroute to https port) use
 ```
 weather.io.grpc.console.exe -s http://localhost:80
 ```

 for direct https test use\
 ```
 weather.io.grpc.console.exe -s https://localhost:5001
 ```

# Dev env

Just checkout, build and run.
1. Debug/x64/weather.io.grpc -> ctrl_f5 
2. Debug/x64/weather.io.grpc.console -> ctrl_f5 

## Handle System.InvalidOperationExceprtion : Unable to configure HTTPS endpoint for dev env

Just create some developer certificates by running the following command
```
dotnet dev-certs https
```
You will get a security warning for a self-signed certificate. just accept and click "yes".
(Note -is you are using VS2019 or above, it will create it for you).
You can see the developer certificate for use with "localhost".
Tou xcan see it in `certmgr`, under `trusted root certificate authorities`->`certificate`, undel localhost.

# create a custom domain on local machine

## edit hosts file (for local name)
Cheat the DNS!, browse to `c:\Windows\System32\drivers\etc\hosts.` file, and add your local ip and map it to weather.io name

```
10.30.50.29 weather.io
```
## Create a new selfsigned certificate

```
$cert = New-SelfSignedCertificate -certstorelocation cert:\localmachine\my -dns weather.io
$pwd = ConvertTo-SecureString -String "Ngsoft123" -Force -AsPlainText
$certpath = "Cert:\localmachine\my\$(cert.Thumbprint)"
Export-PfxCertificate -Cert $certpath -FilePath c:\temp\weather.pfx -Password $pwd
```

the run `certmgr`, and under `trusted root certificate authorities`->`certificate` rightclick, import the certificate at `c:\temp\weather.pfx` , using the password `Ngsoft123`

! Even in development environemt it would be wise to use developer secret ! Put a simple JSON based on your windows profile!
in csproj add under propertygroup a secret
```
    <UserSecretsId>e54ee127-f902-4a7b-96c1-18e23446aacd</UserSecretsId>
```
Then run 
```
dotnet user-secrets set "CertPassword" "Ngsoft123"
```
!note it's in `c:\Users\<your name here>\AppData\Roaming\Microsoft\UserSecrets\e54ee127-f902-4a7b-96c1-18e23446aacd\secrets.json`

Add path of certificate to your `appsetting.json` -
```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*",
  "CertPath": "c:\\temp\\weather.pfx"
}
```

# TODO

[ ] Fix issues with running the console app in docker container.

