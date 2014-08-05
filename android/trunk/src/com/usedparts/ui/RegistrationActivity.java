package com.usedparts.ui;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.support.v7.app.ActionBar;
import android.view.Menu;
import android.view.View;
import android.widget.Spinner;
import android.widget.Toast;
import com.usedparts.domain.AccountInfo;
import com.usedparts.services.ServiceException;
import com.usedparts.ui.base.FormActivity;
import com.usedparts.ui.base.ServiceActivity;
import com.usedparts.ui.util.ViewModelHelper;


public class RegistrationActivity extends FormActivity<String> {

    private AccountInfo profile;

    @Override
    protected Integer getTitleId() {
        return R.string.registration;
    }

    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.registration);
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {

        ActionBar actionBar = getSupportActionBar();
        actionBar.setDisplayShowTitleEnabled(true);
        actionBar.setDisplayHomeAsUpEnabled(true);

        return super.onCreateOptionsMenu(menu);
    }


    @Override
    protected void onSubmit(){
        View v = this.getWindow().getDecorView();
        profile = new AccountInfo();
        profile.email = ViewModelHelper.getEditText(v, R.id.editUsername);
        profile.password = ViewModelHelper.getEditText(v, R.id.editPassword);
        profile.name = ViewModelHelper.getEditText(v, R.id.editName);
        profile.phone = ViewModelHelper.getEditText(v, R.id.editPhone);

        if (profile.email == null || profile.email.length() == 0){
            showLocalErrorMessage(R.string.email_error);
            return;
        }
        if (profile.password == null || profile.password.length() == 0){
            showLocalErrorMessage(R.string.password_empty_error);
            return;
        }
        if (profile.name == null || profile.name.length() == 0){
            showLocalErrorMessage(R.string.name_empty_error);
            return;
        }


        String chosenOrganizationTypeName = ((Spinner)findViewById(R.id.spinnerOrganizationType))
                .getSelectedItem().toString();
        profile.organizationType = ViewModelHelper.getDictionaryIdByText(this,
                R.array.organization_type_ids,
                R.array.organization_type_strings,
                chosenOrganizationTypeName);

        runServiceRequestAsync();
    }

    @Override
    protected String loadData() throws ServiceException {
        return client.register(profile);
    }

    @Override
    protected void onDataLoaded(String token) {
        showSuccessMessage(R.string.registration_success);

        Intent intent = new Intent();
        if (getParent() == null) {
            setResult(Activity.RESULT_OK, intent);
        } else {
            getParent().setResult(Activity.RESULT_OK, intent);
        }

        finish();
    }
}