package com.usedparts.ui.base;

import android.app.ProgressDialog;
import android.content.Context;
import android.os.AsyncTask;
import android.util.Log;
import com.usedparts.ui.R;

public abstract class ProgressDialogAsyncTask<T,V,U> extends AsyncTask<T,V,U> {

    protected Context context;

    private ProgressDialog dialog;

    public ProgressDialogAsyncTask(Context context){
        this.context = context;
    }

    @Override
    protected void onPreExecute() {
        dialog = new ProgressDialog(context);
        dialog.setIndeterminate(true);
        dialog.setCancelable(false);
        dialog.setMessage(context.getResources().getString(R.string.loading));
        dialog.show();
    }

    @Override
    protected void onPostExecute(U data) {
        if (dialog.isShowing()) {
            dialog.dismiss();
        }
    }

}
