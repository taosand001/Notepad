using ApI_Integration_Test.Specimen_Builder;
using AutoFixture;
using AutoFixture.Xunit2;

namespace ApI_Integration_Test.Data_Attribute
{
    public class UserDataAttribute : AutoDataAttribute
    {
        public UserDataAttribute() : base(() =>
        {
            var fixture = new Fixture();
            fixture.Customizations.Add(new UserSpecimenBuilder());
            return fixture;
        })
        { }
    }
}
