using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Employee
{
    public string Name { get; set; }
    public string Department { get; set; }
    public virtual ICollection<Contact> Contacts { get; set; }


    public Employee(string name, string department)
    {
        this.Name = name;
        this.Department = department;
    }
}
