This is the main GUI for running Clarity, start here to see how things work.

The file GuiTemplate.cs is the main window to run clarity, load instruments, etc.  You can customize 
this file as you like.  Note that the instruments won't really load as the configuration file has 
the "Skip load" property set to true for all of them.  You should be able to see the basics though.

The file MakeProtocols.cs contains the GUI for users to make protocols with any of the loaded instruments.

Note that by default it will go to a server at Harvard, by the ip address of this can be changed 
(or the usealarm property set to off in the ConfigurationFile.xml file).