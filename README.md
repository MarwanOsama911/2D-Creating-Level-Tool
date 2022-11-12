# Creating Level Tool for 2D Platformer Games 

This is a tool -Side Project- to facilitate creating the level for 2D games with some constraints. I made Trying to increase my experience in the "Editor Scripts" area by making this tool, in the future I will add more features to the tool also to increase my knowledge and I made it public repo so the developers who care about creating tools in Unity engine can see it and maybe in the future I will make a tutorial videos for it.
 

## Description

I made this tool in terms of facilitating:
* you can create new levels directly through it with the level some configuration.
* you can resize the level and the tool responsible for deleting what is outside the level boundary.
* you have multiple modes for editing the level.
* you can use a palette window to create new level pieces with categories for them.

## Demo Video
https://youtu.be/C7QeO0dusns

## Playlist Tutorials Videos
[Tutorail](https://youtube.com/playlist?list=PL1c6Q9q5NIViB9i0FmERzEFvE85Fc8cmp)

## Getting Started

### Dependencies

Unity and any C# code editor 

### Installing

You can import the package form the realese page of the project or you can donwload the repo and customize yours and build your package 

### Configuration

* You can create new level from toolbar and click Tools > Level Creator > New Level Scene
* You can Config your level columns and rows in Level inspector
* The default size for the gird is 1 Unit you can change it Open Level.cs Script which will be responsible for the level configurations and details and update variable GRID_SIZE with your level pieces colliders size, you can config the level cells color
* You can View, Paint, Edit and Erase Items from you level through the actions in the scene window
* To Open the Palette to draw you Level Go to toolbar and click Tools > Level Creator > Show Pallete
* The default catorgeries for the palette are Background, Collectables, Player, Diffs and Spikes
* you can change Catogeries through PaletteItem.cs script
* To show the level pieces art/Prefabs you can config the path of them at PaletteWindow.cs and Open the level pieces prefabs and assign the paletteItem.cs -this script is running on editor mode only- to it.
* You can customize the palette window for every tab button through PaletteWindow.cs
