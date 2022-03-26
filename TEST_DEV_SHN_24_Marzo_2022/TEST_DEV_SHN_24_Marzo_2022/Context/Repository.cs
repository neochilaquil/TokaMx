using TEST_DEV_SHN_24_Marzo_2022.Contracts;
using TEST_DEV_SHN_24_Marzo_2022.Models;

namespace TEST_DEV_SHN_24_Marzo_2022.Context
{
    public class Repository : IPersonaRepository
    {
        private string Conexion = "Data Source=localhost;Initial Catalog=Personas;Integrated Security=True";
        public IEnumerable<PersonasModel> ListPersons()
        {
            //using ()
            //{

            //}
            throw new NotImplementedException();
        }
        public PersonasModel GetPersona(string id)
        {
            throw new NotImplementedException();
        }
       
        public PersonasModel AddPersona(PersonasModel persona)
        {
            throw new NotImplementedException();
        }

        public bool DeletePersona(int id)
        {
            throw new NotImplementedException();
        }      

        public PersonasModel UpdatePersona(PersonasModel persona)
        {
            throw new NotImplementedException();
        }
    }
}
