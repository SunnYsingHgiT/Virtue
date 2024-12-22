namespace Virtue.UserService.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } 
        public string LastName { get; set; }  
        public string FullName => $"{FirstName} {LastName}"; 
        public string Email { get; set; }     
        public string PhoneNumber { get; set; } 
        public Address Address { get; set; }   
        public string Gender { get; set; }    
        public DateTime DateOfBirth { get; set; }
        public bool IsDeleted { get; set; } 
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }  
        public DateTime? LastLogin { get; set; } 
    }
}

