namespace ExampleGenerator
{
    class Program
    {

        public static void Main(string[] args)
        {
            new ComplexProperty.ComplexPropertyExample().Run();
            new DynamicValues.DynamicValuesExample().Run();
            new BasicExample.BasicCodeExample().Run();
            new ComplexProperty.NPDExample().Run();

            string sourceFile;
            string targeFile;

            new LibraryExamples.ProductDataLibraryBricks().Run();

            sourceFile = @"..\..\..\..\..\SampleFiles\TriluxLightingProducts\SourceDataFromPimSystem\TRILUX_Baselist_RH190520.xlsx";
            targeFile = @"..\..\..\..\..\SampleFiles\TriluxLightingProducts\OpenProductLibrary\TriluxLightingProducts.ifczip";
            //new LibraryExamples.ProductDataLibrary().RunFromFile(sourceFile,targeFile);

            sourceFile = @"..\..\..\..\..\SampleFiles\DoPLibrary\SourceData\BaustoffindustrieTemplatesAndSheets.xlsx";
            targeFile = @"..\..\..\..\..\SampleFiles\DoPLibrary\OpenProductLibrary\DoPLibrary.ifczip";
            new LibraryExamples.ProductDataLibrary().RunFromFile(sourceFile, targeFile);

        }
    }
}
