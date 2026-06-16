using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModels
{
    public class Category
    {
        // private fields
        private readonly int _id;
        private readonly string _name;
        private readonly string _description;
        private readonly DateTime _createdAt;

        // constructor
        public Category(int id, string name, string description, DateTime createdAt)
        {
            _id = id;
            _name = name;
            _description = description;
            _createdAt = createdAt;
        }

        // properties
        public int Id { get { return _id; } }
        public string Name { get { return _name; } }
        public string Description { get { return _description; } }
        public DateTime CreatedAt { get { return _createdAt; } }
    }
}
