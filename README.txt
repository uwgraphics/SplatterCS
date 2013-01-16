Notes by Adrian Mayorga
1/16/2013

Splatterplots:

This is the C# version of Splatterplots, and is the implementation that was used to make the examples in the 2012 TVCG submission. 

The meat of splatterplots is in two different files, SplatterView and DensityRenderer. The code base is separated into 3 big parts:
	UI
		Winform controlls and dialog that create the overall user interface
	Opengl
		OpenGL related code. We make use of the OpenTK library (included in the external folder) to enable usage of opengl in c#.
	Models
		data models for everything: a single splatterplot, a matrix, a datafile, a converted dataseries, a projection in 2d of a dataseries, etc

SplatterView extends OpenTK.GLControl, which gives us an encapsulation of an OpenGL context, yet still works as a WindowsForms control. Each instance of GLControl will create its own context. SplatterView takes in a SplatterModel object and is responsible for rendering the splatterplot. DensityRenderer encapsulates all of the bufferobjects and textures required to make spaltterplots work. The whole system is event based, so it only redraw when needed.

The typical workflow is as follows:

First load a file into the system by using the load file button. This will add it to the list on the first dialog.

Select the file on the list and click on one of the three options: 
Single splatterplot
splatterplot Matrix
One versus Rest Splatterplot Matrix

If you get really stuck, you can contact me at adr.mayorga@gmail.com