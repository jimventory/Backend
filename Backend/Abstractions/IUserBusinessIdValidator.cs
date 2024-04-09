namespace Backend1.Abstractions;

public interface IUserBusinessIdValidator : IUserBusinessIdResolver
{
    bool Validate(uint id);
}
