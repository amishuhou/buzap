package com.usedparts.ui.base;

import android.content.Context;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.util.Log;
import com.usedparts.services.DataClient;
import com.usedparts.services.ServiceException;
import com.usedparts.ui.util.DependencyResolver;
import com.usedparts.ui.util.ViewModelHelper;

public abstract class ServiceFragment<T> extends Fragment {

    protected final String TAG = "UsedParts";
    protected DataClient client;

    protected abstract T loadData() throws ServiceException;
    protected abstract void onDataLoaded(T data);

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        client = DependencyResolver.getDataClient(getActivity());
    }

    protected void runServiceRequestAsync(){
        new DataLoaderAsyncTask(getActivity()).execute();
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

            try{
                onDataLoaded(data);
            }catch(Exception e){
                ViewModelHelper.showUnexpectedError(context, e);
            }
        }

        @Override
        protected T doInBackground(Object... objects) {
            ViewModelHelper.logFatalErrors();
            try {
                return loadData();
            } catch (ServiceException e) {
                Log.e(TAG, "Data loading error", e);
                serviceException = e;
                return null;
            }
        }
    }

}
