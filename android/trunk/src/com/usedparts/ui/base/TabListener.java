package com.usedparts.ui.base;

import android.app.Activity;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentTransaction;
import android.support.v7.app.ActionBar;

import java.util.ArrayList;
import java.util.List;

public class TabListener<T extends Fragment> implements ActionBar.TabListener {
    protected Fragment fragment;
    protected final Activity activity;
    protected final String tag;
    private final Class<T> mClass;

    //private List<SelectedListener> listeners = new ArrayList<SelectedListener>();

    /** Constructor used each time a new tab is created.
     * @param activity  The host Activity, used to instantiate the fragment
     * @param tag  The identifier tag for the fragment
     * @param clz  The fragment's Class, used to instantiate the fragment
     */
    public TabListener(Activity activity, String tag, Class<T> clz) {
        this.activity = activity;
        this.tag = tag;
        mClass = clz;
    }

    /* The following are each of the ActionBar.TabListener callbacks */

    public void onTabSelected(ActionBar.Tab tab, FragmentTransaction ft) {
        // Check if the fragment is already initialized
        if (fragment == null) {
            // If not, instantiate and add it to the activity
            fragment = Fragment.instantiate(activity, mClass.getName());
            ft.add(android.R.id.content, fragment, tag);
        } else {
            // If it exists, simply attach it in order to show it
            ft.attach(fragment);
        }
        onTabSelected(true);
    }

    public void onTabUnselected(ActionBar.Tab tab, FragmentTransaction ft) {
        if (fragment != null) {
            // Detach the fragment, because another one is being attached
            ft.detach(fragment);
        }
        onTabSelected(false);
    }

    @Override
    public void onTabReselected(ActionBar.Tab tab, FragmentTransaction fragmentTransaction) {
        onTabSelected(true);
    }

    public void onTabSelected(boolean isSelected){
    }

//    public void addSelectedListener(SelectedListener listener) {
//        listeners.add(listener);
//    }
//
//    private void notifySelected(boolean isSelected){
//        for (SelectedListener listener : listeners)
//            listener.onSelected(isSelected);
//    }
//
//    public interface SelectedListener{
//        public void onSelected(boolean isSelected);
//    }
}
