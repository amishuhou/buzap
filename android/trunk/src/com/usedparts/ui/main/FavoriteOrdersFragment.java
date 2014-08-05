package com.usedparts.ui.main;

import android.os.Bundle;
import android.view.*;
import android.widget.AdapterView;
import android.widget.ListView;
import com.usedparts.domain.Order;
import com.usedparts.domain.OrdersResult;
import com.usedparts.services.ServiceException;
import com.usedparts.ui.R;

import java.util.List;

public class FavoriteOrdersFragment extends BaseOrdersFragment {

    private final int MENU_REMOVE_FROM_BOOKMARKS_ID = 1;

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        View v = super.onCreateView(inflater, container, savedInstanceState);
        reset();
        runServiceRequestAsync();
        return v;
    }


    @Override
    protected OrdersResult loadData() throws ServiceException {
        List<Order> items = favorites.getAll();
        OrdersResult result = new OrdersResult();
        result.items = items;
        result.pages = 1;
        return result;
    }

    @Override
    protected boolean onAddContextMenuOperations(ContextMenu menu, Order dataItem) {
        menu.add(Menu.NONE, MENU_REMOVE_FROM_BOOKMARKS_ID, 0,
                getResources().getString(R.string.remove_from_bookmarks));
        return true;
    }

    @Override
    protected void onContextMenuOperation(int menuId, int position, Order dataItem) {
        if (menuId == MENU_REMOVE_FROM_BOOKMARKS_ID){
            favorites.remove(position);
            reset();
            runServiceRequestAsync();
        }
    }

}
