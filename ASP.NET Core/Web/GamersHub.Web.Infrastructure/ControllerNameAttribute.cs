namespace GamersHub.Web.Infrastructure
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public class ControllerNameAttribute : Attribute
    {
        public ControllerNameAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; }
    }
}
