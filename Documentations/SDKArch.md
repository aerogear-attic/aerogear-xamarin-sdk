# AeroGear Xamarin SDK architecture

![AeroGear Xamarin SDK architecture](images/ag-xamarin-arch.png)

## Core

SDK contains the Core module that provides access to network, storage and logging capabilities. Core is .NET Standard project, so it can be used in any platform. We currently support only Xamarin Android and iOS projects. 

[Core details](Core.md)

## Service modules

To extend the functionality of the SDK we are using modular architecture. Modules are configured through the Core module and are using Core's functionality (like network access).

### Platform independent module

It's possible to write a module completely platform-independent. You develop it only as the .NET Standard project. 

### Platform dependent module

The platform dependent module consists of three separate projects - .NET Standard, Xamarin Android and Xamarin iOS. Usually you write shared functionality as an abstract service module class and you extend it to contain a platform-specific functionality.

