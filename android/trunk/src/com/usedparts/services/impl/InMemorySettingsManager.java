package com.usedparts.services.impl;

import com.usedparts.services.SettingChangedListener;
import com.usedparts.services.SettingsManager;

import java.util.HashMap;

public class InMemorySettingsManager implements SettingsManager {

    private final HashMap<String, Object> settings = new HashMap<String, Object>();

    @Override
    public <T> void setValue(String key, T v, Class<T> c) {
        settings.put(key, v);
    }

    @Override
    public void resetValue(String key) {
        if (settings.containsKey(key))
            settings.remove(key);
    }

    @Override
    public <T> T getValue(String key, Class<T> c) {
        if (settings.containsKey(key))
            return (T) settings.get(key);
        return null;
    }

    @Override
    public void setSettingChangedListener(SettingChangedListener listener) {
    }

    @Override
    public void removeSettingChangedListener(SettingChangedListener listener) {
    }
}
