# Running the application against library deployed on a local repository

The role of this document is describe a simple procedure to enable the linking of the application to libraries deployed on a local NUGET repository
instead of the remote one, to facilitate debugging new implementations.

## Unix like systems

### Envinronment setup

Before you can create your own local repository, you have to install `nuget` on your machine. Since `nuget` is a .NET application, you will need to 
install mono.

#### Installing MONO

1. Download Mono from http://www.mono-project.com/download/stable/ (choose the Visual Studio version)
2. Edit your `.zshrc` (zsh) or `.bashrc` (bash) file adding:
```
MONO_PATH=`cat /etc/paths.d/mono-commands`
PATH="${PATH}:$MONO_PATH"; export PATH;
```
3. Restart you shell

#### Installing `nuget`

1. Download `nuget` with the following command:
```
sudo curl -o /usr/local/bin/nuget.exe https://dist.nuget.org/win-x86-commandline/latest/nuget.exe
```
2. Give `nuget` the execution permission:
```
sudo chmod 755 /usr/local/bin/nuget.exe
```
3. Create a nuget alias
```
alias nuget="mono /usr/local/bin/nuget.exe"
```

To make the alias permanent:

 - **bash**: add the command above to the `.bashrc` file
 - **zsh**: add the command above to the `.zshrc` file
 - **oh-my-zsh**: create a file called `aliases.zsh` in the $HOME/.oh-my-zsh/custom folder containing the command above

After the shell is restarted, the `nuget` command should work:

```
$ nuget
```

#### Creating the local repo

1. Compile the sources of the `aerogear-xamarin-sdk` as `debug` in VisualStudio
2. Create 2 environment variables called `SOLUTION_DIR` and `REPO_DIR`. The first one will point to the folder containing the `aerogear-xamarin-sdk` Visual Studio solution, the second one will be the path of the repository.
If the repository already exists it will be updated, otherwise it will be created.
```
export SOLUTION_DIR=$HOME/work/aerogear-xamarin-sdk
export REPO_DIR=$HOME/nuget
```
3. Run the [[updaterepo.sh](./updaterepo.sh)] script
If everything worked, it should end with something like:
`
Repository successfully created/updated in /home/username/nuget
`
4. Open Visual Studio Community -> Preferences -> Nuget -> Sources and add, as a source, the path visualized by the script
5. Ensure that the source you just added is put before the remote one


That's all. Now your project will search for libraries locally and will fall back searching remotely.

## Windows

### Environment setup

Before you can create your own local repository, you have to install `nuget` on your machine and enable powershell script execution.

#### Installing nuget

1. Download [[nuget](https://www.nuget.org/downloads)] and save it somewhere in yout `PATH`
2. Ensure that `nuget` works by running `nuget` from a command prompt

#### Enable powershell execution

1. Open a command prompt
2. Run the `powershell` command
3. In the `powershell` prompt, issue this command:
```
Set-ExecutionPolicy -Scope CurrentUser Unrestricted
```

### Creating the local repo

1. Compile the sources of the `aerogear-xamarin-sdk` as `debug` in VisualStudio
2. Create 2 environment variables called `SOLUTION_DIR` and `REPO_DIR`. The first one will point to the folder containing the `aerogear-xamarin-sdk` Visual Studio solution, the second one will be the path of the repository.
If the repository already exists it will be updated, otherwise it will be created.
```
SET SOLUTION_DIR=c:\path\to\aerogear-xamarin-sdk
SET REPO_DIR=c:\path\to\repo
```
3. Run the `powershell` command
4. Move to the folder containing the [[updaterepo.ps1](./updaterepo.ps1)] script
5. Run the [[updaterepo.ps1](./updaterepo.ps1)] script
