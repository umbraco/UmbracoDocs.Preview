# UmbracoDocs Preview

The [UmbracoDocs project](https://github.com/umbraco/UmbracoDocs) repository contains only markdown which will not render correctly unless served by the
[Our.Umbraco](https://github.com/umbraco/OurUmbraco) project...

**Until now!.**

Introducing UmbracoDocs Preview the new application which renders UmbracoDocs without all of the overhead of
setting up our.umbraco.

Be warned this project is somewhat experimental and definitely has some limitations, it's unlikely to ever gain any
sort of official support.

## Getting started

Clone this repo then initialize submodules (UmbracoDocs)

```bash
# from repo root

$ git submodule init
$ git submodule update
$ dotnet run -p ./src/umbraco.Docs.Preview.UI
# server now running on http://localhost:5000
```

## Watching markdown files

Optional but recommended when editing.

```bash 
# from repo root
$ npm ci
$ npm start
# proxy server now running on http://localhost:5001
```

And you're ready to view the docs by default at http://localhost:5000 or via the proxy (auto reloads changes) a http://localhost:5001

This could be useful for testing contributions to UmbracoDocs or just to facilitate reading the docs without an internet
connection.
