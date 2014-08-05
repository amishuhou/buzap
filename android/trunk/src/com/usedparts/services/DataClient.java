package com.usedparts.services;

import java.util.List;
import com.usedparts.domain.*;

public interface DataClient {

    public Boolean isAuthorized();
    public AccountInfo getProfile();

    public String login(String email, String password) throws ServiceException;
    public String register(AccountInfo info) throws ServiceException;

    public void logout() throws ServiceException;
    public AccountInfo getStatus() throws ServiceException;

    public boolean registerDevice(String deviceId) throws ServiceException;

    public OrdersResult getRequestsByManufacturer(int manufacturerId, int page/* = 0*/) throws ServiceException;
    public OrdersResult getRequestsBySeller(int sellerId, int page/* = 0*/) throws ServiceException;
    public Order getRequest(long id) throws ServiceException;
    public List<Manufacturer> getManufacturers() throws ServiceException;
    public List<Offer> getRequestOffers(long requestId) throws ServiceException;
    public long makeOfferForRequest(Offer offer) throws ServiceException;
}
