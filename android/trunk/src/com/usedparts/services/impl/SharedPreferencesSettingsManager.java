package com.usedparts.services.impl;

import android.content.Context;
import android.content.SharedPreferences;
import android.preference.PreferenceManager;
import com.usedparts.services.JsonMapperEx;
import com.usedparts.services.SettingChangedListener;
import com.usedparts.services.SettingsManager;

import java.util.ArrayList;

public class SharedPreferencesSettingsManager implements SettingsManager {


    private Context context;
    private JsonMapperEx mapper = new JacksonJsonMapperEx();

    ArrayList<SettingChangedListener> listeners = new ArrayList<SettingChangedListener> ();

    public SharedPreferencesSettingsManager(Context context){
        this.context = context;
    }

    private SharedPreferences getSettings(){
        return PreferenceManager.getDefaultSharedPreferences(context);
    }

    @Override
    public <T> void setValue(String key, T v, Class<T> c) {
        String oldJson = getSettings().getString(key, null);
        SharedPreferences.Editor editor = getSettings().edit();
        String json = mapper.fromSimpleObject(v, c);
        if (oldJson == json ||
                oldJson != null && json != null && oldJson.equals(json))
            return;
        editor.putString(key, json);
        editor.commit();
        notifySettingChangedListeners(key);
    }

    @Override
    public void resetValue(String key) {
        SharedPreferences.Editor editor = getSettings().edit();
        editor.remove(key);
        editor.commit();
        notifySettingChangedListeners(key);
    }

    @Override
    public <T> T getValue(String key, Class<T> c) {
        String json = getSettings().getString(key, null);
        if (json == null)
            return null;
        return (T)mapper.toSimpleObject(json, c);
    }

    @Override
    public void setSettingChangedListener(SettingChangedListener listener) {
        listeners.add(listener);
    }

    @Override
    public void removeSettingChangedListener(SettingChangedListener listener) {
        if (listeners.contains(listener))
            listeners.remove(listener);
    }

    private void notifySettingChangedListeners(String key){
        ArrayList<SettingChangedListener> tempListeners = new ArrayList<SettingChangedListener>(listeners);
        for (SettingChangedListener listener : tempListeners) {
            listener.onSettingChanged(key);
        }
    }
}
