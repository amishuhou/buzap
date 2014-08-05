package com.usedparts.services.impl.dto;

import com.fasterxml.jackson.annotation.JsonProperty;
import com.usedparts.domain.Offer;

import java.util.List;

public class OffersResult {
    @JsonProperty("items")
    public List<Offer> items;
}
