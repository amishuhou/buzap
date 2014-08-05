package com.usedparts.ui.base;

import android.content.Context;
import android.os.Bundle;
import android.util.Log;
import com.usedparts.services.DataClient;
import com.usedparts.services.ServiceException;
import com.usedparts.ui.util.DependencyResolver;
import com.usedparts.ui.util.ViewModelHelper;

public abstract class ServiceActivity<T> extends MenuActivity {

    protected DataClient client;

    private T currentData;

    protected abstract T loadData() throws ServiceException;
    protected abstract void onDataLoaded(T data);

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        client = DependencyResolver.getDataClient(this);
        ViewModelHelper.logFatalErrors();
    }

    @Override
    protected void onResume() {
        super.onResume();
        setIsRestarting(false);
    }

    protected void runServiceRequestAsync(){
        if (getIsRestarting()){
            // TODO: load currentData
            if (currentData != null) {
                onDataLoaded(currentData);
                // TODO: clear currentData
                return;
            }
        }
        new DataLoaderAsyncTask(this).execute();
    }

    @Override
    protected void restartActivity() {
        // TODO: save currentData
        setIsRestarting(true);
        super.restartActivity();
    }

    private String getIsRestartingKey(){
        return String.format("isRestarting_%s", this.getLocalClassName());
    }

    private void setIsRestarting(Boolean v){
        settingsManager.setValue(getIsRestartingKey(), v, Boolean.class);
    }

    private Boolean getIsRestarting(){
        Boolean result = settingsManager.getValue(getIsRestartingKey(), Boolean.class);
        if (result == null)
            result = false;
        return result;
    }

    private class DataLoaderAsyncTask extends ProgressDialogAsyncTask<Object, Object, T> {

        private ServiceException serviceException;

        public DataLoaderAsyncTask(Context context) {
            super(context);
        }

        @Override
        protected void onPostExecute(T data) {
            super.onPostExecute(data);
            if (serviceException != null){
                ViewModelHelper.showServiceError(context, serviceException);
                return;
            }

            currentData = data;

            onDataLoaded(data);
        }

        @Override
        protected T doInBackground(Object... objects) {
            ViewModelHelper.logFatalErrors();
            try {
                serviceException = null;
                return loadData();
            } catch (ServiceException e) {
                Log.e(TAG, "Data loading error", e);
                serviceException = e;
                return null;
            }
        }

    }

}
