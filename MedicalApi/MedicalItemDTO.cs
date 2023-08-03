public class MedicalItemDTO
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Gender { get; set; }
    public DateTime Birthdate { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? StreetAddress { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public string? Country { get; set; }

    public MedicalItemDTO() { }

   public MedicalItemDTO(Medical medicalItem) =>
    (Id, FirstName, LastName, Gender, Birthdate, 
    PhoneNumber, Email, StreetAddress, City, State, 
    ZipCode, Country) = (medicalItem.Id, medicalItem.FirstName, medicalItem.LastName, medicalItem.Gender,
    medicalItem.Birthdate, medicalItem.PhoneNumber, medicalItem.Email, medicalItem.StreetAddress, medicalItem.City, medicalItem.State, medicalItem.ZipCode, medicalItem.Country);
}