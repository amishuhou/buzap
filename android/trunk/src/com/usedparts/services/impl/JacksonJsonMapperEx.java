package com.usedparts.services.impl;

import android.util.Log;
import com.fasterxml.jackson.core.type.TypeReference;
import com.fasterxml.jackson.databind.DeserializationFeature;
import com.fasterxml.jackson.databind.JsonNode;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.usedparts.domain.*;
import com.usedparts.services.JsonMapperEx;
import com.usedparts.services.impl.dto.AuthResult;
import com.usedparts.services.impl.dto.MakeOfferResult;
import com.usedparts.services.impl.dto.OffersResult;
import com.usedparts.services.impl.dto.Response;

import java.io.IOException;
import java.util.List;

public class JacksonJsonMapperEx implements JsonMapperEx {

    private final String TAG = "UsedParts";

    private ObjectMapper mapper = new ObjectMapper();

    public JacksonJsonMapperEx(){
        mapper.configure(DeserializationFeature.FAIL_ON_UNKNOWN_PROPERTIES, false);
    }

    @Override
    public Response toResponse(String json) {

        try {
            JsonNode tree = mapper.readTree(json);
            Response response = new Response();
            response.errorCode = tree.get("err").asInt();
            JsonNode errorNode = tree.get("errstr");
            if (errorNode != null)
                response.errorDescription = errorNode.asText();
            JsonNode dataNode = tree.get("info");
            if (dataNode != null){
                String rawData = dataNode.toString();
                response.rawData = rawData;
            }

            return response;
            //return toSimpleObject(json, Response.class);
        } catch (IOException e) {
            Log.w(TAG, "can't read json: " + json, e);
            return null;
        }
    }

    @Override
    public AuthResult toAuthResult(String json) {
        return toSimpleObject(json, AuthResult.class);
    }

    @Override
    public AccountInfo toAccountInfo(String json) {
        return toSimpleObject(json, AccountInfo.class);
    }

    @Override
    public OffersResult toOffersResult(String json) {
        return toSimpleObject(json, OffersResult.class);
    }

    @Override
    public List<Offer> toOffers(String json) {
        return toSimpleObject(json, new TypeReference<List<Offer>>(){});
    }

    @Override
    public List<Order> toOrders(String json) {
        return toSimpleObject(json, new TypeReference<List<Order>>(){});
    }

    @Override
    public List<Manufacturer> toManufacturers(String json) {
        return toSimpleObject(json, new TypeReference<List<Manufacturer>>(){});
    }

    @Override
    public Order toOrder(String json) {
        return toSimpleObject(json, Order.class);
    }

    @Override
    public OrdersResult toOrdersResult(String json) {
        return toSimpleObject(json, OrdersResult.class);
    }

    @Override
    public MakeOfferResult toMakeOfferResult(String json) {
        return toSimpleObject(json, MakeOfferResult.class);
    }


    public <T> T toSimpleObject(String json, Class<T> c){
        if (json == null)
            return null;
        T v;
        try {
            return mapper.readValue(json, c);
        } catch (IOException e) {
            Log.w(TAG, "can't read json: " + json, e);
            return null;
        }
    }

    public <T> String fromSimpleObject(T v, Class<T> c){
        try {
            return mapper.writeValueAsString(v);
        } catch (IOException e) {
            Log.w(TAG, "can't write json from: " + v.toString(), e);
            return null;
        }
    }

    public <T> T toSimpleObject(String json, TypeReference<T> typeRef){
        if (json == null)
            return null;
        T v;
        try {
            return mapper.readValue(json, typeRef);
        } catch (IOException e) {
            Log.w(TAG, "can't read json: " + json, e);
            return null;
        }
    }
}
