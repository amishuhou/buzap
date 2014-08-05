package com.usedparts.domain;

import com.fasterxml.jackson.annotation.JsonProperty;

public class AccountInfo {

    @JsonProperty("email")
    public String email;

    public String password;
    public String phone;
    public int organizationType;

    public int id;

    @JsonProperty("name")
    public String name;
    @JsonProperty("offers")
    public int offersCount;
    @JsonProperty("cars")
    public int carsCount;
    @JsonProperty("vcurr")
    public double balance;
}