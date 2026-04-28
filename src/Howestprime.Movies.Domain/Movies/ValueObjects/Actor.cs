using Howestprime.Movies.Domain.Shared;

namespace Howestprime.Movies.Domain.Movies.ValueObjects;

public sealed record Actor(string Value) : ValueObject
{
    public static Actor Create(string value)
    {
        EnsureActorIsValid(value);
        return new Actor(value);
    }

    private static void EnsureActorIsValid(string actor)
    {
        if (string.IsNullOrWhiteSpace(actor)) throw new ArgumentException("Actor name cannot be empty.");
    }

    public override string ToString()
    {
        return base.ToString();
    }
}