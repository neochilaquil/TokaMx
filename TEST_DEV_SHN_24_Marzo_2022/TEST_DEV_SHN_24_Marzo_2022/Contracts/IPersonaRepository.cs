using TEST_DEV_SHN_24_Marzo_2022.Models;

namespace TEST_DEV_SHN_24_Marzo_2022.Contracts
{
    public interface IPersonaRepository
    {
        IEnumerable<PersonasModel> ListPersons();
        PersonasModel GetPersona(string id);
        PersonasModel AddPersona(PersonasModel persona);
        PersonasModel UpdatePersona(PersonasModel persona);
        bool DeletePersona(int id);

    }
}
