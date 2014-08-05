package com.usedparts.ui.util;

import android.graphics.Bitmap;
import android.graphics.BitmapFactory;

public final class ImageDecoder {

    private static final int IMAGE_MAX_SIZE = 800;


    public static Bitmap decodeFile(String path){
        Bitmap b = null;

        //Decode image size
        BitmapFactory.Options testOptions = new BitmapFactory.Options();
        testOptions.inJustDecodeBounds = true;

        BitmapFactory.decodeFile(path, testOptions);

        int scale = 1;
        if (testOptions.outHeight > IMAGE_MAX_SIZE || testOptions.outWidth > IMAGE_MAX_SIZE) {
            scale = (int)Math.pow(2, (int) Math.round(Math.log(IMAGE_MAX_SIZE /
                    (double) Math.max(testOptions.outHeight, testOptions.outWidth)) / Math.log(0.5)));
        }

        //Decode with inSampleSize
        BitmapFactory.Options finalOptions = new BitmapFactory.Options();
        finalOptions.inSampleSize = scale;

        b = BitmapFactory.decodeFile(path, finalOptions);

        return b;
    }
}
