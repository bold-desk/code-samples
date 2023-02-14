node ('BoldDesk') 
{
timestamps
{
  timeout(time: 2700000, unit: 'MILLISECONDS') 
  {
	stage 'Install Software'
	try
	{
        //Set Time Zone
        echo "Setting Time Zone as India Standard Time..."
        bat 'tzutil /s "India Standard Time"'
        echo "Time Zone changed successfully..."
	}
	catch(Exception e)
    {
        echo "Exception in Install Software stage \r\n"+e
        currentBuild.result = 'FAILURE'
    }

    stage 'Checkout' 
    try
    {	
	    checkout scm
    }
    catch(Exception e)
    {
        echo "Exception in checkout stage \r\n"+e
        currentBuild.result = 'FAILURE'
    }     
	
if(currentBuild.result != 'FAILURE')
{ 
	stage 'Build Source'
	try
	{		
	    gitlabCommitStatus("Build")
		{
			bat 'powershell.exe -ExecutionPolicy ByPass -File build/build.ps1 -Script '+env.WORKSPACE+"/build/build.cake -Target build -NugetServerUrl "+env.nugetserverurl + " -settings_skipverification=true"
	 	}
            def files = findFiles(glob: '**/cireports/errorlogs/*.txt')

            if(files.size() > 0)
            {
                currentBuild.result = 'FAILURE'
            }
    }
	catch(Exception e) 
    {
        echo "Exception in build source stage \r\n"+e
		currentBuild.result = 'FAILURE'
    }
} 

if(currentBuild.result != 'FAILURE')
{
	stage 'Code violation'
	try
	{
		gitlabCommitStatus("Code violation")
		{
			bat 'powershell.exe -ExecutionPolicy ByPass -File build/build.ps1 -Script '+env.WORKSPACE+"/build/build.cake -Target codeviolation"+ " -settings_skipverification=true"
		}
	}
	catch(Exception e) 
	{
		echo "Exception in code violation stage \r\n"+e
		currentBuild.result = 'FAILURE'
	}
}
 
 if(currentBuild.result != 'FAILURE' && env.publishBranch.contains(githubSourceBranch))
 { 
	 stage 'Publish'
	 try
	 {	    
	     gitlabCommitStatus("Publish")
		 {			
			  bat 'powershell.exe -ExecutionPolicy ByPass -File build/build.ps1 -Script '+env.WORKSPACE+"/build/build.cake -Target publish -nugetapikey "+env.nugetapikey+' -revisionNumber '+env.revisionNumber+' -nugetserverurl '+env.nexusnugetserverurl+" -StudioVersion "+env.studio_version
	 	 }
     } 
	  catch(Exception e) 
     {
		 currentBuild.result = 'FAILURE'
     }
  }	


	stage 'Delete Workspace'
	
	// Archiving artifacts when the folder was not empty
	
    def files = findFiles(glob: '**/cireports/**/*.*')      
    
    if(files.size() > 0) 		
    { 		
        archiveArtifacts artifacts: 'cireports/', excludes: null 		
    }
	
	   step([$class: 'WsCleanup']) 	
	   }
}
}
