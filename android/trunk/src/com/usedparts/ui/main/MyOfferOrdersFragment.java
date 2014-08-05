package com.usedparts.ui.main;

import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Toast;
import com.usedparts.domain.AccountInfo;
import com.usedparts.domain.OrdersResult;
import com.usedparts.services.ServiceException;
import com.usedparts.ui.R;

public class MyOfferOrdersFragment extends BaseOrdersContextMenuFragment {

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        View v = super.onCreateView(inflater, container, savedInstanceState);
        if (!isLoaded)
            runServiceRequestAsync();
        return v;
    }

    @Override
    protected OrdersResult loadData() throws ServiceException {
        if (!client.isAuthorized())
        {
//            Toast.makeText(getActivity().getApplicationContext(),
//                    getResources().getString(R.string.need_authentication),
//                    Toast.LENGTH_LONG)
//                    .show();
            return new OrdersResult();
        }
        AccountInfo profile = client.getProfile();
        if (profile == null)
            return new OrdersResult();
        return client.getRequestsBySeller(profile.id, nextPage);
    }

}
