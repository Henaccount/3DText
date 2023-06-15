# 3DText - example code - use at own risk
Example code that creates an ACAD line based string twin for each Text and MText object in the drawing.
That string made out of lines will be placed in the position of the Text / MText label. It is not grouped.
To set the right scale and rotation of that text twin is not implemented.
The available character set is limited, but can be expanded.

To use this in AutoCAD you need to compile it and load the resulting dll in AutoCAD with the "NETLOAD" command.
There will be then a new command available, called "3DText".
