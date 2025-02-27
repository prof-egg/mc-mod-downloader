# Variables
### Overview
These are all the internal variables recognized by the client.

### .tml Variables 
| Name | Type | Description | Required | Default |
| - | - | - | - | - |
| version | singular | The minecraft version to target. | ✔️ |  |
| downloadDir | singular | The destination folder the downloaded files. | ✖️ | The directory from which the client was launched. |
| slugs | list | The slugs for the mods to download. A slug is the last part of a modrinth link for a mod page. Ex: https://modrinth.com/mod/fabric-api = fabric-api | ✔️ |  |


### .tml Special Values
Some variables have recognized values which the client will evaluate upon reading.
| Variable | Special Value | Description |
| - | - | - |
| downloadDir | . | Will evaluate to the current working directory. |
