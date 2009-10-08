using System;
 using System.Collections.Generic;
 using System.Data.Services;
 using System.Linq;

namespace Apprenda.Taskr.Service
{
    public class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public IList<Contact> Contacts{get; set;}
    }
    public class Contact
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
    public class ContactsData //IUpdatable
    {
        #region Populate Service Data
        static User[] _users;
        static Contact[] _contacts;
        static ContactsData()
        {
            _users = new User[]{
              new User(){ ID=0, Name="Mike",Contacts= new List<Contact>()},
              new User(){ ID=1, Name="Saaid",Contacts= new List<Contact>()},
              new User(){ ID=2, Name="John",Contacts= new List<Contact>()},
              new User(){ ID=3, Name="Pablo",Contacts= new List<Contact>()}};
            _contacts = new Contact[]{
              new Contact(){ ID=0, Name="Mike", Email="mike@contoso.com" },
              new Contact(){ ID=1, Name="Saaid", Email="Saaid@hotmail.com"},
              new Contact(){ ID=2, Name="John", Email="j123@live.com"},
              new Contact(){ ID=3, Name="Pablo", Email="Pablo@mail.com"}};
            _users[0].Contacts.Add(_contacts[0]);
            _users[0].Contacts.Add(_contacts[1]);
            _users[1].Contacts.Add(_contacts[2]);
            _users[1].Contacts.Add(_contacts[3]);
        }
        #endregion
        public IQueryable<User> Users
        {
            get { return _users.AsQueryable<User>(); }
        }
        public IQueryable<Contact> Contacts
        {
            get { throw new DataServiceException(403, "Requests directly to /Contacts are not allowed");}
        }
    }
    /// <summary>
    /// Sample In Memory DataService that will be self-hosted
    /// To support IUpdate see http://code.msdn.microsoft.com/IUpdateableLinqToSql
    /// </summary>
    public class ContactService : DataService<ContactsData>, IRequestHandler
    {
        // This method is called only once to initialize
        //service-wide policies.
        public static void InitializeService(IDataServiceConfiguration
                                             config)
        {
            config.SetEntitySetAccessRule("*", EntitySetRights.All);
        }
       //Service operations, query interceptors & change interceptors go here
    }
}
