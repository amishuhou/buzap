package com.usedparts.ui.base;

import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import com.usedparts.services.DataClient;
import com.usedparts.services.ServiceException;
import com.usedparts.services.gcm.GCMManager;
import com.usedparts.services.gcm.SendRegIdToBackendListener;
import com.usedparts.ui.*;
import com.usedparts.ui.util.DependencyResolver;
import com.usedparts.ui.util.ViewModelHelper;

public abstract class AuthMenuActivity extends LocalizationActivity {

    protected DataClient client;

    private Menu menu;

    public Menu getMenu(){
        return menu;
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        client = DependencyResolver.getDataClient(this);
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {

        boolean result = super.onCreateOptionsMenu(menu);

        getMenuInflater().inflate(getMenuId(), menu);
        boolean isAuth = client.isAuthorized();
        menu.findItem(R.id.action_login).setVisible(!isAuth);
        menu.findItem(R.id.action_registration).setVisible(!isAuth);
        menu.findItem(R.id.action_logout).setVisible(isAuth);
        menu.findItem(R.id.action_profile).setVisible(true);

        this.menu = menu;

        return result;
    }

    protected abstract int getMenuId();

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        switch (item.getItemId()) {
            case R.id.action_registration:{
                startChildActivity(RegistrationActivity.class);
                return true;
            }
            case R.id.action_login:{
                startChildActivity(LoginActivity.class);
                return true;
            }
            case R.id.action_logout:{
                new LogoutAsyncTask(this).execute();
                return true;
            }
            case R.id.action_profile:{
                startChildActivity(ProfileActivity.class);
                return true;
            }
        }

        return super.onOptionsItemSelected(item);
    }

    @Override
    protected void onResume() {
        super.onResume();

        initAuthButtons();
    }

//    protected void startChildActivity(Class c, int requestCode){
//        Intent intent = new Intent(this, c);
//        startActivityForResult(intent, requestCode);
//    }

    private class LogoutAsyncTask extends ProgressDialogAsyncTask<Void, Void, Void> {

        private ServiceException serviceException;

        public LogoutAsyncTask(Context context) {
            super(context);
        }

        @Override
        protected Void doInBackground(Void... params) {
            ViewModelHelper.logFatalErrors();
            try {
                serviceException = null;
                client.logout();
            } catch (ServiceException e) {
                serviceException = e;
            }
            return null;
        }

        @Override
        protected void onPostExecute(Void aVoid) {
            super.onPostExecute(aVoid);
            if (serviceException != null){
                ViewModelHelper.showServiceError(context, serviceException);
                return;
            }
            initAuthButtons();
        }
    }

    private void initAuthButtons(){
        invalidateOptionsMenu();
    }
}
