node ('BoldDesk') 
{
timestamps
{
  timeout(time: 2700000, unit: 'MILLISECONDS') 
  {
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
	stage 'Code violation'
	try
	{
		gitlabCommitStatus("Code violation")
		{
			bat 'powershell.exe -ExecutionPolicy ByPass -File build/build.ps1 -Script '+env.WORKSPACE+"/build/build.cake -Target GitLeaks"+ " -settings_skipverification=true"
		}
	}
	catch(Exception e) 
	{
		echo "Exception in code violation stage \r\n"+e
		currentBuild.result = 'FAILURE'
	}
}
	stage 'Delete Workspace'
	
	// Archiving artifacts when the folder was not empty
    if(fileExists('cireports'))
    {
        archiveArtifacts artifacts: 'cireports/', excludes: null
    }
    
    step([$class: 'WsCleanup'])
  }
}
}
