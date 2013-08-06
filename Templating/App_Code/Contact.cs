using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Contact
/// </summary>
public class Contact
{
    public string Name { get; set; }
    public string Phone { get; set; }

	public Contact(string name, string phone)
	{
        this.Name = name;
        this.Phone = phone;
	}
}