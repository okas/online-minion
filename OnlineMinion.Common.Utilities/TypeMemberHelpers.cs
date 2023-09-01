using System.Linq.Expressions;

namespace OnlineMinion.Common.Utilities;

public static class TypeMemberHelpers
{
    /// <summary>
    ///     Gets property's nested name from expression.
    /// </summary>
    /// <example>
    ///     Expression: <code>x => x.Property1.Property2.Property3</code>
    ///     Result: <code>Property1.Property2.Property3</code>
    /// </example>
    /// <exception cref="ArgumentException">If it is not a <see cref="MemberExpression" />.</exception>
    public static string NestedNameOf<T>(Expression<Func<T, object>> memberExpression)
    {
        if (memberExpression.Body is not MemberExpression expression)
        {
            throw new ArgumentException("Expression body must be a member expression", nameof(memberExpression));
        }

        var body = expression.ToString();
        var startIdx = body.IndexOf('.', StringComparison.Ordinal) + 1;

        return body[startIdx..];
    }
}
