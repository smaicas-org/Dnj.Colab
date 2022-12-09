param(
	[string]$Name,
	[string]$RemoteUrl
)
if(-not($Name)) { Throw "You must supply a value for -Name" }
if(-not($RemoteUrl)) { Throw "You must supply a value for -RemoteUrl" }

$lowerName = $Name.ToLower()
$upstreamName = "upstream-$lowername"

$status = Invoke-WebRequest $RemoteUrl | Select-Object -Expand StatusCode

if ($status -eq 200){
	git init
	git add .
	git commit -m "Create sub repository $Name"
	git branch -M "master"
	git remote add $upstreamName $remoteUrl
	git push -u $upstreamName master
}
else {
	Throw "$RemoteUrl does not exist. Ensure it is correct and repository is created"
}
