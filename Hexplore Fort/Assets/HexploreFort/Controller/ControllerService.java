package com.hkucsfyp.controller;

import android.app.Activity;
import android.bluetooth.BluetoothAdapter;
import android.bluetooth.BluetoothClass;
import android.bluetooth.BluetoothDevice;
import android.bluetooth.BluetoothSocket;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.os.AsyncTask;
import android.os.Handler;
import android.util.Log;
import android.widget.Toast;

import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.util.HashMap;
import java.util.Iterator;
import java.util.Map;
import java.util.Set;
import java.util.UUID;

import com.unity3d.player.UnityPlayer;

public class ControllerService {
    private static final String TAG = "ControllerServiceLog";

    private static ControllerService instance;
    private Activity activity;

    // Bluetooth
    public static final int ENABLE_BLUETOOTH = 1;
    private static final UUID HEXAPOD_UUID = UUID.fromString("00001101-0000-1000-8000-00805F9B34FB");
    private BluetoothAdapter bluetoothAdapter;
    private BluetoothDiscoveryCallback discoveryCallback;
    private BluetoothDiscoveryResultCallback discoveryResultCallback;
    private Map<String, BluetoothDevice> bluetoothDeviceMap;
    private BluetoothDevice bluetoothDevice;
    private BluetoothSocket bluetoothSocket;
    private boolean connected;
    private boolean connecting;

    // Command Handler
    private static final int REGULAR_TIME_INTERVAL = 200;
    private Handler regularHandler;

    // Command Format
    private static final char HEADER = 'V';
    private static final char VERSION = '1';
    private static final int LENGTH = 8;
    public enum MODE_LETTER {
        W('W'),
        D('D'),
        F('F');

        private char value;

        MODE_LETTER(char value) {
            this.value = value;
        }

        public char getValue() {
            return value;
        }
    }
    public enum MODE_NUMBER {
        ONE('1'),
        TWO('2'),
        THREE('3'),
        FOUR('4');

        private char value;

        MODE_NUMBER(char value) {
            this.value = value;
        }

        public char getValue() {
            return value;
        }
    }
    public enum DPAD_LETTER {
        s('s'),
        f('f'),
        b('b'),
        l('l'),
        r('r'),
        w('w');

        private char value;

        DPAD_LETTER(char value) {
            this.value = value;
        }

        public char getValue() {
            return value;
        }
    }
    private static final char BEEP = 'B';

    private MODE_LETTER modeLetter;
    private MODE_NUMBER modeNumber;
    private DPAD_LETTER dPADLetter;
    private int beepFrequency, beepDuration;

    public static final String GAMEOBJECT = "Bluetooth";

    public static ControllerService createInstance(Activity activity) {
        if (instance == null) {
            instance = new ControllerService(activity);
        }
        return instance;
    }

    private ControllerService(Activity activity) {
        Log.d(TAG, "Created new instance");
        this.activity = activity;

        bluetoothAdapter = BluetoothAdapter.getDefaultAdapter();
        bluetoothDeviceMap = new HashMap<>();
        discoveryCallback = new BluetoothDiscoveryCallback();
        discoveryResultCallback = new BluetoothDiscoveryResultCallback();
        connected = false;
        connecting = false;

        modeLetter = MODE_LETTER.W;
        modeNumber = MODE_NUMBER.ONE;
        dPADLetter = DPAD_LETTER.s;
        beepFrequency = 0;
        beepDuration = 0;

        regularHandler = new Handler();
        regularHandler.postDelayed(sendMessage, REGULAR_TIME_INTERVAL);
    }

    public boolean startDiscovery() {
        if (bluetoothAdapter == null) {
            Log.d(TAG, "Bluetooth is not supported");
            Toast.makeText(activity, "Bluetooth is not supported", Toast.LENGTH_LONG).show();
        } else if (!bluetoothAdapter.isEnabled()) {
            Log.d(TAG, "Bluetooth is not enabled");
            Intent intent = new Intent(BluetoothAdapter.ACTION_REQUEST_ENABLE);
            activity.startActivityForResult(intent, ENABLE_BLUETOOTH);
        } else {
            bluetoothDeviceMap.clear();
            Set<BluetoothDevice> bondedDevices = bluetoothAdapter.getBondedDevices();
            Iterator<BluetoothDevice> loop = bondedDevices.iterator();
            while (loop.hasNext()) {
                BluetoothDevice remoteDevice = loop.next();
                BluetoothClass bluetoothClass = remoteDevice.getBluetoothClass();
                if (bluetoothClass.getMajorDeviceClass() == BluetoothClass.Device.Major.UNCATEGORIZED) {
                    Log.d(TAG, "Found " + remoteDevice.getName());
                    bluetoothDeviceMap.put(remoteDevice.getAddress(), remoteDevice);
                    String message = remoteDevice.getAddress() + "%split%" + remoteDevice.getName();
                    UnityPlayer.UnitySendMessage(GAMEOBJECT, "OnScanResult", message);
                }
            }

            if (!bluetoothAdapter.isDiscovering()) {
                Log.d(TAG, "Start discovery");
                activity.registerReceiver(discoveryCallback, new IntentFilter(BluetoothAdapter.ACTION_DISCOVERY_STARTED));
                activity.registerReceiver(discoveryCallback, new IntentFilter(BluetoothAdapter.ACTION_DISCOVERY_FINISHED));
                activity.registerReceiver(discoveryResultCallback, new IntentFilter(BluetoothDevice.ACTION_FOUND));
                bluetoothAdapter.startDiscovery();
            }
            return true;
        }
        return false;
    }

    public class BluetoothDiscoveryCallback extends BroadcastReceiver {
        @Override
        public void onReceive(Context context, Intent intent) {
            String intentActon = intent.getAction();

            if (BluetoothAdapter.ACTION_DISCOVERY_STARTED.equals(intentActon)) {
                Log.d(TAG, "Scanning");
                Toast.makeText(context, "Scanning...", Toast.LENGTH_SHORT).show();
            } else if (BluetoothAdapter.ACTION_DISCOVERY_FINISHED.equals(intentActon)) {
                Log.d(TAG, "Scanning completed");
                Toast.makeText(context, "Scanning completed", Toast.LENGTH_SHORT).show();
            }
        }
    }

    public class BluetoothDiscoveryResultCallback extends BroadcastReceiver {
        @Override
        public void onReceive(Context context, Intent intent) {
            BluetoothDevice remoteDevice = intent.getParcelableExtra(BluetoothDevice.EXTRA_DEVICE);
            if (remoteDevice != null) {
                Log.d(TAG, "Found " + remoteDevice.getName());
                bluetoothDeviceMap.put(remoteDevice.getAddress(), remoteDevice);
                String message = remoteDevice.getAddress() + "%split%" + remoteDevice.getName();
                UnityPlayer.UnitySendMessage(GAMEOBJECT, "OnScanResult", message);
            }
        }
    }

    public void connectHexapod(String bluetoothDeviceAddress) {
        Log.d(TAG, "Trying to connect " + bluetoothDeviceAddress);
        this.bluetoothDevice = bluetoothDeviceMap.get(bluetoothDeviceAddress);
        BluetoothSocketConnectAsyncTask btSocketConnectAsyncTask = new BluetoothSocketConnectAsyncTask(activity, this.bluetoothDevice);
        btSocketConnectAsyncTask.execute();
    }

    //  AsyncTask
    public class BluetoothSocketConnectAsyncTask extends AsyncTask<Void, Void, String> {
        private Activity mActivity;
        private BluetoothDevice mBluetoothDevice;

        public BluetoothSocketConnectAsyncTask(Activity activity, BluetoothDevice bluetoothDevice) {
            super();
            this.mActivity = activity;
            this.mBluetoothDevice = bluetoothDevice;
        }

        @Override
        protected void onPreExecute() {
            super.onPreExecute();
            Log.d(TAG, "Connecting");
            Toast.makeText(mActivity, "Connecting...", Toast.LENGTH_SHORT).show();
        }

        @Override
        protected String doInBackground(Void... voids) {
            try {
                bluetoothSocket = mBluetoothDevice.createInsecureRfcommSocketToServiceRecord(HEXAPOD_UUID);
                bluetoothSocket.connect();
                connected = true;
                connecting = true;
            } catch (IOException e) {
                bluetoothSocket = null;
                connected = false;
                connecting = true;
            }
            return null;
        }
    }

    private Runnable sendMessage = new Runnable() {
        @Override
        public void run() {
            if (connecting) {
                if (connected) {
                    Log.d(TAG, "Connection succeed");
                    Toast.makeText(activity, "Connection succeed", Toast.LENGTH_SHORT).show();
                } else {
                    Log.d(TAG, "Connection failed");
                    Toast.makeText(activity, "Connection failed", Toast.LENGTH_SHORT).show();
                }
                connecting = false;
            }

            if (connected) {
                try {
                    bluetoothSocket.getOutputStream().write(formatMessage());
                } catch (Exception e) {
                    Log.e(TAG, e.toString());
                }
            }
            regularHandler.postDelayed(this, REGULAR_TIME_INTERVAL);
        }
    };

    private byte[] formatMessage() {
        ByteArrayOutputStream outputStream = new ByteArrayOutputStream();

        outputStream.write((byte) HEADER);
        outputStream.write((byte) VERSION);
        outputStream.write((byte) LENGTH);
        outputStream.write((byte) modeLetter.getValue());
        outputStream.write((byte) modeNumber.getValue());
        outputStream.write((byte) dPADLetter.getValue());
        outputStream.write((byte) BEEP);

        int highFreq = (beepFrequency>>8) & 0xff;
        int lowFreq = beepFrequency & 0xff;
        outputStream.write((byte) highFreq);
        outputStream.write((byte) lowFreq);

        int highDur = (beepDuration>>8) & 0xff;
        int lowDur = beepDuration & 0xff;
        outputStream.write((byte) highDur);
        outputStream.write((byte) lowDur);

        int checksum = LENGTH + modeLetter.getValue() + modeNumber.getValue() + dPADLetter.getValue() + BEEP + highFreq + lowFreq + highDur + lowDur;
        checksum = checksum % 256;
        outputStream.write((byte) ((char)checksum));

        return outputStream.toByteArray();
    }

    public void setModeLetter(MODE_LETTER modeLetter) {
        this.modeLetter = modeLetter;
    }

    public void setModeNumber(MODE_NUMBER modeNumber) {
        this.modeNumber = modeNumber;
    }

    public void setDPADLetter(DPAD_LETTER dPADLetter) {
        this.dPADLetter = dPADLetter;
    }

    public void setBeepFrequency(int beepFrequency) {
        this.beepFrequency = beepFrequency;
    }

    public void setBeepDuration(int beepDuration) {
        this.beepDuration = beepDuration;
    }

    public void stopDiscovery() {
        Log.d(TAG, "Stop discovery");
        if (!bluetoothAdapter.isEnabled()) {
            return;
        }
        if (bluetoothAdapter.isDiscovering()) {
            bluetoothAdapter.cancelDiscovery();
            activity.unregisterReceiver(discoveryCallback);
            activity.unregisterReceiver(discoveryResultCallback);
        }
    }

    public void disconnectBluetooth() {
        Log.d(TAG, "Disconnect bluetooth");
        try {
            if (bluetoothSocket.getOutputStream() != null) {
                bluetoothSocket.getOutputStream().close();
            }
            if (bluetoothSocket != null) {
                bluetoothSocket.close();
            }
        } catch (Exception e) {
            Log.e(TAG, e.toString());
        } finally {
            bluetoothSocket = null;
        }
        connected = false;
    }

    public void onDestroy() {
        regularHandler.removeCallbacks(sendMessage);
        stopDiscovery();
        disconnectBluetooth();
    }
}
