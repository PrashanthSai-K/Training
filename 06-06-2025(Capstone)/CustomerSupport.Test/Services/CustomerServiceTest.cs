using AutoMapper;
using CustomerSupport.Interfaces;
using CustomerSupport.Models;
using CustomerSupport.Models.Dto;
using CustomerSupport.Services;
using Moq;

[TestFixture]
public class CustomerServiceTests
{
    private Mock<IRepository<int, Customer>> _customerRepoMock;
    private Mock<IRepository<string, User>> _userRepoMock;
    private Mock<IAuditLogService> _auditMock;
    private Mock<IHashingService> _hashingMock;
    private IMapper _mapper;
    private CustomerService _service;

    [SetUp]
    public void Setup()
    {
        _customerRepoMock = new Mock<IRepository<int, Customer>>();
        _userRepoMock = new Mock<IRepository<string, User>>();
        _auditMock = new Mock<IAuditLogService>();
        _hashingMock = new Mock<IHashingService>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CustomerRegisterDto, Customer>();
            cfg.CreateMap<CustomerRegisterDto, User>();
            cfg.CreateMap<CustomerUpdateDto, Customer>();
        });

        _mapper = config.CreateMapper();

        _service = new CustomerService(_customerRepoMock.Object, _userRepoMock.Object, _mapper, _auditMock.Object, _hashingMock.Object);
    }

    [Test]
    public async Task CreateCustomer_ShouldCreateUserAndCustomer()
    {
        var dto = new CustomerRegisterDto { Email = "cust@test.com", Name = "Cust", Password = "pass" };
        _hashingMock.Setup(h => h.HashData("pass")).Returns("hashed");

        _userRepoMock.Setup(r => r.Create(It.IsAny<User>())).ReturnsAsync(new User());
        _customerRepoMock.Setup(r => r.Create(It.IsAny<Customer>())).ReturnsAsync(new Customer());

        var result = await _service.CreateCustomer(dto);

        Assert.That(result, Is.Not.Null);
        _userRepoMock.Verify(r => r.Create(It.Is<User>(u => u.Password == "hashed" && u.Roles == "Customer")), Times.Once);
    }

    [Test]
    public void DeleteCustomer_WrongUser_ShouldThrow()
    {
        _customerRepoMock.Setup(r => r.GetById(1)).ReturnsAsync(new Customer { Email = "true@test.com" });

        Assert.ThrowsAsync<UnauthorizedAccessException>(() => _service.DeleteCustomer("wrong@test.com", 1));
    }
}
