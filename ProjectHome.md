Clarity is...

  * a robot manager for laboratory automation.
  * free and open source.
  * actively developed.

Clarity has...

  * [good documentation](http://www.people.fas.harvard.edu/~rojasechenique/claritydocs/)
  * [a mail list](https://groups.google.com/forum/?fromgroups#!forum/clarity-dev)
  * [doxygen sources](http://www.people.fas.harvard.edu/~rojasechenique/claritydocs/sourcecode/html/)


# A tour of Clarity #

Clarity is a suite of software designed to run automation equipment (robots) in scientific labs.  With Clarity one or many users can simultaneously run protocols using any hardware setup (such as plate readers or liquid handlers). Clarity automates processes with these machines by running protocols, or list of instructions, specified by users.  Such instructions might be a list of commands for each instrument (such as using a robot arm to move a plate to an instrument, and then taking a measurement of that plate with another robot).  Clarity uses XML files to specify these protocols (and includes a GUI to create them).

In contrast to closed source automation programs, Clarity is exceptionally flexible and stable.  Protocol instructions can be dynamically altered using complex flow control, allowing for data generated in real time to determine what procedures happen next.  Similarly, multiple users can run protocols simultaneously, adding them to and from the common pool of protocols.  The program is designed to rapidly recover from and alert the user of virtually any error, and it isolates instruments so that recovering one does not require a complete restart of the entire system.

Clarity is written completely in C# and is compatible with the Mono .NET framework implementation so can run on any operating system.  It is composed of essentially four parts:

  * A set of classes that provide an interface between the Clarity software and hardware resources.
  * A runtime engine that manages these resources and runs protocols using them.
  * A set of graphical user interfaces that allow users to observe the runtime engine, add or alter protocols, create new XML protocols using a GUI and interface directly with the instruments.
  * A set of tools involved in error reporting.  This includes a client application that allows users to watch video feeds and a remote monitor that uses Skype to call users in the event of a failure.

# Acknowledgements #

Clarity's authors are [Nigel Delaney](http://www.evolvedmicrobe.com) and [José Rojas Echenique](http://jireva.org)