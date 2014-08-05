package com.usedparts.ui.main;

import android.view.ContextMenu;
import android.view.Menu;
import com.usedparts.domain.Order;
import com.usedparts.ui.R;

public abstract class BaseOrdersContextMenuFragment extends BaseOrdersFragment {

    private final int MENU_ADD_TO_BOOKMARKS_ID = 2;

    @Override
    protected boolean onAddContextMenuOperations(ContextMenu menu, Order dataItem) {
        if (favorites.has(dataItem))
            return false;
        menu.add(Menu.NONE, MENU_ADD_TO_BOOKMARKS_ID, 0, getResources().getString(R.string.add_to_bookmarks));
        return true;
    }

    @Override
    protected void onContextMenuOperation(int menuId, int position, Order dataItem) {
        if (menuId == MENU_ADD_TO_BOOKMARKS_ID){
            favorites.add(dataItem);
            //runServiceRequestAsync();
        }
    }

}
