using System.Linq.Expressions;

namespace OnlineMinion.Presentation.Utilities;

public static class TypeMemberHelpers
{
    /// <summary>
    ///     Gets property's nested name from expression.
    /// </summary>
    /// <example>
    ///     Expression: <code>x => x.Property1.Property2.Property3</code>
    ///     Result: <code>Property1.Property2.Property3</code>
    /// </example>
    /// <param name="expression">Member access expression, 1 to n- levels deep.</param>
    /// <exception cref="ArgumentException">If it is not a <see cref="MemberExpression" />.</exception>
    public static string NestedNameOf<T>(Expression<Func<T, object>> expression)
    {
        if (expression.Body is not MemberExpression memberExpression)
        {
            throw new ArgumentException("Expression body must be a member expression", nameof(expression));
        }

        var body = memberExpression.ToString().AsSpan();

        return body.Slice(body.IndexOf('.') + 1).ToString();
    }
}
