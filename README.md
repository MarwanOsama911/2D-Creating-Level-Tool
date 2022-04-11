# Creating Level Tool for 2D Platformer Games 

This is a tool -Side Project- to facilitate creating the level for 2D games with some constraints. I made Trying to increase my experience in the "Editor Scripts" area by making this tool, in the future I will add more features to the tool also to increase my knowledge and I made it public repo so the developers who care about creating tools in Unity engine can see it and maybe in the future I will make a tutorial videos for it.
 

## Description

I made this tool in terms of facilitating:
* you can create new levels directly through it with the level some configuration.
* you can resize the level and the tool responsible for deleting what is outside the level boundary.
* you have multiple modes for editing the level.
* you can use a palette window to create new level pieces with categories for them.


## Getting Started

### Dependencies

Unity and any C# code editor 

### Installing

You can import the package form the realese page of the project or you can donwload the repo and customize yours and build your package 

### Configuration

* You will create your level pieces prefabs and save them into your assets 
* Open Level.cs Script which will be responsible for the level configurations and details and update variable GRID_SIZE with your level pieces colliders size
* You can Config the level columns and rows
* you can config the level cell color 
* Open PaletteItem.cs script and you can config what category for the items of the palette you want to show in.
* Open level pieces prefabs and assign the paletteItem.cs to it.
* Open PaletteWindow.cs script and config your level pieces prefabs path
* You can customize the palette window for every tab button through PaletteWindow.cs
