using System.Collections.Generic;
using System.Threading.Tasks;
using XamarinFireBase.Models;
using Firebase.Database;
using System.Linq;
using Firebase.Database.Query;

namespace XamarinFireBase.Helper
{
    public class FirebaseHelper
    {
        // https://xamarinfirebase-4e521.firebaseio.com/
        //FirebaseClient firebase = new FirebaseClient("https://xamarinfirebase-xxxxx.firebaseio.com/");
        FirebaseClient firebase = new FirebaseClient("https://xamarinfirebase-4e521.firebaseio.com/");
        public async Task<List<Person>> GetAllPersons()
        {

            return (await firebase
              .Child("Persons")
              .OnceAsync<Person>()).Select(item => new Person
              {
                  Name = item.Object.Name,
                  PersonId = item.Object.PersonId
              }).ToList() ?? new List<Person>();
        }

        public async Task AddPerson(int personId, string name)
        {
            await firebase
              .Child("Persons")
              .PostAsync(new Person() { PersonId = personId, Name = name });
        }

        public async Task<Person> GetPerson(int personId)
        {
            var allPersons = await GetAllPersons();
            await firebase
              .Child("Persons")
              .OnceAsync<Person>();
            return allPersons.Where(a => a.PersonId == personId).FirstOrDefault();
        }

        public async Task UpdatePerson(int personId, string name)
        {
            var toUpdatePerson = (await firebase
              .Child("Persons")
              .OnceAsync<Person>()).Where(a => a.Object.PersonId == personId).FirstOrDefault();

            await firebase
              .Child("Persons")
              .Child(toUpdatePerson.Key)
              .PutAsync(new Person() { PersonId = personId, Name = name });
        }

        public async Task DeletePerson(int personId)
        {
            var toDeletePerson = (await firebase
              .Child("Persons")
              .OnceAsync<Person>()).Where(a => a.Object.PersonId == personId).FirstOrDefault();
            await firebase.Child("Persons").Child(toDeletePerson.Key).DeleteAsync();
        }

    }
}
