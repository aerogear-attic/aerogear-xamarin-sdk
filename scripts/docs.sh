# Generate Doxygen documentation for the AeroGear Xamarin SDK.
# NOTE: This script requires Doxygen to be installed and available on the path.
#
# Installation:
# macOS using Homebrew: brew install doxygen
# Using yum: yum -y install doxygen

# Confirm that Doxygen is installed. If it's not exit and provide an error.
command -v doxygen >/dev/null 2>&1 || { echo >&2 "Doxygen is not found. To install, run: (yum) yum -y install doxygen | (Homebrew) brew install doxygen."; exit 1; }

echo "Creating documentation for the Xamarin SDK."
doxygen docs/Doxyfile

echo "Documentation is stored in the Documentations/html/ directory."
