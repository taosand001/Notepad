using ApI_Integration_Test.Specimen_Builder;
using AutoFixture;
using AutoFixture.Xunit2;

namespace ApI_Integration_Test.Data_Attribute
{
    public class CategoryDataAttribute : AutoDataAttribute
    {
        public CategoryDataAttribute() : base(() =>
        {
            var fixture = new Fixture();
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            fixture.Customizations.Add(new CategorySpecimenBuilder());
            return fixture;
        })
        { }
    }
}
