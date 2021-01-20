namespace API.Entities
{
    public class AppUser
    {
        //public that can be get or set from any other class in our app
        //protected access from that class or any class inherite from it
        //private access only from this class
        public int Id { get; set; }
        
        public string UserName { get; set; }
    }
}