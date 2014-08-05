package com.usedparts.domain;

import com.fasterxml.jackson.annotation.JsonFormat;
import com.fasterxml.jackson.annotation.JsonProperty;

import java.util.*;

public class Order {
    @JsonProperty("ix")
    public long id;

    @JsonProperty("head")
    public String name;

    @JsonProperty("body")
    public String body;

    @JsonProperty("offers")
    public int offersCount;

    @JsonProperty("advDate")
    @JsonFormat(shape=JsonFormat.Shape.STRING, pattern="dd.MM.yyyy")
    public Date posted;

    @JsonProperty("imgs")
    public List<String> images;

    @JsonProperty("thumbs")
    public List<String> thumbs;

    @Override
    public boolean equals(Object obj) {
        if (this == obj) {
            return true;
        } else if (obj instanceof Order) {
            Order order = (Order) obj;
            return order.id == this.id;
        } else {
            return false;
        }
    }
}
