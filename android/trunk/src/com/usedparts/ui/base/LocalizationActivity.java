package com.usedparts.ui.base;

import android.content.Intent;
import android.os.Bundle;
import android.support.v7.app.ActionBarActivity;
import com.usedparts.services.SettingChangedListener;
import com.usedparts.services.SettingsManager;
import com.usedparts.ui.R;
import com.usedparts.ui.util.DependencyResolver;
import com.usedparts.ui.util.ViewModelHelper;

import java.util.Locale;

public class LocalizationActivity extends ActionBarActivity {

    public static final String TAG = "UsedParts";

    protected SettingsManager settingsManager;
    private SettingChangedListener listener;

    private Boolean isForeground;
    private String currentLanguageId;

    protected Integer getTitleId(){
        return 0;
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        isForeground = true;

        settingsManager = DependencyResolver.getAppSettings(getApplicationContext());

        setLocaleFromSettings();

//        currentLanguageId = settingsManager.getValue(SettingsManager.LanguageId, String.class);
//        if (listener != null)
//            settingsManager.removeSettingChangedListener(listener);
//        listener = new SettingChangedListener() {
//            @Override
//            public void onSettingChanged(String key) {
//                if (key.equals(SettingsManager.LanguageId) && isForeground)
//                    restartActivity();
//            }
//        };
//        settingsManager.setSettingChangedListener(listener);
    }

    protected void setLocaleFromSettings(){
        String languageId = settingsManager.getValue(SettingsManager.LanguageId, String.class);
        if (languageId == null){
            languageId = Locale.getDefault().getLanguage();
            if (!languageId.equals("en") && !languageId.equals("ru"))
                languageId = "ru";
            settingsManager.setValue(SettingsManager.LanguageId, languageId, String.class);
        }
        ViewModelHelper.setLocale(getApplicationContext(), languageId);
    }

    @Override
    protected void onResume() {
        super.onResume();
        isForeground = true;

        setLocaleFromSettings();

//        String languageId = settingsManager.getValue(SettingsManager.LanguageId, String.class);
//        if (currentLanguageId == null || !currentLanguageId.equals(languageId)){
//            restartActivity();
//        }
        Integer stringId = getTitleId();
        if (stringId != 0)
            setTitle(stringId);
        else
            setTitle("");
            //stringId = R.string.app_name;
    }

    @Override
    protected void onPause() {
        super.onPause();
        isForeground = false;
    }

    protected void startChildActivity(Class c){
        Intent intent = new Intent(this, c);
        startActivity(intent);
    }


    protected void restartActivity() {
//        settingsManager.removeSettingChangedListener(listener);
//        Intent intent = getIntent();
//        finish();
//        startActivity(intent);
    }


}
