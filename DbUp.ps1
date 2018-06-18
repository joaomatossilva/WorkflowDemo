.\build.ps1

$job = Start-Job -ScriptBlock {
	Set-Location $using:PWD
    Add-Type -Path ".\WorkflowDemo1\bin\WorkflowDemo1.dll"
	[WorkflowDemo1.Data.Migrations.DemoMigrationRunner]::Up()
}
Wait-Job $job
Receive-Job $job

