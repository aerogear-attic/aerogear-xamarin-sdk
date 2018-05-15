#!/bin/bash

errorHandler() {
	echo "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!"
	echo "ERROR: An error has occurred while creating the repository"
	echo "Please check that you have successfully compiled the aerogear-xamarin-sdk modules and that you have used the DEBUG profile"
	echo "See messages above for details on the error"
	exit -1
}

trap 'errorHandler' ERR

if [ -z "$SOLUTION_DIR" ] || [ -z "$REPO_DIR" ] ; then
	echo "Please configure the SOLUTION_DIR and REPO_DIR with the folder of your VS solution and the folder of your nuget repo"
	exit -1
fi

# creating repo dir if does not exists
mkdir -p "$REPO_DIR"

# The nuspec file is configured to target the 'release' dll. We want to change it to target the `debug` dll
find "$SOLUTION_DIR" -name "*nuspec" | grep -v "Debug\|Release" | xargs -L1 sed -ie 's/\\Release\\/\\Debug\\/g'

echo "CREATING PACKAGES TO BE INSTALLED INTO THE LOCAL REPOSITORY"
echo "==========================================================="
for nuspec in `find "$SOLUTION_DIR" -name "*nuspec" | grep -v "Debug\|Release"` ; do
	echo "Creating package for $(basename $nuspec)"
	nuget pack "$nuspec" > /dev/null
done

# Reverting to 'Release' again
find "$SOLUTION_DIR" -name "*nuspec" | grep -v "Debug\|Release" | xargs -L1 sed -ie 's/\\Debug\\/\\Release\\/g'

echo
echo "INSTALLING PACKAGES INTO THE REPOSITORY"
echo "======================================"
# installing all packages
for package in `find "$SOLUTION_DIR" -name "*nupkg" | grep -v "packages\|Debug\|Release" ` ; do
   echo "Installing package $(basename $package)"
   nuget add $package -source "$REPO_DIR" > /dev/null
done

echo
echo "Repository successfully created/updated in $REPO_DIR"

