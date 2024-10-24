using MongoDB.Bson.Serialization.Attributes;

namespace LagerhotellAPI.Models.DbModels;

public class CompanyUserDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string CompanyUserId { get; set; }
    public string Auth0Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Name { get; set; }
    public string CompanyNumber { get; set; }
    public string Email { get; set; }
    public bool IsEmailVerified { get; set; }
    public string PhoneNumber { get; set; }
    public Address Address { get; set; }

    public CompanyUserDocument(string companyUserId, string firstName, string lastName, string name, string companyNumber, string email, string phoneNumber, Address address, bool isEmailVerified, string auth0Id)
    {
        CompanyUserId = companyUserId;
        FirstName = firstName;
        LastName = lastName;
        Name = name;
        CompanyNumber = companyNumber;
        Email = email;
        PhoneNumber = phoneNumber;
        Address = address;
        IsEmailVerified = isEmailVerified;
        Auth0Id = auth0Id;
    }
    public CompanyUserDocument(string id, string companyUserId, string firstName, string lastName, string name, string companyNumber, string email, string phoneNumber, Address address, bool isEmailVerified, string auth0Id)
    {
        Id = id;
        CompanyUserId = companyUserId;
        FirstName = firstName;
        LastName = lastName;
        Name = name;
        CompanyNumber = companyNumber;
        Email = email;
        PhoneNumber = phoneNumber;
        Address = address;
        IsEmailVerified = isEmailVerified;
        Auth0Id = auth0Id;
    }
}