package com.usedparts.services.impl;

import android.util.Log;
import com.usedparts.domain.*;
import com.usedparts.services.*;
import com.usedparts.services.impl.dto.*;

import java.net.URLEncoder;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class RestDataClient implements DataClient {

    private final String BASE_URL = "http://api.ucsp.cybermall.by";
    private final String PLATFORM = "android";

    private final String AUTH_TOKEN_KEY = "auth_token";
    private final String PROFILE_KEY  = "profile";

    private final String TAG  = "UsedParts";

    private SettingsManager settings;
    private JsonMapperEx mapper;

    public  RestDataClient(SettingsManager settings, JsonMapperEx mapper){
        this.settings = settings;
        this.mapper = mapper;
    }

    @Override
    public Boolean isAuthorized() {
        String token = settings.getValue(AUTH_TOKEN_KEY, String.class);
        return token != null && token.length() > 0;
    }

    @Override
    public AccountInfo getProfile() {
        return settings.getValue(PROFILE_KEY, AccountInfo.class);
    }

    @Override
    public String login(String email, String password) throws ServiceException {
        Map<String, String> ps = new HashMap<String, String>();
        ps.put("ln", email);
        ps.put("pwd", password);
        AuthResult result = mapper.toAuthResult(get("login", ps, false));
        settings.setValue(AUTH_TOKEN_KEY, result.token, String.class);
        getStatus();
        return result.userId;
    }

    @Override
    public String register(AccountInfo info) throws ServiceException {
        Map<String, String> ps = new HashMap<String, String>();
        ps.put("nln", info.email);
        ps.put("npwd", info.password);
        ps.put("acc_type", Integer.toString(info.organizationType));
        ps.put("acc_name", info.name);
        ps.put("acc_phone", info.phone);
        AuthResult result = mapper.toAuthResult(get("registration", ps, false));
        settings.setValue(AUTH_TOKEN_KEY, result.token, String.class);
        getStatus();
        return result.userId;
    }

    @Override
    public void logout() throws ServiceException {
        get("logout", null, true);
        resetCurrentUser();
    }

    @Override
    public AccountInfo getStatus() throws ServiceException {
        AccountInfo profile = mapper.toAccountInfo(get("usr/status", null, true));
        settings.setValue(PROFILE_KEY, profile, AccountInfo.class);
        return profile;
    }

    @Override
    public boolean registerDevice(String deviceId) throws ServiceException {
        Map<String, String> ps = new HashMap<String, String>();
        ps.put("device", deviceId);
        get("push/android", ps, true);
        return true;
    }

    @Override
    public OrdersResult getRequestsByManufacturer(int manufacturerId, int page) throws ServiceException {
        return mapper.toOrdersResult(get(String.format("requests/%s/%s", manufacturerId != 0 ? manufacturerId : "all", page), null, true));
    }

    @Override
    public OrdersResult getRequestsBySeller(int sellerId, int page) throws ServiceException {
        return mapper.toOrdersResult(get(String.format("requestsbyseller/%s/%s", sellerId, page), null, true));
    }

    @Override
    public Order getRequest(long id) throws ServiceException {
        return mapper.toOrder(get(String.format("request/%s", id), null, true));
    }

    @Override
    public List<Manufacturer> getManufacturers() throws ServiceException {
        return mapper.toManufacturers(get("mfa", null, true));
    }

    @Override
    public List<Offer> getRequestOffers(long requestId) throws ServiceException {
        OffersResult result = mapper.toOffersResult(get(String.format("offers/%s", requestId), null, true));
        return result.items;
    }

    @Override
    public long makeOfferForRequest(Offer offer) throws ServiceException {

        Map<String,String> ps = new HashMap<String, String>();
        ps.put("reqId", String.valueOf(offer.orderId));
        ps.put("cond", String.valueOf(offer.condition));
        ps.put("gara", String.valueOf(offer.warranty));
        ps.put("deli", String.valueOf(offer.delivery));
        ps.put("ava", String.valueOf(offer.available));
        ps.put("price", String.valueOf(offer.price));
        String[] files = null;
        if (offer.images != null)
        {
            files = new String[offer.images.size()];
            offer.images.toArray(files);
        }
        MakeOfferResult result = mapper.toMakeOfferResult(post("makeoffer", ps, files, true));
        return result.OfferId;
    }

    private void resetCurrentUser(){
        settings.resetValue(AUTH_TOKEN_KEY);
        settings.resetValue(PROFILE_KEY);
    }

    private String post(String method, Map<String, String> ps, String[] files, Boolean isAuth) throws ServiceException {

        String authToken = settings.getValue(AUTH_TOKEN_KEY, String.class);
        if (authToken == null)
            authToken = "anonymous";
        String lang = getLang();
        String url = isAuth ? String.format("%s/%s/%s/%s/%s", BASE_URL, lang , PLATFORM, authToken, method)
                : String.format("%s/%/%s/%s", BASE_URL, lang, PLATFORM, method);

        Log.d(TAG, String.format("Posting '%s'", url));

        String json = RestClient.post(url, ps, files);

        Log.d(TAG, String.format("Result '%s':\n%s", url, json));

        if (json == null)
        {
            Log.e(TAG, "Service error: empty result");
            throw new ServiceException(null);
        }

        // validate response
        Response response = mapper.toResponse(json);
        if (response == null)
            throw new ServiceException(null);
        if (response.errorCode != 0)
            throw new ServiceException(response.errorDescription);
        return response.rawData.toString();
    }


    private String get(String method, Map<String, String> ps, Boolean isAuth) throws ServiceException {
        // get JSON

        String authToken = settings.getValue(AUTH_TOKEN_KEY, String.class);
        if (authToken == null)
            authToken = "anonymous";
        String lang = getLang();
        String url = isAuth ? String.format("%s/%s/%s/%s/%s", BASE_URL, lang, PLATFORM, authToken, method)
                : String.format("%s/%s/%s/%s", BASE_URL, lang, PLATFORM, method);

        if (ps != null){
            Boolean isFirst = true;
            for (String k : ps.keySet()){
                if (isFirst)
                    url += "?";
                else
                    url += "&";
                url += String.format("%s=%s", k, URLEncoder.encode(ps.get(k)));
                isFirst = false;
            }
        }

        Log.d(TAG, String.format("Getting '%s'", url));

        String json = RestClient.get(url);

        Log.d(TAG, String.format("Result '%s':\n%s", url, json));

        if (json == null)
        {
            Log.e(TAG, "Service error: empty result");
            throw new ServiceException(null);
        }

        // validate response
        Response response = mapper.toResponse(json);
        if (response == null)
            throw new ServiceException(null);
        if (response.errorCode != 0){
            if (response.errorCode == 19)
                resetCurrentUser();
            throw new ServiceException(response.errorDescription);
        }
        return response.rawData != null ? response.rawData.toString() : null;
    }

    private String getLang(){
        return settings.getValue(SettingsManager.LanguageId, String.class);
    }

}
