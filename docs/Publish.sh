cd ../Clarity_Suite/
doxygen Doxyfile 

cd ../docs/
make html

rsync -avz build/html/ rojasechenique@fas.harvard.edu:/home/r/o/rojasechenique/public_html/claritydocs
