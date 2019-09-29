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
            new LibraryExamples.ProductDataLibraryBricks().Run();

            new LibraryExamples.ProductDataLibrary()
                .RunFromFile(_sourceFile: @"..\..\..\..\..\SampleFiles\TriluxLightingProducts\SourceDataFromPimSystem\TRILUX_Baselist_RH190520.xlsx", 
                             _targetFile:@"..\..\..\..\..\SampleFiles\TriluxLightingProducts\OpenProductLibrary\TriluxLightingProducts.ifczip");
        }
    }
}
