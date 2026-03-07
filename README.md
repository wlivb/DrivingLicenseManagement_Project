# نظام إدارة رخص القيادة (DVLD) - Driving License Management System

مشروع متكامل لإدارة عمليات استخراج وتجديد رخص القيادة، تم بناؤه باستخدام لغة **C#** وقواعد بيانات **SQL Server**، مع التركيز على تطبيق أفضل الممارسات البرمجية.

## 🏗️ هيكلية المشروع (Architecture)
المشروع مصمم بنظام **4-Tier Architecture** لضمان سهولة الصيانة والتطوير:
1.  **Presentation Layer (UI)**: واجهات المستخدم باستخدام WinForms.
2.  **Business Logic Layer (BLL)**: الطبقة المسؤولة عن منطق العمل والتحقق من البيانات.
3.  **Data Access Layer (DAL)**: المسؤولة عن الاتصال المباشر بقاعدة البيانات.
4.  **Data Transfer Objects (DTOs)**: لتبادل البيانات بين الطبقات بشكل آمن ومنظم.

## 🚀 المميزات التقنية
* **Custom SQL Helper**: استخدام كلاس `clsSqlHelper` يعتمد على الـ **Delegates** و `Action<SqlCommand>` لتقليل تكرار الكود.
* **DTO Pattern**: فصل البيانات عن المنطق البرمجي باستخدام `UserDTO` وغيرها.
* **Database Management**: توفير سكريبت SQL نظيف وعالمي لإنشاء قاعدة البيانات بضغطة زر.

## 🛠️ المتطلبات وكيفية التشغيل
1.  **قاعدة البيانات**: قم بتشغيل ملف `DatabaseScript.sql` المرفق في المستودع على **SQL Server Management Studio** لإنشاء قاعدة `DVLD`.
2.  **الاتصال**: تأكد من تعديل الـ `ConnectionString` في ملف `clsDataAccessSettings.cs` ليتناسب مع اسم السيرفر لديك.
