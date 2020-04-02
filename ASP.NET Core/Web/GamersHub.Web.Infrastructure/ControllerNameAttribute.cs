using System;

namespace GamersHub.Web.Infrastructure
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ControllerNameAttribute : Attribute
    {
        public string Name { get; }

        public ControllerNameAttribute(string name)
        {
            this.Name = name;
        }
    }
}