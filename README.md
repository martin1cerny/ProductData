[![Official repository by buildingSMART International](https://img.shields.io/badge/buildingSMART-Official%20Repository-orange.svg)](https://www.buildingsmart.org/)
[![This repo is managed by the ProductRoom](https://img.shields.io/badge/-ProductRoom-blue.svg)](https://www.buildingsmart.org/standards/rooms-and-groups/product-room)

## Product Data for Building Information Modeling (BIM)
Validity of ifcXML files aginst the XSD-schema 
[![Build Status](https://travis-ci.org/buildingSMART/ProductData.svg?branch=master)](https://travis-ci.org/buildingSMART/ProductData)

Validity of ifcXML files aginst the SchemaValidator 
[![Build status](https://ci.appveyor.com/api/projects/status/yjoess7g50xqasdb/branch/master?svg=true)](https://ci.appveyor.com/project/klacol/productdata/branch/master)

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
All other related files are store alongside in the this folder.

### Serialization formats 
The sample should be one of the following serialization formats

| Format        | file ending   |
| ------------- |---------------| 
| ifcXML        | .ifcXML       | 
| Step          | .ifc          |   
| Json          | .json         |  

Please be aware, that the file endings are case sentitive.

The file can be also in the compressed form as an ".ifcZIP". (Look [here](https://technical.buildingsmart.org/standards/ifc/ifc-formats) to get an overview of all possible file formats to transport IFC-compliant data.)

### Description
Please add a readme.md in the folder and describe your sample. Please describe the use case and the specialities of your sample, so that everybody can easily understand your sample.
