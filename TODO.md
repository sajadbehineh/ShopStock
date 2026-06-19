# Project TODO

## Backlog
- [ ] بررسی نگهداری یا حذف تصویر پروفایل در Soft Delete
- [ ] پیاده‌سازی Restore User در آینده
- [ ] پیاده‌سازی Cleanup Job برای حذف تصاویر قدیمی
- [ ] بهتر است DateTime.Now را بعداً به DateTime.UtcNow تغییر بدم. تا اگر سرور به خارج از کشور منتقل شد، تاریخ‌ها به مشکل نخورند.
- [ ] آیا برای کاربر فروشگاه هم باید در جدول نقش ها نقشی در نظر گرفته شود؟
- [ ] آیا ادمین نیاز دارد یا باید بتواند اطلاعاتی مثل شماره موبایل یا ایمیل کاربر را ویرایش کند؟
- [ ] آیا مرسوم هست بجای ProfilePictureName از ProfilePictureId استفاده کنیم؟
- [ ] بررسی excludeUserId برای ورودی متد EditProfileAsync در AccountService
- [ ] پراپرتی های CreatedAt,.... باید در ریپازیتوری مقدار بگیرند یا در ToMap ?
- [ ] تصویر قبلی فقط بعد از Save موفق حذف شود.
- [ ] Send Activation email in EditUser if Email property was edited by admin.
- [ ] !Important: SaveAsync() in UserRepository Most be return bool to handle failed save

## Improvements
- [ ] استانداردسازی Mapperها بین DTO و ViewModel


## Notes

- [ ] Password most be always hashed in service layer, not in Mapper extension. Mapper should only map data, not perform business logic.