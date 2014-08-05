package com.usedparts.services;

public interface SettingsManager {

    public final static String Manufacturer = "manufacturer";
    public final static String LanguageId = "languageId";

    public <T> void setValue(String key, T v, Class<T> c);
    public void resetValue(String key);
    public <T> T getValue(String key, Class<T> c);
    public void setSettingChangedListener(SettingChangedListener listener);
    public void removeSettingChangedListener(SettingChangedListener listener);
}
