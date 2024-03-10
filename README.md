This API is used for effectively managing the Top up beneficieries.
It is written in .net 8 ,sql server and uses the below packages,
Dapper,Swagger,Serilog,Identity4

It has the token authentication setup but for now it is disabled.

To Test this API follow the below steps.
1. Clone the Repository
2. Download the Queries to be executed in SQL server.
3. Update the Connection strings in the AppSettings.Development.json
4. Install/Update the nesscary packages from Nugetpackage manager in VS
5. Also, Clone the  ExternalBalanceService API  - https://github.com/dotnetcorenoob/ExternalBalanceService.git
   and makes sure it is running
6. Run the solution ,you should be taken to the SwaggerUI
   
