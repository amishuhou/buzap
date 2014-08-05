package com.usedparts.services.impl;

import java.io.*;
import java.util.*;

import android.util.Log;

import org.apache.http.HttpEntity;
import org.apache.http.HttpResponse;
import org.apache.http.NameValuePair;
import org.apache.http.client.ClientProtocolException;
import org.apache.http.client.HttpClient;
import org.apache.http.client.entity.UrlEncodedFormEntity;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.entity.mime.MultipartEntity;
import org.apache.http.entity.mime.content.FileBody;
import org.apache.http.entity.mime.content.StringBody;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.message.BasicNameValuePair;

public class RestClient {

    private static final String LOG_NAME = "RestClient";

    private static String convertStreamToString(InputStream is) {
        /*
         * To convert the InputStream to String we use the BufferedReader.readLine()
         * method. We iterate until the BufferedReader return null which means
         * there's no more data to read. Each line will appended to a StringBuilder
         * and returned as String.
         */
        BufferedReader reader = new BufferedReader(new InputStreamReader(is));
        StringBuilder sb = new StringBuilder();

        String line = null;
        try {
            while ((line = reader.readLine()) != null) {
                sb.append(line + "\n");
            }
        } catch (IOException e) {
            e.printStackTrace();
        } finally {
            try {
                is.close();
            } catch (IOException e) {
                e.printStackTrace();
            }
        }
        return sb.toString();
    }

    /* This is a test function which will connects to a given
     * rest service and prints it's response to Android Log with
     * labels LOG_NAME.
     */
    public static String get(String url)
    {

        HttpClient httpclient = new DefaultHttpClient();

        // Prepare a request object
        HttpGet httpget = new HttpGet(url);

        // Execute the request
        HttpResponse response;
        try {
            response = httpclient.execute(httpget);
            // Examine the response status
            Log.i(LOG_NAME,response.getStatusLine().toString());

            // get hold of the response entity
            HttpEntity entity = response.getEntity();
            // If the response does not enclose an entity, there is no need
            // to worry about connection release

            if (entity != null) {

                // A Simple JSON Response Read
                InputStream inputStream = entity.getContent();
                String result = convertStreamToString(inputStream);
                Log.i(LOG_NAME,result);
                // Closing the input stream will trigger connection release
                inputStream.close();

                return result;
            }


        } catch (ClientProtocolException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        } catch (IOException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }

        return null;
    }

    public static String post(String url, Map<String, String> params, String[] attachments)
    {

        HttpClient httpclient = new DefaultHttpClient();

        // Prepare a request object
        HttpPost post = new HttpPost(url);

        if (attachments != null && attachments.length > 0){
            try{

                MultipartEntity entity = new MultipartEntity();
                if(params != null && params.size() > 0){
                    Iterator iterator = params.keySet().iterator();
                    while(iterator.hasNext()) {
                        String key = (String)iterator.next();
                        String value = params.get(key);
                        entity.addPart(key, new StringBody(value));
                    }
                }
                for (int i = 0; i < attachments.length; i++){
                    File file = new File(attachments[i]);
                    entity.addPart(String.format("img%s", i+1), new FileBody(file));
                }

                post.setEntity(entity);
            } catch (Exception ex){
                Log.e("UsedParts", "Can't create post request, ex");
                return null;
            }
        } else if (params != null){
            List<NameValuePair> nameValuePairs = new ArrayList<NameValuePair>();
            Iterator iterator = params.keySet().iterator();
            while(iterator.hasNext()) {
                String key = (String)iterator.next();
                String value = params.get(key);
                nameValuePairs.add(new BasicNameValuePair(key, value));
            }
            try{
                post.setEntity(new UrlEncodedFormEntity(nameValuePairs));
            } catch (Exception ex){
                Log.e("UsedParts", "Can't create post request, ex");
                return null;
            }
        }

        // Execute the request
        HttpResponse response;
        try {
            response = httpclient.execute(post);
            // Examine the response status
            Log.i(LOG_NAME,response.getStatusLine().toString());

            // get hold of the response entity
            HttpEntity entity = response.getEntity();
            // If the response does not enclose an entity, there is no need
            // to worry about connection release

            if (entity != null) {

                // A Simple JSON Response Read
                InputStream inputStream = entity.getContent();
                String result = convertStreamToString(inputStream);
                Log.i(LOG_NAME,result);
                // Closing the input stream will trigger connection release
                inputStream.close();

                return result;
            }


        } catch (ClientProtocolException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        } catch (IOException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }

        return null;
    }

}
