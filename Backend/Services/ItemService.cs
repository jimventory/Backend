using Backend1.Abstractions;
using Backend1.Models;
using System.Net;
using System.Net.Mail;

namespace Backend1.Services;

public class ItemService : IItemService
{
    private readonly IItemRepository _repository;
    private readonly ILogger _logger;
    
    public ItemService(IItemRepository repo, ILogger<ItemService> logger)
    {
        _repository = repo;
        _logger = logger;
    }

    public bool Add(Item item)
    {
        try
        {
            _repository.Add(item);
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to add item: {ExMessage}", e.Message);
            return false;
        }
        
        return true;
    }

    public bool Update(Item item)
    {
        try
        {
            // This call throws an exception if it can't find the item.
            _repository.GetById(item.Id);
            
            // Item exists, can update.
            
            _repository.Update(item);
        }
        catch (Exception e)
        {
            // Item did not exist.  Can't update it.
            // Should we just add it..?
            // For now, no.
            _logger.LogError("Failed to update item: {ExMessage}", e.Message);
            return false;
        }

        return true;
    }

    // No reason for this overload to exist, probably will delete in future refactoring if we don't use.
    public bool Delete(Item item)
    {
        try
        {
            _repository.Delete(item);
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to delete item: {ExMessage}", e.Message);
            return false;
        }
        
        return true;
    }
    
    public bool Delete(uint id)
    {
        try
        {
            // This call throws an exception if it can't find the item.
            var item = _repository.GetById(id);
            
            // We found the item, delete it.
            _repository.Delete(item);
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to delete item: {ExMessage}", e.Message);
            return false;
        }
        
        return true;
    }

    public IEnumerable<Item> GetBusinessInventoryItems(uint businessId)
    {
        try
        {
            var globalInventory = _repository.GetAll();
            var businessItems = globalInventory.Where((item) => item.BusinessId == businessId);

            return businessItems;
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to get inventory: {ExMessage}", e.Message);
        }

        return Enumerable.Empty<Item>();
    }

    public void SendEmailTest()
    {
        try
        {
            string fromMail = "streamlineinventorymanager@gmail.com";
            string fromPassword = EnvVarHelper.GetVariable("STREAMLINE_EMAIL_PASSWORD");

            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = "Test Subject";
            message.To.Add(new MailAddress("vbroda@vols.utk.edu"));
            message.Body = "<html><body>Hello world from Streamline </body></html>";
            message.IsBodyHtml = true;

            using (var smtpClient = new SmtpClient("smtp.gmail.com", 587))
            {
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential(fromMail, fromPassword);
                smtpClient.Send(message);
            }
        }
        catch (SmtpException ex)
        {
            _logger.LogError("Failed to send test email: {ExMessage}", ex.Message);
            throw; // Re-throw the exception to propagate it further if needed
        }
    }

}