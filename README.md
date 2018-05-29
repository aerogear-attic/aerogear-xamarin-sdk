# AeroGear Xamarin SDK
Current Build : [![Build status](https://ci.appveyor.com/api/projects/status/ja8kvbwbn8duth1k?svg=true)](https://ci.appveyor.com/project/AeroGear/aerogear-xamarin-sdk)

.Net and Xamarin libraries for the AeroGear Mobile Core


|                 | Project Info                                                     |
| --------------- | ---------------------------------------------------------------- |
| License:        | Apache License, Version 2.0                                      |
| Build:          | MSBuild                                                           |
| Documentation:  | TBD							                                     |
| Issue tracker:  | https://issues.jboss.org/browse/AEROGEAR                         |
| Mailing lists:  | [aerogear-users](http://aerogear-users.1116366.n5.nabble.com/)   | 
|                 | [aerogear-dev](https://groups.google.com/forum/#!forum/aerogear) |


## Documentation

1. [End User Getting Started Guide](./docs/modules/getting-started/pages/getting-started.adoc)
2. [SDK architecture](./docs/modules/architecture/pages/architecture.adoc)
3. [NuGet distributing](./docs/modules/contrib/pages/make-dist.adoc)
4. [Development Guide](./docs/modules/contrib/pages/development-guide.adoc)

## List of SDKs

AeroGear Services SDK consist of set of separate SDKs
* [Core](./docs/modules/getting-started/pages/core.adoc): Common base for all SDKs
* [Auth](./docs/modules/getting-started/pages/auth.adoc): Mobile authentication SDK

## License 

 See [LICENSE file](./LICENSE)

## Development

If you would like to help develop AeroGear you can join our [developer's mailing list](https://groups.google.com/forum/#!forum/aerogear), join #aerogear on Freenode, or shout at us on Twitter @aerogears.

## Generate API Docs

The API docs are generated using [Doxygen](http://www.stack.nl/~dimitri/doxygen/).

1. Install Doxygen following the [instructions](http://www.stack.nl/~dimitri/doxygen/manual/install.html)
2. Locate the `doxygen` command line tool.
	* For Mac, if `Doxygen` is installed in `/Applications`, you can find the command line tool in `/Applications/Doxygen.app/Contents/Resources/doxygen`
3. To generate the API docs, run the command line tool in the root of the SDK directory:
	
	```bash
	cd aerogear-xamarin-sdk
	# on Mac, run this command
	/Applications/Doxygen.app/Contents/Resources/doxygen docs/Doxyfile
	```

	The generated files can be found in the [Documentations](./Documentations) directory.
4. To add more directories for API doc generation, update the `INPUT` configuration in the [Doxygen configuration file](./docs/Doxyfile). Use spaces to separate the files or directories.

## Testing

Use Visual Studio for Windows Test Explorer (`Test > Windows > Test Explorer`)  or Visual Studio for Mac (`View > Test`). 

You can run tests also from the Developer Command Line (in Windows).


```bash
msbuild Core\Tests\Core.Tests.csproj /t:Test 
```	


## Questions
Join our [user mailing list](https://groups.google.com/forum/#!forum/aerogear) for any questions or help! We really hope you enjoy app development with AeroGear!

## Found a bug?

If you found a bug please create a ticket for us on [Jira](https://issues.jboss.org/browse/AEROGEAR) with some steps to reproduce it.