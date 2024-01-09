# Projects
## tcgct-mud
The actual website, appsettings.json
`BackendTech` Should be able to take 
```json
    "ConnectionStrings": {
        "MainDB": "CONNECTIONSTRING"
    },
    "BackendTech": "MSSQL"
```
### Todo

## tcgct-service-framework
A library of objects, classes, interfaces etc to help interacting with the db, built around services and interfaces so that you can simply create a new folder, implement each interface, and straight away use that in the site just by changing the appsettings.json.

## tcgct-mssql
A mssql implementation of the site.

## Updaters/MTG
A program to populate the SQL db with data using webapi from https://scryfall.com/docs/api

# SQL DB Project
Located in a seperate project
https://github.com/edenobrega/tcgct-sql
