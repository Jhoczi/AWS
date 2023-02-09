using ConsumerAPI.Contracts.Data;
using ConsumerAPI.Domain;

namespace ConsumerAPI.Mapping;

public static class DomainToDtoMapper
{
    public static CustomerDto ToCustomerDto(this Customer customer)
    {
        return new CustomerDto
        {
            Id = customer.Id,
            Email = customer.Email,
            GitHubUsername = customer.GitHubUsername,
            FullName = customer.FullName,
            DateOfBirth = customer.DateOfBirth
        };
    }
}