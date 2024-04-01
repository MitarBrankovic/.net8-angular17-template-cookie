# ASP.NET Core 8.0 and Angular 17 project template

This template is simplified version of [Jason-Taylor template](https://github.com/jasontaylordev/CleanArchitecture). ASP.NET Identity library is used for authorization and authentification via [cookie](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/cookie?view=aspnetcore-8.0).

## How to use it?

- clone / download
- rename solution _(optional)_
- change connection string in **appsettings.json**
- add APIs for google and facebook login in **appsettings.json** _(optional)_
- start **TemplateBackend.Web** for .Net application
- `npm i` --> `npm start` for Angular application

---

### Rename solution

- rename solution in Visual studio
- close solution
- rename folder
  ![](/Documentation/Images/renameExample.png)
- right click on sln file in file explorer --> _Edit in notepad_ --> change folder names of projects
- change all namespaces in project with Visual studio using _replace all_

More info at [link1](https://www.youtube.com/watch?v=4-kSmqUxvd8&ab_channel=BrianPringle) and [link2](https://jd-bots.com/2021/06/11/how-to-rename-solution-and-project-name-in-visual-studio/).

### Certificate development expiration

- go to `$\"%APPDATA%\\ASP.NET\\https\"` on windows or `$\"$HOME/.aspnet/https/` on linux and delete all certificates (key and pem files)
- restart computer
- start application - certificates will be generated again

More info at [link](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-dev-certs).
