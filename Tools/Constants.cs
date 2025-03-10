using System.Reflection;

namespace SuperFilter;

public enum Operator
{
    Equals,
    LessThan,
    GreaterThan,
    StartsWith,
    Contains,
    IsEqualToYear,
    IsEqualToYearAndMonth,
    IsEqualToFullDate
}

public static class Constants
{
    private static Dictionary<Operator, MethodInfo?> MethodInfosForStringFiltering => new ()
    {
        { Operator.StartsWith, typeof(string).GetMethod("StartsWith", [typeof(string)]) },
        { Operator.Contains, typeof(string).GetMethod("Contains", [typeof(string)]) },
        { Operator.Equals, typeof(string).GetMethod("Equals", [typeof(string)]) }
    };
    
    private static Dictionary<Operator, MethodInfo?> MethodInfosForDateTimeFiltering => new()
    {
        { Operator.Equals, typeof(DateTime).GetMethod("Equals", new[] { typeof(DateTime) }) },
        { Operator.IsEqualToYear, typeof(CustomOperations).GetMethod(nameof(CustomOperations.CompareDateByYear), new[] { typeof(DateTime), typeof(DateTime) }) },
        { Operator.IsEqualToYearAndMonth, typeof(CustomOperations).GetMethod(nameof(CustomOperations.CompareDateByYearAndMonth), new[] { typeof(DateTime), typeof(DateTime) }) },
        { Operator.IsEqualToFullDate, typeof(CustomOperations).GetMethod(nameof(CustomOperations.CompareDateByYearMonthAndDay), new[] { typeof(DateTime), typeof(DateTime) }) },
        { Operator.LessThan, typeof(DateTime).GetMethod("CompareTo", new[] { typeof(DateTime) }) },
        { Operator.GreaterThan, typeof(DateTime).GetMethod("CompareTo", new[] { typeof(DateTime) }) },
    };
    
    private static Dictionary<Operator, MethodInfo?> MethodInfosForBooleanFiltering => new()
    {
        { Operator.Equals, typeof(bool).GetMethod("Equals", [typeof(bool)]) },
    };
    
    private static Dictionary<Operator, MethodInfo?> MethodInfosForIntegerFiltering => new()
    {
        { Operator.Equals, typeof(int).GetMethod("Equals", [typeof(int)]) },
        { Operator.LessThan, typeof(int).GetMethod("LessThan", [typeof(int)]) },
        { Operator.GreaterThan, typeof(int).GetMethod("GreaterThan", [typeof(int)]) },
    };
    
    public static Dictionary<Operator, MethodInfo?> GetMethodInfos(Type propertyType)
    {
        if (propertyType == typeof(string))
            return MethodInfosForStringFiltering;

        if (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?))
            return MethodInfosForDateTimeFiltering;

        if (propertyType == typeof(bool) || propertyType == typeof(bool?))
            return MethodInfosForBooleanFiltering;

        if (propertyType == typeof(int) || propertyType == typeof(int?))
            return MethodInfosForIntegerFiltering;

        return new Dictionary<Operator, MethodInfo?>();
    }
    
    public static bool IsOperatorLegal<T>(Operator op)
    {
        Dictionary<Operator, MethodInfo?> methodInfos = GetMethodInfos(typeof(T));
        return methodInfos.Count > 0;
    }
}