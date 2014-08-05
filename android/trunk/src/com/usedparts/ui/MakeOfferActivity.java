package com.usedparts.ui;

import android.content.Context;
import android.content.Intent;
import android.database.Cursor;
import android.graphics.Bitmap;
import android.net.Uri;
import android.os.Bundle;
import android.provider.MediaStore;
import android.support.v7.app.ActionBar;
import android.util.Log;
import android.view.ContextMenu;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.inputmethod.InputMethodManager;
import android.widget.*;
import com.usedparts.domain.Offer;
import com.usedparts.domain.Order;
import com.usedparts.services.ServiceException;
import com.usedparts.ui.adapters.ImageListViewAdapter;
import com.usedparts.ui.base.FormActivity;
import com.usedparts.ui.base.ServiceActivity;
import com.usedparts.ui.util.ImageDecoder;
import com.usedparts.ui.util.Parameters;
import com.usedparts.ui.util.ViewModelHelper;

import java.io.FileOutputStream;
import java.util.ArrayList;


public class MakeOfferActivity extends FormActivity<Void> {

    private Offer offer;
    private ArrayList<String> tempImageUris = new ArrayList<String>();
    private long orderId;

    private EditText price;

    private static final int SELECT_PICTURE = 1;
    private final int MENU_REMOVE_IMAGE = 1;

    public static final int NEW_OFFER_CREATED_RESULT = 1;
    //public static final int NOTHING_CHANGED_RESULT = 0;

    @Override
    protected Integer getTitleId() {
        return R.string.make_offer;
    }

    @Override
    protected int getMenuId() {
        return R.menu.make_offer;
    }

    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        Bundle b = getIntent().getExtras();
        orderId = b.getLong(Parameters.OrderId);

        setContentView(R.layout.make_offer);

        ListView list = (ListView)findViewById(R.id.imagesListView);

        int[] ids = new int[] {
                R.id.spinnerAvailability,
                R.id.spinnerCondition,
                R.id.spinnerDelivery,
                R.id.spinnerWarranty
        };
        for (int id : ids){
            Spinner s = (Spinner)findViewById(id);

            s.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener()
            {
                @Override
                public void onItemSelected(AdapterView adapter, View v, int i, long lng) {
                    hideSoftInput();
                }
                @Override
                public void onNothingSelected(AdapterView<?> parentView)
                {
                    hideSoftInput();
                }
            });
        }

        price = (EditText) findViewById(R.id.editPrice);
        price.setOnFocusChangeListener(new View.OnFocusChangeListener() {
            public void onFocusChange(View v, boolean hasFocus) {
                if(!hasFocus){
                    hideSoftInput();
                }

            }
        });

        registerForContextMenu(list);
    }

    private void hideSoftInput(){
        InputMethodManager imm = (InputMethodManager)getSystemService(
                Context.INPUT_METHOD_SERVICE);
        imm.hideSoftInputFromWindow(price.getWindowToken(), 0);
    }


    @Override
    public void onCreateContextMenu(ContextMenu menu, View v, ContextMenu.ContextMenuInfo menuInfo) {
        super.onCreateContextMenu(menu, v, menuInfo);
        if (v.getId() == R.id.imagesListView) {
            menu.add(Menu.NONE, MENU_REMOVE_IMAGE, 0, getResources().getString(R.string.remove_image));
        }
    }

    @Override
    public boolean onContextItemSelected(MenuItem item) {
        AdapterView.AdapterContextMenuInfo info = (AdapterView.AdapterContextMenuInfo)item.getMenuInfo();
        tempImageUris.remove(info.position);
        updateTempImages();
        return super.onContextItemSelected(item);
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        if (item.getItemId() == R.id.action_add_photo){
            addImage();
            return true;
        }
        return super.onOptionsItemSelected(item);
    }


    @Override
    protected void onSubmit(){

        View v = this.getWindow().getDecorView();

        String priceString = ViewModelHelper.getEditText(v, R.id.editPrice);
        if (priceString == null || priceString.length() == 0){
            showLocalErrorMessage(R.string.make_offer_price_error);
            return;
        }

        offer = new Offer();
        offer.price = Double.valueOf(ViewModelHelper.getEditText(v, R.id.editPrice));

        offer.available = getSpinnerSelectedItemId(
                R.id.spinnerAvailability,
                R.array.availability_ids,
                R.array.availability_strings);
        offer.condition = getSpinnerSelectedItemId(
                R.id.spinnerCondition,
                R.array.condition_ids,
                R.array.condition_strings);
        offer.delivery = getSpinnerSelectedItemId(
                R.id.spinnerDelivery,
                R.array.delivery_ids,
                R.array.delivery_strings);
        offer.warranty = getSpinnerSelectedItemId(
                R.id.spinnerWarranty,
                R.array.warranty_ids,
                R.array.warranty_strings);

        offer.images = tempImageUris;

        offer.orderId = orderId;

        runServiceRequestAsync();
    }

    private int getSpinnerSelectedItemId(int spinnerId, int idArray, int stringArray){
        String name = ((Spinner)findViewById(spinnerId))
                .getSelectedItem().toString();
        return ViewModelHelper.getDictionaryIdByText(this,
                idArray,
                stringArray,
                name);

    }

    public void onAddImageClick(View view){
        addImage();
    }

    private void addImage(){
        Intent pickIntent = new Intent();
        pickIntent.setType("image/*");
        pickIntent.setAction(Intent.ACTION_GET_CONTENT);

        Intent takePhotoIntent = new Intent(MediaStore.ACTION_IMAGE_CAPTURE);

        String pickTitle = getResources().getString(R.string.select_picture);
        Intent chooserIntent = Intent.createChooser(pickIntent, pickTitle);
        chooserIntent.putExtra
                (
                        Intent.EXTRA_INITIAL_INTENTS,
                        new Intent[] { takePhotoIntent }
                );

        startActivityForResult(chooserIntent, SELECT_PICTURE);
    }

    protected void onActivityResult(int requestCode, int resultCode, Intent imageReturnedIntent) {
        super.onActivityResult(requestCode, resultCode, imageReturnedIntent);
        switch(requestCode) {
            case SELECT_PICTURE:
                if(resultCode == RESULT_OK){
                    Uri selectedImage = imageReturnedIntent.getData();
                    String[] filePathColumn = { MediaStore.Images.Media.DATA };

                    Cursor cursor = getContentResolver().query(selectedImage,
                            filePathColumn, null, null, null);
                    cursor.moveToFirst();
                    int columnIndex = cursor.getColumnIndex(filePathColumn[0]);
                    String picturePath = cursor.getString(columnIndex);
                    cursor.close();

                    Bitmap bmp = ImageDecoder.decodeFile(picturePath);
                    try{
                        FileOutputStream out = new FileOutputStream(picturePath);
                        bmp.compress(Bitmap.CompressFormat.JPEG, 90, out);
                        out.close();
                    } catch (Exception ex){
                        Log.e(TAG, "issue with image conversion", ex);
                        // TODO: notify
                        break;
                    }

                    tempImageUris.add(picturePath);
                    updateTempImages();
                }

        }
    }

    private void updateTempImages(){
        ImageListViewAdapter adapter = new ImageListViewAdapter(this, tempImageUris);
        ListView list = (ListView)findViewById(R.id.imagesListView);
        list.setAdapter(adapter);

        // show/hide "add photo"
        Menu menu = getMenu();
        if (menu == null)
            return;
        try{
            MenuItem menuItem = menu.findItem(R.id.action_add_photo);
            if (menuItem != null)
                menuItem.setVisible(tempImageUris.size() < 3);
        } catch (NullPointerException e){
        }

        TextView txt = (TextView)findViewById(R.id.textNoPhotos);
        if(tempImageUris.size() > 0)
            txt.setVisibility(View.GONE);
        else
            txt.setVisibility(View.VISIBLE);
    }

    @Override
    protected Void loadData() throws ServiceException {
        client.makeOfferForRequest(offer);
        return null;
    }

    @Override
    protected void onDataLoaded(Void data) {
        showSuccessMessage(R.string.make_offer_success);
        setResult(NEW_OFFER_CREATED_RESULT);
        finish();
    }

}