#addin "nuget:?package=Newtonsoft.Json&version=13.0.2"

var projectFramework = "netcoreapp3.1";
var cireports = Argument("cireports", "../cireports");
var DetectSecretScanReportDir = cireports + "/secret";

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