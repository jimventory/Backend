using Backend1.Types;
using Backend1.Models;
namespace Backend1.Factories;

public class BusinessFactory
{
    public Business MakeBusiness(BusinessRegistrationForm form)
    {
        // Really no reason for this to be so fucky.
        // I started this class with one thing in mind then it didn't really fit that purpose but I didn't feel like deleting.
        // Rework in future.
        // I suppose we could just this as a fancy constructor with validation.
        // But there's really no reason for a Factory to exist for one class.
        
        var business = new Business
        {
            Name = form.Name,
            Location = form.Location,
        };

        return business;
    }
}