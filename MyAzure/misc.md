# Miscellanous

## Account

```powershell
Connect-AzAccount
Get-AzContext
Get-AzLocation
```

## Setup

```powershell
$loc = 'southeastasia'
$rg = 'Lab'
New-AzResourceGroup -Name $rg -Location $loc
#  Remove-AzResourceGroup -Name $rg -Force
```