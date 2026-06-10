using System;
using System.Collections.Generic;
using System.Text;

namespace ShopStock.Application.Contracts
{
    public interface IImageService
    {
        /// <summary>
        /// پردازش و ذخیره تصویر به صورت استاندارد
        /// </summary>
        /// <param name="fileStream">استریم فایل ورودی</param>
        /// <param name="folderName">نام پوشه مقصد (مثلا Avatars یا Products)</param>
        /// <param name="width">عرض مورد نظر</param>
        /// <param name="height">ارتفاع مورد نظر</param>
        /// <returns>نام فایل نهایی ذخیره شده را برمیگرداند</returns>
        Task<string> SaveImageAsync(Stream fileStream, string folderName, int width = 512, int height = 512);

        void DeleteImage(string fileName, string folderName);
    }
}
