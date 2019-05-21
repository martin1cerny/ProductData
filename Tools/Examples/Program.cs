namespace Examples
{
    class Program
    {

        public static void Main(string[] args)
        {
            new ComplexProperty.ComplexPropertyExample().Run();
            new DynamicValues.DynamicValuesExample().Run();
            new BasicExample.BasicCodeExample().Run();
            new ComplexProperty.NPDExample().Run();
            new CatalogueExample.CatalogueExampleBricks().Run();
            new CatalogueExample.CatalogueExampleLightFixtures().Run();
        }
    }
}
