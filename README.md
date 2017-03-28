# VermilionSandbox
Sandbox for ideas for the Vermilion programming language

##### Keywords

```
class struct union 
if then else
for
while
try catch throw

void null
int8 int16 int32 int64 int128
uint8 uint16 uint32 uint64 uint128
bool string
```

##### Primitive data types
-	Integer (signed and unsigned) - ``[u]int8`` ``[u]int16`` ``[u]int32`` ``[u]int64`` ``[u]int128``

	Single char constants are stored in integer types, UTF8 by default (TODO)
	
-	Floating point numbers - ``float`` ``double``
-	Boolean values - ``bool``

	Equivalent in size to ``[u]int8``, ``false`` equals to ``0``, everything else that isn't ``0`` equals to ``true``

-	Strings - ``string``

	Basically arrays of chars, contain length somewhere inside the structure, TODO
	
	
-	Pointer types, declared with the ``*`` symbol. Mandatory glued to the end of the type name - ``any*``

##### Sandbox of ideas
-	Built in dispatch table structure?
