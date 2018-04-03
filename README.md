# aerogear-dotnet-sdk
Current Build : [![Build status](https://ci.appveyor.com/api/projects/status/ja8kvbwbn8duth1k?svg=true)](https://ci.appveyor.com/project/AeroGear/aerogear-xamarin-sdk)


.Net and Xamarin libraries for the AeroGear Mobile Core

## Generate API Docs

The API docs are generated using [Doxygen](http://www.stack.nl/~dimitri/doxygen/).

1. Install Doxygen following the [instructions](http://www.stack.nl/~dimitri/doxygen/manual/install.html)
2. Locate the `doxygen` command line tool.
    * For Mac, if `Doxygen` is installed in `/Applications`, you can find the command line tool in `/Applications/Doxygen.app/Contents/Resources/doxygen`
3. To generate the API docs, run the command line tool in the root of the SDK directory:
    
    ```bash
    cd aerogear-xamarin-sdk
    # on Mac, run this command
    /Applications/Doxygen.app/Contents/Resources/doxygen Documentations/Doxyfile
    ```

    The generated files can be found in the [Documentations](./Documentations) directory.
4. To add more directories for API doc generation, update the `INPUT` configuration in the [Doxygen configuration file](./Documentations/Doxyfile). Use spaces to separate the files or directories.
