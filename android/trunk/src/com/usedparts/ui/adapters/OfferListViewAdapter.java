package com.usedparts.ui.adapters;

import android.content.Context;
import android.net.Uri;
import android.text.format.DateFormat;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.ImageView;
import com.usedparts.domain.Offer;
import com.usedparts.ui.R;
import com.usedparts.ui.util.ImageLoader;
import com.usedparts.ui.util.ViewModelHelper;

import java.util.ArrayList;
import java.util.List;

public class OfferListViewAdapter extends ArrayAdapter<Offer> {
    private final Context context;
    private final List<Offer> values;

    public OfferListViewAdapter(Context context, List<Offer> values) {
        super(context, R.layout.list_manufacturer, values);
        this.context = context;
        this.values = values;
    }

    @Override
    public View getView(int position, View convertView, ViewGroup parent) {
        LayoutInflater layoutInflater = (LayoutInflater) context
                .getSystemService(Context.LAYOUT_INFLATER_SERVICE);
        View rowView = layoutInflater.inflate(R.layout.list_offer, parent, false);
        Offer offer = values.get(position);

        String price = String.format("%.0f $", offer.price);
        ViewModelHelper.setTextView(rowView, R.id.textOfferPrice, price);

        DateFormat df = new DateFormat();
        String date = df.format("yyyy-MM-dd", offer.posted).toString();
        ViewModelHelper.setTextView(rowView, R.id.textOfferPosted, date);

        ViewModelHelper.setTextView(rowView, R.id.textOfferSeller, offer.selllerName);

        ArrayList<String> options = new ArrayList<String>();
        String option = getDetailsOption(R.string.availability,
                R.array.availability_ids,
                R.array.availability_strings,
                offer.available);
        if (option != null)
            options.add(option);

        option = getDetailsOption(R.string.warranty,
                R.array.warranty_ids,
                R.array.warranty_strings,
                offer.warranty);
        if (option != null)
            options.add(option);

        option = getDetailsOption(R.string.condition,
                R.array.condition_ids,
                R.array.condition_strings,
                offer.condition);
        if (option != null)
            options.add(option);

        option = getDetailsOption(R.string.delivery,
                R.array.delivery_ids,
                R.array.delivery_strings,
                offer.delivery);
        if (option != null)
            options.add(option);

        String details = "";
        for(String s : options){
            if (details.length() != 0)
                details += ", ";
            details += s;
        }

        ViewModelHelper.setTextView(rowView, R.id.textOfferDetails, details);

        ImageView imageView = (ImageView)rowView.findViewById(R.id.imageOffer);
        if (offer.thumbs != null && offer.thumbs.size() > 0){
            ImageLoader imageLoader = new ImageLoader(context);
            imageLoader.displayImage(offer.thumbs.get(0), imageView);
        } else {
            //imageView.setImageResource(R.drawable.default_list_image);
            imageView.setVisibility(View.GONE);
        }

        ImageView imageViewLogo = (ImageView)rowView.findViewById(R.id.imageOfferSellerLogo);
        if (offer.selllerLogo != null)
            imageViewLogo.setImageURI(Uri.parse(offer.selllerLogo));


        return rowView;
    }

    private String getDetailsOption(int label, int idArray, int stringArray, int value){
        String text = ViewModelHelper.getDictionaryTextById(getContext(), idArray, stringArray, value);
        if (text == null)
            return null;
        return String.format("%s: %s", getLabel(label), text);
    }

    private String getLabel(int id){
        return getContext().getResources().getString(id);
    }


}