1) Pre-requisite

 * register to [NuGet central repo](https://www.nuget.org/) and generate your api key. Be sure copy them somewhere, since they won't be visualized anymore
 * one NuGet owner of `aerogear-xamarin-core` should have added you to the list of owners to grant you access

Create a branch with your new release version.

2) Build the project for the desired platform:
 
 * open VisualStudio, open solution
 * choose `Release` on the top bar schema selection select `CoreXXX` where XXX is iOS or Android

3) Bump version in `Core/Core.nuspec`

4) Push to NuGet

 * Make sure nuget.exe is installed and on the PATH. Download From [here](https://dist.nuget.org/win-x86-commandline/latest/nuget.exe)
 * Add your keys by running (only the first time):

````batch
nuget setApiKey <api key>
````

After updating the nuspec with the right version you can pack the release and upload:

```bash
nuget pack Core.nuspec -Symbols
nuget push aerogear-xamarin-core.<version>.nupkg
```

5) Merge the branch
