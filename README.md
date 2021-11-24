# UmbracoDocs Preview

The [UmbracoDocs project](https://github.com/umbraco/UmbracoDocs) repository contains only markdown which will not render correctly unless served by the
[Our.Umbraco](https://github.com/umbraco/OurUmbraco) project...

**Until now!.**

Introducing UmbracoDocs Preview the new application which renders UmbracoDocs without all of the overhead of
setting up our.umbraco.

Be warned this project is somewhat experimental and definitely has some limitations, it's unlikely to ever gain any
sort of official support.

## Getting setup

We need to UmbracoDocs markdown & images, run the following to initialize the UmbracoDocs submodule.

```bash
$ git submodule init # from repo root
$ git submodule update
```

## Running the project

Optional but recommended when editing.

```bash 
$ npm ci # from repo root
$ npm start # proxy server now running on http://localhost:5001
```

And you're ready to view the docs by default at http://localhost:5001.

When you update a markdown document your browser will automatically reload.

This could be useful for testing contributions to UmbracoDocs or just to facilitate reading the docs without an internet
connection.
