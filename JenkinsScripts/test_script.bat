echo ----------TESTING----------

cd "C:\Program Files (x86)\Jenkins\jobs\Auth_CheckIn\workspace\AuthServer"

cd Fingo.Auth.DbAccess.Tests
"C:\Program Files\dotnet\dotnet.exe" test  -parallel all -xml ..\..\test-dbaccess.xml
cd ..

cd Fingo.Auth.ManagementApp.Tests
"C:\Program Files\dotnet\dotnet.exe" test  -parallel all -xml ..\..\test-managementapp.xml
cd ..

cd Fingo.Auth.AuthServer.Tests
"C:\Program Files\dotnet\dotnet.exe" test  -parallel all -xml ..\..\test-authserver.xml
cd ..

cd Fingo.Auth.Domain.Projects.Tests
"C:\Program Files\dotnet\dotnet.exe" test  -parallel all -xml ..\..\test-domainprojects.xml
cd ..

cd Fingo.Auth.Domain.Users.Tests
"C:\Program Files\dotnet\dotnet.exe" test  -parallel all -xml ..\..\test-domainusers.xml
cd ..

cd Fingo.Auth.Domain.Models.Tests
"C:\Program Files\dotnet\dotnet.exe" test  -parallel all -xml ..\..\test-domainmodels.xml
cd ..

cd Fingo.Auth.Domain.Infrastructure.Tests
"C:\Program Files\dotnet\dotnet.exe" test  -parallel all -xml ..\..\test-domaininfrastructure.xml
cd ..

cd Fingo.Auth.AuthServer.Client.Tests
"C:\Program Files\dotnet\dotnet.exe" test  -parallel all -xml ..\..\test-authserverclient.xml
cd ..

cd Fingo.Auth.Domain.Policies.Tests
"C:\Program Files\dotnet\dotnet.exe" test  -parallel all -xml ..\..\test-domainpolicies.xml
cd ..

cd Fingo.Auth.Domain.CustomData.Tests
"C:\Program Files\dotnet\dotnet.exe" test  -parallel all -xml ..\..\test-domaincustomdata.xml
cd ..

cd Fingo.Auth.Domain.AuditLogs.Tests
"C:\Program Files\dotnet\dotnet.exe" test  -parallel all -xml ..\..\test-domainauditlogs.xml
cd ..

cd "C:\Program Files (x86)\Jenkins\jobs\Auth_CheckIn\workspace\JenkinsScripts"