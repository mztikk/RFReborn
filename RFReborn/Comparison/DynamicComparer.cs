using System;
using System.Reflection;

namespace RFReborn.Comparison
{
    public class DynamicComparer<T>
    {
        protected DynamicComparisonType comparisonType;

        public DynamicComparer(DynamicComparisonType dynamicComparisonType) => comparisonType = dynamicComparisonType;

        public DynamicComparer() : this(DynamicComparisonType.Full) { }

        public bool Compare(T t) => comparisonType switch
        {
            DynamicComparisonType.Full => FullComparison(t),
            DynamicComparisonType.Any => AnyComparison(t),
            DynamicComparisonType.NonNull => NonNullComparison(t),
            _ => false,
        };

        private bool AnyComparison(T t)
        {
            Type tType = t.GetType();
            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance;

            foreach (PropertyInfo prop in GetType().GetProperties(bindingFlags))
            {
                // check if FileInfo has this property aswell
                PropertyInfo tProp = tType.GetProperty(prop.Name, bindingFlags);
                if (tProp is null)
                {
                    continue;
                }

                // check if types match
                bool match = false;
                if (prop.PropertyType == tProp.PropertyType)
                {
                    match = true;
                }
                // check if our type is nullable and the underlying types match
                else if (prop.PropertyType.IsGenericType)
                {
                    Type innerType = Nullable.GetUnderlyingType(prop.PropertyType);
                    if (!(innerType is null) && innerType == tProp.PropertyType)
                    {
                        match = true;
                    }
                }

                if (!match)
                {
                    continue;
                }

                object val = prop.GetValue(this, null);
                if (val is null)
                {
                    continue;
                }
                object fiPropVal = tProp.GetValue(t, null);
                if (val is IComparable compare && compare.CompareTo(fiPropVal) == 0)
                {
                    return true;
                }
            }

            return false;
        }

        private bool NonNullComparison(T t)
        {
            Type tType = t.GetType();
            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance;

            foreach (PropertyInfo prop in GetType().GetProperties(bindingFlags))
            {
                // check if FileInfo has this property aswell
                PropertyInfo tProp = tType.GetProperty(prop.Name, bindingFlags);
                if (tProp is null)
                {
                    continue;
                }

                // check if types match
                bool match = false;
                if (prop.PropertyType == tProp.PropertyType)
                {
                    match = true;
                }
                // check if our type is nullable and the underlying types match
                else if (prop.PropertyType.IsGenericType)
                {
                    Type innerType = Nullable.GetUnderlyingType(prop.PropertyType);
                    if (!(innerType is null) && innerType == tProp.PropertyType)
                    {
                        match = true;
                    }
                }

                if (!match)
                {
                    continue;
                }

                object val = prop.GetValue(this, null);
                if (val is null)
                {
                    continue;
                }
                object fiPropVal = tProp.GetValue(t, null);
                bool valueEquals = val is IComparable compare && compare.CompareTo(fiPropVal) == 0;
                if (!valueEquals)
                {
                    return false;
                }
            }

            return true;
        }

        private bool FullComparison(T t)
        {
            Type tType = t.GetType();
            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance;

            foreach (PropertyInfo prop in GetType().GetProperties(bindingFlags))
            {
                // check if FileInfo has this property aswell
                PropertyInfo tProp = tType.GetProperty(prop.Name, bindingFlags);
                if (tProp is null)
                {
                    continue;
                }

                // check if types match
                bool match = false;
                if (prop.PropertyType == tProp.PropertyType)
                {
                    match = true;
                }
                // check if our type is nullable and the underlying types match
                else if (prop.PropertyType.IsGenericType)
                {
                    Type innerType = Nullable.GetUnderlyingType(prop.PropertyType);
                    if (!(innerType is null) && innerType == tProp.PropertyType)
                    {
                        match = true;
                    }
                }

                if (!match)
                {
                    return false;
                }

                object val = prop.GetValue(this, null);
                bool valNull = val is null;
                object fiPropVal = tProp.GetValue(t, null);
                bool fiValNull = fiPropVal is null;
                if (valNull && !fiValNull)
                {
                    return false;
                }

                // if both are null we dont need to compare the value
                if (valNull && fiValNull)
                {
                    continue;
                }

                // look into better way of doing this if class doesnt implement IComparable
                bool valueEquals = val is IComparable compare && compare.CompareTo(fiPropVal) == 0;
                if (!valueEquals)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
