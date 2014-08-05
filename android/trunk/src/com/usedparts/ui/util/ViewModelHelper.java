package com.usedparts.ui.util;

import android.app.Activity;
import android.content.Context;
import android.content.res.Configuration;
import android.graphics.drawable.Drawable;
import android.net.Uri;
import android.util.Log;
import android.view.View;
import android.widget.*;
import com.usedparts.services.ServiceException;
import com.usedparts.ui.R;

import java.io.InputStream;
import java.net.URL;
import java.util.Arrays;
import java.util.Locale;

public final class ViewModelHelper {

    public static void setTextView(View view, int id, String text){
        try{
            TextView textView = (TextView)view.findViewById(id);
            textView.setText(text);
        } catch (Exception e){
            Log.e("UsedParts.UI", String.format("Can't set text for '%'", id), e);
        }
    }

    public static String getEditText(View view, int id){
        EditText editText = (EditText)view.findViewById(id);
        return editText.getText().toString();
    }

    public static void setImageView(View view, int id, String url){
        try{
            ImageView imageView = (ImageView)view.findViewById(id);
            imageView.setImageURI(Uri.parse(url));
        } catch (Exception e){
            Log.e("UsedParts.UI", String.format("Can't set image for '%'", id), e);
        }
    }

    public static int getDictionaryIdByText(Context context, int idArray, int stringArray, String selectedItem){
        int[] ids = context.getResources().getIntArray(idArray);
        String[] names = context.getResources().getStringArray(stringArray);
        int pos = Arrays.asList(names).indexOf(selectedItem);
        return ids[pos];
    }

    public static String getDictionaryTextById(Context context, int idArray, int stringArray, int selectedItem){
        if (selectedItem < 0)
            return null; // argument exception
        int[] ids = context.getResources().getIntArray(idArray);
        if (selectedItem > ids.length - 1)
            return null; // argument exception
        String[] names = context.getResources().getStringArray(stringArray);
        //int pos = Arrays.asList(ids).indexOf(selectedItem); // WTF??
        int pos = -1;
        for(int i = 0; i < ids.length; i++){
            if (ids[i] == selectedItem){
                pos = i;
                break;
            }
        }
        if (pos < 0 || pos > names.length - 1)
            return null;
        return names[pos];
    }
    


    public static void showServiceError(Context context, ServiceException serviceException){
        String details = serviceException.getMessage();
        String category = context.getResources().getString(R.string.service_error);
        String message = details != null ?
                String.format("%s: %s", category, details) : category;
        Toast.makeText(context, message, Toast.LENGTH_LONG).show();
    }

    public static void showUnexpectedError(Context context, Exception serviceException){
        Toast.makeText(context,
                context.getResources().getString(R.string.unexpected_error),
                Toast.LENGTH_LONG).show();
    }

    public static void setLocale(Context context, String languageId){
        Locale locale = new Locale(languageId);
        Locale.setDefault(locale);
        Configuration config = new Configuration();
        config.locale = locale;
        context.getResources().updateConfiguration(config, null);
    }

    public static void logFatalErrors(){
        Thread.setDefaultUncaughtExceptionHandler(new Thread.UncaughtExceptionHandler() {
            @Override
            public void uncaughtException(Thread paramThread, Throwable paramThrowable) {
                Log.e("UsedParts.UI", "Uncaught Exception" ,paramThrowable);
            }
        });
    }
}
