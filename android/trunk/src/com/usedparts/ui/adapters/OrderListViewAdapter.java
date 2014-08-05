package com.usedparts.ui.adapters;

import android.content.Context;
import android.graphics.drawable.Drawable;
import android.net.Uri;
import android.text.format.DateFormat;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.BaseAdapter;
import android.widget.ImageView;
import android.widget.TextView;
import com.usedparts.domain.Manufacturer;
import com.usedparts.domain.Order;
import com.usedparts.ui.R;
import com.usedparts.ui.util.ImageLoader;
import com.usedparts.ui.util.ViewModelHelper;

import java.io.InputStream;
import java.net.URL;
import java.util.ArrayList;
import java.util.List;

public class OrderListViewAdapter extends BaseAdapter {

    private final ArrayList<Order> values = new ArrayList<Order>();
    private final Context context;

    public OrderListViewAdapter(Context context){
        this.context = context;
    }

    @Override
    public int getCount() {
        return values.size();
    }

    @Override
    public Object getItem(int position) {
        return values.get(position);
    }

    @Override
    public long getItemId(int position) {
        return position;
    }

    public void clear(){
        values.clear();
        notifyDataSetChanged();
    }

    public void addPage(List<Order> items){
        if (items == null)
            return;
        values.addAll(items);
        notifyDataSetChanged();
    }

    @Override
    public View getView(int position, View convertView, ViewGroup parent) {
        LayoutInflater layoutInflater = (LayoutInflater) context.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
        View rowView = layoutInflater.inflate(R.layout.list_order, parent, false);

        Order order = values.get(position);

        if (order == null)
            return null;

        DateFormat df = new DateFormat();
        String date = df.format("yyyy-MM-dd", order.posted).toString();

        ViewModelHelper.setTextView(rowView, R.id.textOrderName, order.name);
        ViewModelHelper.setTextView(rowView, R.id.textOrderBody, order.body);
        ViewModelHelper.setTextView(rowView, R.id.textOrderPosted, date);
        ViewModelHelper.setTextView(rowView, R.id.textOrderOfferCount, String.valueOf(order.offersCount));
        TextView textViewCounter = (TextView)rowView.findViewById(R.id.textOrderOfferCount);
        textViewCounter.setTextAppearance(context,
                order.offersCount > 0 ? R.style.counter_non_zero : R.style.counter_zero);
        textViewCounter.setBackgroundResource(
                order.offersCount > 0 ? R.drawable.border_counter_active : R.drawable.border_counter);

        ImageView imageView = (ImageView)rowView.findViewById(R.id.imageOrder);
        if (order.thumbs != null && order.thumbs.size() > 0){
            ImageLoader imageLoader = new ImageLoader(context);
            imageLoader.displayImage(order.thumbs.get(0), imageView);
        } else {
            imageView.setVisibility(View.GONE);
            //imageView.setImageResource(R.drawable.default_list_image);
        }

        return rowView;
    }

}