$job = Start-Job -ScriptBlock {
	Set-Location $using:PWD
    Add-Type -Path ".\WorflowDemo1\bin\WorflowDemo1.dll"
	[WorflowDemo1.Data.Migrations.DemoMigrationRunner]::AllDown()
}
Wait-Job $job
Receive-Job $job

