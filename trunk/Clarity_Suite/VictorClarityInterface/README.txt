This is the interface for the Clarity to talk to a Perkin Elmer victor.  When originally set up, this instrument 
was on a separate computer, and so to make the remoting work, we originally had a wrapper call a separate form. 
This is why there is a victorstandaloneform, which loads and runs the victor on it's own threads.