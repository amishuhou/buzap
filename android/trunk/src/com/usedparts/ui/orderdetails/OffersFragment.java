package com.usedparts.ui.orderdetails;

import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ListView;
import android.widget.TextView;
import com.usedparts.domain.Offer;
import com.usedparts.services.ServiceException;
import com.usedparts.ui.R;
import com.usedparts.ui.adapters.OfferListViewAdapter;
import com.usedparts.ui.base.ServiceFragment;
import com.usedparts.ui.util.Parameters;

import java.util.List;

public class OffersFragment extends ServiceFragment<List<Offer>> {
    public static final String TAG = "offers";

    private List<Offer> data;

    private View v;
    public long orderId;

    private boolean isLoaded;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        Bundle b = getActivity().getIntent().getExtras();
        orderId = b.getLong(Parameters.OrderId);
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        v = inflater.inflate(R.layout.order_details_offers, container, false);
        if (isLoaded)
            onDataLoaded(data);
        else
            runServiceRequestAsync();

        return v;
    }

    public void update(){
        isLoaded = false;
        data = null;
        runServiceRequestAsync();
    }

    @Override
    protected List<Offer> loadData() throws ServiceException {
        return client.getRequestOffers(orderId);
    }

    @Override
    protected void onDataLoaded(List<Offer> data) {
        OfferListViewAdapter adapter = new OfferListViewAdapter(v.getContext(), data);
        ListView list = (ListView)v.findViewById(R.id.offerListView);
        list.setAdapter(adapter);
        TextView emptyText = (TextView)v.findViewById(android.R.id.empty);
        list.setEmptyView(emptyText);
        this.data = data;
        isLoaded = true;
    }
}
