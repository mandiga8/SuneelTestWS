# SuneelTestWS
Windows Service Project to fetch data and write to a CSV file on specific time interval

##Technology and Version
1. .NET 6 using C#
2. Visual Studio 2022
3. Windows 10 Operating System

##Reference Documents:
1. Windows Service in .NET 6 : https://docs.microsoft.com/en-us/dotnet/core/extensions/windows-service
2. Windows Service Commands (sc.exe) : https://docs.microsoft.com/en-us/windows-server/administration/windows-commands/sc-create


##Instructions to Setup, Build and Deploy this Application
1. Download of Checkout of the project from master branch
2. Make sure to change the value of "OutputCSVPath" property in appsettings, before running the applicaiton. Also in Test project
3. Open the code in Visual Studio 2022, and add the "PowerService.dll" from https://github.com/kkmoorthy/PetroineosCodingChallenge in both projects. 
	* This DLL reference idly, should be via NuGet package
4. Run the Test cases or the application, to very the code and functionality
5. Run Publish in the Visual Studio. If profile is missing, make sure to create Profile as per the instructios in the Micrisoft Documentation (Reference links)
6. After successful publish, open CMD as Administrator and run below command to install Windows Service
	* sc.exe create "SuneelTestWS" start=auto binpath="D:\Working\dotnet\SuneelTestWS\SuneelTestWS\bin\Release\net6.0\publish\win-x64\SuneelTestWS.exe"
7. To delete the service use below command
	* sc.exe delete "SuneelTestWS"
8. Open the Services window, to see the installation status of our Windows Service. Start if not started
9. Verify the output path, if the files are being generated