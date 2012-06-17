This project contains the code to interface with a Caliper Life Sciences Twister II Robot.

It depends on the COM objects for the twister, which are included with wrappers here as the Interop.* dll files.

Note that it saves specific twister locations in the serialized file TwistPos.nfd.  You can use the TwisterTeacher program 
to retrain these files, but this file must be loaded into the directory to work.