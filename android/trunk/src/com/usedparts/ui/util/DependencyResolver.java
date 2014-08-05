package com.usedparts.ui.util;

import android.content.Context;
import com.usedparts.services.DataClient;
import com.usedparts.services.OrderFavoritesManager;
import com.usedparts.services.SettingsManager;
import com.usedparts.services.gcm.GCMManager;
import com.usedparts.services.impl.*;

public final class DependencyResolver {

    private static SettingsManager settingsManager;
    private static DataClient dataClient;
    private static GCMManager gcmManager;

    public static DataClient getDataClient(Context context){
        if (dataClient == null)
            dataClient = new RestDataClient(getAppSettings(context), new JacksonJsonMapperEx());
        return dataClient;
    }

    public static OrderFavoritesManager getFavoritesManager(Context context){
        return new LocalOrderFavoritesManager(getAppSettings(context), new JacksonJsonMapperEx());
    }

    public static SettingsManager getAppSettings(Context context){
        if (settingsManager == null)
            settingsManager = new SharedPreferencesSettingsManager(context);
        return settingsManager;
    }

    public static GCMManager getGCMManager(Context context){
        if (gcmManager == null)
            gcmManager = new GCMManager(context);
        return gcmManager;
    }

}
