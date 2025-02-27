# Making a .tml file

### Intro
The client has internal variables that determine how it operates, all a .tml does is specify what values should be loaded into those internal variables.
The .tml file iself is just a text file so you can freely edit it using any text editor.

### Assigning Internal Variables
The internal variables are either singular string variables or string list variables and
the file has 4 main tokens for assigning to those internal variables:
- The assignment operators: `=` and `:`.
- The assignment termanator: `;`.
- The list seperator: `,`.

To assign to internal variables
- Use a `=` for recognized singular string variables. 
```
version=1.21.1;
```
- Use a `:` for recognized string list variables. Note that the `,` seperates the values in a list assignment.
```
slugs:fabric-api,e4mc;
```

All assignments must end in a `;` so there parser knows where to locate the value for the internal variable.
For a list of internal variables that you can assign values to, see [docs/variables.md#tml-variables](variables.md#tml-variables).

### Whitespace
Since white space is removed in the parser by default, there is a 5th token for maintaing whitespace:
- The quotation mark: `"`.
```
downloadDir="D:\path to folder\"
``` 
If the quotation marks were not there the in the example above, the value would be read as `D:\pathtofolder\` instead of the correct `D:\path to folder\`.

### Comments
The last and 6th token is:
- The line comment: `#`.

The comment token obviously allows for comments in the file which span from a `#` to the end of that line.

### Example File
```
version=1.21.4;
downloadDir=C:\Users\User\AppData\Roaming\.minecraft\mods;
slugs:

fabric-api,

# PERFORMANCE
entityculling,
ferrite-core,
iris,
lithium,
sodium,
sodium-extra,

# TECH
litematica,
minihud,
malilib, # Dependency for minihud and litematica

# UI
betterf3,
modmenu,

# UTILITY
wi-zoom;
```
