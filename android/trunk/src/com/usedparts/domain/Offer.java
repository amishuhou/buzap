package com.usedparts.domain;
import com.fasterxml.jackson.annotation.JsonFormat;
import com.fasterxml.jackson.annotation.JsonProperty;

import java.util.*;

public class Offer
{
    @JsonProperty("ix")
    public long id;

    @JsonProperty("cond")
    public int condition;

    @JsonProperty("deli")
    public int delivery;

    @JsonProperty("gara")
    public int warranty;

    @JsonProperty("ava")
    public int available;

    @JsonProperty("price")
    public double price;

    @JsonProperty("offDate")
    @JsonFormat(shape=JsonFormat.Shape.STRING, pattern="dd.MM.yyyy")
    public Date posted;

    @JsonProperty("sellerHead")
    public String selllerName;

    @JsonProperty("sellerLogo")
    public String selllerLogo;

    @JsonProperty("usrId")
    public int sellerId;

    @JsonProperty("reqId")
    public long orderId;

    @JsonProperty("imgs")
    public List<String> images;

    @JsonProperty("thumbs")
    public List<String> thumbs;
}
