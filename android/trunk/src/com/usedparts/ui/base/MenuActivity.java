package com.usedparts.ui.base;

import android.support.v7.app.ActionBar;
import android.view.Menu;

public abstract class MenuActivity extends LocalizationActivity {

    private final int NO_MENU = 0;

    private Menu menu;

    public Menu getMenu(){
        return menu;
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {

        ActionBar actionBar = getSupportActionBar();
        actionBar.setDisplayShowTitleEnabled(true);

        if (getMenuId() == NO_MENU)
            return true;
        this.menu = menu;
        getMenuInflater().inflate(getMenuId(), menu);
        return super.onCreateOptionsMenu(menu);
    }

    protected int getMenuId(){
        return NO_MENU;
    }
}
