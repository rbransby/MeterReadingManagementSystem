# MeterReadingManagementSystem

Completed as part of a code challenge for Ensek.

## Requirements
- REQUIRES .NET 6.0 PREVIEW 7 (VERY IMPORTANT) 
- https://dotnet.microsoft.com/download/dotnet/6.0


## Developer Setup

- Install .net 6.0 Preview 7 (link above)
- Install the EF Core CLI tools
  - ```dotnet tool install --global dotnet-ef```
- Clone this repo
- From the root of the repo (where the sln file is) run EF Migrations
  - ```dotnet ef database update --project MeterReadingsManagementSystem\Server\MeterReadingsManagementSystem.Server.csproj```
  - This solution uses sqlite, this will create the sqlite schema and seed the intial data. It will be stored in the local appdata folder for your OS.
  - You can clean this up later by deleting MeterReadingManagementSystem.db from ```%localappdata%``` usually ```Users\[username]\AppData\Local``` on a windows machine.
  
- Run the solution
  - ```dotnet run --project MeterReadingsManagementSystem\Server\MeterReadingsManagementSystem.Server.csproj```
  - Or to make life easier you can navigate to the server folder and ```dotnet run``` from there.
  - Or you can just open the solution in visual studio (I used 2022 preview) and press play :).
 
- Navigate to ```https://localhost:5001/swagger```
- Use the swagger UI to test uploading a CSV file and see the response!

## Things I wanted to do but didnt get time
- A blazor based UI. The thinking behind having the Shared assembly (besides it coming for free in the template) was to consume those model classes from the client which is what Blazor is great for.
- More unit tests. Again had to compromise a little, focussed on testing the complex part (the processing class).
- More error handling in the controller (check for invalid file, etc).
