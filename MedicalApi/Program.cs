using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MedicalDb>(opt => opt.UseInMemoryDatabase("MedicalList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

RouteGroupBuilder Medicalitems = app.MapGroup("/Medicalitems");

Medicalitems.MapGet("/", GetAllMedicals);
Medicalitems.MapGet("/{id}", GetMedical);
Medicalitems.MapPost("/", CreateMedical);
Medicalitems.MapPut("/{id}", UpdateMedical);
Medicalitems.MapDelete("/{id}", DeleteMedical);

app.Run();

static async Task<IResult> GetAllMedicals(MedicalDb db)
{
    return TypedResults.Ok(await db.Medicals.ToArrayAsync());
}

static async Task<IResult> GetMedical(int id, MedicalDb db)
{
    var medical = await db.Medicals.FindAsync(id);

    if (medical is Medical)
    {
        var medicalItemDTO = new MedicalItemDTO
        {
            Id = medical.Id,
            FirstName = medical.FirstName,
            LastName = medical.LastName,
            Gender = medical.Gender,
            Birthdate = medical.Birthdate,
            PhoneNumber = medical.PhoneNumber,
            Email = medical.Email,
            StreetAddress = medical.StreetAddress,
            City = medical.City,
            State = medical.State,
            ZipCode = medical.ZipCode,
            Country = medical.Country
        };

        return TypedResults.Ok(medicalItemDTO);
    }
    else
    {
        return TypedResults.NotFound();
    }
}

static async Task<IResult> CreateMedical(MedicalItemDTO medicalItemDTO, MedicalDb db)
{
    var medicalItem = new Medical
    {
        Id = medicalItemDTO.Id,
        FirstName = medicalItemDTO.FirstName,
        LastName = medicalItemDTO.LastName,
        Gender = medicalItemDTO.Gender,
        Birthdate = medicalItemDTO.Birthdate,
        PhoneNumber = medicalItemDTO.PhoneNumber,
        Email = medicalItemDTO.Email,
        StreetAddress = medicalItemDTO.StreetAddress,
        City = medicalItemDTO.City,
        State = medicalItemDTO.State,
        ZipCode = medicalItemDTO.ZipCode,
        Country = medicalItemDTO.Country
    };

    db.Medicals.Add(medicalItem);
    await db.SaveChangesAsync();

    medicalItemDTO = new MedicalItemDTO(medicalItem);

    return TypedResults.Created($"/Medicalitems/{medicalItem.Id}", medicalItemDTO);
}

static async Task<IResult> UpdateMedical(int id, MedicalItemDTO medicalItemDTO, MedicalDb db)
{
    var medical = await db.Medicals.FindAsync(id);

    if (medical is null) return TypedResults.NotFound();

    medical.Id = medicalItemDTO.Id;
    medical.FirstName = medicalItemDTO.FirstName;
    medical.LastName = medicalItemDTO.LastName;
    medical.Gender = medicalItemDTO.Gender;
    medical.Birthdate = medicalItemDTO.Birthdate;
    medical.PhoneNumber = medicalItemDTO.PhoneNumber;
    medical.Email = medicalItemDTO.Email;
    medical.StreetAddress = medicalItemDTO.StreetAddress;
    medical.City = medicalItemDTO.City;
    medical.State = medicalItemDTO.State;
    medical.ZipCode = medicalItemDTO.ZipCode;
    medical.Country = medicalItemDTO.Country;

    await db.SaveChangesAsync();

    return TypedResults.NoContent();
}

static async Task<IResult> DeleteMedical(int id, MedicalDb db)
{
    if (await db.Medicals.FindAsync(id) is Medical medical)
    {
        db.Medicals.Remove(medical);
        await db.SaveChangesAsync();

        MedicalItemDTO medicalDTO = new MedicalItemDTO(medical);

        return TypedResults.Ok(medical);
    }

    return TypedResults.NotFound();
}