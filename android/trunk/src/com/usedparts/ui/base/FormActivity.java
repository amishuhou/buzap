package com.usedparts.ui.base;

import android.content.res.Resources;
//import android.support.v7.app.ActionBar;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.Toast;
import com.usedparts.ui.R;

public abstract class FormActivity<T> extends ServiceActivity<T> {

    @Override
    protected int getMenuId() {
        return R.menu.form;
    }

    protected abstract void onSubmit();

    public void onSubmitClick(View view){
        onSubmit();
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        //ActionBar actionBar = getSupportActionBar();
        //actionBar.setDisplayHomeAsUpEnabled(true);
        return super.onCreateOptionsMenu(menu);
    }



    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        if (item.getItemId() == R.id.action_ok){
            onSubmit();
            return true;
        }
        if (item.getItemId() == R.id.action_cancel){
            finish();
            return true;
        }
        return super.onOptionsItemSelected(item);
    }

    protected void showLocalErrorMessage(int errorId){
        Resources r = getResources();
        Toast.makeText(getApplicationContext(),
                String.format("%s: %s",
                        r.getString(R.string.form_validation_error),
                        r.getString(errorId)),
                Toast.LENGTH_LONG)
                .show();

    }

    protected void showSuccessMessage(int messageId){
        Toast.makeText(getApplicationContext(),
                getResources().getString(messageId),
                Toast.LENGTH_LONG)
                .show();
    }


}
