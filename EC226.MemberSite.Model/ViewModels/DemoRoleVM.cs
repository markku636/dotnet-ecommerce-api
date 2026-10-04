using System;
using System.Collections.Generic;
using System.Text;

namespace EC226.MemberSite.Model.ViewModels
{
    public class DemoRoleVM
    {
        public int total { get; set; }
        public List<Item> items { get; set; }
    }


    public class Item
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Root> Routes { get; set; }
    }

    public class Root
    {
        public string Path { get; set; }
        public string Component { get; set; }
        public Meta Meta { get; set; }
        public List<Child> Children { get; set; }
        public string Redirect { get; set; }
        public string Name { get; set; }
    }

    public class Child
    {
        public string Path { get; set; }
        public string Component { get; set; }
        public string Name { get; set; }
        public Meta Meta { get; set; }
        public string Redirect { get; set; }
        public List<Child> Children { get; set; }
    }
    public class Meta
    {
        public bool Hidden { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public List<string> Roles { get; set; }
        public bool? AlwaysShow { get; set; }
        public bool Affix { get; set; }
        public bool? NoCache { get; set; }
        public string ActiveMenu { get; set; }
    }
}
