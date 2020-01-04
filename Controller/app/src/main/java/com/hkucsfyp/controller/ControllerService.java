package com.hkucsfyp.controller;

import android.app.Activity;
import android.bluetooth.BluetoothAdapter;
import android.bluetooth.BluetoothDevice;
import android.bluetooth.BluetoothSocket;
import android.content.Intent;
import android.os.Handler;
import android.util.Log;
import android.widget.Toast;

import java.io.ByteArrayOutputStream;
import java.util.UUID;

public class ControllerService {
    private static final String TAG = "ControllerServiceLog";

    private static ControllerService instance;
    private Activity activity;

    // Bluetooth
    public static final int ENABLE_BLUETOOTH = 1;
    private static final UUID HEXAPOD_UUID = UUID.fromString("00001101-0000-1000-8000-00805F9B34FB");
    private BluetoothAdapter bluetoothAdapter;
    private BluetoothDevice bluetoothDevice;
    private BluetoothSocket bluetoothSocket;
    private boolean connected;

    // Command Handler
    private static final int REGULAR_TIME_INTERVAL = 1000;
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

    public static ControllerService createInstance(Activity activity) {
        if (instance == null) {
            instance = new ControllerService(activity);
        }
        return instance;
    }

    private ControllerService(Activity activity) {
        this.activity = activity;

        connected = false;
        initBluetooth();

        modeLetter = MODE_LETTER.W;
        modeNumber = MODE_NUMBER.ONE;
        dPADLetter = DPAD_LETTER.s;
        beepFrequency = 0;
        beepDuration = 0;

        regularHandler = new Handler();
        regularHandler.postDelayed(sendMessage, REGULAR_TIME_INTERVAL);
    }

    private void initBluetooth() {
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
        if (!connected) {
            try {
                bluetoothDevice = bluetoothAdapter.getRemoteDevice("AB:90:78:56:50:9D");
                bluetoothSocket = bluetoothDevice.createInsecureRfcommSocketToServiceRecord(HEXAPOD_UUID);
                bluetoothAdapter.cancelDiscovery();
                bluetoothSocket.connect();
                connected = true;
                Toast.makeText(activity, "Connection succeed", Toast.LENGTH_SHORT).show();
            } catch (Exception e) {
                Toast.makeText(activity, "Connection failed", Toast.LENGTH_LONG).show();
            }
        } else {
            Toast.makeText(activity, "Connected", Toast.LENGTH_SHORT).show();
        }
    }

    private Runnable sendMessage = new Runnable() {
        @Override
        public void run() {
            if (connected) {
                try {
                    bluetoothSocket.getOutputStream().write(formatMessage());
                } catch (Exception e) {
                    e.printStackTrace();
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

    public void setdPADLetter(DPAD_LETTER dPADLetter) {
        this.dPADLetter = dPADLetter;
    }

    public void setBeepFrequency(int beepFrequency) {
        this.beepFrequency = beepFrequency;
    }

    public void setBeepDuration(int beepDuration) {
        this.beepDuration = beepDuration;
    }
}
