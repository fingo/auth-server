echo ----------PUBLISHING----------

Path=%PATH%;C:\CSharpInternship16DontDelete\VSWebExternalTools;
cd "C:\Program Files (x86)\Jenkins\jobs\Auth_CheckIn\workspace\AuthServer"

cd Fingo.Auth.ManagementApp
"C:\Program Files\dotnet\dotnet.exe" publish
cd ..

cd Fingo.Auth.AuthServer
"C:\Program Files\dotnet\dotnet.exe" publish
cd ..

appcmd stop site Auth_ManagementApp
taskkill /F /IM dotnet.exe
"C:\Program Files\IIS\Microsoft Web Deploy V3\msdeploy.exe" -source:contentPath="C:\Program Files (x86)\Jenkins\jobs\Auth_CheckIn\workspace\AuthServer\Fingo.Auth.ManagementApp\bin\Debug\netcoreapp1.0\publish" -dest:contentPath="C:\inetpub\Auth\ManagementApp" -verb:sync
appcmd start site Auth_ManagementApp

appcmd stop site Auth_AuthServer
taskkill /F /IM dotnet.exe
"C:\Program Files\IIS\Microsoft Web Deploy V3\msdeploy.exe" -source:contentPath="C:\Program Files (x86)\Jenkins\jobs\Auth_CheckIn\workspace\AuthServer\Fingo.Auth.AuthServer\bin\Debug\netcoreapp1.0\publish" -dest:contentPath="C:\inetpub\Auth\AuthServer" -verb:sync
copy "C:\Program Files (x86)\Jenkins\jobs\Auth_CheckIn\workspace\AuthServer\Fingo.Auth.AuthServer\common_passwords_list.txt" "C:\inetpub\Auth\AuthServer"
appcmd start site Auth_AuthServer

cd "C:\Program Files (x86)\Jenkins\jobs\Auth_CheckIn\workspace\JenkinsScripts"