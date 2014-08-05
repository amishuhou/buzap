package com.usedparts.ui;

import android.content.Intent;
import android.os.Bundle;
import android.support.v7.app.ActionBar;
import android.util.Log;
import android.view.*;
import com.usedparts.services.ServiceException;
import com.usedparts.services.SettingsManager;
import com.usedparts.services.gcm.GCMManager;
import com.usedparts.services.gcm.SendRegIdToBackendListener;
import com.usedparts.ui.base.AuthMenuActivity;
import com.usedparts.ui.base.ServiceActivity;
import com.usedparts.ui.main.FavoriteOrdersFragment;
import com.usedparts.ui.main.MyOfferOrdersFragment;
import com.usedparts.ui.main.OrdersFragment;
import com.usedparts.ui.base.TabListener;
import com.usedparts.ui.util.DependencyResolver;
import com.usedparts.ui.util.ViewModelHelper;

import java.util.Locale;

public class MainActivity extends AuthMenuActivity {

    private final int REQUEST_CODE = 0;
    private final String orderTag = "allOrders";
    private boolean isTabsCreated;
    private boolean isAuthorizedDuringTabsCreation;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        initPushNotifications();
        super.onCreate(savedInstanceState);
    }

    @Override
    protected void onResume() {
        super.onResume();

        if (client.isAuthorized()){
            GCMManager gcmManager = DependencyResolver.getGCMManager(this);
            gcmManager.run();
        }

//        ActionBar actionBar = getSupportActionBar();
//        actionBar.setNavigationMode(ActionBar.NAVIGATION_MODE_STANDARD);
//        actionBar.setNavigationMode(ActionBar.NAVIGATION_MODE_TABS);
    }


    private void initPushNotifications(){
        // init push notifications
        GCMManager gcmManager = DependencyResolver.getGCMManager(this);
        gcmManager.setSendRegIdToBackendListener(new SendRegIdToBackendListener() {
            @Override
            public boolean onSendRegIdToBackend(String regId) {
                try{
                    client.registerDevice(regId);
                    return true;
                } catch (ServiceException e) {
                    Log.e(ServiceActivity.TAG, "Device registration error", e);
                    return false;
                }
            }
        });
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {

        if (client.isAuthorized() != isAuthorizedDuringTabsCreation)
            isTabsCreated = false;

        if (!isTabsCreated){

            ActionBar actionBar = getSupportActionBar();
            actionBar.setNavigationMode(ActionBar.NAVIGATION_MODE_TABS);
            actionBar.removeAllTabs();

            ActionBar.Tab tabOrders = actionBar.newTab()
                    .setText(getResources().getString(R.string.orders))
                    .setTabListener(new TabListener<OrdersFragment>(this,
                            orderTag,
                            OrdersFragment.class){
                        @Override
                        public void onTabSelected(boolean isSelected) {
                            super.onTabSelected(isSelected);

                            // show/hide manufacturers filter
                            Menu menu = ((MainActivity)activity).getMenu();
                            if (menu == null)
                                return;
                            try{
                                int[] ids = new int[]{R.id.action_filter, R.id.action_refresh};
                                for (int id : ids){
                                    MenuItem menuItem = menu.findItem(id);
                                    if (menuItem != null)
                                        menuItem.setVisible(isSelected);
                                }
                            } catch (NullPointerException e){
                            }

                        }
                    });
            actionBar.addTab(tabOrders);

            if (client.isAuthorized()){
                ActionBar.Tab tabMyOfferOrders = actionBar.newTab()
                        .setText(getResources().getString(R.string.myOfferOrders))
                        .setTabListener(
                                new TabListener<MyOfferOrdersFragment>(this,
                                        "myOfferOrders",
                                        MyOfferOrdersFragment.class));
                actionBar.addTab(tabMyOfferOrders);
            }


            ActionBar.Tab tabFavoriteOrders = actionBar.newTab()
                    .setText(getResources().getString(R.string.favoriteOrders))
                    .setTabListener(
                            new TabListener<FavoriteOrdersFragment>(this,
                                    "favoriteOrders",
                                    FavoriteOrdersFragment.class));
            actionBar.addTab(tabFavoriteOrders);

            isTabsCreated = true;

        }

        isAuthorizedDuringTabsCreation = client.isAuthorized();

        return super.onCreateOptionsMenu(menu);
    }

    @Override
    protected int getMenuId() {
        return R.menu.main;
    }


    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        if (item.getItemId() == R.id.action_filter){
            openFilter();
            return true;
        }
        if (item.getItemId() == R.id.action_refresh){
            getOrdersFragment().update();
            return true;
        }
        return super.onOptionsItemSelected(item);
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        if (resultCode == RESULT_OK && requestCode == REQUEST_CODE)
            getOrdersFragment().update();

        super.onActivityResult(requestCode, resultCode, data);
    }

    private OrdersFragment getOrdersFragment(){
        return (OrdersFragment)getSupportFragmentManager().findFragmentByTag(orderTag);
    }

    private void openFilter(){
        Intent i = new Intent(this, ManufacturersActivity.class);
        startActivityForResult(i, REQUEST_CODE);
    }

//    public void onRefreshClick(View v){
//        getOrdersFragment().update();
//    }

//    public void onFilterClick(View view){
//        openFilter();
//    }
}

