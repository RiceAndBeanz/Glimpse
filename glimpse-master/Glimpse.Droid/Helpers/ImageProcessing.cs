using Android.Graphics;
using System.Threading.Tasks;
using static Android.Graphics.BitmapFactory;
using System.IO;
using Android.Content;

namespace Glimpse.Droid.Helpers
{
    public static class ImageProcessing
    {

        //Used to sub sample image before loading into memory, instead of loading high memory image, load scaled down version
        //Calculate the InSampleSize value as a power of 2 based on a target width and height, any other factor can be used but BitmapFactory is optimzed to use factor of 2
        public static int CalculateInSampleSize(BitmapFactory.Options options, int requestedWidth, int requestedHeight)
        {
            // Raw height and width of image
            float height = options.OutHeight;
            float width = options.OutWidth;
            double inSampleSize = 1;

            if (height > requestedHeight || width > requestedWidth)
            {
                int halfHeight = (int)(height / 2);
                int halfWidth = (int)(width / 2);

                // Calculate a inSampleSize that is a power of 2 - the decoder will use a value that is a power of two anyway.
                while ((halfHeight / inSampleSize) > requestedHeight && (halfWidth / inSampleSize) > requestedWidth)
                {
                    inSampleSize *= 2;
                }
            }

            return (int) inSampleSize;
        }

        public static async Task<Bitmap> DecodeBitmapFromStream(Context context, Android.Net.Uri data, int requestedWidth, int requestedHeight)
        {
            //Decode with InJustDecodeBounds = true, to check dimensions first
            Stream stream = context.ContentResolver.OpenInputStream(data);
            //BitmapFactory.Options options = new BitmapFactory.Options();
            //options.InJustDecodeBounds = true;
            //BitmapFactory.DecodeStreamAsync(stream, null, options);

            Options options = await GetBitmapOptionsOfImageAsync(context, data);

            //Calculate InSampleSize
            options.InSampleSize = CalculateInSampleSize(options, requestedWidth, requestedHeight);

            //Decode bitmap with InSampleSize set
            stream = context.ContentResolver.OpenInputStream(data); //Required to read again
            options.InJustDecodeBounds = false;

            return await BitmapFactory.DecodeStreamAsync(stream, null, options);
        }

        public static async Task<BitmapFactory.Options> GetBitmapOptionsOfImageAsync(Context context, Android.Net.Uri data)
        {
            BitmapFactory.Options options = new BitmapFactory.Options
            {
                InJustDecodeBounds = true
            };

            Stream stream = context.ContentResolver.OpenInputStream(data);

            // The result will be null because InJustDecodeBounds == true.
            Bitmap result = await BitmapFactory.DecodeStreamAsync(stream, null, options);

            int imageHeight = options.OutHeight;
            int imageWidth = options.OutWidth;

            return options;
        }


    }
}