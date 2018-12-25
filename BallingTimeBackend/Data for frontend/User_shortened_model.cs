using System.Collections.Generic;

namespace BallingTimeBackend.Data_for_frontend
{
    public class User_shortened_model
    {
        public int Id { get; set; }
        
        public string Email { get; set; }
        
        public string Password { get; set; }
        
        public string Name { get; set; }
        
        public List<int> PracticeDays { get; set; }

    }
}
