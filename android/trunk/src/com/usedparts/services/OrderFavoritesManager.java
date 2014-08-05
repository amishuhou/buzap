package com.usedparts.services;

import com.usedparts.domain.Order;

import java.util.List;

public interface OrderFavoritesManager {
    public List<Order> getAll();
    public void add(Order order);
    public void remove(int index);
    public boolean has(Order order);
}
