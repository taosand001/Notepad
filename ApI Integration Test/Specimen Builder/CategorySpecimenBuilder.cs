using AutoFixture.Kernel;
using Notepad.Database.Model;
using Notepad.Shared.Dto;

namespace ApI_Integration_Test.Specimen_Builder
{
    public class CategorySpecimenBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (request is Type type && type == typeof(CategoryDto))
            {
                return new CategoryDto(Name: "TestCategory");
            }

            if (request is Type type2 && type2 == typeof(List<string>))
            {
                return new List<string> { "TestCategory" };
            }

            if (request is Type type3 && type3 == typeof(Category))
            {
                return new Category { Id = 1, Name = "TestCategory", Notes = [] };
            }

            return new NoSpecimen();

        }
    }
}
