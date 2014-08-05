package com.usedparts.domain;

import com.fasterxml.jackson.annotation.JsonProperty;

import java.util.List;

public class OrdersResult {
    @JsonProperty("pages")
    public int pages;

    @JsonProperty("total")
    public int total;

    @JsonProperty("items")
    public List<Order> items;
}
