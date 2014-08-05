package com.usedparts.ui;

import android.content.Intent;
import android.os.Bundle;
import android.support.v7.app.ActionBar;
import android.view.Menu;
import android.view.View;
import android.widget.AbsListView;
import android.widget.AdapterView;
import android.widget.ListView;
import com.usedparts.domain.Manufacturer;
import com.usedparts.services.ServiceException;
import com.usedparts.services.SettingsManager;
import com.usedparts.services.impl.InMemorySettingsManager;
import com.usedparts.ui.adapters.ManufacturerListViewAdapter;
import com.usedparts.ui.base.ServiceActivity;
import com.usedparts.ui.util.DependencyResolver;
import com.usedparts.ui.util.Parameters;

import java.util.ArrayList;
import java.util.List;

public class ManufacturersActivity extends ServiceActivity<List<Manufacturer>> {

    private Manufacturer selectedManufacturer;

    private SettingsManager settingsManager;

    private static List<Manufacturer> cachedItems;

    @Override
    protected Integer getTitleId() {
        return R.string.manufacturers;
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        setContentView(R.layout.manufacturers);

        settingsManager = DependencyResolver.getAppSettings(this);
//        Bundle b = getIntent().getExtras();
//        if (b != null)
//            selectedManufacturerId = b.getInt(Parameters.ManufacturerId);
        selectedManufacturer = settingsManager.getValue(Parameters.ManufacturerId, Manufacturer.class);

        if (cachedItems != null){
            onDataLoaded(cachedItems);
            return;
        }

        runServiceRequestAsync();
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {

        ActionBar actionBar = getSupportActionBar();
        actionBar.setDisplayShowTitleEnabled(true);
        actionBar.setDisplayHomeAsUpEnabled(true);

        return super.onCreateOptionsMenu(menu);
    }


    @Override
    protected List<Manufacturer> loadData() throws ServiceException {
        List<Manufacturer> items = client.getManufacturers();
        Manufacturer all = new Manufacturer();
        all.name = getResources().getString(R.string.all);
        all.id = 0;
        items.add(0, all);
        return items;
    }

    @Override
    protected void onDataLoaded(List<Manufacturer> data) {
        ManufacturerListViewAdapter adapter = new ManufacturerListViewAdapter(this, data);
        ListView list = (ListView)findViewById(R.id.manufacturerListView);
        list.setAdapter(adapter);
        list.setChoiceMode(AbsListView.CHOICE_MODE_SINGLE);

        // set selected ID
        if (selectedManufacturer != null){
            for (int i = 0; i < data.size(); i++){
                if (data.get(i).id == selectedManufacturer.id){
                    list.setSelection(i);
                    break;
                }
            }
        }

        list.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> parent, View view,
                                    int position, long id) {

                selectedManufacturer = (Manufacturer)parent.getAdapter().getItem(position);
                settingsManager.setValue(SettingsManager.Manufacturer, selectedManufacturer, Manufacturer.class);

                Intent data = new Intent();
//                data.putExtra(Parameters.ManufacturerId, dataItem.id);
//                data.putExtra(Parameters.ManufacturerName, dataItem.name);
                setResult(RESULT_OK, data);
                finish();
            }
        });

        cachedItems = data;
    }
}
