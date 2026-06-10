/**
 * Image Upload Preview Plugin
 * کدهای مدیریت پیش‌نمایش، لغو و حذف تصویر به صورت بازمصرف‌پذیر
 */

// ساختار HTML پیشنهادی برای استفاده از این پلاگین:

// <div class="preview-image-group row align-items-center mb-4">
//     <div class="col-md-8 mb-3 mb-md-0">
//         <label asp-for="ProfilePicture" class="form-label"></label>
//         <div class="input-group">
//             <input asp-for="ProfilePicture" type="file" class="form-control preview-input" accept="image/*" />

//             <button class="btn btn-outline-danger preview-remove-btn" type="button" title="حذف عکس">
//                 <i class="bi bi-trash"></i> حذف عکس
//             </button>
//         </div>
//         <div class="form-text mt-1">فرمت‌های مجاز: JPG, PNG</div>
//     </div>

//     <div class="col-md-4 text-center">
//         <img class="rounded-circle img-thumbnail preview-box"
//             src="/ProfilePictures/no-image.jpg"
//             data-default-src="/ProfilePictures/no-image.jpg"
//             style="width: 100px; height: 100px; object-fit: cover;" alt="پیش‌نمایش" />
//     </div>

//     <input type="hidden" class="preview-delete-marker" name="DeleteCurrentPicture" value="false" />
// </div>

function initializeImagePreview() {
    // پیدا کردن تمام فرم‌هایی که المان‌های آپلود تصویر با قابلیت پیش‌نمایش دارند
    $('.preview-image-group').each(function () {
        var $group = $(this);

        // پیدا کردن المان‌های داخلی هر گروه به صورت نسبی
        var $input = $group.find('.preview-input');
        var $img = $group.find('.preview-box');
        var $btnRemove = $group.find('.preview-remove-btn');
        var $hiddenDelete = $group.find('.preview-delete-marker');

        var defaultImageSrc = $img.data('default-src') || "/ProfilePictures/no-image.jpg";
        var initialImageSrc = $img.attr('src');
        var lastValidFile = null;

        $input.on('change', function () {
            var file = this.files[0];

            // سناریوی لغو انتخاب (Cancel)
            if (!file) {
                if (lastValidFile) {
                    var dataTransfer = new DataTransfer();
                    dataTransfer.items.add(lastValidFile);
                    this.files = dataTransfer.files;
                    $img.attr("src", URL.createObjectURL(lastValidFile));
                } else {
                    $img.attr("src", initialImageSrc);
                }
                return;
            }

            // بررسی فرمت فایل
            if (file.type.startsWith('image/')) {
                lastValidFile = file;
                var reader = new FileReader();
                reader.onload = function (e) {
                    $img.attr("src", e.target.result);
                    if ($hiddenDelete.length) $hiddenDelete.val("false");
                }
                reader.readAsDataURL(file);
            } else {
                alert("لطفاً فقط فایل تصویری انتخاب کنید.");
                if (lastValidFile) {
                    var dataTransfer = new DataTransfer();
                    dataTransfer.items.add(lastValidFile);
                    this.files = dataTransfer.files;
                } else {
                    $(this).val('');
                    $img.attr("src", initialImageSrc);
                }
            }
        });

        // دکمه حذف عکس
        $btnRemove.on('click', function () {
            lastValidFile = null;
            $input.val('');
            $img.attr("src", defaultImageSrc);
            if ($hiddenDelete.length) $hiddenDelete.val("true");
        });
    });
}

// اجرای خودکار تابع پس از لود کامل صفحه
$(document).ready(function () {
    initializeImagePreview();
});