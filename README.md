## Product Data for Building Information Modeling (BIM)
Validity of ifcXML files: [![Build Status](https://travis-ci.org/buildingSMART/ProductData.svg?branch=master)](https://travis-ci.org/buildingSMART/ProductData)

This official repository is maintained by the community around the ProductRoom of [buildingSMART International](https://www.buildingsmart.org). It is used for open communication of ideas, samples and agreements.

### Product data template (PDT) and product data sheets (PDS)
System to describe product data templates and product data sheets with IFD and IFC (ISO 16739). This is ongoing work within the working group of buildingSMART International. The repository is open for sharing other samples and discussing related questions.

**Please do not use in production yet.**

### Structure of the sample files

The sample files for BIM product data in this repo should follow this rules and structure:
#### Structure of folders
One folder per sample file as a subfolder in "SampleFiles" e.g. like this
```
SampleFiles/MyExample
```
Store all files, related to the sample, in the this folder.

### Serialization formats 
The sample should be one of the following serialization formats

| Format        | file ending   |
| ------------- |---------------| 
| ifcXML        | .ifcXML       | 
| Step          | .ifc          |   
| Json          | .json         |  

Please be aware, that the file endings are case sentitive.

The file can be also in the compressed form as an ".ifcZIP". (Look [here](http://www.buildingsmart-tech.org/ifc) to get an overview of all possible file formats to transport IFC-compliant data.)

### Description
Please add a readme.md in the folder and describe you sample. Please describe the use case and the specialities of your sample, so that everbody can easily understand your sample.
