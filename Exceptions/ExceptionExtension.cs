namespace UserService.Exceptions;

public static class ExceptionExtension
{
    public static Exception Get(this Exception exception)
    {
        if (exception.InnerException != null)
        {
            exception = exception.InnerException;
        }

        return exception;
    }
        
    public static string ClassName(this Exception exception)
    {
        var targetSite = exception.Get().TargetSite;
        if (targetSite == null || targetSite.DeclaringType == null)
        {
            return null;
        }
        return targetSite.DeclaringType.Name;
    }

    public static string MethodName(this Exception exception)
    {
        var targetSite = exception.Get().TargetSite;
        if (targetSite == null)
        {
            return null;
        }
        return targetSite.Name;
    }
}