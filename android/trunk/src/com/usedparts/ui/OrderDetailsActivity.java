package com.usedparts.ui;

import android.content.Intent;
import android.os.Bundle;
import android.support.v7.app.ActionBar;
import android.view.Menu;
import android.view.MenuItem;
import android.widget.Toast;
import com.usedparts.ui.base.AuthMenuActivity;
import com.usedparts.ui.base.TabListener;
import com.usedparts.ui.orderdetails.*;
import com.usedparts.ui.util.Parameters;
import com.usedparts.ui.util.ViewModelHelper;

public class OrderDetailsActivity extends AuthMenuActivity {

    private boolean isTabsCreated;

    private final Integer MAKE_OFFER_CODE = 0;
    private final String OFFERS_TAG = "offers";

    @Override
    protected Integer getTitleId() {
        return R.string.order_details;
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {

        if (!isTabsCreated){
            ActionBar actionBar = getSupportActionBar();
            actionBar.setNavigationMode(ActionBar.NAVIGATION_MODE_TABS);

            ActionBar.Tab tabDetails = actionBar.newTab()
                    .setText(getResources().getString(R.string.order_details))
                    .setTabListener(
                            new TabListener<DetailsFragment>(this,
                                    "details",
                                    DetailsFragment.class));
            actionBar.addTab(tabDetails);

            ActionBar.Tab tabOffers = actionBar.newTab()
                    .setText(getResources().getString(R.string.order_offers))
                    .setTabListener(
                            new TabListener<OffersFragment>(this,
                                    OFFERS_TAG,
                                    OffersFragment.class));
            actionBar.addTab(tabOffers);

            isTabsCreated = true;
        }

        return super.onCreateOptionsMenu(menu);
    }

    @Override
    protected int getMenuId() {
        return R.menu.order_details;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        if (item.getItemId() == R.id.action_add){
            // check if user is logged in
            if (!client.isAuthorized()){

                Toast.makeText(getApplicationContext(),
                        getResources().getString(R.string.please_login),
                        Toast.LENGTH_LONG)
                        .show();

                return true;
            }

            Intent intent = new Intent(this, MakeOfferActivity.class);
            Bundle b = getIntent().getExtras();
            long orderId = b.getLong(Parameters.OrderId);
            intent.putExtra(Parameters.OrderId, orderId);
            startActivityForResult(intent, MAKE_OFFER_CODE);
            return true;
        }
        return super.onOptionsItemSelected(item);
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        if (requestCode == MAKE_OFFER_CODE && resultCode == MakeOfferActivity.NEW_OFFER_CREATED_RESULT){
            OffersFragment offers = getOffersFragment();
            if (offers != null)
                offers.update();
        }
    }

    private OffersFragment getOffersFragment(){
        return (OffersFragment)getSupportFragmentManager().findFragmentByTag(OFFERS_TAG);
    }

}