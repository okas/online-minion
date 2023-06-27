namespace OnlineMinion.Common.Utilities;

public static class TypeHelpers
{
    public static bool IsAssignableToGenericInterface(this Type givenType, Type genericType) =>
        givenType
            .GetInterfaces()
            .Any(
                interfaceType => interfaceType.IsGenericType
                                 && interfaceType.GetGenericTypeDefinition() == genericType
            );
}
