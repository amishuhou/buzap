package com.usedparts.ui.orderdetails;

import android.os.Bundle;
import android.text.format.DateFormat;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ListView;
import com.usedparts.domain.Order;
import com.usedparts.services.ServiceException;
import com.usedparts.ui.R;
import com.usedparts.ui.adapters.ImageListViewAdapter;
import com.usedparts.ui.adapters.OfferListViewAdapter;
import com.usedparts.ui.base.ServiceFragment;
import com.usedparts.ui.util.Parameters;
import com.usedparts.ui.util.ViewModelHelper;

public class DetailsFragment extends ServiceFragment<Order> {
    public static final String TAG = "UserParts.UI";

    private Order data;

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
        v = inflater.inflate(R.layout.order_details_details, container, false);
        if (isLoaded)
            onDataLoaded(data);
        else
            runServiceRequestAsync();

        return v;
    }

    @Override
    protected Order loadData() throws ServiceException {
        return client.getRequest(orderId);
    }

    @Override
    protected void onDataLoaded(Order data) {
        ViewModelHelper.setTextView(v, R.id.textOrderName, data.name);
        ViewModelHelper.setTextView(v, R.id.textOrderBody, data.body);

        DateFormat df = new DateFormat();
        String date = df.format("yyyy-MM-dd", data.posted).toString();
        ViewModelHelper.setTextView(v, R.id.textOrderPosted, date);

        ImageListViewAdapter adapter = new ImageListViewAdapter(v.getContext(), data.images);
        ListView list = (ListView)v.findViewById(R.id.offerImagesListView);
        list.setAdapter(adapter);

        this.data = data;
        isLoaded = true;
    }
}
