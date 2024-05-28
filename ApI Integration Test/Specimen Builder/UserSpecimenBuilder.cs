using AutoFixture.Kernel;
using Notepad.Database.Model;
using Notepad.Shared.Dto;

namespace ApI_Integration_Test.Specimen_Builder
{
    public class UserSpecimenBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (request is Type type && type == typeof(UserDto))
            {
                return new UserDto(Username: "TestUser", Password: "TestPassword");
            }

            if (request is Type type2 && type2 == typeof(User))
            {
                return new User { Id = 1, Username = "TestUser", PasswordHash = [1, 2, 3], PasswordSalt = [4, 5, 6] };
            }

            return new NoSpecimen();
        }
    }
}
