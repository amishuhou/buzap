package com.usedparts.ui.main;

import android.content.Intent;
import android.os.Bundle;
import android.view.*;
import android.widget.AbsListView;
import android.widget.AdapterView;
import android.widget.ListView;
import android.widget.TextView;
import com.usedparts.domain.Order;
import com.usedparts.domain.OrdersResult;
import com.usedparts.services.OrderFavoritesManager;
import com.usedparts.services.impl.LocalOrderFavoritesManager;
import com.usedparts.services.impl.SharedPreferencesSettingsManager;
import com.usedparts.ui.OrderDetailsActivity;
import com.usedparts.ui.R;
import com.usedparts.ui.adapters.OrderListViewAdapter;
import com.usedparts.ui.base.ServiceFragment;
import com.usedparts.ui.util.DependencyResolver;
import com.usedparts.ui.util.Parameters;

import java.util.ArrayList;

public abstract class BaseOrdersFragment extends ServiceFragment<OrdersResult>
        implements AbsListView.OnScrollListener {

    protected int nextPage = 0;
    protected Integer totalPages;
    protected Integer total;
    protected boolean isLoaded;

    private ListView list;

    protected OrderFavoritesManager favorites;

    private OrderListViewAdapter adapter;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        adapter = new OrderListViewAdapter(getActivity());
        favorites = DependencyResolver.getFavoritesManager(getActivity());
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        View v = inflater.inflate(getLayoutId(), container, false);

        list = (ListView)v.findViewById(R.id.orderListView);
        list.setAdapter(adapter);
        list.setOnScrollListener(this);

        list.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> parent, View view,
                                    int position, long id) {

                Order dataItem = (Order) parent.getAdapter().getItem(position);

                Intent intent = new Intent(view.getContext(), OrderDetailsActivity.class);
                intent.putExtra(Parameters.OrderId, dataItem.id);
                startActivity(intent);
            }
        });

        TextView emptyText = new TextView(getActivity());
        emptyText.setText("No data");
        list.setEmptyView(emptyText);

        registerForContextMenu(list);

        return v;
    }

    @Override
    public void onCreateContextMenu(ContextMenu menu, View v, ContextMenu.ContextMenuInfo menuInfo) {
        super.onCreateContextMenu(menu, v, menuInfo);
        if (v.getId() == R.id.orderListView) {
            AdapterView.AdapterContextMenuInfo info = (AdapterView.AdapterContextMenuInfo)menuInfo;
            Order dataItem = getOrderByPosition(info.position);
            menu.setHeaderTitle(dataItem.name);
            boolean isVisible = onAddContextMenuOperations(menu, dataItem);
            if (isVisible)
                menu.setHeaderTitle(dataItem.name);
        }
    }

    @Override
    public boolean onContextItemSelected(MenuItem item) {
        AdapterView.AdapterContextMenuInfo info = (AdapterView.AdapterContextMenuInfo)item.getMenuInfo();
        int menuId = item.getItemId();
        Order dataItem = getOrderByPosition(info.position);
        onContextMenuOperation(menuId, info.position, dataItem);
        return true;
    }

    private Order getOrderByPosition(int position){
        return (Order)list.getAdapter().getItem(position);
    }

    protected boolean onAddContextMenuOperations(ContextMenu menu, Order dataItem){
        return false;
    }

    protected void onContextMenuOperation(int menuId, int position, Order dataItem){
    }

    protected int getLayoutId() {
        return R.layout.orders;
    }

    public void onScroll(AbsListView view, int firstVisible, int visibleCount, int totalCount) {

        boolean isScrolledToBottom = firstVisible + visibleCount >= totalCount;
        boolean hasMoreData = totalPages != null && totalPages > nextPage;

        if (isScrolledToBottom && hasMoreData) {
            runServiceRequestAsync();
        }
    }

    public void onScrollStateChanged(AbsListView v, int s) {
    }

    @Override
    protected void onDataLoaded(OrdersResult data) {
        isLoaded = true;
        adapter.addPage(data.items);
        totalPages = data.pages;
        total = data.total;
        nextPage++;
    }

    protected void reset(){
        isLoaded = false;
        nextPage = 0;
        totalPages = null;
        total = null;
        adapter.clear();
    }
}
