package com.usedparts.ui.main;

import android.content.res.Resources;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.LinearLayout;
import com.usedparts.domain.Manufacturer;
import com.usedparts.domain.OrdersResult;
import com.usedparts.services.ServiceException;
import com.usedparts.services.SettingsManager;
import com.usedparts.ui.R;
import com.usedparts.ui.util.DependencyResolver;
import com.usedparts.ui.util.Parameters;
import com.usedparts.ui.util.ViewModelHelper;

public class OrdersFragment extends BaseOrdersContextMenuFragment {

    //private Manufacturer manufacturer;

    //private SettingsManager settingsManager;

    private Manufacturer getManufacturer(){
        SettingsManager settingsManager =
                DependencyResolver.getAppSettings(getActivity().getApplicationContext());
        Manufacturer manufacturer = settingsManager.getValue(SettingsManager.Manufacturer, Manufacturer.class);
        return manufacturer;
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {

        View v = super.onCreateView(inflater, container, savedInstanceState);
        LinearLayout filter = (LinearLayout)v.findViewById(R.id.filter);
        filter.setVisibility(View.VISIBLE);
        updateFilterText(v);
        if (!isLoaded)
            runServiceRequestAsync();
        return v;
    }

    public void update(){
        reset();
        updateFilterText(getView());
        runServiceRequestAsync();
    }

    @Override
    protected OrdersResult loadData() throws ServiceException {
        Manufacturer manufacturer = getManufacturer();
        int manufacturerId = manufacturer != null ? manufacturer.id : 0;
        return client.getRequestsByManufacturer(manufacturerId, nextPage);
    }

    @Override
    protected void onDataLoaded(OrdersResult data) {
        super.onDataLoaded(data);
        updateTotalText(getView());
    }

    private void updateFilterText(View v){
        Manufacturer manufacturer = getManufacturer();
        Resources r = getResources();
        String filterText = "";
        if (manufacturer != null && manufacturer.id != 0)
            filterText = String.format("%s: %s", r.getString(R.string.manufacturer), manufacturer.name);
        ViewModelHelper.setTextView(v, R.id.textCurrentFilter, filterText);
    }

    private void updateTotalText(View v){
        Resources r = getResources();
        String totalText = r.getString(R.string.no_orders);
        if (total != null && total > 0)
            totalText = String.format("%s: %s", r.getString(R.string.total), total);
        ViewModelHelper.setTextView(v, R.id.textTotal, totalText);
    }
}
