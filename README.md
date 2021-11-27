# UmbracoDocs Preview App

The [UmbracoDocs project](https://github.com/umbraco/UmbracoDocs) repository contains only markdown which will not render correctly unless served by the
[Our.Umbraco](https://github.com/umbraco/OurUmbraco) project...

**Until now!.**

Introducing UmbracoDocs Preview App! This tool renders UmbracoDocs without all of the overhead of
setting up our.umbraco.

Be warned this project is somewhat experimental and definitely has some limitations, it's unlikely to ever gain any
sort of official support as the documentation platform is scheduled for a refresh.

Until 1.0.0 everything is subject to change and it's highly likely there will never be a 1.0.0 release.

## Installing from nuget.org

This project can be found on nuget at https://www.nuget.org/packages/Umbraco.Docs.Preview.App/

You can run the following to install as a global tool.

```bash
$ dotnet tool install --global Umbraco.Docs.Preview.App --version 0.1.0-beta
```

## Running the project

```bash
$ umbracodocs # run from UmbracoDocs repo root
```

And you're ready to view the docs by default at http://localhost:5000.

When you update a markdown document your browser will automatically reload.

This could be useful for testing contributions to UmbracoDocs or just to facilitate reading the docs without an internet
connection.


## install local build

```bash
$ dotnet pack -o dist
$ dotnet tool install -g --add-source ./dist --version 0.1.0-beta Umbraco.Docs.Preview.App
```

## I don't want this anymore
```bash
$ dotnet tool uninstall -g umbraco.docs.preview.app
```
