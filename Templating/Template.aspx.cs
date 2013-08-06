using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Linq.Expressions;

public partial class Template : System.Web.UI.Page
{
    Employee em = new Employee("Stefaan Christiaens", "Development");
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            CreateEntities();
            ReadFile();
            ParseFile(Server.MapPath("~/template.txt"), em);
        }
    }

    private void ParseFile<T>(string template, T toInsert)
    {
        String parsedTemplate = "";

        Type ty = toInsert.GetType();
        PropertyInfo[] pi =  ty.GetProperties();
        List<string> classProperties = new List<string>();
        foreach (PropertyInfo p in pi)
        {
            classProperties.Add(p.Name);
        }
        
        StreamReader sr = new StreamReader(template);
        while (sr.Peek() != -1)
        {
            String line = sr.ReadLine();

            //IF IS WITH BRACES
            Regex rgx = new Regex(@"{\w", RegexOptions.IgnoreCase);
            if (rgx.IsMatch(line))
            {
                foreach (var item in classProperties)
                {

                    //IF VALUE NEEDS TO BE INSERTED
                    rgx = new Regex(@"{" + ty.Name + "_" + item.ToString() +"}", RegexOptions.IgnoreCase);
                    if (rgx.IsMatch(line))
                    {
                        parsedTemplate += GetPropValue(toInsert, item);
                    }
                }

                //IF A CONDITIONAL STATEMENT OCCURS
                rgx = new Regex("{IF ", RegexOptions.IgnoreCase);
                if (rgx.IsMatch(line))
                {
                    //startConditional(line, sr);
                }

                //IF A LOOP OCCURS
                rgx = new Regex("{LOOP ", RegexOptions.IgnoreCase);
                if (rgx.IsMatch(line))
                {
                    string classToLoop = rgx.Split(line)[1].Remove(rgx.Split(line)[1].Length - 1, 1);
                   
                    string property = FirstCharToUpper(classToLoop.Split('_')[1]);
                    IEnumerable<Object> nestedCollection;
                    try
                    {
                        //PUT DATA FROM LOOP OF PROPERTY
                        nestedCollection = (IEnumerable<Object>)GetPropValue(toInsert, property);
                    }
                    catch (Exception)
                    {
                        //PUT DATA OF CURRENT ITEM
                        nestedCollection = (IEnumerable<Object>)toInsert;
                    }
                    
                    
                    parsedTemplate += parseLoop(nestedCollection, sr);
                }
            }
            else
            {
                parsedTemplate += line;
            }
        }
        parsed.InnerHtml = parsedTemplate;
    }

    private string parseLoop(IEnumerable<object> nestedCollection, StreamReader sr)
    {
        List<string> htmlOutput = new List<string>();
        
        while (true)
        {
            string loopline = sr.ReadLine();
            Regex rgx = new Regex("{ENDLOOP}", RegexOptions.IgnoreCase);
            if (rgx.IsMatch(loopline))
            {
                break;
            }

            foreach (object item in nestedCollection)
            {
                string htmlOut = "";
                Type ty = item.GetType();
                PropertyInfo[] pi = ty.GetProperties();
                List<string> classProperties = new List<string>();
                foreach (PropertyInfo p in pi)
                {
                    classProperties.Add(p.Name);
                }

                rgx = new Regex(@"{\w", RegexOptions.IgnoreCase);
                if (rgx.IsMatch(loopline))
                {
                    foreach (var it in classProperties)
                    {

                        //IF VALUE NEEDS TO BE INSERTED
                        rgx = new Regex(@"{" + ty.Name + "_" + it.ToString() + "}", RegexOptions.IgnoreCase);
                        if (rgx.IsMatch(loopline))
                        {
                            htmlOut += GetPropValue(item, it);
                        }
                    }

                    rgx = new Regex("{IF", RegexOptions.IgnoreCase);
                    if (rgx.IsMatch(loopline))
                    {
                        //htmlOut += parseConditional(sr);
                    }
                }
                else
                {
                    htmlOut += loopline;
                }
                htmlOutput.Add(htmlOut);
            }
        }
        string html = "";
        int start = 0;
        foreach (var obj in nestedCollection)
        {
            for (int i = start; i < htmlOutput.Count; i=i+nestedCollection.Count())
            {
                html += htmlOutput[i];
            }
            start++;
        }
        return html;
    }

    private string parseConditional(StreamReader sr)
    {
        throw new NotImplementedException();
    }

    public static object GetPropValue(object src, string propName)
    {
        return src.GetType().GetProperty(propName).GetValue(src, null);
    }


    private void StartLoop(string line, StreamReader sr)
    {
        Regex rgx = new Regex("{LOOP ", RegexOptions.IgnoreCase);
        string[] loop = rgx.Split(line);
        string classToLoop = loop[1].Remove(loop[1].Length - 1, 1);

    }

    public static string FirstCharToUpper(string input)
    {
        if (String.IsNullOrEmpty(input))
            throw new ArgumentException("ARGH!");
        return input.First().ToString().ToUpper() + String.Join("", input.Skip(1));
    }
    
    private void ReadFile()
    {
        try
        {
            using (StreamReader sr = new StreamReader(Server.MapPath("~/template.txt")))
            {
                String line = sr.ReadToEnd();
                template.InnerHtml = line;
            }
        }
        catch (Exception ex)
        {
            template.InnerHtml = "Could not read the file. Message:" + ex.Message;
        }
    }

    private void CreateEntities()
    {

        em.Contacts = new List<Contact>();
        Contact co = new Contact("campus", "0479403264");
        Contact co1 = new Contact("work", "0129403264");
        Contact co2 = new Contact("home", "0479425264");
        em.Contacts.Add(co);
        em.Contacts.Add(co1);
        em.Contacts.Add(co2);
    }
}