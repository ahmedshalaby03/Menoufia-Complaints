# نظام الشكاوى الحكومية الموحدة — محافظة المنوفية

نظام إلكتروني لإدارة وتتبع الشكاوى الحكومية، مبني على معمارية **Onion Architecture** في الباك إند، وواجهة **Angular** حديثة في الفرونت إند، مع خاصية **كشف الشكاوى المتكررة بالذكاء الاصطناعي (RAG)**.

---

## 📋 نظرة عامة

النظام مخصص للجهات الحكومية، حيث يقوم **الموظف (Employee)** بإدخال بيانات الشكاوى الواردة من المواطنين، بينما يتولى **المشرف (Admin)** متابعة الشكاوى، تحديث حالتها، وتعيينها للكيانات الحكومية المختصة، بالإضافة إلى مراجعة التقارير والإحصائيات.

### أبرز الميزات
- 🔐 تسجيل دخول وصلاحيات مبنية على الأدوار (Admin / Employee) عبر ASP.NET Core Identity + JWT
- 🧠 **كشف الشكاوى المتكررة تلقائيًا (RAG)**: عند كتابة وصف شكوى جديدة، يبحث النظام في الشكاوى السابقة عبر OpenAI Embeddings، ويحذّر الموظف لو وجدت شكوى مشابهة، مع عرض حالتها الحالية
- 📊 لوحات تحكم منفصلة للموظف والمشرف (إحصائيات، توزيع الأولويات، آخر الشكاوى)
- 📥 صندوق وارد للمشرف لمتابعة الشكاوى المعينة له
- 📈 صفحة تقارير شاملة (معدل الإنجاز، أداء الكيانات الحكومية، توزيع الحالات والأولويات)
- 📎 إرفاق ملفات مع كل شكوى
- 🗂️ سجل كامل لتاريخ تغييرات حالة كل شكوى

---

## 🏗️ البنية التقنية

### الباك إند — `Backend/`
ASP.NET Core Web API (.NET 10) بمعمارية **Onion Architecture** من 4 مشاريع:

```
ComplaintSystem.Domain          → Entities, Enums, Interfaces (بدون أي dependency خارجي)
ComplaintSystem.Application     → DTOs, Services, Interfaces (business logic)
ComplaintSystem.Infrastructure  → EF Core (SQL Server), Identity, JWT, RAG (OpenAI)
ComplaintSystem.API             → Controllers, Program.cs, Swagger
```

| الطبقة | المسؤولية |
|---|---|
| Domain | الكيانات الأساسية (Complaint, ApplicationUser, GovernmentEntity...) |
| Application | منطق العمل، الـ DTOs، وعقود الخدمات |
| Infrastructure | الوصول للبيانات (EF Core)، تسجيل الدخول (Identity/JWT)، خدمة الـ RAG |
| API | نقاط الوصول (Endpoints)، التوثيق (Swagger)، إعدادات التطبيق |

### الفرونت إند — `Frontend/`
Angular (Standalone Components) + SCSS design system مخصص:

```
core/       → services (auth, complaints, lookups, dashboard) + guards + models
shared/     → مكونات مشتركة (sidebar, stat-card, badges)
features/   → auth/login, employee/*, admin/*, complaint-details
styles/     → نظام التصميم الكامل (الألوان، الخطوط، المسافات) في tokens.scss
```

---

## ⚙️ التشغيل من الصفر

### المتطلبات
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) (نسخة 18 أو أحدث) + Angular CLI (`npm i -g @angular/cli`)
- SQL Server (Express أو LocalDB كافي)
- مفتاح [OpenAI API](https://platform.openai.com/api-keys) (لخاصية كشف التكرار — اختياري، يمكن تعطيله)
- Visual Studio 2022 (أو أي IDE يدعم .NET)

### 1️⃣ الباك إند

```powershell
cd Backend
```

انسخ `appsettings.Example.json` إلى `appsettings.json` داخل مشروع `ComplaintSystem.API`، وعدّل فيه:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=MenoufiaComplaintsDb;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "Jwt": {
    "Key": "ضع مفتاحًا سريًا عشوائيًا لا يقل عن 32 حرفًا"
  },
  "OpenAI": {
    "ApiKey": "ضع مفتاح OpenAI الخاص بك"
  },
  "Rag": {
    "Enabled": true
  }
}
```

> 💡 لو مش عايز تفعّل خاصية كشف التكرار حاليًا، خلّي `Rag:Enabled` بـ `false` والنظام هيشتغل عادي من غيرها.

بعد كده، من **Package Manager Console** (Default project: `ComplaintSystem.Infrastructure`):

```powershell
Add-Migration InitialCreate -StartupProject ComplaintSystem.API
Update-Database -StartupProject ComplaintSystem.API
```

شغّل المشروع (F5) — هيعمل seed تلقائي لـ:
- حساب أدمن افتراضي: `admin@gmail.com` / `Admin@123`
- محافظات مصر، مراكز المنوفية، القطاعات والخدمات، والكيانات الحكومية

الـ API هيفتح على Swagger تلقائيًا (المسار الظاهر في `launchSettings.json`).

### 2️⃣ الفرونت إند

```powershell
cd Frontend
npm install
```

عدّل `src/environments/environment.ts` بالبورت الصحيح بتاع الـ API عندك:

```typescript
export const environment = {
  production: false,
  apiUrl: 'https://localhost:XXXXX/api'
};
```

```powershell
ng serve -o
```

هيفتح على `http://localhost:4200`.

---

## 🔑 حسابات تجريبية

| الدور | البريد الإلكتروني | كلمة المرور |
|---|---|---|
| مشرف (Admin) | `admin@gmail.com` | `Admin@123` |

> لإنشاء حسابات موظفين جدد: سجّل دخول كأدمن، اعمل Authorize في Swagger بالتوكن، وانده على `POST /api/auth/register`.

---

## 🧠 كيف تعمل خاصية كشف التكرار (RAG)؟

1. أثناء كتابة وصف شكوى جديدة، وبعد توقف الموظف عن الكتابة، يرسل الفرونت إند الوصف إلى `POST /api/complaints/check-duplicates`
2. الباك إند يحوّل النص إلى embedding عبر OpenAI (`text-embedding-3-small`)
3. يقارن الـ embedding بجميع الشكاوى المفتوحة سابقًا عبر cosine similarity
4. لو وجدت شكوى بنسبة تشابه ≥ 83%، يظهر تحذير للموظف يوضح رقم الشكوى المشابهة وحالتها الحالية
5. يمكن للموظف تجاهل التحذير والمتابعة، أو مراجعة الشكوى القديمة أولًا

---

## 📁 هيكل المشروع

```
شكاوي/
├── Backend/
│   ├── ComplaintSystem.Domain/
│   ├── ComplaintSystem.Application/
│   ├── ComplaintSystem.Infrastructure/
│   ├── ComplaintSystem.API/
│   └── ComplaintSystem.sln
├── Frontend/
│   ├── src/
│   └── package.json
└── .gitignore
```

---

## 🛣️ خارطة الطريق (لسه ناقص)

- [ ] شاشة تسجيل موظفين جدد من واجهة الأدمن مباشرة (بدل Swagger)
- [ ] Pagination كامل على قوائم الشكاوى
- [ ] بيانات مراكز باقي محافظات مصر (حاليًا المنوفية فقط)
- [ ] تحسين الاستجابة على شاشات الموبايل (Responsive)

---

## 🛠️ التقنيات المستخدمة

**الباك إند:** ASP.NET Core 10 · Entity Framework Core · SQL Server · ASP.NET Identity · JWT · OpenAI API
**الفرونت إند:** Angular (Standalone) · TypeScript · SCSS

---
