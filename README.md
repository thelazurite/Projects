# Projects 0.2.31 release readme #

Projects is a simple Task managment application which is written in C#

### Requirements ###

* Operating system:
     * Windows/Linux/Mac OS should be compatible with the software. 
     * Please let me know if you run into any issues regarding compatibility/bugs

* Dependencies:
    * GTK3/GTK+ [(Link to git here)](https://github.com/GNOME/gtk) - Licensed under LGPL-2.1
	* GTK-Sharp [(Link to git here)](https://github.com/mono/gtk-sharp) (Binaries are already included in project) - Licensed under LGPL-2.1
    * [SQLite 3](https://system.data.sqlite.org/index.html/doc/trunk/www/index.wiki) - Public Domain
    * Usage of .NET/Mono version 4.6
    * Latest version of MonoDevelop/Visual Studio 2015 should be sufficient to build source code. 

### Work in progress/Planned features ###
* Toggle visibility of Project items by category
* Ability to mark tasks as completed
* View descriptions for tasks
* beautify Task/Category creation interfaces

### Known issues ###
* Issue with interface elements on multi-monitor workstation set-ups not displaying on the same monitor as the main window
* Application ignores user Primary window preference

### Change Log ###
#### [0.2.3X] ###
* Minor changes and bug fixes ( Issue 2 and Issue 3)

#### [0.2.2] ####
* Converted category window to VBox template/tab
     * Disabled expansion of UI elements
* Finished refactoring Projects.Cs

#### [0.2.1] ####
* General bugfixes and refactoring

#### [0.2.0] ####
* Calendar dates are now marked when there is a task due
* Started to add functionality for adding tasks/categories in a tabbed layout instead of a pop-out window
* Saving confirmation logic has been added
* ALT shortcuts added to main window
* General logic optimisations/modifications

### Contact Me ###
E-mail: [me@dylaneddies.co.uk](mailto:me@dylaneddies.co.uk)
