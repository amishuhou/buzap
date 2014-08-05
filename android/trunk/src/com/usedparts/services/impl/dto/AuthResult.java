package com.usedparts.services.impl.dto;

import com.fasterxml.jackson.annotation.JsonProperty;

public class AuthResult {
    @JsonProperty("token")
    public String token;
    @JsonProperty("loged")
    public String userId;
}
