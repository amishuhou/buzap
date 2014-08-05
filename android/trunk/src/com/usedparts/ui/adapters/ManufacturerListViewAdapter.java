package com.usedparts.ui.adapters;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.TextView;
import com.usedparts.domain.Manufacturer;
import com.usedparts.ui.R;

import java.util.List;

public class ManufacturerListViewAdapter extends ArrayAdapter<Manufacturer> {
    private final Context context;
    private final List<Manufacturer> values;

    public ManufacturerListViewAdapter(Context context, List<Manufacturer> values) {
        super(context, R.layout.list_manufacturer, values);
        this.context = context;
        this.values = values;
    }

    @Override
    public View getView(int position, View convertView, ViewGroup parent) {
        LayoutInflater layoutInflater = (LayoutInflater) context
                .getSystemService(Context.LAYOUT_INFLATER_SERVICE);
        View rowView = layoutInflater.inflate(R.layout.list_manufacturer, parent, false);
        TextView textView = (TextView) rowView.findViewById(R.id.textManufacturerName);
        textView.setText(values.get(position).name);
        return rowView;
    }
}