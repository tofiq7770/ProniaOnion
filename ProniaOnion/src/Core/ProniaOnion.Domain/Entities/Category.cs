namespace ProniaOnion.Domain.Entities
{
    public class Category:BaseNameableEntity
    {   
        public ICollection<Product> Products { get; set; }
    }
}
