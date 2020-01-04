package com.hkucsfyp.controller;

import android.app.Activity;
import android.bluetooth.BluetoothAdapter;
import android.bluetooth.BluetoothDevice;
import android.bluetooth.BluetoothManager;
import android.bluetooth.BluetoothSocket;
import android.content.Context;
import android.content.Intent;
import android.util.Log;
import android.widget.Toast;

import java.io.ByteArrayOutputStream;
import java.util.UUID;

public class ControllerService {
    private static final String TAG = "ControllerServiceLog";

    public static final int ENABLE_BLUETOOTH = 1;
    private static final UUID hexapodUUID = UUID.fromString("00001101-0000-1000-8000-00805F9B34FB");
    private static ControllerService instance;

    private Activity activity;
    private BluetoothAdapter bluetoothAdapter;
    private BluetoothSocket bluetoothSocket = null;

    public static ControllerService createInstance(Activity activity) {
        if (instance == null) {
            instance = new ControllerService(activity);
        }
        return instance;
    }

    private ControllerService(Activity activity) {
        this.activity = activity;
    }

    public void initBluetooth() {
        bluetoothAdapter = BluetoothAdapter.getDefaultAdapter();
        if (bluetoothAdapter == null) {
            Toast.makeText(activity, "Bluetooth is not supported", Toast.LENGTH_LONG).show();
        } else if (!bluetoothAdapter.isEnabled()) {
            Intent intent = new Intent(BluetoothAdapter.ACTION_REQUEST_ENABLE);
            activity.startActivityForResult(intent, ENABLE_BLUETOOTH);
        } else {
            connectHexapod();
        }
    }

    public void connectHexapod() {
        try {
            BluetoothDevice bluetoothDevice = bluetoothAdapter.getRemoteDevice("AB:90:78:56:50:9D");
            bluetoothSocket = bluetoothDevice.createInsecureRfcommSocketToServiceRecord(hexapodUUID);
            bluetoothAdapter.cancelDiscovery();
            bluetoothSocket.connect();
        } catch (Exception e) {
        }
    }

    public void sd() {
        try {
            bluetoothSocket.getOutputStream().write(formatMessage());
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    private byte[] formatMessage() {
        int input = 100;
        int high=(input>>8) & 0xff;
        int low=input & 0xff;
        ByteArrayOutputStream outputStream = new ByteArrayOutputStream( );
        outputStream.write((byte) 'V');
        outputStream.write((byte) '1');
        outputStream.write((byte) 8);
        outputStream.write((byte) 'W');
        outputStream.write((byte) '1');
        outputStream.write((byte) 'r');
        outputStream.write((byte) 'B');
        outputStream.write((byte) high);
        outputStream.write((byte) low);
        outputStream.write((byte) high);
        outputStream.write((byte) low);
        int checksum = high + low + high + low + 8 + 'W' + '1' + 'r' + 'B';
        checksum = (checksum % 256);
        outputStream.write((byte) ((char)checksum));
        return outputStream.toByteArray();
    }
}
