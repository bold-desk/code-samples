#tool "nuget:?package=xunit.runner.console"
#tool "nuget:?package=JetBrains.dotCover.CommandLineTools"
#addin "nuget:?package=Cake.Npm&version=1.0.0"
#addin nuget:?package=Cake.FileHelpers&version=4.0.1
#addin "nuget:?package=Cake.WebDeploy"
#addin "nuget:?package=Newtonsoft.Json&version=13.0.2"
using System.Text.RegularExpressions

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////


var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var nugetapikey = Argument<string>("nugetapikey","");
var nugetserverurl = Argument<string>("nugetserverurl","");
var studioversion = Argument<string>("studio_version","");
var buildNumber = Argument("buildNumber", "");

var logFilename = "Syncfusion.Azure.ServiceBus.txt";
var applicationPath = @"/src/AzureServiceBus";
var applicationSolutionPath = @"/Syncfusion.Azure.ServiceBus.sln";
var projectName = @"/Syncfusion.Azure.ServiceBus.csproj";

var sourceIncludedCodeCoverage = new List<string>(){"Syncfusion"};
var sourceExcludedFromCodeCoverage = new List<string>(){"Syncfusion.Azure.ServiceBus.Test"};

var includeFilterValue = string.Join("+:", sourceIncludedCodeCoverage.Select(i=> "*"+i+"*;"));
var excludeFilterValue = string.Join("-:", sourceExcludedFromCodeCoverage.Select(i=> "*"+i+"*;"));
var codeCoverageFilter = string.Format("+:{0}-:{1}", includeFilterValue, excludeFilterValue);

var unitTestProjectDirPathPattern = @"../test/**/*.csproj";

var studioVersionList = studioversion.Split('.').ToList();
var versionList = studioVersionList.Take(studioVersionList.Count - 1).ToList();
versionList.Add(buildNumber);
var packageVersion = string.Join(".", versionList);

var projectFramework = "netstandard2.1";

var cireports = Argument("cireports","../cireports");
var SCSReportDir = cireports + "/securitycodescan";
var FXReportDir = cireports + "/fxcopviolation";
var StyleCopReportsDir = cireports + "/stylecopviolation";
var xUnitViolationReportDir = cireports + "/xunitviolation";
var codecoverageReportDir = cireports + "/codecoverage";
var xunitReportDir = cireports + "/xunitreport";
var packageDir = cireports + "/package";
var DetectSecretScanReportDir = cireports + "/secret";

var styleCopReport = StyleCopReportsDir + "/StyleCopViolations.txt";
var fxCopReport = FXReportDir + "/FXCopViolations.txt";
var securityCodeScanReport = SCSReportDir + "/SecurityCodeScanViolations.txt";
var xunitReport = xUnitViolationReportDir + "/xUnitViolations.txt";
var dotCodeCoverageReport = codecoverageReportDir+"/UnitTestCover.dcvr";
var dotCodeCoverageHTMLReport = codecoverageReportDir+"/UnitTestCover.html";
var dotCodeCoverageXMLReport = codecoverageReportDir+"/UnitTestCover.xml";
var unitTestingReport = xunitReportDir+"/TestResult.xml";

var buildStatus = true;

var errorlogFolder = cireports + "/errorlogs/";
var waringsFolder = cireports + "/warnings/";

var apiServerIP = Argument<string>("apiServerIP","");
var apiServerPort = Argument<string>("apiServerPort","");
var apiSiteName = Argument<string>("apiSiteName","");
var apiServerUserName = Argument<string>("apiServerUserName","");
var apiServerPassword = Argument<string>("apiServerPassword","");
 
var currentDirectory = MakeAbsolute(Directory("../"));
var currentDirectoryInfo = new DirectoryInfo(currentDirectory.FullPath);

var projDir =  currentDirectory + applicationPath;
var projectPath = projDir + projectName;
var binDir = String.Concat(projDir,"bin" ) ;

var solutionFile = currentDirectory + applicationSolutionPath; 

var outputDir = Directory(binDir) + Directory(configuration);
var fxCopViolationCount = 0;
var styleCopViolationCount = 0;
var securityCodeScanWarningCount = 0;
var xUnitWarningCount = 0;

var securityCodeRegex = "warning SCS";
var fxCopRegex = "warning CA";
var styleCopRegex = "warning SA";
var styleCopAnalyzersRegex = "warning SX";
var xUnitRegex = "warning xUnit";
var apiAnalyzerRegex = "warning API";
var asyncAnalyzerRegex = "warning AsyncFixer";
var cSharpAnalyzerRegex = "warning RS";
var mvcAnalyzerRegex = "warning MVC";
var entityFrameworkRegex = "warning EF";
var rosylnatorAnalyzerRegex = "warning RCS";

var framework = Argument("framework", projectFramework);

var buildSettings = new DotNetCoreBuildSettings
     {
         Framework = framework,
         Configuration = configuration,
         OutputDirectory = outputDir
     };

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
	var binDirectories = currentDirectoryInfo.GetDirectories("bin", SearchOption.AllDirectories);
    var objDirectories = currentDirectoryInfo.GetDirectories("obj", SearchOption.AllDirectories);
    
    foreach(var directory in binDirectories){
        CleanDirectories(directory.FullName);
    }
    
    foreach(var directory in objDirectories){
        CleanDirectories(directory.FullName);
    }
    if (DirectoryExists(outputDir))
        {
            DeleteDirectory(outputDir, recursive:true);
        }
});

Task("Restore")
    .Does(() => {
        DotNetCoreRestore(solutionFile);
    });
	
Task("DeleteLogFile")
	.Does(()=>{
		
		if(FileExists(errorlogFolder + logFilename)){
			DeleteFile(errorlogFolder + logFilename);
		}
		
		if(FileExists(waringsFolder + logFilename)){
			DeleteFile(waringsFolder + logFilename);
		}				
	});
	
	var npmSettings = 
        new NpmInstallSettings 
        {
            WorkingDirectory = projDir 			     
        }; 

Task("Build")   
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .Does(() => {	
	try { 	
  
	 // NpmInstall(npmSettings); 

	  MSBuild(solutionFile , settings => 
	   settings.SetConfiguration(configuration)
	   .WithProperty("DeployOnBuild","true")
       .AddFileLogger(new MSBuildFileLogger{LogFile = waringsFolder + logFilename, MSBuildFileLoggerOutput=MSBuildFileLoggerOutput.WarningsOnly})
	  );  
	   } 	
	catch(Exception ex) {        
		throw new Exception(String.Format("Please fix the project compilation failures"));  
	}
    }); 

Task("Get-Security-Scan-Reports")
 .Does(() =>
 { 
    var securityCodeScanWarning = FileReadText(waringsFolder + logFilename);    
	securityCodeScanWarningCount = Regex.Matches(securityCodeScanWarning, securityCodeRegex).Count;

	if (DirectoryExists(SCSReportDir))
    {
	 DeleteDirectory(SCSReportDir, recursive:true);
	}

    if(securityCodeScanWarningCount != 0)
    {        
       Information("There are {0} Security Code violations found", securityCodeScanWarningCount);
    }
	
	if (!DirectoryExists(SCSReportDir)) {
		CreateDirectory(SCSReportDir);
	}

	FileWriteText(securityCodeScanReport, "Security Violations Error(s) : " + securityCodeScanWarningCount);
});

Task("Get-Fx-cop-Reports")
 .Does(() =>
 { 
	if (DirectoryExists(FXReportDir))
    {
	 DeleteDirectory(FXReportDir, recursive:true);
	}	 

	var fxCopWarning = FileReadText(waringsFolder + logFilename);
	fxCopViolationCount = Regex.Matches(fxCopWarning, fxCopRegex).Count;
    fxCopViolationCount += Regex.Matches(fxCopWarning, apiAnalyzerRegex).Count;
	fxCopViolationCount += Regex.Matches(fxCopWarning, asyncAnalyzerRegex).Count;
	fxCopViolationCount += Regex.Matches(fxCopWarning, cSharpAnalyzerRegex).Count;
	fxCopViolationCount += Regex.Matches(fxCopWarning, mvcAnalyzerRegex).Count;
	fxCopViolationCount += Regex.Matches(fxCopWarning, entityFrameworkRegex).Count; 
    fxCopViolationCount += Regex.Matches(fxCopWarning, rosylnatorAnalyzerRegex).Count; 

    if(fxCopViolationCount != 0)
    {        
       Information("There are {0} FXCop violations found", fxCopViolationCount);
    }
	
	if (!DirectoryExists(FXReportDir)) {
		CreateDirectory(FXReportDir);
	}
	
	FileWriteText(fxCopReport, "FXCop Error(s) : " + fxCopViolationCount);
});

Task("Get-StyleCop-Reports")
 .Does(() =>
 { 
	if (DirectoryExists(StyleCopReportsDir))
    {
	 DeleteDirectory(StyleCopReportsDir, recursive:true);
	}	

	var styleCopWarning = FileReadText(waringsFolder + logFilename);
	styleCopViolationCount += Regex.Matches(styleCopWarning, styleCopRegex).Count;
	styleCopViolationCount += Regex.Matches(styleCopWarning, styleCopAnalyzersRegex).Count;

    if(styleCopViolationCount != 0)
    {        
       Information("There are {0} StyleCop violations found", styleCopViolationCount);
    }
	
	if (!DirectoryExists(StyleCopReportsDir)) {
		CreateDirectory(StyleCopReportsDir);
	}

	FileWriteText(styleCopReport, "Style Cop Error(s) : " + styleCopViolationCount);
});

Task("Get-xUnit-Reports")
 .Does(() =>
 { 
	if (DirectoryExists(xUnitViolationReportDir))
    {
	 DeleteDirectory(xUnitViolationReportDir, recursive:true);
	}	

	var xUnitWarning = FileReadText(waringsFolder + logFilename);
	xUnitWarningCount += Regex.Matches(xUnitWarning, xUnitRegex).Count;

    if(xUnitWarningCount != 0)
    {        
       Information("There are {0} xUnit violations found", xUnitWarningCount);
    }
	
	if (!DirectoryExists(xUnitViolationReportDir)) {
		CreateDirectory(xUnitViolationReportDir);
	}
	
	FileWriteText(xunitReport, "xUnit Violations Error(s) : " + xUnitWarningCount);
});

Task("Code-Coverage")
    .ContinueOnError()
    .Does(() =>
    {
		if (!DirectoryExists(cireports))
		{
			CreateDirectory(cireports);
			CreateDirectory(xunitReportDir);
		} else {
			if(!DirectoryExists(xunitReportDir))
			{
				CreateDirectory(xunitReportDir);
			}
			else {
				DeleteDirectory(xunitReportDir, recursive:true);
				CreateDirectory(xunitReportDir);
			}
		}

		if (!DirectoryExists(codecoverageReportDir))
		{
			CreateDirectory(codecoverageReportDir); 
		} else { 
				DeleteDirectory(codecoverageReportDir, recursive:true);
				CreateDirectory(codecoverageReportDir);	
		}

		var projects = GetFiles(unitTestProjectDirPathPattern);
		var settings = new DotCoverCoverSettings()
                    .WithFilter(codeCoverageFilter)
					.WithAttributeFilter("System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute");

        foreach(var project in projects)
        {
            DotCoverCover(
                x => x.DotNetCoreTest(
                     project.FullPath,
                     new DotNetCoreTestSettings() {
						  Configuration = configuration,
						  Logger = $"trx;LogFileName={unitTestingReport}",
        				  NoBuild = true,
						   }
                ),
                dotCodeCoverageReport,
				settings
            );
        }
    });

Task("Unit-Testing")
    .IsDependentOn("Code-Coverage")
    .ContinueOnError()
    .Does(() =>
    {
		DotCoverReport(dotCodeCoverageReport, dotCodeCoverageHTMLReport,
			new DotCoverReportSettings {
				ReportType = DotCoverReportType.HTML
		});			

		DotCoverReport(dotCodeCoverageReport, dotCodeCoverageXMLReport,
			new DotCoverReportSettings {
				ReportType = DotCoverReportType.XML
		});
		
		var  coveragePercent =(from elements in System.Xml.Linq.XDocument.Load(dotCodeCoverageXMLReport).Descendants("Root") 
						select (string)elements.Attribute("CoveragePercent")).FirstOrDefault();
		
		FileStream fs = new FileStream(codecoverageReportDir+"/UnitTestCover.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
		StreamWriter writer = new StreamWriter(fs);
		writer.Write(coveragePercent);
		writer.Close();
		
		Information("CoveragePercent : "+coveragePercent);
    });

Task("codeviolation")
    .IsDependentOn("Get-StyleCop-Reports")
	.IsDependentOn("Get-Fx-cop-Reports")
	.IsDependentOn("Get-Security-Scan-Reports")
	.IsDependentOn("Get-xUnit-Reports")
    .IsDependentOn("Unit-Testing")
	.IsDependentOn("GitLeaks")
    .Does(() =>
{
});

Task("Package")
	.Does(() =>
	{
		if (!DirectoryExists(packageDir)) 
		{
			CreateDirectory(packageDir);
		}

		var packageSettings = new DotNetCorePackSettings
        {
            OutputDirectory = packageDir,
			Configuration = configuration,
            NoBuild = true
        };
        		
		DotNetCorePack(projectPath, packageSettings);

	});

Task("Publish")
	.IsDependentOn("Package")
	.ContinueOnError()
	.Does(()=>{
			var package = GetFiles(packageDir+"\\*.nupkg").FirstOrDefault();
			
			NuGetPush(package, new NuGetPushSettings 
			{
				Source = nugetserverurl,
				ApiKey = nugetapikey
			});
	});

Task("Download-GitLeaks")
  .WithCriteria( !FileExists("./tools/GitLeaks-exe.zip"))
  .ContinueOnError()
  .Does(() =>
{

    DownloadFile("https://github.com/zricethezav/gitleaks/releases/download/v8.15.2/gitleaks_8.15.2_windows_x64.zip", "./tools/GitLeaks-exe.zip");
	Unzip("./tools/GitLeaks-exe.zip", "./tools/GitLeaks/"); 

});

Task("GitLeaks")
  .IsDependentOn("Download-GitLeaks")
  .Does(() =>
{	
	try
	{
		if(DirectoryExists(DetectSecretScanReportDir))
		{
			DeleteDirectory(DetectSecretScanReportDir, recursive:true);
		}
		if(!DirectoryExists(cireports))
		{
			CreateDirectory(cireports);
		}
		if(!DirectoryExists(DetectSecretScanReportDir))
		{
			CreateDirectory(DetectSecretScanReportDir);
		}

		//Download Gitleaks if not exists
		if (!FileExists("./tools/GitLeaks/gitleaks.exe"))
		{
			DownloadFile("https://github.com/zricethezav/gitleaks/releases/download/v8.15.2/gitleaks_8.15.2_windows_x64.zip", "./tools/GitLeaks-exe.zip");
			Unzip("./tools/GitLeaks-exe.zip", "./tools/GitLeaks/"); 
		}

		//Scan for secrets and export report to GitLeaksReport.json
		StartProcess("./tools/GitLeaks/gitleaks.exe", new ProcessSettings{ Arguments ="detect --no-git --report-path "+DetectSecretScanReportDir+"/GitLeaksReport.json --source ../ --config gitleaks.toml"});

		var jsonString = FileReadText(DetectSecretScanReportDir+"/GitLeaksReport.json");
		var jsonObject = Newtonsoft.Json.Linq.JArray.Parse(jsonString);
		var count = jsonObject.Count;

		Information("Number of objects in the JSON file: {0}", count);
	}
	catch(Exception ex)
	{
		throw new Exception(String.Format("Exception thrown in secret detection process, please fix this: " + ex));  
	}
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Build")
    .IsDependentOn("Codeviolation")
	.IsDependentOn("GitLeaks")
	.IsDependentOn("Publish");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);