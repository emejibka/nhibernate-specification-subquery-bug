using System;

namespace TestApp
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    [Serializable]
    public sealed class SpecificationAttribute : Attribute
    {
        public SpecificationAttribute(string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName))
            {
                throw new ArgumentNullException(nameof(fieldName));
            }

            FieldName = fieldName;
        }

        public string FieldName { get; }
    }
}