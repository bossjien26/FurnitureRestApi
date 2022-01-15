using System.Collections.Generic;

namespace Services.Dto
{
    public class CategoryRelationChildren
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<ChildrenCategory> ChildrenCategories { get; set; } = new List<ChildrenCategory>();
    }
}