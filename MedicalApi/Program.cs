using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MedicalDb>(opt => opt.UseInMemoryDatabase("MedicalList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

var Medicalitems = app.MapGroup("/Medicalitems");

Medicalitems.MapGet("/", GetAllMedicals);
Medicalitems.MapGet("/complete", GetCompleteMedicals);
Medicalitems.MapGet("/{id}", GetMedical);
Medicalitems.MapPost("/", CreateMedical);
Medicalitems.MapPut("/{id}", UpdateMedical);
Medicalitems.MapDelete("/{id}", DeleteMedical);

app.Run();

static async Task<IResult> GetAllMedicals(MedicalDb db)
{
    return TypedResults.Ok(await db.Medicals.ToArrayAsync());
}

static async Task<IResult> GetCompleteMedicals(MedicalDb db)
{
    return TypedResults.Ok(await db.Medicals.Where(t => t.IsComplete).ToListAsync());
}

static async Task<IResult> GetMedical(int id, MedicalDb db)
{
    return await db.Medicals.FindAsync(id)
        is Medical medical
            ? TypedResults.Ok(medical)
            : TypedResults.NotFound();
}

static async Task<IResult> CreateMedical(Medical medical, MedicalDb db)
{
    db.Medicals.Add(medical);
    await db.SaveChangesAsync();

    return TypedResults.Created($"/Medicalitems/{medical.Id}", medical);
}

static async Task<IResult> UpdateMedical(int id, Medical inputMedical, MedicalDb db)
{
    var medical = await db.Medicals.FindAsync(id);

    if (medical is null) return TypedResults.NotFound();

    medical.Name = inputMedical.Name;
    medical.IsComplete = inputMedical.IsComplete;

    await db.SaveChangesAsync();

    return TypedResults.NoContent();
}

static async Task<IResult> DeleteMedical(int id, MedicalDb db)
{
    if (await db.Medicals.FindAsync(id) is Medical medical)
    {
        db.Medicals.Remove(medical);
        await db.SaveChangesAsync();
        return TypedResults.Ok(medical);
    }

    return TypedResults.NotFound();
}