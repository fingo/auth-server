# Introduction
Fingo AuthServer is a project made by three students on summer internship programme in [Fingo](http://www.fingo.pl/en/) using .NET Core 1.0.0 and .NET Framework 4.6.1. The aim was to create a server whose API one can use to authorize users  --  something like "login in with Facebook" -- and a web app to manage the server comfortably.

Each institution which wants to use our server creates their own project using management app, gets a unique project id, and later uses it in communication with API. On the other hand each user that wants to log in to an application that uses our server must (once) create an account in management app. The server's main role is to issue and verify authorization tokens ([JSON Web Token](https://tools.ietf.org/html/rfc7519) standard).

In the solution the following architectural and design patterns were used: dependency injection, repository pattern, factory method pattern, representational state transfer, model-view-controller.

# Solution structure
1. Web applications
    *  Fingo.Auth.AuthServer
    *  Fingo.Auth.ManagementApp
2. Domain layer
    * Fingo.Auth.Domain.AuditLogs
    * Fingo.Auth.Domain.CustomData
    * Fingo.Auth.Domain.Infrastructure
    * Fingo.Auth.Domain.Models
    * Fingo.Auth.Domain.Policies
    * Fingo.Auth.Domain.Projects
    * Fingo.Auth.Domain.Users
3. Persistance layer
    * Fingo.Auth.DbAccess
4. Core layer
    * Fingo.Auth.AuthServer.Client
    * Fingo.Auth.Infrastructure
    * Fingo.Auth.JsonWrapper
5. Tests

## 1. Web applications
### AuthServer
The server is a ASP NET Core web API with a few HTTP POST methods.

(I) /api/authentication/**AcquireToken** function takes three parameters: user's login, password, and a unique project id. Possible results are:
*  *authenticated* - when one was successful to log in; then there is also a *jwt* property with JSON Web Token, which user should store in cookies and send everytime they need to prove their identity.
*  *not_authenticated* - wrong login or password were given.
*  *wrong_access_token* - wrong unique project id.
*  *password_expired* - password of given user expired; this HTTP response has code "302 Found" and redirects to API method where one can change password to a new one.
*  *account_expired* - account expired, administrator of the project can log in to Management App and set a new expiration date.
*  *error* - see *error_details* property.

Example:
```
POST authserver/api/authentication/acquiretoken HTTP/1.1
(..)

login=usr1&password=complicated_password&projectGuid=8dbc7265-97b6-4d3e-bc56-2cfc777b9ec7
```
```
HTTP/1.1 200 OK
(..)

{
    "result": "authenticated",
    "jwt": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJsb2dpbiI6InNvbWVfbG9naW4iLCJwcm9qZWN0LWd1aWQiOiI0OTlkYjAwNC0yNTQxLTQ1YjEtYTljZC1mZmU2YjI0MmY4ZTIiLCJleHAiOjE0NzAzMTMwMjd9.IybdaTYOkwFShk-_b66nnYiEmoaCxu6jcl5wkZYzHWw"
}
```

(II) /api/authentication/**VerifyToken** function takes two parameters: JSON Web Token to verify and a unique project id. Server's answer is JSON with *result* property:
* *token_valid* - given JSON Web Token is valid, it did not expire and was cryptographically signed with server's secret key.
* *token_invalid* - bad token format or token wasn't signed with proper key.
* *token_expired*
* *wrong_access_token* - wrong unique project id.
* *error* - see *error_details* property.

Example:
```
POST authserver/api/authentication/verifytoken HTTP/1.1
(..)

jwt=doesitlook.like.jsonwebtoken?&projectGuid=a01a050c-baaa-aaa-0aaa-e5ccccccc1ce
```
```
HTTP/1.1 200 OK
(..)

{"result":"token_invalid"}
```

(III) /api/account/**CreateNewUserInProject** - takes 6 parameters: activationToken, login, password, firstName, lastName and a unique project id. Created user will have to activate his account by entering URL managementapp/activate/activationToken. Possible results:
* *user_created_in_project* - action was successful.
* *password_length_incorrect* - project has enabled "Minimum password length" policy and given password is too short.
* *password_violates_required_characters_constraint* - project has enabled "Required password characters" policy and given password does not satisfy it.
* *error* - see *error_details* property in JSON result.

Example in which wrong project id was given:
```
POST authserver/api/account/createnewuserinproject HTTP/1.1
(..)

login=new_login&password=their_password&projectGuid=aaaaaaaa-aaaa-43da-90b0-2bb25cb418c1&activationToken=random_token&firstName=John&lastName=Smith
```
```
HTTP/1.1 200 OK
(..)

{
    "result": "error",
    "errordetails": "Could not find id of project with guid: aaaaaaaa-aaaa-43da-90b0-2bb25cb418c1"
}
```

(IV) /api/account/**PasswordChangeForUser** - takes userEmail, password, newPassword, confirmedNewPassword and returns:
* *users_password_changed* - everything ok.
* *password_length_incorrect* - some project in which user is has enabled "Minimum password length" policy and given password is too short.
* *password_violates_required_characters_constraint* - some project in which user is has enabled "Required password characters" policy and given password does not satisfy it.
* *error* - see *error_details* included in response.

Example:
```
POST authserver/api/account/passwordchangeforuser HTTP/1.1
(..)

userEmail=login&password=password&newPassword=new&confirmedNewPassword=new&projectGuid=ba909b38-1e35-43da-90b0-2bb25cb418c1
```
```
HTTP/1.1 200 OK
(..)

{"result":"password_violates_required_characters_constraint"}
```

### ManagementApp

A web application for managing authorization server. Depending on your privileges, you can modify projects, users, see list of all users assigned to a project, etc. On the authorization layer, management app is like any other institution using the server -- has its own project in the database, a unique project id, and uses API to authorize users.

App was made using a model-view-controller architectural pattern. There are two main models: user and project model. Eight controllers: AccountController which is responsible for logging in, signing up etc., AuditLogController for "Audit log" tab, BaseController from which other controllers inherit and contains some generic methods such as Alert(), CustomDataController, ModalWindowsController which contain redirects to partial views of modal windows, PolicyController, ProjectsController and UsersController for managing projects and users. A few views: login page, signing up page, changing password page, projects list, project details, users list, user details, audit log.

Configuration -- you have to set a connection string to your database, server's API adresses, management app unique project id and some other things in configuration file (appsettings.(environment).json, where (environment) is development, production etc.).

## 2. Domain layer
The layer between projection layer and persistance layer.
### Domain.AuditLogs
There is a factory that gets all entries from our audit log in database. In fact, audit log is a storage for every event published in EventBus (see Domain.Infrastructure).
### Domain.CustomData
A project's administrator can add a set of fully configurable custom data to each user. There are three types: text (with values from configured set), number (with values from configured range) and boolean. E.g. project administrator can add "role" which is a string from set {"user", "moderator", "admin"} and "age" which is an integer from range [1, 100] etc. Data has its default value but in Management App it is possible to set it manually for each user (let's say in order to choose forum's moderators). If you use our server to authorize people, you'll get user's data from **AcquireToken**. For instance when user logs in, you send his credentials to authserver/api/authentication/AcquireToken and in returned JSON Web Token payload you see:
```
{
  "login": "user_login",
  "project-guid": "ac00001c-abcd-abcd-abcd-e550000511ce",
  "exp": 1473074815,
  "role": "moderator"
}
```
In Domain.CustomData there are implementations of the feature -- factories that expose single actions on custom data, serializing and deserializing service to store it in database etc.
### Domain.Infrastructure
There is a EventBus -- a bus that anyone can add events to, or subscribe to certain types of events / to all of them. For instance logger is subscribed to every type of event and each time someone publishes an event to EventBus, logger has it's handler function called (which logs the event to the log).

EventBus keeps a dictionary of pairs (EventType, ListOfSubsciptions) and every subscription contains a handler function to call. Each event is a class, e.g. "ProjectAdded", which may contain some detailed information like string with message.
### Domain.Models
It's a place for models: a few of user and a few of project -- they differ from one another in level of detail they contain. It's good to expose as few information as is deemed necessary at a particular moment.
### Domain.Policies
Projects (services that use the AuthServer) have a possibility to enable policies. There are four of them:
* *Account expiration date* - checked every logging in - user's account expires at a day which administrator can set in Management App.
* *Password expiration date* - checked every logging in - user is forced to change his password after his current one is older than e.g. half a year.
* *Required password characters* - checked at creating account/changing password - there are three options that may be set: at least one special character, at least one digit, at least one upper-case character. 
* *Minimum password length* - checked at creating account/changing password.

### Domain.Users and Domain.Projects
This library has implementations of single action factories. As far as projects are concerned, there are AddProject, DeleteProject, GetAllProject, GetProject, UpdateProject factories. They are later injected in order to interact with database.
## 3. Persistance layer
The layer closest to the database.
### DbAccess
Database connection, configuration and access using Entity Framework. Here are all tables and relations defined. There is also a GenericRepository implementation with more specialised repositories (UserRepository, ...), folder with database migrations, basic models and enums.
## 4. Core layer
### AuthServer.Client
Server client has three services: PostService which is able to send a HTTP POST and receive an answer and RemoteTokenService & RemoteAccountService which use PostService and communicate with server's API. PostService is extracted from token and account services so that one is able to mock it in unit tests.

### Infrastructure
Infrastructure contains Serilog (logging framework) wrapper -- it was made generic in order that one can determine (explicitly type) a class into which logger is injected and then logger will include this information in the logs.  

### JsonWrapper
As much information passed between our projects is in JSON format, a JsonWrapper was created so it's easy for everyone to use the same standard. JSONs are objects of JsonObject class, and there are specified strings for properties and values that we use. E.g. JsonValues.NotAuthenticated is equal to "not_authenticated". It makes using JSON format both easy (due to IntelliSense) and clean -- there are no hardcoded strings.

## 5. Tests

Each project has a related test project. Additionaly, there is a UiTests project which emulates a user with web browser which logs in, uses management app and checks whether everything is as it should be. 

# Continuous integration

During development Jenkins was used as a continuous integration tool. After each
```
git commit
git push
```
Jenkins was building, testing and publishing the code. One can find scripts that were used in JenkinsScripts and AuthServer folders. Besides built-in Jenkins integration with Git, Slack and a plugin to read XUnit tests results, our scripts and other shell commands were executed as follows:
```
JenkinsScripts\build_script.bat
JenkinsScripts\publish_scipt.bat
JenkinsScripts\test_script.bat
JenkinsScripts\nuget_restore_script.bat
msbuild.exe /p:PublishProfile=FingoJenkins /p:Configuration=Release AuthServer\Fingo.Auth.UiTests\Fingo.Auth.UiTests.csproj
JenkinsScripts\XUnit4JenkinsExecutor.bat <full path to AuthServer folder>
```
# Frameworks used

*  [Entity Framework](https://github.com/aspnet/EntityFramework)
*  [Serilog](https://github.com/serilog/serilog)
*  [Moq](https://github.com/moq/moq4)
*  [XUnit](https://xunit.github.io/)
*  [Selenium](https://github.com/SeleniumHQ/selenium)
*  [JWT](https://github.com/jwt-dotnet/jwt)
*  [Json.NET](http://www.newtonsoft.com/json)
*  [Autofac](https://autofac.org/)
*  [Bootstrap Pagination](http://www.pontikis.net/labs/bs_pagination/)