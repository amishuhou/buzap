package com.usedparts.services.impl;

import com.usedparts.domain.Order;
import com.usedparts.services.JsonMapperEx;
import com.usedparts.services.OrderFavoritesManager;
import com.usedparts.services.SettingsManager;

import java.util.ArrayList;
import java.util.List;

public class LocalOrderFavoritesManager implements OrderFavoritesManager {

    private SettingsManager settings;
    private JsonMapperEx mapper;
    private final String KEY = "favorite_orders";

    public LocalOrderFavoritesManager(SettingsManager settings, JsonMapperEx mapper){
        this.settings = settings;
        this.mapper = mapper;
    }

    @Override
    public List<Order> getAll() {
        return getSavedOrEmptyList();
    }

    @Override
    public void add(Order order) {
        List<Order> items = getSavedOrEmptyList();
        items.add(order);
        saveList(items);
    }

    @Override
    public void remove(int index) {
        List<Order> items = getSavedOrEmptyList();
        items.remove(index);
        saveList(items);
    }

    @Override
    public boolean has(Order order) {
        List<Order> items = getSavedOrEmptyList();
        return items.contains(order);
    }

    private List<Order> getSavedOrEmptyList() {
        String json = settings.getValue(KEY, String.class);
        List<Order> items = mapper.toOrders(json);

        if (items == null)
            return new ArrayList<Order>();
        return items;
    }

    private void saveList(List<Order> items){
        String json = mapper.fromSimpleObject(items, List.class);
        settings.setValue(KEY, json, String.class);
    }

}
