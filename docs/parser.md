# Parser
### Overview
If you wanted to know specifically how the client reads the information in a .tml file, here it is.
The parser is split into two phases.

### Phase 1 - Loading and Cleaning
The first phase loads the .tml file as a string and cleans it:
- Read text file as string.
- Remove any segements that start with `#` and end with `\n` (or is end of file).
- Remove any and all whitespace except that which is in between a set of quotation marks.
- Remove any quotation marks.
  
For the example file in [docs/tml-file#example-file](tml-file.md#example-file), the cleaned string would like like this:
```
version=1.21.4;downloadDir=C:\Users\BMike\AppData\Roaming\.minecraft\mods;slugs:fabric-api,entityculling,ferrite-core,iris,lithium,sodium,sodium-extra,litematica,minihud,malilib,betterf3,modmenu,wi-zoom;
```

### Phase 2 - Assigning
After cleaning the string the actual parser gets the information from it.
It will look for keys in the file and assign the corresponding internal variables to the string located between the assignment operator (either `=` or `:`) for that key, and the next `;` in the string.
- For variables (keys marked with an `=`), they get assigned that plain value.
- For lists (keys marked with a `:`), they get assigned that value as an array where the splits are at every `,`.
