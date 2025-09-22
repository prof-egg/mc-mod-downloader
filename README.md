## About
A client that downloads minecraft mods from modrinth through a custom .tml config file specification.

> [!WARNING]
> _It is currently hardcoded to only download fabric mods._

## How To Use
### Overview
There are two parts, the client, and the .tml file. The client is just a download script
and the .tml file is just a config file for the client.
- The client first searches the current working directory for a .tml file to parse. If there is more than one file than the client uses the first one it finds.
- Next it will parse that file, and use the contents to search and download the specified mods using the [Modrinth API](https://docs.modrinth.com/api/).

### Docs
- [Make a .tml File](docs/tml-file.md)
- [Internal Variables List](docs/variables.md)
- [Parser](docs/parser.md)
