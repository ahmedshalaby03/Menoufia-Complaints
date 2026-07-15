using ComplaintSystem.Domain.Entities;
using ComplaintSystem.Domain.Enums;
using ComplaintSystem.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;

namespace ComplaintSystem.Infrastructure.Seed;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
    {
        // Roles
        foreach (var role in new[] { UserRoles.Admin, UserRoles.Employee })
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        // Default admin (غيّر الباسورد بعد أول تشغيل)
        if (await userManager.FindByEmailAsync("admin@gmail.com") == null)
        {
            var admin = new ApplicationUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                FullName = "مشرف النظام",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(admin, "Admin@123");
            await userManager.AddToRoleAsync(admin, UserRoles.Admin);
        }

        if (!context.Governorates.Any())
        {
            var governorates = new[]
            {
                "القاهرة","الجيزة","الإسكندرية","الدقهلية","البحر الأحمر","البحيرة","الفيوم",
                "الغربية","الإسماعيلية","المنوفية","المنيا","القليوبية","الوادي الجديد","السويس",
                "أسوان","أسيوط","بني سويف","بورسعيد","دمياط","الشرقية","جنوب سيناء","كفر الشيخ",
                "مطروح","الأقصر","قنا","شمال سيناء","سوهاج"
            };
            foreach (var g in governorates) context.Governorates.Add(new Governorate { NameAr = g });
            await context.SaveChangesAsync();

            var monoufia = context.Governorates.First(g => g.NameAr == "المنوفية");
            var centers = new[] { "شبين الكوم", "منوف", "قويسنا", "أشمون", "الباجور", "بركة السبع", "تلا", "الشهداء", "السادات", "أبو المطامير" };
            foreach (var c in centers) context.Centers.Add(new Center { NameAr = c, GovernorateId = monoufia.Id });
            await context.SaveChangesAsync();
        }

        if (!context.Sectors.Any())
        {
            var sectorsWithServices = new Dictionary<string, string[]>
            {
                ["الزراعة"] = new[] { "الأراضي الزراعية", "الأسمدة الزراعية", "التراخيص بالزراعة", "الثروة الحيوانية", "الجمعيات الزراعية", "المحاصيل الزراعية" },
                ["الإسكان والمنشآت والأراضي"] = new[] { "تراخيص البناء", "أراضي الدولة", "المرافق" },
                ["الصحة"] = new[] { "المستشفيات", "الوحدات الصحية", "التأمين الصحي" },
                ["التعليم والبحث العلمي"] = new[] { "المدارس", "الجامعات", "محو الأمية" },
                ["الكهرباء"] = new[] { "فواتير الكهرباء", "أعطال الشبكة", "التوصيلات الجديدة" },
                ["مياه الشرب والصرف الصحي"] = new[] { "شبكة المياه", "الصرف الصحي", "فواتير المياه" },
                ["الانتقالات والطرق والموانئ"] = new[] { "الطرق العامة", "النقل العام", "المرور" }
            };

            foreach (var (sectorName, services) in sectorsWithServices)
            {
                var sector = new Sector { NameAr = sectorName };
                foreach (var s in services) sector.Services.Add(new Service { NameAr = s });
                context.Sectors.Add(sector);
            }
            await context.SaveChangesAsync();
        }

        if (!context.GovernmentEntities.Any())
        {
            var entities = new[] { "بلدية شبين الكوم", "الصحة العامة بالمنوفية", "التعليم بالمنوفية", "مرافق ومياه المنوفية", "الكهرباء بالمنوفية" };
            foreach (var e in entities) context.GovernmentEntities.Add(new GovernmentEntity { NameAr = e });
            await context.SaveChangesAsync();
        }
    }
}
