using System.Collections.Generic;

namespace WorkflowDemo1.Data
{
    public class Category
    {
        public int Id { get; set; }
        public  string Name { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}