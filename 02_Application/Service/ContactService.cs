using Application.Service.Base;
using Application.Service.Interface;
using Core.Entity;
using Core.Repository.Interface;

namespace Application.Service;
public class ContactService : BaseService<Contact>, IContactService
{
    private readonly IContactRepository _contactRepository;  

    public ContactService(IContactRepository contactRepository) : base(contactRepository)
    {
        _contactRepository = contactRepository;        
    }

    public async Task<IList<Contact>> GetAllByDddAsync(int dddId)
    {
        return await _contactRepository.GetAllByDddAsync(dddId);
    }
}
