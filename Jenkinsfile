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
		stage 'GitLeaks'
		try
		{
			gitlabCommitStatus("GitLeaks")
			{
				bat 'powershell.exe -ExecutionPolicy ByPass -File build/build.ps1 -Script '+env.WORKSPACE+"/build/build.cake -Target GitLeaks"+ " -settings_skipverification=true"
			}
		}
		catch(Exception e)
		{
			echo "Exception in GitLeaks stage \r\n"+e
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