## Product with many fixed configuration variants

- The files are generated from source data in an [Excel sheet](https://github.com/buildingSMART/ProductData/blob/master/SampleFiles/HC%20(Italy)/Bricks%20Catalogue/RawData.xlsx)
- See this [schedule](https://github.com/buildingSMART/ProductData/blob/master/SampleFiles/HC%20(Italy)/Bricks%20Catalogue/catalogue_bricks_en.jpg)
- Sheet 1: 30 configuration options
- Sheet 2: 4320 configuration options
- The product has four properties: zones, height, width, length
- The source code of the generator is [here](https://github.com/buildingSMART/ProductData/blob/master/Tools/Examples/CatalogueExample/CatalogueExample.cs)

Result (Sheet 2, 4320 configurations as a list of products):

1. [ProductDataGeneratedMassData.ifc](https://raw.githubusercontent.com/buildingSMART/ProductData/master/SampleFiles/HC%20(Italy)/Bricks%20Catalogue/ProductDataGeneratedMassData.ifc)
2. [ProductDataGeneratedMassData.ifcXML](https://raw.githubusercontent.com/buildingSMART/ProductData/master/SampleFiles/HC%20(Italy)/Bricks%20Catalogue/ProductDataGeneratedMassData.ifcXML)

See also the [ProductDataGeneratedMassData.ifc](http://htmlpreview.github.io/?https://raw.githubusercontent.com/buildingSMART/ProductData/master/SampleFiles/HC%20(Italy)/Bricks%20Catalogue/ProductDataGeneratedMassData.ifc.htm) rendered in a nice colored HTML.

Thanks to https://www.harpaceas.it!
