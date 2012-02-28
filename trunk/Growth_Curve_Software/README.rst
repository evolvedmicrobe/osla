Clarity
=======

Lab automation software

*****

At initialization, Clarity looks for all instruments in the assembly and initializes them with any specific settings based on an XML file.  All the instruments classes are in the ``RoboticInstruments.dll`` file.  Each is a class that derives from the abstract BaseInstrumentClass which allows them to be handled in a pretty general way and guarantees they implement certain methods and maintain certain information.  Once all the instruments are up and running it then runs protocols.

The code for the protocols is mostly in the ``Protocol.cs`` file.  Each protocol is a class which basically has an array list of instructions and a collection of variables that it can use.  The protocol class is very basic, as my idea was to have any "complex" code/decision making be handled by having the user/programmer create a "virtual" instrument.  That is, if you wanted to have a program look at the OD readings, to decide what and when to measure things next, etc. you would make a class that derived from the ``BaseInstrumentClass`` and did all this decision making for you.  The protocol itself would merely have an instruction to call this virtual instrument, and the instrument could then modify the protocol class as it saw fit, returning it back to the task scheduler after it was modified.  The ``ProtocolManager`` class is basically a collection of protocols, with additional methods for picking whose next, sending out emails when things go wrong, etc.

Clarity runs the protocols on a background thread, and all the methods for doing this are in the main ``Clarity.cs`` file.  Basically a ``ProtocolItem`` is returned with an instrument name, method name and an object array of parameters, and it then dynamically looks up and runs the relevant method.  This is all done in the ``RunThroughProtocols`` method.  There are also a number of functions for error handling, monitoring things remotely, etc.  I would definitely extend all this code a bit, but I think the basic structure is pretty good with a few tweaks here and there.
