package com.usedparts.domain;

import com.fasterxml.jackson.annotation.JsonProperty;

public class Manufacturer {
    @JsonProperty("mfa_id")
    public int id;
    @JsonProperty("mfa_txt")
    public String name;
}