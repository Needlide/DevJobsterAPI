using FluentValidation;

namespace DevJobsterAPI.Common;

public static class ValidationExtensions
{
    public static RouteHandlerBuilder WithValidation<T>(this RouteHandlerBuilder builder) where T : class
    {
        return builder.AddEndpointFilter(async (context, next) =>
        {
            var validator = context.HttpContext.RequestServices.GetService<IValidator<T>>();
            if (validator == null) return await next(context);

            var parameter = context.Arguments.OfType<T>().FirstOrDefault();
            if (parameter == null) return await next(context);

            var validationResult = await validator.ValidateAsync(parameter);
            if (!validationResult.IsValid) return Results.ValidationProblem(validationResult.ToDictionary());

            return await next(context);
        });
    }
}