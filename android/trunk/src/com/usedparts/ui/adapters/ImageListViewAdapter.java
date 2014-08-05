package com.usedparts.ui.adapters;

import android.content.Context;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.net.Uri;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.ImageView;
import android.widget.TextView;
import com.usedparts.domain.Manufacturer;
import com.usedparts.ui.R;
import com.usedparts.ui.util.ImageDecoder;
import com.usedparts.ui.util.ImageLoader;
import com.usedparts.ui.util.ViewModelHelper;

import java.util.List;

public class ImageListViewAdapter extends ArrayAdapter<String> {
    private final Context context;
    private final List<String> values;

    public ImageListViewAdapter(Context context, List<String> values) {
        super(context, R.layout.list_image, values);
        this.context = context;
        this.values = values;
    }

    @Override
    public View getView(int position, View convertView, ViewGroup parent) {
        LayoutInflater layoutInflater = (LayoutInflater) context
                .getSystemService(Context.LAYOUT_INFLATER_SERVICE);
        View rowView = layoutInflater.inflate(R.layout.list_image, parent, false);
        ImageView imageView = (ImageView) rowView.findViewById(R.id.image);
        String url = values.get(position);
        if (url.startsWith("http")){
            ImageLoader imageLoader = new ImageLoader(getContext());
            imageLoader.displayImage(url, imageView);
        } else {
            Bitmap bitmap = ImageDecoder.decodeFile(url);
            imageView.setImageBitmap(bitmap);
        }
        return rowView;
    }

}