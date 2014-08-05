package com.usedparts.ui;

import android.content.Intent;
import android.os.Bundle;
import android.support.v7.app.ActionBar;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.Toast;
import com.usedparts.services.ServiceException;
import com.usedparts.services.gcm.GCMManager;
import com.usedparts.ui.base.FormActivity;
import com.usedparts.ui.base.ServiceActivity;
import com.usedparts.ui.util.DependencyResolver;
import com.usedparts.ui.util.ViewModelHelper;


public class LoginActivity extends FormActivity<String> {

    private String login;
    private String password;

    public int REGISTRATION_VIA_LOGIN_CODE = 99;

    @Override
    protected Integer getTitleId() {
        return R.string.login;
    }

    @Override
    protected int getMenuId() {
        return R.menu.login;
    }

    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.login);
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        if (item.getItemId() == R.id.action_registration){
            //startChildActivity(RegistrationActivity.class);
            Intent intent = new Intent(this, RegistrationActivity.class);
            startActivityForResult(intent, REGISTRATION_VIA_LOGIN_CODE);
            return true;
        }
        return super.onOptionsItemSelected(item);
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        if (requestCode == REGISTRATION_VIA_LOGIN_CODE) {
            if (resultCode == RESULT_OK) {
                finish();
            }
        }
    }

    @Override
    protected void onSubmit(){
        View v = this.getWindow().getDecorView();
        login = ViewModelHelper.getEditText(v, R.id.editUsername);
        password = ViewModelHelper.getEditText(v, R.id.editPassword);

        if (login == null || login.length() == 0){
            showLocalErrorMessage(R.string.email_error);
            return;
        }
        if (password == null || password.length() == 0){
            showLocalErrorMessage(R.string.password_empty_error);
            return;
        }

        runServiceRequestAsync();
    }

    @Override
    protected String loadData() throws ServiceException {
        return client.login(login, password);
    }

    @Override
    protected void onDataLoaded(String token) {
        showSuccessMessage(R.string.login_success);
//        if (client.isAuthorized()){
//            GCMManager gcmManager = DependencyResolver.getGCMManager(this);
//            gcmManager.run();
//        }
        finish();
    }
}