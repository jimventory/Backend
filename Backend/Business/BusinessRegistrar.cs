using Backend1.Types;

namespace Backend1.Business;

public class BusinessRegistrar
{
    public void Register(BusinessRegistrationForm form)
    {
        // Now that I think about it, there's really no particular reason to use the hash as the ID.
        var hash = form.GetHashCode();
    }
}