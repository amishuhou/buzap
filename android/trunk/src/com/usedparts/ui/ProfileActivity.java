package com.usedparts.ui;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.res.Configuration;
import android.os.Bundle;
import android.support.v7.app.ActionBar;
import android.view.Menu;
import android.view.View;
import android.widget.AdapterView;
import android.widget.LinearLayout;
import android.widget.Spinner;
import com.usedparts.domain.AccountInfo;
import com.usedparts.services.ServiceException;
import com.usedparts.services.SettingsManager;
import com.usedparts.ui.base.ServiceActivity;
import com.usedparts.ui.util.DependencyResolver;
import com.usedparts.ui.util.Parameters;
import com.usedparts.ui.util.ViewModelHelper;

import java.util.Locale;


public class ProfileActivity extends ServiceActivity<AccountInfo> {

    private SettingsManager settingsManager;
    private String tempLanguageId;
    private Activity self;
    //public final String TempLanguageKey = "temp_lang_id";

    @Override
    protected Integer getTitleId() {
        return R.string.profile;
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        self = this;

        setContentView(R.layout.profile);

        LinearLayout block = (LinearLayout)findViewById(R.id.profile_auth_info);

        if (client.isAuthorized()){
            runServiceRequestAsync();
            block.setVisibility(View.VISIBLE);
        } else
            block.setVisibility(View.GONE);

        // local preferences

        settingsManager = DependencyResolver.getAppSettings(this);

        setLanguageFromSettings();

        initLanguageHandlers();
    }

    private void initLanguageHandlers(){
        Spinner spinnerLanguage = (Spinner)findViewById(R.id.spinnerLanguage);

        spinnerLanguage.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
                String[] ids = getResources().getStringArray(R.array.language_ids);
                String languageId = ids[position];
                //ViewModelHelper.setLocale(getApplicationContext(), languageId);
                String currentLanguageId = settingsManager.getValue(SettingsManager.LanguageId, String.class);
                if (!currentLanguageId.equals(languageId)){

                    //settingsManager.setValue(TempLanguageKey, languageId, String.class);
                    tempLanguageId = languageId;

                    AlertDialog.Builder builder = new AlertDialog.Builder(self);
                    builder
                            .setTitle(getString(R.string.need_restart))
                            .setMessage(getString(R.string.are_you_sure))
                            .setIcon(android.R.drawable.ic_dialog_alert)
                            .setPositiveButton(getString(R.string.yes),
                                    new DialogInterface.OnClickListener() {
                                        public void onClick(DialogInterface dialog, int which) {

                                            settingsManager.setValue(SettingsManager.LanguageId, tempLanguageId, String.class);
                                            // restart app
                                            Intent i = getBaseContext().getPackageManager()
                                                    .getLaunchIntentForPackage( getBaseContext().getPackageName() );
                                            i.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                                            startActivity(i);
                                        }
                                    })
                            .setNegativeButton(getString(R.string.no),
                                    new DialogInterface.OnClickListener() {
                                        public void onClick(DialogInterface dialog, int which) {
                                            // revert
                                            setLanguageFromSettings();
                                        }
                                    })
                            .show();

                }
            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {
            }
        });
    }

    private void setLanguageFromSettings(){
        Spinner spinnerLanguage = (Spinner)findViewById(R.id.spinnerLanguage);
        String languageId = settingsManager.getValue(SettingsManager.LanguageId, String.class);
        if (languageId != null){
            String[] ids = getResources().getStringArray(R.array.language_ids);
            for (int i = 0; i < ids.length; i++){
                if (ids[i].equals(languageId)){
                    spinnerLanguage.setSelection(i);
                    break;
                }
            }
        }
    }

    @Override
    protected AccountInfo loadData() throws ServiceException {
        return client.getProfile();
    }

    @Override
    protected void onDataLoaded(AccountInfo data) {
        View v = getWindow().getDecorView();
        ViewModelHelper.setTextView(v, R.id.textProfileName, data.name);
        ViewModelHelper.setTextView(v, R.id.textProfileEmail, data.email);
        //ViewModelHelper.setTextView(v, R.id.textProfilePhone, data.phone);
        ViewModelHelper.setTextView(v, R.id.textProfileBalance, String.valueOf(data.balance));
        ViewModelHelper.setTextView(v, R.id.textCarCount, String.valueOf(data.carsCount));
        ViewModelHelper.setTextView(v, R.id.textOfferCount, String.valueOf(data.offersCount));
    }

}