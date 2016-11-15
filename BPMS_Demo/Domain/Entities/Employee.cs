namespace Domain.Entities
{
    public class Employee : EntityBase
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public virtual Company Company { get; set; }

        public virtual Employee Manager { get; set; }

        public string FullName
        {
            get
            {
                return string.Join(" ", FirstName, LastName);
            }
        }
    }
}
