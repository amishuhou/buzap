package com.usedparts.services;

import com.fasterxml.jackson.core.type.TypeReference;
import com.usedparts.domain.*;
import com.usedparts.services.impl.dto.AuthResult;
import com.usedparts.services.impl.dto.MakeOfferResult;
import com.usedparts.services.impl.dto.OffersResult;
import com.usedparts.services.impl.dto.Response;

import java.util.List;

public interface JsonMapperEx {
    public Response toResponse(String json);
    public AuthResult toAuthResult(String json);
    public AccountInfo toAccountInfo(String json);
    public OffersResult toOffersResult(String json);
    public List<Offer> toOffers(String json);
    public List<Order> toOrders(String json);
    public List<Manufacturer> toManufacturers(String json);
    public Order toOrder(String json);
    public OrdersResult toOrdersResult(String json);
    public MakeOfferResult toMakeOfferResult(String json);

    public <T> T toSimpleObject(String json, Class<T> c);
    public <T> String fromSimpleObject(T v, Class<T> c);
}
